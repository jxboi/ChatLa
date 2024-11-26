using Microsoft.AspNetCore.Mvc;
using chatlaapp.Backend.Models;
using chatlaapp.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace chatlaapp.Backend.Controllers;

public class LikesController : BaseApiController
{
    private readonly ApplicationDbContext _context;

    public LikesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("posts/{postId}/like")]
    public async Task<IActionResult> LikePost(Guid postId, [FromBody] string username)
    {
        var existingLike = await _context.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.Username == username);

        if (existingLike != null) return BadRequest("Post already liked");

        var like = new Like
        {
            LikeId = Guid.NewGuid(),
            PostId = postId,
            Username = username
        };

        _context.Likes.Add(like);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("posts/{postId}/like")]
    public async Task<IActionResult> UnlikePost(Guid postId, [FromBody] string username)
    {
        var like = await _context.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.Username == username);

        if (like == null) return NotFound();

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();

        return Ok();
    }
} 