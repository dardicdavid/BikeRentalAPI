using System.Collections;
using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class BikeController : Controller
{
    private readonly ApplicationDbContext _context;

    public BikeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("getBikes")]
    public async Task<ActionResult<IEnumerable<Bike>>> GetBikes()
    {
        if (_context.Bikes is null)
        {
            return NotFound();
        }

        return await _context.Bikes.ToListAsync();
    }

    [AllowAnonymous]
    [HttpGet("{Id}")]
    public async Task<ActionResult<Bike>> GetBike(int Id)
    {
        if (_context.Bikes is null)
        {
            return NotFound();
        }
        
        var bike = await _context.Bikes.FindAsync(Id);
        if (bike == null)
        {
            return NotFound();
        }

        return bike;
    }
    
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [HttpPost("addBike")]
    public async Task<ActionResult<Bike>> AddBike(Bike bike)
    {
        
        //checking if license plate already exists to prevent duplicates
        var licensePlateCheck = _context.Bikes.AnyAsync(b => b.LicensePlate == bike.LicensePlate);

        if (licensePlateCheck.Result) return Conflict("License Plate already exists");
        
        _context.Bikes.Add(bike);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBike), new { id = bike.Id }, bike);

    }
}