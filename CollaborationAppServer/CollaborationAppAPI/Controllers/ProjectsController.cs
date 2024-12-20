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
    [HttpPost("CreateProject")]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "Invalid token or user not authenticated" });
            }

            var project = new Project
            {
                Project_name = dto.ProjectName,
                Tag_id = dto.TagId,
                User_id = dto.CreatorId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            foreach (var member in dto.Members)
            {
                var newMember = new Member
                {
                    Project_id = project.Project_id,
                    User_id = member.UserId,
                };
                _context.Members.Add(newMember);
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Project created successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }

    // DTO for creating a project
    public class CreateProjectDto
    {
        public string ProjectName { get; set; }
        public int TagId { get; set; }
        public int CreatorId { get; set; }
        public List<MemberDto> Members { get; set; }
    }

    public class MemberDto
    {
        public int UserId { get; set; }

    }

}