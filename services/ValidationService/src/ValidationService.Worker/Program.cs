using ValidationService.Worker.Application.Validation;
using ValidationService.Worker.Infrastructure.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<DeliveryValidator>();
builder.Services.AddSingleton<RabbitMqEventPublisher>();
builder.Services.AddSingleton<DeliveryCreatedConsumer>();
builder.Services.AddHostedService(serviceProvider =>
    serviceProvider.GetRequiredService<DeliveryCreatedConsumer>());

var host = builder.Build();
host.Run();