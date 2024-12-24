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

            return Ok(new { Message = "Task created successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }
    [HttpPut("UpdateTask/{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] CollaborationAppAPI.Models.Task updatedTask)
    {
        try
        {
            var existingTask = await _context.Tasks
                .Include(t => t.User)
                .Include(t => t.Tag)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Task_id == taskId);

            if (existingTask == null)
            {
                return NotFound(new { Message = "Task not found" });
            }

            existingTask.Task_name = updatedTask.Task_name;
            existingTask.Task_detail = updatedTask.Task_detail;
            existingTask.Task_end = updatedTask.Task_end;
            existingTask.Task_color = updatedTask.Task_color;
            existingTask.Task_status = updatedTask.Task_status;
            existingTask.User_id = updatedTask.User_id;
            existingTask.Tag_id = updatedTask.Tag_id;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Task updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message, Details = ex.InnerException?.Message });
        }
    }
    [HttpDelete("DeleteTask/{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(a => a.Task_id == id);

            if (task == null)
            {
                return NotFound(new { Message = "Task not found" });
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Task deleted successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }


}

