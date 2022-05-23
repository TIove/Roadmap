using Serilog;
using Tiove.Roadmap.Infrastructure.Extensions;

namespace Tiove.Roadmap;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception exc)
        {
            Log.Fatal(exc, "Can not properly start UserService.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            .AddInfrastructure()
            .AddHttp();
}