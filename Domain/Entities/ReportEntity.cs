using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ReportEntity
{
    [Key] public Guid Id { get; set; }

    public DateTime RequestDate { get; set; }
    public ReportStatus Status { get; set; }
    public int HotelCount { get; set; }
    public int HotelContactPhoneCount { get; set; }
    public string Location { get; set; }
}

public enum ReportStatus
{
    Preparing,
    Completed,
    Failed 
}