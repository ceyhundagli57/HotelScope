using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class HotelService: IHotelService
{
   private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

  
    public async Task AddHotelAsync(CreateHotelDto hotelDto)
    {
        var hotelEntity = _mapper.Map<HotelEntity>(hotelDto);

        await _unitOfWork.Hotel.AddAsync(hotelEntity);
        await _unitOfWork.SaveAsync();
    }


    public async Task UpdateHotelAsync(UpdateHotelDto hotelDto)
    {
        var hotel = await _unitOfWork.Hotel.GetAsync(hotelDto.Id);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with ID {hotelDto.Id} not found.");
        }

        hotel.CompanyTitle = hotelDto.CompanyTitle;

        _unitOfWork.Hotel.Update(hotel);
        await _unitOfWork.SaveAsync();
    }


    public async Task DeleteHotelAsync(Guid hotelId)
    {
        var hotel = await _unitOfWork.Hotel.GetAsync(hotelId);
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with ID {hotelId} not found.");
        }

        _unitOfWork.Hotel.Remove(hotel);
        await _unitOfWork.SaveAsync();
    }


    public async Task<HotelDto> GetHotelByIdAsync(Guid hotelId)
    {
        var hotel = await _unitOfWork.Hotel.GetAsync(hotelId, includeProperties: "HotelStaffs,HotelContactInfos");
        if (hotel == null)
        {
            throw new KeyNotFoundException($"Hotel with ID {hotelId} not found.");
        }

        return _mapper.Map<HotelDto>(hotel);
    }

    public async Task<IEnumerable<HotelDto>> GetAllHotelsAsync()
    {
        var hotels = await _unitOfWork.Hotel.GetAllAsync(includeProperties: "HotelStaffs,HotelContactInfos");
        return _mapper.Map<IEnumerable<HotelDto>>(hotels);
    }
    
    
}