using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Linq;
namespace SecureClient
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Makingthe call  hope it will work out");
            RunAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }
        private static async Task RunAsync()
        {
            AuthConfig config = AuthConfig.ReadJsonFromFile("appsettings.json");
            IConfidentialClientApplication application;
            application = ConfidentialClientApplicationBuilder.Create(config.ClientId)
            .WithClientSecret(config.ClientSecret)
            .WithAuthority(new Uri(config.Authority))
            .Build();
            string[] resourceIds = new string[] { config.ResourceId };
            AuthenticationResult result = null;
            try
            {
                result = await application.AcquireTokenForClient(resourceIds).ExecuteAsync();
                // Console.ForegroundColor = ConsoleColor.Green;
                // Console.WriteLine("Token acquired");
                // Console.WriteLine(result.AccessToken);
                // Console.ResetColor();
            }
            catch (MsalClientException exp)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exp.Message);
                Console.ResetColor();

            }
            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                var httpClient = new HttpClient();
                var defaultRequestHeaders = httpClient.DefaultRequestHeaders;
                if (defaultRequestHeaders.Accept is null || defaultRequestHeaders.Accept.Any(x => x.MediaType == "application/json"))
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));

                }
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(config.BaseAdress);
                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    string json = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine(json);

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("échec");
                    string content = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine($"Content:{content}");
                }
            }
        }
    }
}
