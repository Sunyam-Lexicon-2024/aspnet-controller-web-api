using ControllerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Serilog;
using ControllerWebAPI.MappingProfiles;

namespace ControllerWebAPI.Extensions;

public static class WebAppExtensions
{

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSerilog()
            .AddDbContext<ApiDbContext>(opt =>
                opt.UseInMemoryDatabase("ApiDb"))
            .AddLogging()
            .AddAutoMapper(config =>
        {
            config.AddProfile<ItemMappingProfile>();
        });

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Controllers Web API",
                Description = "An ASP.NET Core Controller based Web API for managing Items",
                Contact = new OpenApiContact
                {
                    Name = "Suny-Am",
                    Email = "visualarea.1@gmail.com",
                    Url = new Uri("https://github.com/suny-am")
                },
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}