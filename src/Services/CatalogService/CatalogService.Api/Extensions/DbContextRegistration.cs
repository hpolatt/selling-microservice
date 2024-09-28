using System;
using System.Reflection;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Extensions;

public static class DbContextRegistration
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) {
        services.AddEntityFrameworkSqlServer()
            .AddDbContext<CatalogContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("CatalogConnection"));
            });
            
        return services;
    }

}
