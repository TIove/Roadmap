using Microsoft.AspNetCore.JsonPatch;
using Roadmap.Models.Dto.Dto;
using Roadmap.Models.Dto.Requests.User;

namespace Roadmap.Domain.Services.Interfaces;

public interface IUserService
{
    Task<Guid> CreateUser(CreateUserRequest request, CancellationToken token);

    Task<UserDto> GetUser(Guid userId, CancellationToken token);

    Task<bool> EditUser(JsonPatchDocument<EditUserRequest> patch, Guid userId, CancellationToken token);

    Task<bool> DeleteUser(Guid userId, CancellationToken token);

    Task<List<UserDto>> GetAllUsers(CancellationToken token);
}