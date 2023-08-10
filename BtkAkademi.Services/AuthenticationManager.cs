using AutoMapper;
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Models;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BtkAkademi.Services;

public class AuthenticationManager : IAuthenticationService
{
    private readonly ILoggerService _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

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
}