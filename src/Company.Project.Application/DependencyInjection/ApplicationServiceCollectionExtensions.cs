using Microsoft.Extensions.DependencyInjection;
using Company.Project.Domain.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Company.Project.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        services.AddDomainDependencies(config); 
        services.AddDataDependencies(config);

        // MediatR registration scanning this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly));

        return services;
    }
}
