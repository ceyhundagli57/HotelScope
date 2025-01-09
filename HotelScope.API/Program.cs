using Application.DTOs;
using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using HotelScope.API.Handlers;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),   new MySqlServerVersion(new Version(8,0,38))));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IHotelContactInfoService, HotelContactInfoService>();
builder.Services.AddScoped<IHotelStaffService, HotelStaffService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddSingleton<IRabbitMqConnectionManager>(
    new RabbitMqConnectionManager("localhost", "guest", "guest"));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Services.ApplyMigrationsAndSeed();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/hotels", HotelHandlers.AddHotel)
    .WithName("AddHotel")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPut("/hotels", HotelHandlers.UpdateHotel)
    .WithName("UpdateHotel")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapDelete("/hotels/{hotelId:guid}", HotelHandlers.DeleteHotel)
    .WithName("DeleteHotel")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/hotels/{hotelId:guid}", HotelHandlers.GetHotelById)
    .WithName("GetHotelById")
    .Produces<HotelDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/hotels", HotelHandlers.GetAllHotels)
    .WithName("GetAllHotels")
    .Produces<IEnumerable<HotelDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPost("/contact-info", HotelContactInfoHandlers.AddContactInfo)
    .WithName("AddContactInfo")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPut("/contact-info", HotelContactInfoHandlers.UpdateContactInfo)
    .WithName("UpdateContactInfo")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapDelete("/contact-info/{contactInfoId:guid}", HotelContactInfoHandlers.DeleteContactInfo)
    .WithName("DeleteContactInfo")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/contact-info/hotel/{hotelId:guid}", HotelContactInfoHandlers.GetContactInfosByHotelId)
    .WithName("GetContactInfosByHotelId")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPost("/hotel-staff", HotelStaffHandlers.AddHotelStaff)
    .WithName("AddHotelStaff")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPut("/hotel-staff", HotelStaffHandlers.UpdateHotelStaff)
    .WithName("UpdateHotelStaff")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapDelete("/hotel-staff/{staffId:guid}", HotelStaffHandlers.DeleteHotelStaff)
    .WithName("DeleteHotelStaff")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/hotel-staff/hotel/{hotelId:guid}", HotelStaffHandlers.GetStaffByHotelId)
    .WithName("GetStaffByHotelId")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPost("/reports", ReportHandlers.CreateReport)
    .WithName("CreateReport")
    .Produces(StatusCodes.Status202Accepted)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPost("/reports/request", ReportHandlers.GenerateReport)
    .WithName("GenerateReport")
    .Produces(StatusCodes.Status202Accepted)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/reports/{reportId:guid}/status", ReportHandlers.GetReportStatus)
    .WithName("GetReportStatus")
    .Produces<ReportStatusDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/reports/{reportId:guid}", ReportHandlers.GetReportById)
    .WithName("GetReportById")
    .Produces<ReportDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/reports", ReportHandlers.GetAllReports)
    .WithName("GetAllReports")
    .Produces<IEnumerable<ReportSummaryDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError);

app.Run();
