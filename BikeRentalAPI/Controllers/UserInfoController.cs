using System.Security.Claims;
using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using BikeRentalAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace BikeRentalAPI.Controllers;

[Authorize]
[Route("users/[controller]")]
public class UserInfoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserInfoController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("getUserInfo")]
    public async Task<ActionResult<UserInfo>> GetUserInfo()
    {
        //gets current logged in user's Id
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.UserInfos is null)
        {
            return NotFound();
        }
        
        //finds user information column by UserId adds it to a list 
        var user = await _context.UserInfos.FirstOrDefaultAsync(uid => uid.UserId == userId);
        
        if (user is null)
        {
            return NotFound();
        }

        UserInfoGetResponse userDto = new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName, 
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber,
        };

        return Ok(userDto);
    }
    
    [HttpPost("updateUserInfo")]
    public async Task<ActionResult<UserInfo>> UpdateUserInfo([FromBody]UserInfoPutRequest userInfo)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.UserInfos is null)
        {
            return NotFound();
        }
        
        //finds user information column by UserId adds it to a list 
        var user = await _context.UserInfos.FirstOrDefaultAsync(uid => uid.UserId == userId);
        
        if (user is null)
        {
            return NotFound();
        }

        
        if (!string.IsNullOrEmpty(userInfo.FirstName))
        {
            user.FirstName = userInfo.FirstName;
        }
        
        if (!string.IsNullOrEmpty(userInfo.LastName))
        {
            user.LastName = userInfo.LastName;
        }
        
        if (!string.IsNullOrEmpty(userInfo.Gender))
        {
            user.Gender = userInfo.Gender;
        }

        if (userInfo.PhoneNumber != null)
        {
            user.PhoneNumber = userInfo.PhoneNumber;
        }
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if (!UserExists(userId))
            {
                return NotFound();
            }

            throw;
        }
        
        return NoContent();
        
    }
    

    private bool UserExists(string userId)
    {
        return (_context.UserInfos?.Any(e => e.UserId == userId)).GetValueOrDefault();
    }
}