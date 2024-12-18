using CollaborationAppAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api")]
[ApiController]
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
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _context.Projects
            .Include(p => p.User)
            .Include(p => p.Tag)
            .Select(p => new
            {
                ProjectId = p.Project_id,
                ProjectName = p.Project_name,
                UserName = p.User.User_name,
                TagName = p.Tag.Tag_name
            })
            .ToListAsync();

        return Ok(projects);
    }
}
