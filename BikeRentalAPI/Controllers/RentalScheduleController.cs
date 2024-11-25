using System.Security.Claims;
using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class RentalScheduleController : Controller
{
    public readonly ApplicationDbContext _context;

    public RentalScheduleController(ApplicationDbContext context)
    {
        _context = context;
    }

    /*[HttpPost]
    public async Task<ActionResult> Rent(RentalSchedule rentalSchedule)
    {
        
    }*/
    
    [HttpGet]
    [Route("getUserRentalSchedule")]
    public async Task<ActionResult<IEnumerable<RentalSchedule>>> GetUserRentalSchedule()
    {
        var temp = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = _context.UserInfos.Where(u => u.UserId == temp).FirstOrDefault().Id;
        
        if (_context.RentalSchedules is null)
        {
            return NotFound();
        }
        
        var user = await _context.RentalSchedules.FirstOrDefaultAsync(us => us.IsActive == true && us.UserId == userId);

        return await _context.RentalSchedules.ToListAsync();
    }
    
}