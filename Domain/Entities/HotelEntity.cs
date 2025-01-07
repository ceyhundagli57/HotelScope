using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class HotelEntity
{
    [Key] public Guid Id { get; set; }

    [Required] 
    public string CompanyTitle { get; set; }
        
    public List<HotelStaffEntity> HotelStaffs { get; set; }
    public List<HotelContactInfoEntity> HotelContactInfos { get; set; }
}