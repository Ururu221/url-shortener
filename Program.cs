using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Models;
using System.Data.Common;
using UrlShortener.Entities;
using UrlShortener.Extensions;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<ShortenUrlService>();

            var connectionString = builder.Configuration.GetConnectionString("DbConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(connectionString));

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "A simple API built with ASP.NET Core"
                });
            });

            var app = builder.Build();

            app.ApplyMigrations();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; 
            });

            app.MapPost("/url/shorten", async (
                ShortenUrlRequest req,
                ApplicationDbContext dbContext,
                ShortenUrlService shortenUrlService,
                HttpContext httpContext
                ) =>
            {
                if (!Uri.TryCreate(req.Url, UriKind.Absolute, out _))
                {
                    return Results.BadRequest($"invalid URI: {req.Url}");
                }

                var code = await shortenUrlService.GenerateUniqueCode();

                var shortenedUrl = new ShortenedUrl
                {
                    Id = Guid.NewGuid(),
                    LongUrl = req.Url,
                    Code = code,
                    CreatedOn = DateTime.UtcNow,
                    ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/url/{code}"
                };

                dbContext.ShortenedUrl.Add(shortenedUrl);

                await dbContext.SaveChangesAsync();

                return Results.Ok(shortenedUrl);
            });

            app.MapGet("/url/{code}", async (string code, ApplicationDbContext dbContext) =>
            {
                var shortenedUrl = await dbContext.ShortenedUrl.FirstOrDefaultAsync(s => s.Code == code);

                if (shortenedUrl == null)
                {
                    return Results.NotFound();
                }

                return Results.Redirect(shortenedUrl.LongUrl);
            });

            app.MapGet("/", () => 
            {
                return Results.Ok("Hello world!");
            });

            app.Run();
        }
    }
}
