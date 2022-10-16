using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GameNight2.Models;
using GameNight2.Utils;

namespace GameNight2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Utilities.CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var canContinue = await Utilities.WaitForMigrations(host, context);

                if (!canContinue)
                {
                    return;
                }
            }

            var task = host.RunAsync();

            Utilities.Notify("GameNight2 Running!");

            WebHostExtensions.WaitForShutdown(host);
        }
    }
}
