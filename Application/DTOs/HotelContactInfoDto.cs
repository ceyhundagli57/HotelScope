using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class HotelContactInfoDto
{
    [JsonPropertyName("id")]

    public Guid Id { get; set; }
    [JsonPropertyName("email")]

    public string Email { get; set; }
    [JsonPropertyName("location")]

    public string Location { get; set; }
    [JsonPropertyName("phoneNumber")]

    public string PhoneNumber { get; set; }
    [JsonPropertyName("hotelId")]

    public Guid HotelId { get; set; } 
}

public class CreateContactInfoDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public Guid HotelId { get; set; } 
}

public class UpdateContactInfoDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
}