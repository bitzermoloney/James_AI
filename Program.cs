using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JamesAI
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("James AI HTTP request sample");

            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync("https://api.github.com/");
                response.EnsureSuccessStatusCode();

                Console.WriteLine($"Response status: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
            }
        }
    }
}
