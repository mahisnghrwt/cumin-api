using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace cumin_api {
    public class Program {
        public static void Main(string[] args) {
            Console.WriteLine("Starting cumin-api");
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
