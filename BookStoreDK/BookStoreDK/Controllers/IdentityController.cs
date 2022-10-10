using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.Models.Models.Users;
using BookStoreDK.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreDK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        //private readonly IUserInfoService _userInfoService;
        private readonly IIDentityService _identityService;
       

        public IdentityController(IConfiguration configuration, IIDentityService identityService)
        {
            _configuration = configuration;
            _identityService = identityService;            
        }

        [AllowAnonymous]
        [HttpPost(nameof(Register))]

        public async Task<IActionResult> Register([FromBody] UserInfo user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest($"Username or password is missing");
            }

            var result = await _identityService.CreateAsync(user);
            return result != null? Ok(result) : BadRequest();
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
           
            if (loginRequest != null && !string.IsNullOrEmpty(loginRequest.UserName) && !string.IsNullOrEmpty(loginRequest.Password))
            {
                var user = await _identityService.CheckUserAndPassword(loginRequest.UserName, loginRequest.Password);

              

                if (user != null)
                {
                    var userRoles = await _identityService.GetRoles(user);
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration.GetSection("Jwt:Subject").Value),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("DisplayName", user.DisplayName ?? string.Empty),
                        new Claim("UserName", user.UserName ?? string.Empty),
                        new Claim("Email", user.Email ?? string.Empty),   
                        new Claim("View","View"),
                        new Claim("Admin","Admin"),
                    };

                    foreach (var role in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }

                else
                {
                    return BadRequest("Invalid credentials");
                }
            }

            else
            {
                return BadRequest("Missing username and/or password");
            }
        }
    }
}
