using DeliveryService.Api.Application.Abstractions;
using DeliveryService.Api.Application.Deliveries;
using DeliveryService.Api.Infrastructure.Messaging;
using DeliveryService.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddDbContext<DeliveryDbContext>(options =>
    options.UseInMemoryDatabase("DeliveryService"));
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddScoped<CreateDeliveryUseCase>(serviceProvider =>
    new CreateDeliveryUseCase(
        serviceProvider.GetRequiredService<IDeliveryRepository>(),
        serviceProvider.GetRequiredService<IEventPublisher>(),
        serviceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value.DeliveryCreatedExchange));
builder.Services.AddScoped<GetDeliveryByIdUseCase>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
