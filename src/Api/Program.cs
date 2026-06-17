using Api.Common.Behaviors;
using Api.Features.ProjectTasks.CreateProjectTask;
using Api.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));

});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/openapi/v1.json", "DotNet.VerticalSliceArchitecture API V1");
        config.RoutePrefix = string.Empty; // This makes Swagger the home page
    });
}

app.UseHttpsRedirection();

app.MapCreateProjectTask();

app.Run();