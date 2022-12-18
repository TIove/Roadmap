using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Roadmap.Domain.Services.Interfaces;
using Roadmap.Models.Dto.Dto;
using Roadmap.Models.Dto.Requests.User;

namespace Tiove.Roadmap.Controllers.V1;

[ApiController]
[Route("v1/api/user")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController([FromServices] IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateUser(CreateUserRequest request, CancellationToken token)
    {
        return Ok(await _userService.CreateUser(request, token));
    }

    [HttpGet("get/{userId}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid userId, CancellationToken token)
    {
        UserDto response;
        try
        {
            response = await _userService.GetUser(userId, token);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }

        return Ok(response);
    }

    [HttpPatch("edit/{userId}")]
    public async Task<ActionResult<bool>> EditUser(
        JsonPatchDocument<EditUserRequest> request,
        Guid userId,
        CancellationToken token)
    {
        return Ok(await _userService.EditUser(request, userId, token));
    }

    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<bool>> DeleteUser(Guid userId, CancellationToken token)
    {
        return Ok(await _userService.DeleteUser(userId, token));
    }
}