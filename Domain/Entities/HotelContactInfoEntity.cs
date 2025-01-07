using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HotelContactInfoEntity
{
    [Key] public Guid Id { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Location { get; set; }
    [Required] public string PhoneNumber { get; set; }
    
    public Guid HotelId { get; set; }
    [ForeignKey("HotelId")] public HotelEntity Hotel { get; set; }
}