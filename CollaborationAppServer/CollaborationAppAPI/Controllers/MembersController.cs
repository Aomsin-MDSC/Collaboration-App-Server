using CollaborationAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api")]
[ApiController]

public class MembersController : ControllerBase
{
    private readonly AppDbContext _context;

    public MembersController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("GetMembers")]
    public async Task<IActionResult> GetMembers()
    {
        try
        {
            var members = await _context.Members
                .Include(m => m.User)
                .GroupBy(m => m.User_id)
                .Select(g => new {
                    User_id = g.Key,
                    User_name = g.FirstOrDefault().User.User_name,
                })
                .ToListAsync();

            return Ok(members);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpGet("GetMember/{projectId}")]
    public async Task<IActionResult> GetMemberById(int projectId)
    {
        try
        {
            var members = await _context.Members
                .Where(t => t.Project_id == projectId)
                .Include(m => m.User)
                .GroupBy(m => m.User_id)
                .Select(g => new {
                    User_id = g.Key,
                    User_name = g.FirstOrDefault().User.User_name,
                })
                .ToListAsync();

            return Ok(members);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

}

