namespace Shipping.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shipping.Api.Data;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddControllers();
        services.AddDbContext<StockItemDbContext>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(builder => builder.MapControllers());
    }
}