using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using UrlShortener.Entities;
using UrlShortener.Extensions;
using UrlShortener.Models;
using UrlShortener.Services;
using UrlShortener.Services.Interfaces;

namespace UrlShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<ICodeGenerator, ShortenUrlService>();

            var connectionString = builder.Configuration.GetConnectionString("DbConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "My API",
            //        Version = "v1",
            //        Description = "A simple API built with ASP.NET Core"
            //    });
            //});

            var app = builder.Build();

            app.ApplyMigrations();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.RoutePrefix = string.Empty;
            //});

            app.MapPost("/url/shorten", async (
                ShortenUrlRequest req,
                ApplicationDbContext dbContext,
                HttpContext httpContext,
                ICodeGenerator codeGenerator
                ) =>
            {
                if (!Uri.TryCreate(req.Url, UriKind.Absolute, out _))
                {
                    return Results.BadRequest($"invalid URI: {req.Url}");
                }

                var code = await codeGenerator.GenerateUniqueCodeAsync();

                // new check
                var repeatUrl = await dbContext.ShortenedUrl
                    .FirstOrDefaultAsync(s => s.LongUrl == req.Url);

                if (repeatUrl != null)
                {
                    string returnString = $"You already added this URL to the database. " +
                    $"Short URL: {repeatUrl.ShortUrl}";
                    return Results.Ok(returnString);
                }
                //

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

                return Results.Ok(shortenedUrl.ShortUrl);
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
