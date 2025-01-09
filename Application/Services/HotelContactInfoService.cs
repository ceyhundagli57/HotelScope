using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class HotelContactInfoService : IHotelContactInfoService
{
private readonly IUnitOfWork _unitOfWork;
private readonly IMapper _mapper;

public HotelContactInfoService(IUnitOfWork unitOfWork, IMapper mapper)
{
    _unitOfWork = unitOfWork;
    _mapper = mapper;
}

public async Task<IEnumerable<HotelContactInfoDto>> GetContactInfosByHotelIdAsync(Guid hotelId)
{
    var contactInfos = await _unitOfWork.HotelContactInfo.GetAllAsync(ci => ci.HotelId == hotelId);
    return _mapper.Map<IEnumerable<HotelContactInfoDto>>(contactInfos);
}

public async Task AddContactInfoAsync(CreateContactInfoDto contactInfoDto)
{
    var contactInfo = _mapper.Map<HotelContactInfoEntity>(contactInfoDto);
    await _unitOfWork.HotelContactInfo.AddAsync(contactInfo);
    await _unitOfWork.SaveAsync();
}

public async Task UpdateContactInfoAsync(UpdateContactInfoDto contactInfoDto)
{
    var contactInfo = await _unitOfWork.HotelContactInfo.GetAsync(contactInfoDto.Id, tracked: true);
    if (contactInfo == null) throw new KeyNotFoundException($"Contact info with ID {contactInfoDto.Id} not found.");

    _mapper.Map(contactInfoDto, contactInfo);
    await _unitOfWork.SaveAsync();
}

public async Task DeleteContactInfoAsync(Guid contactInfoId)
{
    var contactInfo = await _unitOfWork.HotelContactInfo.GetAsync(contactInfoId, tracked: true);
    if (contactInfo == null) throw new KeyNotFoundException($"Contact info with ID {contactInfoId} not found.");

    _unitOfWork.HotelContactInfo.Remove(contactInfo);
    await _unitOfWork.SaveAsync();
}

    
}