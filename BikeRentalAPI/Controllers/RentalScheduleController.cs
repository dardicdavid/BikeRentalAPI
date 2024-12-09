using System.Security.Claims;
using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using BikeRentalAPI.Models.DTO;
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

    [HttpPost]
    [Route("Rent")]
    public async Task<ActionResult> Rent([FromBody]RentalSchedulePostRequest rentalSchedulePostRequest)
    {
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_context.UserInfos is null)
        {
            return NotFound();
        }
        
        //finds user information column by UserId adds it to a list 
        var user = await _context.UserInfos.FirstOrDefaultAsync(uid => uid.UserId == userId);
        
        List<RentalSchedule> currentRentals = await _context.RentalSchedules.Where(r => 
                r.BikeId == rentalSchedulePostRequest.BikeId &&
                r.IsActive == true
            ).ToListAsync();

        if (currentRentals.Count == 0)
        {
            RentalSchedule rentalSchedule = new()
            {
                StartDate = rentalSchedulePostRequest.StartDate,
                EndDate = rentalSchedulePostRequest.EndDate,
                IsActive = true,
                BikeId = rentalSchedulePostRequest.BikeId,
                UserId = user.Id,
            }; 
            
            _context.RentalSchedules.Add(rentalSchedule);
            
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(GetRental), new { id = rentalSchedule.Id }, rentalSchedulePostRequest);
        }
        
        
        
        return Ok("!");
    }
    
    [HttpGet]
    [Route("getUserRentalSchedule")]
    public async Task<ActionResult<IEnumerable<RentalSchedule>>> GetUserRentalSchedule()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userInfoId = _context.UserInfos.Where(u => u.UserId == userId).FirstOrDefault().Id;
        
        if (_context.RentalSchedules is null)
        {
            return NotFound();
        }
        
        var user = await _context.RentalSchedules.FirstOrDefaultAsync(us => us.IsActive == true && us.UserId == userInfoId);

        return await _context.RentalSchedules.ToListAsync();
    }
    
    [HttpGet("{Id}")]
    public async Task<ActionResult<RentalSchedule>> GetRental(int Id)
    {
        if (_context.RentalSchedules is null)
        {
            return NotFound();
        }
        
        var rental = await _context.RentalSchedules.FindAsync(Id);
        
        if (rental == null)
        {
            return NotFound();
        }

        return rental;
    }
    
}