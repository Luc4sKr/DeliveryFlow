using ValidationService.Api.Application.Validation;
using ValidationService.Api.Infrastructure.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<DeliveryValidator>();
builder.Services.AddSingleton<RabbitMqEventPublisher>();
builder.Services.AddSingleton<DeliveryCreatedConsumer>();
builder.Services.AddHostedService(serviceProvider =>
    serviceProvider.GetRequiredService<DeliveryCreatedConsumer>());

var host = builder.Build();
host.Run();