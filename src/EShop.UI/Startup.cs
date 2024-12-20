﻿namespace EShop.UI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceComposer.AspNetCore;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddRouting();
        services.AddControllersWithViews()
            .AddRazorRuntimeCompilation();

        // Configure ServiceComposer
        services.AddViewModelComposition(options =>
        {
            options.EnableCompositionOverControllers();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
       
        app.UseRouting();
        app.UseStaticFiles();
        app.UseEndpoints(builder =>
        {
            builder.MapControllers();

            // Configure ServiceComposer
            builder.MapCompositionHandlers();
        });
    }
}