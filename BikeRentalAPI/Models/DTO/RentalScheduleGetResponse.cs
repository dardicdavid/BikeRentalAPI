namespace BikeRentalAPI.Models.DTO;

public record RentalScheduleGetResponse()
{
    public int BikeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}