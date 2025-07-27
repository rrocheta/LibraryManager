
using LibraryManager.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var enableSwagger = builder.Configuration.GetValue<bool>("AppSettings:EnableSwagger");

            // Add services to the container.
            builder.Services.AddScoped<Repositories.IBookRepository, Repositories.BookRepository>();
            builder.Services.AddScoped<Repositories.IAuthorRepository, Repositories.AuthorRepository>();
            builder.Services.AddScoped<Repositories.IPublisherRepository, Repositories.PublisherRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("LibraryDb"));

            // CORS policy to allow React app
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
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

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(
                        new Author { Id = 1, Name = "Fernando Pessoa" },
                        new Author { Id = 2, Name = "Hernandes Dias Lopes" },
                        new Author { Id = 3, Name = "Reinhard Bonnke" }
                    );
                }

                if (!context.Publishers.Any())
                {
                    context.Publishers.AddRange(
                        new Publisher { Id = 1, Name = "Porto Editora" },
                        new Publisher { Id = 2, Name = "Hagnos" },
                        new Publisher { Id = 3, Name = "CPAD" }
                    );
                }

                context.SaveChanges();
            }

            // TODO docker
            //using (var scope = app.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    db.Database.Migrate();
            //}

            app.Run();
        }
    }
}
