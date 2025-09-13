using AttrectoTest.Application.Contracts.Identity;

using FastEndpoints;

namespace AttrectoTest.ApiService.Endpoints.Auth;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    public IAuthService AuthService { get; set; } = default!;

    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "User login";
            s.Description = "This endpoint validate the username and the password and return a JWT token.";
            s.ExampleRequest = new LoginRequest("test", "Passw0rd!");
        });
  
        Description(b => b
            .Produces(200, typeof(LoginResponse), "application/json")
            .Produces(401)
);

        Tags("Auth");
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        if (!await AuthService.ValidateUser(req.UserName, req.Password)) { 
            await Send.UnauthorizedAsync();
        }
        var (token, expires) = await AuthService.GenerateJwtToken(req.UserName);
        await Send.OkAsync(new LoginResponse(token, expires), ct);
    }
}

public record LoginRequest(string UserName, string Password);
public record LoginResponse(string Token, DateTime ExpiresAt);
