using CollaborationAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api")]
[ApiController]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("Test")]
    public IActionResult Test()
    {
        return Ok("API is working!");
    }

    [HttpGet("GetProjects")]
    public async Task<IActionResult> GetUserProjects()
    {
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim == null)
        {
            return Unauthorized(new { Message = "Invalid token or user not authenticated" });
        }

        int userId = int.Parse(userIdClaim.Value);

        var projects = await _context.Members
            .Where(m => m.User_id == userId)
            .Include(m => m.Project)
            .Select(m => new
            {
                m.Project.Project_id,
                m.Project.Project_name
            })
            .ToListAsync();

        return Ok(projects);
    }
}