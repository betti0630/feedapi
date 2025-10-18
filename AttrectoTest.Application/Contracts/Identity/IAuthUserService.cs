﻿using AttrectoTest.Domain;

namespace AttrectoTest.Application.Contracts.Identity;

public interface IAuthUserService
{
    public int? UserId { get; }
    public string? UserName { get; }
}
