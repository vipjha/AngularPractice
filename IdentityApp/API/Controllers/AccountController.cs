using API.DTOs.Accounts;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly JWTService _jwtService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(JWTService jwtService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /*[Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<UserDto>> RefreshUserToken()
        {
            var userName = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User name claim not found.");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            return CreateApplicationDto(user);
        }
*/

        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<UserDto>> RefreshUserToken()
        {
            // Try several common claim names
            var email = User.FindFirst(ClaimTypes.Email)?.Value
                     ?? User.FindFirst("email")?.Value
                     ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email claim not found.");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            return CreateApplicationDto(user);
        }

        

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            /*if (ModelState.IsValid)
            
            }*/

            var user = await _userManager.FindByNameAsync(model.UserName);
            
            if(user== null) return Unauthorized("Invalid username or password.");
            
            if(user.EmailConfirmed == false) return Unauthorized("Email not confirmed.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false); 
            
            if(!result.Succeeded) return Unauthorized("Invalid username or password.");

            return CreateApplicationDto(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest($"An existing account is using (model.Email), email addres. Please try with another email");
            }

            var  userToAdd = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email.ToLower(),
                UserName = model.Email.ToLower(),
                EmailConfirmed = true, // Set to true for testing purposes, in production you may want to send a confirmation email
            };

            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);

            return Ok("User registered successfully.");
        
        }


        #region Private helper method
        private UserDto CreateApplicationDto(User user)
        {
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = _jwtService.CreateJWT(user)
            };
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            //var user = await _userManager.FindByEmailAsync(email);
            //return user != null;
            return await _userManager.Users.AnyAsync(x=>x.Email == email.ToLower());
        }

        #endregion

    }
}
