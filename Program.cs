using cumin_api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace cumin_api {
    public class Program {
        public static async Task Main(string[] args) {
            Console.WriteLine("Starting cumin-api");
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope()) {
                try {
                   

                    var iwss = serviceScope.ServiceProvider.GetRequiredService<IWebSocketService>();
                    await iwss.Connect();
                } catch { 
                    
                }
            }

           //  await GetProjects("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI3IiwicmVtb3RlSXAiOiI6OjEiLCJuYmYiOjE2MjQxMjU2ODAsImV4cCI6MTYyNDczMDQ4MCwiaWF0IjoxNjI0MTI1NjgwfQ.--k7LarhTEHU_36SSDoKRxdXB6uyXCNFwPoa-FRqEUE");


            host.Run();

        }
        private static  async Task<IEnumerable<int>> GetProjects(string token) {
            List<int> projects = new List<int>();
            string uri = "http://localhost:47247/api/v1/project";
            try {

                // api/v1/project get
                // make a http client
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await httpClient.GetAsync(uri);

                // read the body as bytes array
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(response.StatusCode);

            } catch (HttpRequestException e) {
                Console.WriteLine(e.Message);
            }
            return projects;
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
