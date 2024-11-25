using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalAPI.Models;

[Table("Bike")]
public class Bike
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; set; }
    
    public string Type { get; set; }
    public string? LicensePlate { get; set; }
    
    public int StationId { get; set; }
}