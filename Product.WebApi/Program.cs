using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Mappers;
using ProductApi.Application.Services;
using ProductApi.Domain.Repositories;
using ProductApi.Persistence.DatabaseContexts;
using ProductApi.Persistence.Repositories;
using ProductApi.WebApi.Mappers;
using ProductApi.WebApi.Validators;
using ProductApi.WebApi.ViewModels;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        var webApplicationBuilder = WebApplication.CreateBuilder(args);

        // Build Service
        // Logging
        webApplicationBuilder.Host.ConfigureLogging(logging => logging.AddConfiguration(webApplicationBuilder.Configuration.GetSection("Logging")));

        // Add CORS
        webApplicationBuilder.Services.AddCors(policy =>
        {
            policy.AddPolicy("CorsPolicy", options =>
            {
                options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Controllers
        webApplicationBuilder.Services.AddControllers();

        // Fluent Validation
        webApplicationBuilder.Services.AddFluentValidationAutoValidation();
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        // API Explorer
        webApplicationBuilder.Services.AddEndpointsApiExplorer();
        webApplicationBuilder.Services.AddSwaggerGen();

        // API Versioning
        webApplicationBuilder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(), new HeaderApiVersionReader("x-api-version"));
        });

        webApplicationBuilder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // Services
        webApplicationBuilder.Services.AddScoped<IValidator<ProductRequest>, ProductRequestValidator>();

        webApplicationBuilder.Services
            .AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlite(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnectionString"));
            })
            .AddScoped<DatabaseContext>();

        webApplicationBuilder.Services
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IProductService, ProductService>()
            .AddAutoMapper(typeof(EntitiesMappingProfile))
            .AddAutoMapper(typeof(ViewModelsMappingProfile));


        // Build Web Application
        var webApplication = webApplicationBuilder.Build();

        // Configure the HTTP request pipeline.
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();
            DeleteDatabaseFile();

            // Delete database files everytime when starting the application
            void DeleteDatabaseFile()
            {
                try
                {
                    foreach (var file in Directory.GetFiles(Path.GetDirectoryName(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnectionString"))))
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting database file: {ex.Message}");
                }
            }
        }
        webApplication.UseHttpsRedirection();
        webApplication.UseCors("CorsPolicy");

        webApplication.MapControllerRoute("Default",
            "{controller}/{action}/{type}",
            new { controller = "Home", action = "Index" });

        webApplication.Run();
    }
}