using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace HotelScopeTest;

public class HotelStaffTests {
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HotelStaffService _hotelStaffService;

    public HotelStaffTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _hotelStaffService = new HotelStaffService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetStaffByHotelIdAsync_Should_ReturnMappedStaff()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var staffEntities = new List<HotelStaffEntity>
        {
            new HotelStaffEntity { Id = Guid.NewGuid(), HotelId = hotelId, FirstName = "John" },
            new HotelStaffEntity { Id = Guid.NewGuid(), HotelId = hotelId, FirstName = "Jane" }
        };

        var staffDtos = new List<HotelStaffDto>
        {
            new HotelStaffDto { Id = staffEntities[0].Id, FirstName = "John Doe" },
            new HotelStaffDto { Id = staffEntities[1].Id, FirstName = "Jane Doe" }
        };

        _unitOfWorkMock.Setup(uow => uow.HotelStaff.GetAllAsync(
                It.IsAny<Expression<Func<HotelStaffEntity, bool>>>(),
                null,
                false))
            .ReturnsAsync(staffEntities);
        _mapperMock.Setup(m => m.Map<IEnumerable<HotelStaffDto>>(staffEntities)).Returns(staffDtos);

        // Act
        var result = await _hotelStaffService.GetStaffByHotelIdAsync(hotelId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("John Doe", result.First().FirstName);
    }
    

    [Fact]
    public async Task UpdateStaffAsync_Should_UpdateAndSave()
    {
        // Arrange
        var staffDto = new UpdateStaffDto { Id = Guid.NewGuid(), FirstName = "Updated FirstName" };
        var staffEntity = new HotelStaffEntity { Id = staffDto.Id, FirstName = "Old FirstName" };

        _unitOfWorkMock.Setup(uow => uow.HotelStaff.GetAsync(staffDto.Id, null, true)).ReturnsAsync(staffEntity);
        _mapperMock.Setup(m => m.Map(staffDto, staffEntity));
        _unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _hotelStaffService.UpdateStaffAsync(staffDto);

        // Assert
        _mapperMock.Verify(m => m.Map(staffDto, staffEntity), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteStaffAsync_Should_RemoveAndSave()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var staffEntity = new HotelStaffEntity { Id = staffId, FirstName = "John Doe" };

        _unitOfWorkMock.Setup(uow => uow.HotelStaff.GetAsync(staffId, null, true)).ReturnsAsync(staffEntity);
        _unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _hotelStaffService.DeleteStaffAsync(staffId);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.HotelStaff.Remove(staffEntity), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteStaffAsync_Should_ThrowIfStaffNotFound()
    {
        // Arrange
        var staffId = Guid.NewGuid();

        _unitOfWorkMock.Setup(uow => uow.HotelStaff.GetAsync(staffId,null,true)).ReturnsAsync((HotelStaffEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _hotelStaffService.DeleteStaffAsync(staffId));
    }
}