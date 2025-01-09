using Application.Interfaces;
using Infrastructure.Messaging;
using ReportBackgroundService;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IRabbitMqConnectionManager>(
            new RabbitMqConnectionManager("localhost", "guest", "guest"));
        services.AddHostedService<ReportConsumer>();
        services.AddLogging();
    });

var app = builder.Build();
await app.RunAsync();