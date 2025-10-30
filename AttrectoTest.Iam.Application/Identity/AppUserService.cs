﻿using AttrectoTest.Application.Exceptions;
using AttrectoTest.Iam.Application.Contracts.Identity;
using AttrectoTest.Iam.Application.Contracts.Notification;
using AttrectoTest.Iam.Application.Contracts.Persistence;
using AttrectoTest.Iam.Application.Identity.Dtos;
using AttrectoTest.Iam.Application.Models;
using AttrectoTest.Iam.Domain;
using AttrectoTest.Iam.Infrastructure.Model;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttrectoTest.Iam.Application.Identity;

internal sealed class AppUserService(IPasswordHasher<AppUser> hasher, IGenericRepository<AppUser> db,
    INotificationService notificationService, IOptions<JwtSettings> jwtSettings, IOptions<ApiSettings> apiSettings) : IAppUserService
{
    public async Task SeedNewUser(string userName, string password, string firstName, string lastName, string email, string? roles, CancellationToken cancellationToken = default)
    {
        await AddNewUserInner(true, userName, password, firstName, lastName, email, roles, cancellationToken);
    }
    public async Task AddNewUser(string userName, string password, string firstName, string lastName, string email, string? roles, CancellationToken cancellationToken = default)
    { 
        await AddNewUserInner(false, userName, password, firstName, lastName, email, roles, cancellationToken);
    }

    private async Task AddNewUserInner(bool isSeeded, string userName, string password, string firstName, string lastName, string email, string? roles, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new InvalidOperationException("Username cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(password)) {
            throw new InvalidOperationException("Password cannot be empty.");
        }
        if (userName.Length < 3) {
            throw new InvalidOperationException("Username must be at least 3 characters long.");
        }

        if (await db.AnyAsync(u => u.UserName == userName, cancellationToken))
        {
            throw new InvalidOperationException($"User with username '{userName}' already exists.");
        }
        if (string.IsNullOrWhiteSpace(roles))
        {
            roles = "User";
        }
        if (string.IsNullOrWhiteSpace(password)) {
            throw new InvalidOperationException("Password cannot be empty.");
        }
        if (password.Length < 6) {
            throw new InvalidOperationException("Password must be at least 6 characters long.");
        }
        if (password.Length > 100) {
            throw new InvalidOperationException("Password cannot be longer than 100 characters.");
        }
        var user = new AppUser
        {
            UserName = userName,
            RolesCsv = roles,
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };
        user.PasswordHash = hasher.HashPassword(user, password);
        if (isSeeded) {
            user.ValidatedEmail = true;
        }
        await db.CreateAsync(user, cancellationToken);
        if (!isSeeded) {
            var token = GenerateEmailVerificationToken(user.Id);
            await notificationService.SendRegistrationEmail(user.Id, token, apiSettings.Value.EmailVerificationUrl);
        }
    }

    private string GenerateEmailVerificationToken(int userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim("userId", userId.ToString()),
        new Claim("type", "emailVerification")
    };

        var token = new JwtSecurityToken(
            jwtSettings.Value.Issuer,
            jwtSettings.Value.Audience,
            claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private int ValidateAndGetUserIdFromEmailVerificationToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Value.Issuer,
            ValidAudience = jwtSettings.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key))
        };

        try
        {
            var principal = handler.ValidateToken(token, parameters, out _);
            var tokenUserId = principal.FindFirst("userId")?.Value;
            return Int32.Parse(tokenUserId);
        }
        catch
        {
            throw new BadRequestException("Invalid token");
        }
    }

    public async Task<int> GetUserIdByUserName(string userName, CancellationToken cancellationToken = default)
    {
        var result = await db.GetByAsync(u => u.UserName.Equals(userName.Trim(), StringComparison.OrdinalIgnoreCase), cancellationToken);
        if (result == null) {
            throw new InvalidOperationException($"Not existing userName {userName}");
        }
        return result.Id;
    }

    public async Task<UserDataDto> GetUserData(int userId, CancellationToken cancellationToken = default)
    {
        var result = await db.GetByIdAsync(userId, cancellationToken);
        if (result == null)
        {
            throw new InvalidOperationException($"Not existing userId {userId}");
        }
        return new UserDataDto
        {
            Id = userId,
            UserName = result.UserName,
            RolesCsv = result.RolesCsv,
            FirstName=result.FirstName,
            LastName=result.LastName,
            Email = result.Email
        };
    }

    public async Task<int> MarkEmailAsVerified(string token, CancellationToken cancellationToken = default)
    {
        var userId = ValidateAndGetUserIdFromEmailVerificationToken(token);
        var user = await db.GetByIdAsync(userId, cancellationToken);
        if (user == null) {
            throw new BadRequestException("Invalid User");
        }
        user.ValidatedEmail = true;
        await db.UpdateAsync(user, cancellationToken);
        return user.Id;
    }
}
