using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalAPI.Models;

[Table("Station")]
public class Station
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; } 
}