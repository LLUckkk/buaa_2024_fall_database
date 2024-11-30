using CampusMarketApi.Core.Models.Requests;
using CampusMarketApi.Core.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;
using CampusMarketApi.Core.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CampusMarketApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [SwaggerOperation(
        Summary = "Register a new user",
        Description = "Registers a new user with the provided username, password, email, and student ID. All fields are required. Returns a RegisterResponse with status code `200` if successful, or a RegisterResponse with status code `400` if the registration fails."
    )]
    [HttpPost("register")]
    [SwaggerResponse(200, "The user was successfully registered", typeof(RegisterResponse))]
    [SwaggerResponse(400, "The user could not be registered")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _userService.RegisterAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }


    [SwaggerOperation(
        Summary = "Login a user",
        Description = "Authenticates a user with username and password. Returns a JWT token if successful."
    )]
    [HttpPost("login")]
    [SwaggerResponse(200, "The user was successfully logged in", typeof(LoginResponse))]
    [SwaggerResponse(401, "The user could not be logged in")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var resp = await _userService.LoginAsync(request);
        return resp != null ? Ok(resp) : Unauthorized(resp);
    }
}

