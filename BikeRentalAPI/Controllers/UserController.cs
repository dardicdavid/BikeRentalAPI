using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalAPI.Controllers;

[Route("users/")]
public class UserController : Controller
{
    
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;
    
    public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterModel model)
    {
        var user = new IdentityUser()
        {
            UserName = model.Email,
            Email = model.Email,
            PasswordHash = model.Password
        };
        
        //registering previously created user into auth table
        var result1 = await _userManager.CreateAsync(user, user.PasswordHash!);
        
        var roleResult = await _userManager.AddToRoleAsync(user, "User");

        if (!roleResult.Succeeded)
        {
            return BadRequest(roleResult.Errors);
        }
        
        //getting user id
        var userId = await _userManager.GetUserIdAsync(user);
        UserInfo userInfo = new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserId = userId
        };
        
        int result2 = 0;
        if (result1.Succeeded)
        {
            //adding additional user information into separate table
            _context.UserInfos.Add(userInfo);
            result2 = await _context.SaveChangesAsync();
        }
        else
        {
            return BadRequest("Failed to register user credentials!");
        }


        if (result1.Succeeded && result2 > 0)
        {
            return Ok("User registered successfully!");
        }
        
        return BadRequest("Failed to register user!");
    }

    [Authorize(Roles = "User, Admin")]
    [HttpPost("logout")]
    public async Task<ActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return Ok("User logged out!");
    }

}
