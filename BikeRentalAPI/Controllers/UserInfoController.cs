using System.Security.Claims;
using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace BikeRentalAPI.Controllers;

[Route("users/[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserInfoController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [Authorize]
    [HttpGet("getUserInfo")]
    public async Task<ActionResult<UserInfo>> GetUserInfo()
    {
        //gets current logged in user's Id
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.userInfos is null)
        {
            return NotFound();
        }
        
        //finds user information column by UserId adds it to a list 
        var user = await _context.userInfos.Where(uid => uid.UserId.Contains(userId)).ToListAsync();

        if (user is null)
        {
            return NotFound();
        }
        
        return Ok(user);
        
    }
}