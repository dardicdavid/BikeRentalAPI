using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalAPI.Models;

[Table("Bike")]
public class Bike
{

    public int Id { get; set; }
    
    public string Type { get; set; }
    public string? LicensePlate { get; set; }
    
    public int StationId { get; set; }
}