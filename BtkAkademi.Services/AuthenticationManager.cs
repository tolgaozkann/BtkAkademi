﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Exceptions;
using BtkAkademi.Entities.Models;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BtkAkademi.Services;

public class AuthenticationManager : IAuthenticationService
{
    private readonly ILoggerService _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    private User? _user;
    public AuthenticationManager(ILoggerService logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUser(UserRegisterDto userRegisterDto)
    {
        var user = _mapper.Map<User>(userRegisterDto);
        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userRegisterDto.Roles);
        return result;
    }

    public async Task<bool> ValidateUser(UserAuthenticationDto userAuthenticationDto)
    {
        _user = await _userManager.FindByNameAsync(userAuthenticationDto.UserName);

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userAuthenticationDto.Password));

        if (!result)
        {
            _logger.LogWarning($"{nameof(ValidateUser)} : Authentication is failed. Wrong username or password.");
        }

        return result;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signinCredentials = GetSigninCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

        var refreshToken = GenerateRefreshToken();

        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(_user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);


        return new TokenDto
        {
            RefreshToken = refreshToken,
            AccessToken = accessToken
        };
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principle = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var user = await _userManager.FindByNameAsync(principle.Identity.Name);

        if (user is null ||
            user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpireTime <= DateTime.Now)
        {
            throw new RefreshTokenBadRequestException();
        }

        _user = user;

        return await CreateToken(populateExp: false);

    }

    private SigningCredentials GetSigninCredentials()
    {
        var settings = _configuration.GetSection("JwtSettings");

        var key = Encoding.UTF8.GetBytes(settings["secretKey"]);

        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,_user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
    {
        var settings = _configuration.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken(
            issuer: settings["validIssuer"],
            audience: settings["validAuidence"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(settings["expires"])),
            signingCredentials: signinCredentials
            );

        return tokenOptions;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["secretKey"];

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidAudience = jwtSettings["validAuidence"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))

        };

        var tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
            out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken is null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid Token!");

        return principal;
    }
}