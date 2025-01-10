using Application.Interfaces;
using Infrastructure.Messaging;
using ReportWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IRabbitMqConnectionManager>(
    new RabbitMqConnectionManager(Environment.GetEnvironmentVariable("RabbitMqHost"),
        Environment.GetEnvironmentVariable("RabbitMqUserName"),
        Environment.GetEnvironmentVariable("RabbitMqPassword")));
builder.Services.AddLogging();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();