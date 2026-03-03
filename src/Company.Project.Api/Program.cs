using Company.Project.Api;
using Company.Project.Application.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationDependencies();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Host.ConfigureSerilog();

var corsPolicyName = "AllowAngular";
builder.AddCors(corsPolicyName);

var app = builder.Build();

app.UseSwaggerPage();

//Disabled so that it can run on docker. We can make necessary changes in docker file to include SSL certificates in future.
//app.UseHttpsRedirection();
app.UseCors(corsPolicyName);

app.UseGlobalExceptionHandler();

app.MapControllers();

// Seed accounts from CSV
await app.SeedData();

app.Run();