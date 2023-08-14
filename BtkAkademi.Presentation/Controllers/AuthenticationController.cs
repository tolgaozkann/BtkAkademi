using BtkAkademi.Entities.Dtos;
using BtkAkademi.Presentation.ActionFilters;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademi.Presentation.Controllers;

[ApiController]
[Route("api/Authentication")]
[ApiExplorerSettings(GroupName = "v1")]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _service;

    public AuthenticationController(IServiceManager service)
    {
        _service = service;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {   
        var result = await _service.AuthenticationService.RegisterUser(userRegisterDto);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate(UserAuthenticationDto userAuthenticationDto)
    {
        if (!await _service.AuthenticationService.ValidateUser(userAuthenticationDto))
            return Unauthorized(); //401

        var tokenDto = await _service
            .AuthenticationService
            .CreateToken(populateExp:true);

        return Ok(tokenDto);
    }

    [HttpPost("refresh")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        var newTokenDto = await _service
            .AuthenticationService
            .RefreshToken(tokenDto);
        return Ok(newTokenDto);
    }
}