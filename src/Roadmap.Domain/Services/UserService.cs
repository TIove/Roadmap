using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Roadmap.Data;
using Roadmap.Domain.Services.Interfaces;
using Roadmap.Models.Db;
using Roadmap.Models.Dto.Dto;
using Roadmap.Models.Dto.Requests.User;

namespace Roadmap.Domain.Services;

public class UserService : IUserService
{
    private readonly IDataProvider _provider;
    private readonly IMapper _mapper;

    public UserService(IDataProvider provider, IMapper mapper)
    {
        _provider = provider;
        _mapper = mapper;
    }

    public async Task<Guid> CreateUser(CreateUserRequest request, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var dbUser = _mapper.Map<DbUser>(request);

        dbUser.CreatedBy = Guid.Empty; // TODO with authentication
        dbUser.CreatedAtUtc = DateTime.UtcNow;

        await _provider.Users.AddAsync(dbUser, token);
        await _provider.SaveAsync(token);

        return dbUser.Id;
    }

    public async Task<UserDto> GetUser(Guid userId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var dbUser = await _provider.Users.FirstOrDefaultAsync(x => x.Id == userId, token);

        if (dbUser == null)
        {
            throw new ArgumentException($"User with id = '{userId}' not found");
        }

        var userDto = _mapper.Map<UserDto>(dbUser);

        return userDto;
    }

    public async Task<bool> EditUser(JsonPatchDocument<EditUserRequest> patch, Guid userId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var dbUser = await _provider.Users.FirstOrDefaultAsync(x => x.Id == userId, token);

        if (dbUser == null)
        {
            throw new ArgumentException($"User with id = '{userId}' not found");
        }

        var dbPatch = _mapper.Map<JsonPatchDocument<EditUserRequest>, JsonPatchDocument<DbUser>>(patch);

        dbPatch.ApplyTo(dbUser);
        await _provider.SaveAsync(token);

        return true;
    }

    public async Task<bool> DeleteUser(Guid userId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var dbUser = _provider.Users.FirstOrDefault(x => x.Id == userId);

        if (dbUser == null)
        {
            throw new ArgumentException($"User with id = '{userId}' not found");
        }

        if (dbUser.IsActive == false)
        {
            throw new ArgumentException($"User with id = '{userId}' already inactive");
        }

        dbUser.IsActive = false;
        await _provider.SaveAsync(token);

        return true;
    }
}