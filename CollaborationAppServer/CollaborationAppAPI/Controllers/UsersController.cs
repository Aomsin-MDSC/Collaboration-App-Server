using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollaborationAppAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CollaborationAppAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                // ดึง User_id ของผู้ใช้ปัจจุบัน (สมมติคุณใช้ Claims ใน Token)
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "Invalid token or user not authenticated" });
                }

                // แปลง User_id ให้เป็นตัวเลข
                int userId = int.Parse(userIdClaim.Value);

                // กรองไม่ให้ส่งข้อมูลของผู้ใช้ปัจจุบัน
                var members = await _context.Users
                    .Where(m => m.User_id != userId) // กรองผู้ใช้
                    .GroupBy(m => m.User_id)
                    .Select(g => new
                    {
                        User_id = g.Key,
                        User_name = g.FirstOrDefault().User_name
                    })
                    .ToListAsync();

                return Ok(members);
            }
            catch (Exception ex)
            {
                // Log the exception here using a logger (e.g., ILogger)
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.User_name))
            {
                return BadRequest(new { Message = "Username cannot be empty or whitespace" });
            }

            if (string.IsNullOrWhiteSpace(user.User_password))
            {
                return BadRequest(new { Message = "Password cannot be empty or whitespace" });
            }

            var existingUser = await _context.Users
                                             .FirstOrDefaultAsync(u => u.User_name == user.User_name);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Username is already taken." });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.User_name == userLogin.UserName);

            if (user == null || user.User_password != userLogin.Password)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                UserId = user.User_id,
                UserName = user.User_name
            });
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateTag(int id, [FromBody] User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                existingUser.User_name = user.User_name;
                existingUser.User_password = user.User_password;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "User updated successfully!" });
            }

            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
            }
        }

        [HttpPut("TokenDevice/{id}")]
        public async Task<IActionResult> TokenDevice(int id, [FromBody] UserTokenUpdateDTO dto)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                existingUser.User_token = dto.User_token;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "UserToken updated successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
            }
        }

        public class UserTokenUpdateDTO
        {
            public string User_token { get; set; } 
        }   

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim("userId", user.User_id.ToString()),  
                new Claim(ClaimTypes.Name, user.User_name)   
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class UserLogin
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
