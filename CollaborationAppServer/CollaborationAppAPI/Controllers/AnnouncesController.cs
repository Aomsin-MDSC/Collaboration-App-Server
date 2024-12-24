using CollaborationAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api")]
[ApiController]

public class AnnouncesController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnnouncesController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("GetAnnounces")]
    public async Task<IActionResult> GetAnnounces()
    {
        try
        {
            var announces = await _context.Announces
                .Where(a => a != null)
                .Select(a => new { a.Announce_id, a.Announce_text, a.Project_id, a.Announce_date })
                .ToListAsync();
            if (announces == null || !announces.Any())
            {
                return NotFound(new { Message = "No announces found." });
            }

            return Ok(announces);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpPost("CreateAnnounces")]
    public async Task<IActionResult> CreateAnnounces([FromBody] Announce announce)
    {
        try
        {
            _context.Announces.Add(announce);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Announce created successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }
    [HttpPut("UpdateAnnounce/{id}")]
    public async Task<IActionResult> UpdateAnnounce(int id, [FromBody] Announce announce)
    {
        try
        {
            var existingAnnounce = await _context.Announces
                .FirstOrDefaultAsync(a => a.Announce_id == id);

            if (existingAnnounce == null)
            {
                return NotFound(new { Message = "Tag not found" });
            }

            existingAnnounce.Announce_text = announce.Announce_text;
            existingAnnounce.Announce_date = announce.Announce_date;
            
            _context.Announces.Update(existingAnnounce);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Announce updated successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }
    [HttpDelete("DeleteAnnounce/{id}")]
    public async Task<IActionResult> DeleteAnnounce(int id)
    {
        try
        {
            var announce = await _context.Announces
                .FirstOrDefaultAsync(a => a.Announce_id == id);

            if (announce == null)
            {
                return NotFound(new { Message = "Announce not found" });
            }

            _context.Announces.Remove(announce);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Announce deleted successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }


}

