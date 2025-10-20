using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AttrectoTest.Iam.Application.Identity.Dtos.Auth;
using AttrectoTest.Iam.Application.Contracts.Identity;

namespace AttrectoTest.IamService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IAppUserService userService) : ControllerBase
{

    /// <summary>
    /// Login (JWT)
    /// </summary>
    /// <returns>Successful login</returns>
    [HttpPost, Route("login", Name = "login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginRequest request, CancellationToken cancellationToken)
    {
        if (!await authService.ValidateUser(request.UserName, request.Password, cancellationToken))
        {
            return Unauthorized();
        }
        var (token, expires) = await authService.GenerateJwtToken(request.UserName);
        return Ok(new LoginResponse(token));
    }

    /// <summary>
    /// UserDto registration
    /// </summary>
    /// <returns>Created</returns>
    [HttpPost, Route("register", Name = "register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        await userService.AddNewUser(request.UserName, request.Password, "User", cancellationToken);
        return CreatedAtRoute("register", request.UserName, "User created");    
    }

}


