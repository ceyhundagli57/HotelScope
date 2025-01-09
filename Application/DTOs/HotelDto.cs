using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs;

public class HotelDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("companyTitle")]

    public string CompanyTitle { get; set; }
    [JsonPropertyName("hotelStaffs")]

    public List<HotelStaffDto> HotelStaffs { get; set; }
    [JsonPropertyName("hotelContactInfos")]

    public List<HotelContactInfoDto> HotelContactInfos { get; set; }
}

public class CreateHotelDto
{
    [Required]
    public string CompanyTitle { get; set; }
}

public class UpdateHotelDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string CompanyTitle { get; set; }
}