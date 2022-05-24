using System;
using System.Linq;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Roadmap.Data;
using Roadmap.DataProvider.MsSql.Ef;
using Roadmap.Domain.Services;
using Roadmap.Domain.Services.Interfaces;
using Roadmap.Models.Db;
using Roadmap.Models.Dto.Dto;
using Roadmap.Models.Dto.Requests.User;
using Tiove.Roadmap.Infrastructure.Mapping;

namespace Roadmap.Domain.UserTests;

public class UserServiceTests
{
    #region Configuration

    #region Private fields

    private IDataProvider _provider;
    private IMapper _mapper;
    private DbContextOptions<RoadmapDbContext> _dbContext;
    private IUserService _repository;

    #endregion

    private CreateUserRequest _correctCreateUserRequest;
    private JsonPatchDocument<EditUserRequest> _userFirstNamePatchDocument;
    private UserDto _createdUser;

    private const string FirstName = "FirstName";
    private const string LastName = "LastName";
    private const string MiddleName = "MiddleName";
    private const string EditedFirstName = "EditedFirstName";

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _dbContext = new DbContextOptionsBuilder<RoadmapDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile<MappingProfile>(); });

        _mapper = mapperConfig.CreateMapper();

        InitializeModels();
    }

    [SetUp]
    public void Setup()
    {
        _provider = new RoadmapDbContext(_dbContext);
        _repository = new UserService(_provider, _mapper);
    }

    [TearDown]
    public void CleanDb()
    {
        if (_provider.IsInMemory())
        {
            _provider.EnsureDeleted();
        }
    }

    #endregion

    #region Private methods
    
    private Guid AddNewDbUser()
    {
        var newDbUser = _mapper.Map<DbUser>(_correctCreateUserRequest);
        _provider.Users.Add(newDbUser);
        _provider.Save();

        return newDbUser.Id;
    }

    private void InitializeModels()
    {
        _correctCreateUserRequest = new CreateUserRequest()
        {
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName
        };

        _userFirstNamePatchDocument = new JsonPatchDocument<EditUserRequest>()
        {
            Operations = {new Operation<EditUserRequest>("replace", "firstname", null, EditedFirstName)}
        };

        _createdUser = new UserDto()
        {
            FirstName = FirstName,
            LastName = LastName,
            MiddleName = MiddleName,
            Status = 0,
            IsActive = true,
            IsAdmin = false
        };
    }

    #endregion
    
    [Test]
    public void ShouldCreateNewUser()
    {
        var userId = _repository.CreateUser(_correctCreateUserRequest, CancellationToken.None).Result;
        var userInDbId = _provider.Users.First().Id;
        Assert.AreEqual(userInDbId, userId);
    }

    [Test]
    public void ShouldGetExistingUser()
    {
        var userId = AddNewDbUser();

        var createdUser = _repository.GetUser(userId, CancellationToken.None).Result;

        Assert.AreEqual(_createdUser.FirstName, createdUser.FirstName);
        Assert.AreEqual(_createdUser.LastName, createdUser.LastName);
        Assert.AreEqual(_createdUser.MiddleName, createdUser.MiddleName);
        Assert.AreEqual(_createdUser.Status, createdUser.Status);
        Assert.AreEqual(_createdUser.IsActive, createdUser.IsActive);
        Assert.AreEqual(_createdUser.IsAdmin, createdUser.IsAdmin);
    }

    [Test]
    public void ShouldEditExistingUser()
    {
        var userId = AddNewDbUser();

        var result = _repository.EditUser(_userFirstNamePatchDocument, userId, CancellationToken.None).Result;

        var dbUserFirstName = _provider.Users.First(x => x.Id == userId).FirstName;

        Assert.IsTrue(result);
        Assert.AreEqual(EditedFirstName, dbUserFirstName);
    }

    [Test]
    public void ShouldDeleteExistingUser()
    {
        var userId = AddNewDbUser();

        _repository.DeleteUser(userId, CancellationToken.None);

        bool isActive = _provider.Users.First(x => x.Id == userId).IsActive;

        Assert.IsFalse(isActive);
    }

    [Test]
    public void ArgumentExceptionWhenNoUser()
    {
        Assert.ThrowsAsync<ArgumentException>(() => _repository.GetUser(Guid.NewGuid(), CancellationToken.None));
    }

    [Test]
    public void ArgumentExceptionWhenUserIsInactiveWhileDelete()
    {
        var userId = AddNewDbUser();

        Assert.IsTrue(_repository.DeleteUser(userId, CancellationToken.None).Result);
        Assert.ThrowsAsync<ArgumentException>(() => _repository.DeleteUser(userId, CancellationToken.None));
    }
}