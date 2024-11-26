using Microsoft.AspNetCore.Mvc;
using chatlaapp.Backend.Models;
using chatlaapp.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace chatlaapp.Backend.Controllers;

public class UsersController : BaseApiController
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<User>>> SearchUsers([FromQuery] string username)
    {
        var users = await _context.Users
            .Where(u => u.Username.ToLower().StartsWith(username.ToLower()))
            .ToListAsync();
        
        return Ok(users);
    }

    [HttpPost("{username}/follow")]
    public async Task<IActionResult> FollowUser(string username, [FromBody] string currentUsername)
    {
        var follow = new Follow
        {
            FollowerUsername = currentUsername,
            FollowingUsername = username
        };

        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{username}/follow")]
    public async Task<IActionResult> UnfollowUser(string username, [FromBody] string currentUsername)
    {
        var follow = await _context.Follows
            .FirstOrDefaultAsync(f => 
                f.FollowerUsername == currentUsername && 
                f.FollowingUsername == username);

        if (follow == null) return NotFound();

        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();

        return Ok();
    }
} 