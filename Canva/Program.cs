using Canva.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ThreeView;
using Canva;
using Microsoft.Extensions.FileProviders;
using MotionEngine;

public class Program
{
    public static void Main(string[] args)
    {
        // Create and run the host
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    // Add services to the container.
                    services.AddRazorPages();
                    services.AddServerSideBlazor();
                    services.AddSingleton<WeatherForecastService>();
                    services.AddSingleton<TestState>();
                    services.AddScoped<ThreeViewService>();
                    services.AddHostedService<MotionEngine.MotionEngine>();
                    services.AddScoped<MapService>();
                });

                webBuilder.Configure(app =>
                {
                    // Configure the HTTP request pipeline.
                    var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                    if (!env.IsDevelopment())
                    {
                        app.UseExceptionHandler("/Error");
                        // The default HSTS value is 30 days. You may want to change this for production scenarios.
                        app.UseHsts();

                    }

                    app.UseHttpsRedirection();
                    app.UseStaticFiles();
                    app.UseRouting();

                    // Adjust the path based on your project structure
                    var threeViewPath = Path.Combine(env.ContentRootPath, "..", "ThreeView", "wwwroot");

                    // Ensure the directory exists before creating the file provider
                    if (!Directory.Exists(threeViewPath))
                    {
                        throw new DirectoryNotFoundException($"The specified directory was not found: {threeViewPath}");
                    }

                    var fileProvider = new PhysicalFileProvider(threeViewPath);
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = fileProvider,
                        RequestPath = "/ThreeView"
                    });

                    // Use endpoint routing for Blazor Server
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapBlazorHub();
                        endpoints.MapFallbackToPage("/_Host"); // Ensure 'App' matches your main component
                    });

                });
            });
}
