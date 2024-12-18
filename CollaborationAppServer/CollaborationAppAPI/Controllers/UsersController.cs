using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollaborationAppAPI.Models;

namespace CollaborationAppAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Invalid user data.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully" });
        }
        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.User_name == userLogin.UserName);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            if (user.User_password != userLogin.Password)   
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }
            else
            {
                return Ok(new { Message = "Login successful", UserName = user.User_password });
            }

            
        }
    }
    public class UserLogin
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }

}

