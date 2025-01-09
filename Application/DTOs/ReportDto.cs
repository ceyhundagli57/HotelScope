using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public class ReportDto
{    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("requestDate")]

    public DateTime RequestDate { get; set; }
    [JsonPropertyName("location")]

    public string Location { get; set; }
    [JsonPropertyName("hotelCount")]

    public int HotelCount { get; set; }
    [JsonPropertyName("hotelContactPhoneCount")]

    public int HotelContactPhoneCount { get; set; }
    [JsonPropertyName("status")]

    public ReportStatus Status { get; set; }

}public class ReportResponseDto
{
    public Guid ReportId { get; set; } 
}
public class SaveReportRequestDto
{
    public Guid Id { get; set; }
    public string Location { get; set; }

}
public class ReportHotelDto
{
    public List<HotelContactInfoEntity> Contacts { get; set; }
    public HotelEntity Hotel { get; set; }
}
public class ReportSummaryDto
{
    public Guid Id { get; set; }
    public DateTime RequestDate { get; set; }
    public ReportStatus Status { get; set; }
}
public class ReportStatusDto
{
    public Guid Id { get; set; }
    public ReportStatus Status { get; set; }
}
public class GenerateReportRequestDto
{
    public string Location { get; set; }
}