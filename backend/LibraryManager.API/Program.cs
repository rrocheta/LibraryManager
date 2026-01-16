using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManager.API
{
    /// <summary>
    /// Entry point of the LibraryManager API application.
    /// Configures services, middleware, and runs the web app.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var enableSwagger = builder.Configuration.GetValue<bool>("AppSettings:EnableSwagger");

            // Register services and repositories for DI
            builder.Services.AddScoped<Repositories.IBookRepository, Repositories.BookRepository>();
            builder.Services.AddScoped<Repositories.IAuthorRepository, Repositories.AuthorRepository>();
            builder.Services.AddScoped<Repositories.IPublisherRepository, Repositories.PublisherRepository>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            if (enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryManager API V1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapControllers();

            // Apply any pending EF Core migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                const int maxAttempts = 10;
                var delay = TimeSpan.FromSeconds(2);

                for (var attempt = 1; attempt <= maxAttempts; attempt += 1)
                {
                    try
                    {
                        context.Database.Migrate();
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (attempt == maxAttempts)
                        {
                            throw;
                        }

                        logger.LogWarning(
                            ex,
                            "Database migration failed (attempt {Attempt}/{Max}). Retrying in {DelaySeconds}s.",
                            attempt,
                            maxAttempts,
                            delay.TotalSeconds);
                        Thread.Sleep(delay);
                    }
                }

                context.SaveChanges();
            }

            app.Run();
        }
    }
}
