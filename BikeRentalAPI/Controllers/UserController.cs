using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BikeRentalAPI.Controllers;

[Route("users/[controller]")]
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
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var user = new IdentityUser()
        {
            UserName = model.Email,
            Email = model.Email,
            PasswordHash = model.Password
        };
        
        var result1 = await _userManager.CreateAsync(user, user.PasswordHash!);
        
        var userId = await _userManager.GetUserIdAsync(user);
        UserInfo userInfo = new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserId = userId
        };
        
        _context.userInfos.Add(userInfo);
        var result2 = await _context.SaveChangesAsync();
        

        if (result1.Succeeded && result2 > 0)
        {
            return Ok("User registered successfully!");
        }
        return BadRequest("Failed to register user!");
    }

}
