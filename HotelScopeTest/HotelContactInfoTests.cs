using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Moq;

namespace HotelScopeTest;

public class HotelContactInfoTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HotelContactInfoService _hotelContactInfoService;

    public HotelContactInfoTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _hotelContactInfoService = new HotelContactInfoService(_unitOfWorkMock.Object, _mapperMock.Object);
    }
    
    [Fact]
    public async Task UpdateContactInfoAsync_Should_UpdateAndSave()
    {
        // Arrange
        var contactInfoDto = new UpdateContactInfoDto { Id = Guid.NewGuid(), Email = "updated@example.com" };
        var contactInfoEntity = new HotelContactInfoEntity { Id = contactInfoDto.Id, Email = "old@example.com" };

        _unitOfWorkMock.Setup(uow => uow.HotelContactInfo.GetAsync(contactInfoDto.Id,null,true)).ReturnsAsync(contactInfoEntity);
        _mapperMock.Setup(m => m.Map(contactInfoDto, contactInfoEntity));
        _unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _hotelContactInfoService.UpdateContactInfoAsync(contactInfoDto);

        // Assert
        _mapperMock.Verify(m => m.Map(contactInfoDto, contactInfoEntity), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteContactInfoAsync_Should_RemoveAndSave()
    {
        // Arrange
        var contactInfoId = Guid.NewGuid();
        var contactInfoEntity = new HotelContactInfoEntity { Id = contactInfoId, Email = "test@example.com" };

        _unitOfWorkMock.Setup(uow => uow.HotelContactInfo.GetAsync(contactInfoId, null,true)).ReturnsAsync(contactInfoEntity);
        _unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.CompletedTask);

        // Act
        await _hotelContactInfoService.DeleteContactInfoAsync(contactInfoId);

        // Assert
        _unitOfWorkMock.Verify(uow => uow.HotelContactInfo.Remove(contactInfoEntity), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteContactInfoAsync_Should_ThrowIfContactInfoNotFound()
    {
        // Arrange
        var contactInfoId = Guid.NewGuid();

        _unitOfWorkMock.Setup(uow => uow.HotelContactInfo.GetAsync(contactInfoId, null,true)).ReturnsAsync((HotelContactInfoEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _hotelContactInfoService.DeleteContactInfoAsync(contactInfoId));
    }
}