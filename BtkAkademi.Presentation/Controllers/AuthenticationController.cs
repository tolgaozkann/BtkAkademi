using BtkAkademi.Entities.Dtos;
using BtkAkademi.Presentation.ActionFilters;
using BtkAkademi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BtkAkademi.Presentation.Controllers;

[ApiController]
[Route("api/Authentication")]
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

        return Ok(new
        {
            Token = await _service.AuthenticationService.CreateToken()
        });
    }
}