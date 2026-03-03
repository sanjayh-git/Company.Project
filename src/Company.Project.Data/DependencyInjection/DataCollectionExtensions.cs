using Company.Project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Project.Domain.DependencyInjection;

public static class DataCollectionExtensions
{
    public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? "Data Source=meterreadings.db");
        });

        return services;
    }
}
