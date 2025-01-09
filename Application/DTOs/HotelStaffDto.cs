using System.Text.Json.Serialization;

namespace Application.DTOs;

public class HotelStaffDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("firstName")]

    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]

    public string LastName { get; set; }
}

public class CreateStaffDto
{
    public Guid HotelId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class UpdateStaffDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
