using ran_product_management_net.Data;
using Microsoft.EntityFrameworkCore;
using ran_product_management_net.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgresConn");
builder.Services
    .AddDbContext<ApplicationDbContext>(Options =>
        Options
            .UseNpgsql(connectionString,
            o => o
                .MapEnum<ProductInventoryStatus>("product_inventory_status")
                .MapEnum<ProductCondition>("product_condition"))
            .EnableSensitiveDataLogging());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
