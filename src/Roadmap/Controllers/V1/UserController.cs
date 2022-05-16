using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Tiove.Roadmap.Controllers.V1;

[ApiController]
[Route("v1/api/user")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    public UserController()
    {
    }
    
    [HttpPost("create")]
    public ActionResult CreateUser(CancellationToken token)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("get/{userId}")]
    public ActionResult GetUser(Guid userId, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    [HttpGet("edit/{userId}")]
    public ActionResult EditUser(Guid userId, CancellationToken token)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("delete/{userId}")]
    public ActionResult DeleteUser(Guid userId, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}