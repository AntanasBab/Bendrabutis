using Bendrabutis.Auth;
using Bendrabutis.Entities;
using Bendrabutis.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bendrabutis.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [EnableCors("CorsApi")]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        public readonly UserManager<User> _userManager;
        private readonly ITokenManager _tokenManager;

        public AuthController(UserManager<User> userManager, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _tokenManager = tokenManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.Username);
            if (user != null)
            {
                return BadRequest("Request invalid");
            }

            var newUser = new User {UserName = registerUserDto.Username, Email = registerUserDto.Username + "@ktu.lt"};
            var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!createUserResult.Succeeded)
            {
                return BadRequest(createUserResult.Errors);
            }

            await _userManager.AddToRoleAsync(newUser, DormitoryRoles.Visitor);
            return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.Email));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.email);
            if (user == null)
            {
                return BadRequest("Username or password is invalid.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.password);
            if (!isPasswordValid)
            {
                return BadRequest("Username or password is invalid.");
            }

            var token = await _tokenManager.CreateAccessTokenAsync(user);
            return Ok(new SuccessfulLoginDto(token));
        }
    }
}
