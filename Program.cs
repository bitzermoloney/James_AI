
// Setting up the system
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

// Declaring variables apiKey, the model of AI in use and the URL
var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "o3";
var baseUrl = Environment.GetEnvironmentVariable("OPENAI_BASE_URL") ?? "https://api.openai.com/v1";

// Ask for an apiKey
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("Set the OPENAI_API_KEY environment variable before running this chatbot.");
    Console.WriteLine("Example: export OPENAI_API_KEY=your_key_here");
    return;
}

// Authorising key
using var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

// AI is a go
Console.WriteLine("James AI is ready. Type 'exit' to quit.");

while (true) // Infinite loop, the program breaks if the user 'puts 'exit'
{
    // User input
    Console.Write("You: ");
    var userInput = Console.ReadLine();

    // Error handling (null / no input)
    if (string.IsNullOrWhiteSpace(userInput))
    {
        continue;
    }

    // Break if the user 'puts 'exit'
    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    // Try catch shows error handling, program is generating a reply using the api key and model
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
    // This function generates a reply using the api key and input from user (parameters)
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
