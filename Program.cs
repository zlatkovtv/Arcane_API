using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ArcaneApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var host = Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel()
                        .UseIIS()
                        .UseStartup<Startup>();
                }).Build();
            host.Run();
        }
    }
}
