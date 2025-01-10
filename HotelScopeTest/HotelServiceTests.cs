using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelScopeTest;

public class HotelServiceTests
{
 private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HotelService _hotelService;

    public HotelServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _hotelService = new HotelService(_unitOfWorkMock.Object, _mapperMock.Object);
    }
    

    [Fact]
    public async Task DeleteHotelAsync_Should_ThrowIfHotelNotFound()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        _unitOfWorkMock.Setup(uow => uow.Hotel.GetAsync(hotelId,null,true)).ReturnsAsync((HotelEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _hotelService.DeleteHotelAsync(hotelId));
    }
    

    [Fact]
    public async Task GetHotelByIdAsync_Should_ThrowIfHotelNotFound()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        _unitOfWorkMock.Setup(uow => uow.Hotel.GetAsync(hotelId, "HotelStaffs,HotelContactInfos",true))
            .ReturnsAsync((HotelEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _hotelService.GetHotelByIdAsync(hotelId));
    }

    [Fact]
    public async Task GetAllHotelsAsync_Should_ReturnMappedHotels()
    {
        // Arrange
        var hotelEntities = new List<HotelEntity>
        {
            new HotelEntity { Id = Guid.NewGuid(), CompanyTitle = "Hotel 1" },
            new HotelEntity { Id = Guid.NewGuid(), CompanyTitle = "Hotel 2" }
        };

        var hotelDtos = new List<HotelDto>
        {
            new HotelDto { Id = hotelEntities[0].Id, CompanyTitle = "Hotel 1" },
            new HotelDto { Id = hotelEntities[1].Id, CompanyTitle = "Hotel 2" }
        };

        _unitOfWorkMock.Setup(uow => uow.Hotel.GetAllAsync(
                It.IsAny<Expression<Func<HotelEntity, bool>>>(),
                "HotelStaffs,HotelContactInfos",
                false))
            .ReturnsAsync(hotelEntities);
        _mapperMock.Setup(m => m.Map<IEnumerable<HotelDto>>(hotelEntities)).Returns(hotelDtos);

        // Act
        var result = await _hotelService.GetAllHotelsAsync();

        // Assert
        Assert.Equal(hotelDtos, result);
    }
}