using CollaborationAppAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api")]
[ApiController]

public class TaskController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context;
    }       

    [HttpGet("GetTasks")]
    public async Task<IActionResult> GetTasks()
    {
        try
        {
            var tasks = await _context.Tasks
                .Select(t => new { t.Task_id, t.Task_name, t.Task_detail, t.Task_end, t.Task_color, t.Task_status, t.User_id, t.Tag_id, t.Project_id })

                .ToListAsync();

            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpPost("CreateTask")]
    public async Task<IActionResult> CreateTask([FromBody] CollaborationAppAPI.Models.Task tasks)
    {
        try
        {
            _context.Tasks.Add(tasks);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tag created successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }

}

