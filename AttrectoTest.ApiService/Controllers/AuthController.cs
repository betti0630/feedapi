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

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        if (!await _authService.ValidateUser(request.UserName, request.Password))
        {
            return Unauthorized();
        }
        var (token, expires) = await _authService.GenerateJwtToken(request.UserName);
        return Ok(new LoginResponse(token, expires));
    }
}

public record LoginRequest(string UserName, string Password);
public record LoginResponse(string Token, DateTime ExpiresAt);
