using Company.Project.Application.Services;
using Company.Project.Data;
using Company.Project.Data.Repositories;
using Company.Project.Data.Repositories.Interfaces;
using Company.Project.Domain.Csv;
using Company.Project.Domain.Csv.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Project.Domain.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? "Data Source=meterreadings.db");
        });
        services.AddScoped<IMeterReadingService, MeterReadingService>();
        services.AddScoped<ICsvParser, CsvParser>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
        return services;
    }
}
