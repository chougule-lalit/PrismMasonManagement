using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrismMasonManagement.Configuration;
using PrismMasonManagement.DTOs.Requests;
using PrismMasonManagement.DTOs.Responses;

namespace PrismMasonManagement.Controllers
{
    [Route("api/[controller]")] //api/authManagement
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(UserManager<IdentityUser> userManager,
        IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                //We can utilize the model
                var exisitingUser = await _userManager.FindByEmailAsync(user.Email);

                if (exisitingUser != null)
                {
                    return BadRequest(new RegistrationResponseDto
                    {
                        Errors = new List<string> { "Email already exists" },
                        IsSuccess = false
                    });
                }

                var newUser = new IdentityUser()
                {
                    Email = user.Email,
                    UserName = user.Username

                };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);
                    return Ok(new RegistrationResponseDto
                    {
                        IsSuccess = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new RegistrationResponseDto
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        IsSuccess = false
                    });
                }
            }
            return BadRequest(new RegistrationResponseDto()
            {
                Errors = new List<string> { "Invalid payload" },
                IsSuccess = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var exisitingUser = await _userManager.FindByEmailAsync(user.Email);
                if (exisitingUser == null)
                {
                    return BadRequest(new RegistrationResponseDto
                    {
                        Errors = new List<string> { "Invalid login request" },
                        IsSuccess = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(exisitingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new RegistrationResponseDto
                    {
                        Errors = new List<string> { "Invalid login request" },
                        IsSuccess = false
                    });
                }

                var jwtToken = GenerateJwtToken(exisitingUser);

                return Ok(new RegistrationResponseDto
                {
                    IsSuccess = true,
                    Token = jwtToken
                });
            }

            return BadRequest(new RegistrationResponseDto()
            {
                Errors = new List<string> { "Invalid payload" },
                IsSuccess = false
            });
        }
        #region  JWT TOKEN  
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim("Id",user.Id),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
        #endregion
    }
}