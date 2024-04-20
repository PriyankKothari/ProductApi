using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using ProductApi.WebApi.Validators;
using ProductApi.WebApi.ViewModels;

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


// Build Web Application
var webApplication = webApplicationBuilder.Build();

// Configure the HTTP request pipeline.
if (webApplication.Environment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}
webApplication.UseHttpsRedirection();
webApplication.UseCors("CorsPolicy");

webApplication.MapControllerRoute("Default",
    "{controller}/{action}/{type}",
    new { controller = "Home", action = "Index" });

webApplication.Run();
