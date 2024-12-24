using CollaborationAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api")]
[ApiController]

    public class TagController : ControllerBase
{
    private readonly AppDbContext _context;

    public TagController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("GetTags")]
    public async Task<IActionResult> GetTags()
    {
        try
        {
            var tags = await _context.Tags
                .Select(t => new { t.Tag_id, t.Tag_name, t.Tag_color })
                .ToListAsync();

            return Ok(tags);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
    [HttpPost("CreateTag")]
    public async Task<IActionResult> CreateTag([FromBody] Tag tag)
    {
        try
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tag created successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }
    [HttpPut("UpdateTag/{id}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] Tag tag)
    {
        try
        {
            var existingTag = await _context.Tags.FindAsync(id);
            if (existingTag == null)
            {
                return NotFound(new { Message = "Tag not found" });
            }

            existingTag.Tag_name = tag.Tag_name;
            existingTag.Tag_color = tag.Tag_color;

            _context.Tags.Update(existingTag);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tag updated successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }
    [HttpDelete("DeleteTag/{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        try
        {
            var tagToDelete = await _context.Tags.FindAsync(id);
            if (tagToDelete == null)
            {
                return NotFound(new { Message = "Tag not found" });
            }

            _context.Tags.Remove(tagToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Tag deleted successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Details = ex.InnerException?.Message });
        }
    }



}

