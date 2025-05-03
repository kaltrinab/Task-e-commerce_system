using E_Commerce_System_API.Application.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.Sqlite;
using E_Commerce_System_API.Application.Services;
using Dapper;

namespace E_Commerce_System_API.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly JWTService _jwt;
        private readonly IConfiguration _configuration;

        public AuthController(JWTService jwt, IConfiguration configuration)
        {
            _jwt = jwt;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            using var conn = new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
            var user = await conn.QueryFirstOrDefaultAsync<dynamic>(
                @"SELECT u.Id, u.Email, u.Password, r.Name as Role
                  FROM Users u
                  JOIN Roles r ON u.RoleId = r.Id
                  WHERE u.Email = @Email AND u.Password = @Password",
                new { dto.Email, dto.Password });

            if (user == null)
                return Unauthorized("Invalid credentials");

            string token = _jwt.GenerateToken((int)user.Id, (string)user.Email, (string)user.Role);
            return Ok(new { Token = token });
        }
    }
}
