using System.Threading.Tasks;
using CollaborationAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("comment")]
[ApiController]

public class CommmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommmentController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("GetComments/{taskId}")]
    public async Task<IActionResult> GetComments(int taskId)
    {
        try
        {
            var comments = await _context.Comments
                .Where(c => c.Task_id == taskId)
                .Include(c => c.User)
                .Select(t => new { t.Comment_id, t.Comment_text, t.Comment_date,t.User_id,t.User.User_name })
                .ToListAsync();

            return Ok(comments);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpPost("CreateComment")]
    public async Task<IActionResult> CreateTag([FromBody] Comment comment)
    {
        try
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Comments created successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }

}

