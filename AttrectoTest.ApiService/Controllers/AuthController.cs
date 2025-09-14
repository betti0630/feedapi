using AttrectoTest.ApiService.Dtos.Auth;
using AttrectoTest.Application.Contracts.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttrectoTest.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login (JWT)
    /// </summary>
    /// <returns>Successful login</returns>
    [HttpPost, Route("login", Name = "login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginRequest request, CancellationToken cancellationToken)
    {
        if (!await _authService.ValidateUser(request.UserName, request.Password))
        {
            return Unauthorized();
        }
        var (token, expires) = await _authService.GenerateJwtToken(request.UserName);
        return Ok(new LoginResponse(token));
    }

    /// <summary>
    /// User registration
    /// </summary>
    /// <returns>Created</returns>
    [HttpPost, Route("register", Name = "register")]
    public Task<ActionResult<User>> Register([FromBody] RegisterRequest body, CancellationToken cancellationToken)
    {

        throw new NotImplementedException();
    }

}


