using BikeRentalAPI.Database;
using BikeRentalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalAPI.Controllers;

[Authorize]
[Route("[controller]")]
public class StationController : Controller
{
    private readonly ApplicationDbContext _context;

    public StationController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpGet("getStations")]
    public async Task<ActionResult<IEnumerable<Station>>> GetStations()
    {
        if (_context.Stations is null)
        {
            return NotFound();
        }

        return await _context.Stations.ToListAsync();
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet("getStationsByCity")]
    public async Task<ActionResult<IEnumerable<Station>>> GetStationsByCity(string city)
    {
        if (_context.Stations is null)
        {
            return NotFound();
        }
        return await _context.Stations.Where(s => s.City == city).ToListAsync();
    }
    
    [Authorize(Roles = "User, Admin")]
    [HttpGet("{Id}")]
    public async Task<ActionResult<Station>> GetStation(int Id)
    {
        if (_context.Stations is null)
        {
            return NotFound();
        }
        
        var station = await _context.Stations.FindAsync(Id);
        if (station == null)
        {
            return NotFound();
        }

        return station;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("addStation")]
    public async Task<ActionResult<Station>> AddStation([FromBody]Station station)
    {
        var stationNameCheck = _context.Stations.AnyAsync(s => s.Name == station.Name);
        var locationCheck = _context.Stations.AnyAsync(c => c.Latitude == station.Latitude && c.Longitude == station.Longitude);

        if (stationNameCheck.Result && locationCheck.Result) return Conflict("Station already exists");
        
        _context.Stations.Add(station);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStation), new { id = station.Id }, station);
        
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("deleteStation")]
    public async Task<IActionResult> DeleteStation(int Id)
    {
        var station = await _context.Stations.FindAsync(Id);
        if (station is null)
        {
            return NotFound();
        }
        
        _context.Stations.Remove(station);
        var result = await _context.SaveChangesAsync();
        //TODO: REMOVE STATION REFERENCE FROM ALL TABLES
        return result > 0 ? Ok("Station deleted successfully.") : BadRequest();
    }
}