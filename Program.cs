using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "o3";
var baseUrl = Environment.GetEnvironmentVariable("OPENAI_BASE_URL") ?? "https://api.openai.com/v1";

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("Set the OPENAI_API_KEY environment variable before running this chatbot.");
    Console.WriteLine("Example: export OPENAI_API_KEY=your_key_here");
    return;
}

using var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

Console.WriteLine("James AI is ready. Type 'exit' to quit.");

while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
    {
        continue;
    }

    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    try
    {
        var reply = await GetChatReplyAsync(client, baseUrl, model, userInput);
        Console.WriteLine($"Assistant: {reply}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

static async Task<string> GetChatReplyAsync(HttpClient client, string baseUrl, string model, string userInput)
{
    var payload = new
    {
        model,
        messages = new object[]
        {
            new { role = "system", content = "You are a friendly and helpful assistant." },
            new { role = "user", content = userInput }
        },
        temperature = 0.7
    };

    var json = JsonSerializer.Serialize(payload);
    using var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl.TrimEnd('/')}/chat/completions");
    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

    using var response = await client.SendAsync(request);
    var responseBody = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception($"Request failed with {(int)response.StatusCode}: {responseBody}");
    }

    using var document = JsonDocument.Parse(responseBody);
    return document.RootElement
        .GetProperty("choices")[0]
        .GetProperty("message")
        .GetProperty("content")
        .GetString() ?? "No response received.";
}
