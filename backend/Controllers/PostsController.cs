using Microsoft.AspNetCore.Mvc;
using chatlaapp.Backend.Models;
using chatlaapp.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace chatlaapp.Backend.Controllers;

public class PostsController : BaseApiController
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public PostsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetPosts([FromQuery] string currentUsername)
    {
        var followedUsers = await _context.Follows
            .Where(f => f.FollowerUsername == currentUsername)
            .Select(f => f.FollowingUsername)
            .ToListAsync();

        followedUsers.Add(currentUsername); // Include user's own posts

        var posts = await _context.Posts
            .Where(p => followedUsers.Contains(p.Username))
            .Include(p => p.Comments)
            .Include(p => p.Likes)
            .OrderByDescending(p => p.Timestamp)
            .ToListAsync();

        return Ok(posts);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromForm] CreatePostDto dto)
    {
        var post = new Post
        {
            PostId = Guid.NewGuid(),
            Content = dto.Content,
            Username = dto.Username,
            Timestamp = DateTime.UtcNow
        };

        if (dto.Image != null)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.Image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder);
            await dto.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));

            post.ImageUrl = $"/uploads/{uniqueFileName}";
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return Ok(post);
    }

    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(Guid postId, [FromForm] UpdatePostDto dto)
    {
        var post = await _context.Posts.FindAsync(postId);

        if (post == null) return NotFound();
        if (post.Username != dto.Username) return Forbid();

        post.Content = dto.Content;

        if (dto.Image != null)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                var oldImagePath = Path.Combine(_env.WebRootPath, post.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Save new image
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.Image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder);
            await dto.Image.CopyToAsync(new FileStream(filePath, FileMode.Create));

            post.ImageUrl = $"/uploads/{uniqueFileName}";
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(Guid postId, [FromBody] string username)
    {
        var post = await _context.Posts.FindAsync(postId);

        if (post == null) return NotFound();
        if (post.Username != username) return Forbid();

        if (!string.IsNullOrEmpty(post.ImageUrl))
        {
            var imagePath = Path.Combine(_env.WebRootPath, post.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return Ok();
    }
} 