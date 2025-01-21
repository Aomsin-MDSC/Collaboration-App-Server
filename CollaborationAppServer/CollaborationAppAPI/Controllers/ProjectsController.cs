using System.Linq;
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

        var memberProjects = _context.Members
            .Where(m => m.User_id == userId)
            .Include(m => m.Project)
                .ThenInclude(p => p.User)
             .Include(m => m.Project)
                .ThenInclude(p => p.Tag)
            .Include(m => m.User)
            .Select(m => new    
            {
                Project_id = m.Project.Project_id,
                Project_name = m.Project.Project_name,
                User_id = m.Project.User_id,
                Tag_id = m.Project.Tag_id,
                Tag_name = m.Project.Tag.Tag_name,
                Tag_color = m.Project.Tag.Tag_color,
                User_name = m.Project.User.User_name
            });
      var ownedProjects = _context.Projects
     .Where(p => p.User_id == userId)
     .Include(p => p.User)
     .Include(p => p.Tag)
     .Select(p => new
     {
         Project_id = p.Project_id,
         Project_name = p.Project_name,
         User_id = p.User_id,
         Tag_id = p.Tag_id,
         Tag_name = p.Tag.Tag_name,
         Tag_color = p.Tag.Tag_color,
         User_name = p.User.User_name
     });

        var projects = await ownedProjects
            .Union(memberProjects)
            .ToListAsync();

        return Ok(projects);
    }
    [HttpGet("GetProject/{projectId}")]
    public async Task<IActionResult> GetProjectById(int projectId)
    {
        try
        {
            var project = await _context.Projects
                .Include(p => p.Members)
                .ThenInclude(m => m.User)
                .Include(p => p.Tag)
                .FirstOrDefaultAsync(p => p.Project_id == projectId);

            if (project == null)
            {
                return NotFound(new { Message = "Project not found." });
            }

            var projectDetails = new
            {
                ProjectId = project.Project_id,
                ProjectName = project.Project_name,
                TagId = project.Tag_id,
                TagName = project.Tag?.Tag_name,
                Members = project.Members?.Select(m => new
                {
                    UserId = m.User_id,
                    UserName = m.User.User_name 
                }).ToList()
            };

            return Ok(projectDetails);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("CreateProject")]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.ProjectName))
            {
                return BadRequest(new { Message = "Project name cannot be empty or whitespace" });
            }

            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "Invalid token or user not authenticated" });
            }
            var userId = int.Parse(userIdClaim.Value);

            var project = new Project
            {
                Project_name = dto.ProjectName,
                Tag_id = dto.TagId,
                User_id = userId
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
    [HttpPut("UpdateProject/{projectId}")]
    public async Task<IActionResult> UpdateProject(int projectId, [FromBody] ProjectUpdateRequest request)
    {
        try
        {
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "Invalid token or user not authenticated" });
            }
            var userId = int.Parse(userIdClaim.Value);

            var project = await _context.Projects
                .Include(p => p.Members)  
                .FirstOrDefaultAsync(p => p.Project_id == projectId);
            if (project == null)
            {
                return NotFound(new { Message = "Project not found." });
            }
            if (project.User_id != userId)
            {
                return Forbid("You do not have permission to update this project.");
            }

            var currentMemberIds = project.Members.Select(m => m.User_id).ToList();
            var membersToRemove = project.Members
                .Where(m => m.User_id.HasValue && !request.Members.Contains(m.User_id.Value))  
                .ToList();
            _context.Members.RemoveRange(membersToRemove); 
            var membersToAdd = request.Members.Where(m => !currentMemberIds.Contains(m)).ToList();

            foreach (var memberId in membersToAdd)
            {
                var newMember = new Member
                {
                    Project_id = projectId,
                    User_id = memberId
                };
                _context.Members.Add(newMember);
            }


            project.Project_name = request.ProjectName;
            project.Tag_id = request.TagId;

            var removedMemberIds = membersToRemove.Select(m => m.User_id.Value).ToList();
            var tasksToUpdate = await _context.Tasks
                .Where(t => t.Project_id == projectId && removedMemberIds.Contains(t.Task_Owner.GetValueOrDefault()))
                .ToListAsync();

            foreach (var task in tasksToUpdate)
            {
                task.Task_Owner = null;
            }


            await _context.SaveChangesAsync(); 

            return Ok(new { Message = "Project updated successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpDelete("DeleteProject/{projectId}")]
    public async Task<IActionResult> DeleteProject(int projectId)
    {
        try
        {
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "Invalid token or user not authenticated" });
            }

            var userId = int.Parse(userIdClaim.Value);

            var project = await _context.Projects
                .Include(p => p.Members)  
                .Include(p => p.Tasks)
                .ThenInclude(p => p.Comments)
                .Include(p => p.Announces)
                .FirstOrDefaultAsync(p => p.Project_id == projectId);

            if (project == null)
            {
                return NotFound(new { Message = "Project not found." });
            }

            if (project.User_id != userId)
            {
                return Forbid("You do not have permission to delete this project.");
            }

            _context.Members.RemoveRange(project.Members);

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Project deleted successfully!" });
        }
        catch (Exception ex)
        {
  
            return BadRequest(new { Error = ex.Message });
        }
    }

}

public class ProjectUpdateRequest
{
    public string ProjectName { get; set; }
    public int? TagId { get; set; }
    public List<int> Members { get; set; }
}

// DTO for creating a project
public class CreateProjectDto
    {
        public required string ProjectName { get; set; }
        public int? TagId { get; set; }
        public List<MemberDto> Members { get; set; }
    }

    public class MemberDto
    {
        public int UserId { get; set; }

    }

