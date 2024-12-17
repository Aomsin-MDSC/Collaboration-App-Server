using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollaborationAppAPI.Models;

namespace CollaborationAppAPI.Controllers
{
    [Route("api/[controller]")]
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

            // เพิ่มข้อมูลผู้ใช้ลงในฐานข้อมูล
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully" });
        }
    }
}
