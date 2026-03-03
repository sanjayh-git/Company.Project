namespace Company.Project.Api
{
    using Company.Project.Data;
    using Company.Project.Infrastructure.Data;
    using Microsoft.AspNetCore.Diagnostics;
    using Serilog;

    public static class BootstrapExtension
    {
        public static string AddCors(this WebApplicationBuilder builder, string corsPolicyName)
        {
            Log.Information("Configuring CORS policy...");
            var allowedOrigins = builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>() ?? [];

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: corsPolicyName, policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins) // Angular dev server origin
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
                return corsPolicyName;
        }

        public static void UseSwaggerPage(this WebApplication app)
        {
            Log.Information("Configuring Swagger...");

            app.UseSwagger();

            // redirect "/" to "/swagger"
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeterReadings API v1");
            });

            // optional: redirect root to swagger
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }

        public static void ConfigureSerilog(this ConfigureHostBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()               // logs to console
                    .WriteTo.Seq("http://localhost:5341")  // logs to Seq
                    .CreateLogger();

            builder.UseSerilog();
        }


        public static void UseGlobalExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var exception = context.Features
                        .Get<IExceptionHandlerFeature>()?.Error;

                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = exception?.Message ?? "An unexpected error occurred."
                    });
                });
            });
        }

        public static async Task SeedData(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();

                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                await DbSeeder.SeedAccountsAsync(db, logger);
            }
        }

    }
}
