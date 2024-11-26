using Microsoft.AspNetCore.Mvc;
using chatlaapp.Backend.Models;
using chatlaapp.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace chatlaapp.Backend.Controllers;

public class CommentsController : BaseApiController
{
    private readonly ApplicationDbContext _context;

    public CommentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("posts/{postId}/comments")]
    public async Task<ActionResult<List<Comment>>> GetComments(Guid postId)
    {
        var comments = await _context.Comments
            .Where(c => c.PostId == postId)
            .OrderBy(c => c.Timestamp)
            .ToListAsync();

        return Ok(comments);
    }

    [HttpPost("posts/{postId}/comments")]
    public async Task<ActionResult<Comment>> CreateComment(Guid postId, [FromBody] CommentCreateDto dto)
    {
        var comment = new Comment
        {
            CommentId = Guid.NewGuid(),
            PostId = postId,
            Content = dto.Content,
            Username = dto.Username,
            Timestamp = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(comment);
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteComment(Guid commentId, [FromBody] string username)
    {
        var comment = await _context.Comments.FindAsync(commentId);

        if (comment == null) return NotFound();
        if (comment.Username != username) return Forbid();

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return Ok();
    }
}

public class CommentCreateDto
{
    public required string Content { get; set; }
    public required string Username { get; set; }
} 