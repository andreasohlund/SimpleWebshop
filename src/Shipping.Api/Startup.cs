namespace Shipping.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    //services.AddDbContext<StockItemDbContext>(opt => opt.UseInMemoryDatabase("StockItemList"));

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddCors(options => { options.AddPolicy("AllowAllOrigins", builder => { builder.AllowAnyOrigin(); }); });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("AllowAllOrigins");
            app.UseEndpoints(builder => builder.MapControllers());
        }
    }

    //public class Startup
    //{
    //    IEndpointInstance endpoint;

    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    public IConfiguration Configuration { get; }

    //    // This method gets called by the runtime. Use this method to add services to the container.
    //    public IServiceProvider ConfigureServices(IServiceCollection services)
    //    {
    //        //services.AddDbContext<StockItemDbContext>(opt => opt.UseInMemoryDatabase("StockItemList"));
    //        services.AddMvc();

    //        var builder = new ContainerBuilder();
    //        builder.Populate(services);

    //        builder.Register(c => endpoint)
    //            .As<IMessageSession>()
    //            .SingleInstance();

    //        var container = builder.Build();
    //        endpoint = BootstrapNServiceBusForMessaging(container);
    //        return new AutofacServiceProvider(container);
    //    }

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }

    //        app.UseMvc();

    //        var context = app.ApplicationServices.GetService<StockItemDbContext>();
    //        DataInitializer.Initialize(context);

    //        appLifetime.ApplicationStopping.Register(() => endpoint.Stop().GetAwaiter().GetResult());
    //    }

    //    IEndpointInstance BootstrapNServiceBusForMessaging(IContainer container)
    //    {
    //        var endpointConfiguration = new EndpointConfiguration("Shipping.Api");

    //        endpointConfiguration.ApplyCommonNServiceBusConfiguration(container, bridgeConfigurator: transport =>
    //        {
    //            // Bridge Shipping
    //            var bridge = transport.Routing().ConnectToBridge("bridge-shipping");

    //            // Subscribe to events from warehouse to be delivered via bridge
    //            bridge.RegisterPublisher(typeof(ItemStockUpdated), "warehouse");
    //        });

    //        // Remove assembly information to be able to reuse message schema from different endpoints w/o sharing messages assembly
    //        endpointConfiguration.RegisterMessageMutator(new RemoveAssemblyInfoFromMessageMutator());

    //        // Configure saga audit plugin
    //        endpointConfiguration.AuditSagaStateChanges("Particular.ServiceControl");

    //        return Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
    //    }
    //}
}