using AttrectoTest.Application.Contracts.Identity;
using AttrectoTest.Application.Contracts.Logging;
using AttrectoTest.Application.Contracts.Persistence;
using AttrectoTest.Application.Models;
using AttrectoTest.Domain;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttrectoTest.Application.Identity;

internal class AuthService : IAuthService
{
    private readonly IPasswordHasher<AppUser> _hasher;
    private readonly IGenericRepository<AppUser> _db;
    private readonly JwtSettings _jwtSettings;
    private readonly IAppLogger<AuthService> _logger;

    public AuthService(IPasswordHasher<AppUser> hasher, IGenericRepository<AppUser> db, 
        IOptions<JwtSettings> jwtSettings, IAppLogger<AuthService> logger)
    {
        _hasher = hasher;
        _db = db;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<bool> ValidateUser(string userName, string password)
    {
        var user = await _db.GetByAsync(u => u.UserName == userName);
        if (user is null) { 
            _logger.LogWarning("User {UserName} not found during login attempt", userName);
            return false;
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed) { 
            _logger.LogWarning("Invalid password for user {UserName} during login attempt", userName);
            return false;
        }
        _logger.LogInformation("User {UserName} successfully authenticated", userName);
        return true;
    }

    public async Task<(string token, DateTime expires)> GenerateJwtToken(string userName)
    {
        var user = await _db.GetByAsync(u => u.UserName == userName);

        if (user is null) throw new InvalidOperationException("User not found");
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (!string.IsNullOrWhiteSpace(user.RolesCsv))
        {
            foreach (var r in user.RolesCsv.Split(',', StringSplitOptions.RemoveEmptyEntries))
                claims.Add(new Claim(ClaimTypes.Role, r));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(2);
        var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, expires: expires, signingCredentials: creds);
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
