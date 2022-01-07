using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PrismMasonManagement.Application.Authorization;
using PrismMasonManagement.Application.Contracts.Configuration;
using PrismMasonManagement.Application.Contracts.DTOs.Permission;
using PrismMasonManagement.Application.Contracts.DTOs.Requests;
using PrismMasonManagement.Application.Contracts.DTOs.Responses;
using PrismMasonManagement.Core.Entities;
using PrismMasonManagement.Infrastructure;

namespace PrismMasonManagement.Api.Controllers.Administration
{
    [Route("api/[controller]")] //api/authManagement
    [ApiController]
    [AllowAnonymous]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptionsMonitor<JwtConfig> _optionsMonitor;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly PrismMasonManagementDbContext _dbContext;
        private readonly JwtConfig _jwtConfig;



        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            PrismMasonManagementDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _optionsMonitor = optionsMonitor;
            _tokenValidationParameters = tokenValidationParameters;
            _dbContext = dbContext;
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
                    var jwtToken = await GenerateJwtTokenAsync(newUser);
                    return Ok(jwtToken);
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

                var jwtToken = await GenerateJwtTokenAsync(exisitingUser);

                return Ok(jwtToken);
            }

            return BadRequest(new RegistrationResponseDto()
            {
                Errors = new List<string> { "Invalid payload" },
                IsSuccess = false
            });
        }


        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequestDto tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await VerifyAndGenerateTokenAsync(tokenRequest);

                if (result == null)
                {
                    return BadRequest(new RegistrationResponseDto
                    {
                        Errors = new List<string> { "Invalid tokens" },
                        IsSuccess = false
                    });
                }

                return Ok(result);
            }

            return BadRequest(new RegistrationResponseDto
            {
                Errors = new List<string> { "Invalid payload" },
                IsSuccess = false
            });
        }

        private async Task<AuthResult> VerifyAndGenerateTokenAsync(TokenRequestDto tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                //Validation 1 - Validate JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                //Validation 2 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return null;
                    }
                }

                //Validation 3 - Validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                Console.WriteLine(expiryDate);
                var checkAgainstDate = DateTime.UtcNow.ToLocalTime();
                Console.WriteLine(checkAgainstDate);
                if (expiryDate > checkAgainstDate)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Token has not yet expired" }
                    };
                }

                //Validation 4 - validate if token exist in db
                var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
                if (storedToken == null)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Token does not exist" }
                    };
                }

                //Validation 5 -- validate if used or not
                if (storedToken.IsUsed)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Token has been used" }
                    };
                }

                //Validation 6 -- validate if revoked or not
                if (storedToken.IsRevoked)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Token has been revoked" }
                    };
                }

                //Validation 7 -- validate id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Token doesn't match" }
                    };
                }

                //Update current token
                storedToken.IsUsed = true;
                _dbContext.RefreshTokens.Update(storedToken);
                await _dbContext.SaveChangesAsync();

                //Generate new token
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtTokenAsync(dbUser);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(JsonConvert.SerializeObject(ex, Formatting.Indented));
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeValue = dateTimeValue.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTimeValue;
        }

        #region  JWT TOKEN  

        private async Task<List<Claim>> GetValidClaimsAsync(IdentityUser user)
        {
            IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName),
            };

            var userClaims = await _userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);
            return claims;
        }

        private async Task<AuthResult> GenerateJwtTokenAsync(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var claims = await GetValidClaimsAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthResult
            {
                Token = jwtToken,
                IsSuccess = true,
                RefreshToken = refreshToken.Token
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
        #endregion
    }
}