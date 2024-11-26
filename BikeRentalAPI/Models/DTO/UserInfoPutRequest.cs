namespace BikeRentalAPI.Models.DTO;

public record UserInfoPutRequest()
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? PhoneNumber { get; set; }
    public string? Gender { get; set; }
};