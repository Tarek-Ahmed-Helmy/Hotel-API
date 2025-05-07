using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Utilities;

namespace Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });


        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var unitOfWork = services.GetRequiredService<IUnitOfWork>();
            await BasicDataSeeder.SeedBasicDataAsync(unitOfWork);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
