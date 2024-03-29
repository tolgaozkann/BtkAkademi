﻿using BtkAkademi.Entities.Dtos;
using Microsoft.AspNetCore.Identity;

namespace BtkAkademi.Services.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserRegisterDto userRegisterDto);
    Task<bool> ValidateUser(UserAuthenticationDto userAuthenticationDto);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}