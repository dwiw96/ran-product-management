using ran_product_management_net.Database.Postgresql;
using Microsoft.EntityFrameworkCore;
using ran_product_management_net.Database.Postgresql.Models;
using System.Text.Json.Serialization;
using ran_product_management_net.Database.Mongodb;
using System.Text.Json;
using ran_product_management_net.Services;


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
                .MapEnum<ProductStatus>("product_status")
                .MapEnum<ProductCondition>("product_condition")
                .MapEnum<ProductMediaType>("product_media_type"))
            .EnableSensitiveDataLogging());

builder.Services.AddSingleton<MongoDBService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // or use a custom snake_case policy
        options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
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
