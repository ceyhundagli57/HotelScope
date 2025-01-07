using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HotelStaffEntity
{
    [Key] public Guid Id { get; set; }

    [Required] 
    public string FirstName { get; set; }
    [Required] 
    public string LastName { get; set; }

    public Guid HotelId { get; set; }
    [ForeignKey("HotelId")] 
    public HotelEntity Hotel { get; set; }
}