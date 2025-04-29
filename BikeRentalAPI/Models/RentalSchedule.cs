using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalAPI.Models;

[Table("RentalSchedule")]
public class RentalSchedule
{
    public int Id { get; set; }
    public int UserId { get; set; } // Id from user information table
    
    public int BikeId { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsCancelled { get; set; }
}