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
[Route("[controller]")]
public class RentalScheduleController : Controller
{
    private readonly ApplicationDbContext _context;

    public RentalScheduleController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "User, Admin")]
    [HttpPost]
    [Route("Rent")]
    public async Task<ActionResult> Rent([FromBody]RentalSchedulePostRequest rentalSchedulePostRequest)
    {
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var newRental = rentalSchedulePostRequest;

        if (newRental.StartDate > rentalSchedulePostRequest.EndDate)
        {
            return BadRequest("Start date cannot be earlier than end date");
        }

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

        RentalSchedule rentalSchedule = new()
        {
            StartDate = newRental.StartDate,
            EndDate = newRental.EndDate,
            IsActive = true,
            IsCancelled = false,
            BikeId = newRental.BikeId,
            UserId = user.Id,
        };
        
        if (currentRentals.Count == 0)
        {
            
            _context.RentalSchedules.Add(rentalSchedule);
            
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(GetRental), new { id = rentalSchedule.Id }, rentalSchedulePostRequest);
        }
        
        for(int i = 0; i < currentRentals.Count; i++)
        {
            if (newRental.EndDate <= currentRentals[i].StartDate && newRental.StartDate >= currentRentals[i].EndDate)
            {
                continue;
            }
            else
            {
                return BadRequest("Desired schedule overlaps with already existing schedule.");
            }

        }
        
        
        _context.RentalSchedules.Add(rentalSchedule);
            
        _context.SaveChanges();
            
        return CreatedAtAction(nameof(GetRental), new { id = rentalSchedule.Id }, rentalSchedulePostRequest);
        
    }
    [Authorize(Roles = "User, Admin")]
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
    
    [Authorize(Roles = "User, Admin")]
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

    [Authorize(Roles = "User, Admin")]
    [HttpPatch]
    public async Task<ActionResult> CancelRental(int Id)
    {
        if (_context.RentalSchedules is null)
        {
            return NotFound();

        }
        
        var rental = await _context.RentalSchedules.FindAsync(Id);

        if (rental is null)
        {
            return NotFound();
        }
        
        rental.IsActive = false;
        rental.IsCancelled = true;

        _context.RentalSchedules.Update(rental);

        return Ok();

    }
    
}