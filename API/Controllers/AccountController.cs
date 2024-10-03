using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
        private readonly UserManager<AppUser> _UserManager;
        private readonly TokenService _tokenService;
    public AccountController(UserManager<AppUser> UserManager, TokenService tokenService)
    {
            _tokenService = tokenService;
            _UserManager = UserManager;        
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _UserManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized();

        var result = await _UserManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }

        return Unauthorized();
    }
}