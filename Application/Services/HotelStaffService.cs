using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class HotelStaffService: IHotelStaffService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HotelStaffService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HotelStaffDto>> GetStaffByHotelIdAsync(Guid hotelId)
    {
        var staff = await _unitOfWork.HotelStaff.GetAllAsync(s => s.HotelId == hotelId);
        return _mapper.Map<IEnumerable<HotelStaffDto>>(staff);
    }

    public async Task AddStaffAsync(CreateStaffDto staffDto)
    {
        var staff = _mapper.Map<HotelStaffEntity>(staffDto);
        await _unitOfWork.HotelStaff.AddAsync(staff);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateStaffAsync(UpdateStaffDto staffDto)
    {
        var staff = await _unitOfWork.HotelStaff.GetAsync(staffDto.Id, tracked: true);
        if (staff == null) throw new KeyNotFoundException($"Staff with ID {staffDto.Id} not found.");

        _mapper.Map(staffDto, staff);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteStaffAsync(Guid staffId)
    {
        var staff = await _unitOfWork.HotelStaff.GetAsync(staffId, tracked: true);
        if (staff == null) throw new KeyNotFoundException($"Staff with ID {staffId} not found.");

        _unitOfWork.HotelStaff.Remove(staff);
        await _unitOfWork.SaveAsync();
    }
    
}