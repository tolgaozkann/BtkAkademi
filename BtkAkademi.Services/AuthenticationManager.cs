using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BtkAkademi.Entities.Dtos;
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
        var result = await _userManager.CreateAsync(user,userRegisterDto.Password);

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

    public async Task<string> CreateToken()
    {
        var signinCredentials = GetSigninCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
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
            claims.Add(new Claim(ClaimTypes.Role,role));
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
}