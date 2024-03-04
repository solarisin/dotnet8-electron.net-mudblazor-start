using MudBlazor.Services;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace StartingPoint;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        builder.WebHost.UseElectron(args)
            .UseUrls("http://localhost:8085"); // Specify the port to use both here and in electron.manifest.json

        builder.Services.AddElectron();
        builder.Services.AddMudServices();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode();

        await app.StartAsync();
        
        if (HybridSupport.IsElectronActive)
        {
            WebPreferences wp = new WebPreferences();
            // Remove or set to true if you need ipc
            wp.NodeIntegration = false;
            BrowserWindowOptions browserWindowOptions = new BrowserWindowOptions
            {
                WebPreferences = wp
            };
                
            var window = await Electron.WindowManager.CreateWindowAsync();
            window.RemoveMenu();
        }

        app.WaitForShutdown();
    }
}