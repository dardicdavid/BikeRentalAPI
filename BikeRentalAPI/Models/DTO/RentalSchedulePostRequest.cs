namespace BikeRentalAPI.Models.DTO;

public record RentalSchedulePostRequest()
{
    public int BikeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
};