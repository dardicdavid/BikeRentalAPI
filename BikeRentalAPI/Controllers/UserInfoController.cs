using System.Security.Claims;
using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using BikeRentalAPI.Models.DTO;
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
}