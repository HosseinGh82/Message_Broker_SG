using ConsumerLib;

public class ExampleConsumer : IConsumer<Person>
{
    // Set the base URI for the API or service that will handle the message requests.
    // Replace with the appropriate URI based on your environment or configuration.
    private readonly HttpClient _httpClient;
    private const string Uri = "http://localhost:5022";

    public ExampleConsumer()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(Uri) };
    }


    [RateLimit(2, 3000)]
    public async Task<string> RecieveMessage(Person person)
    {
        try
        {
            var response = await _httpClient.GetStringAsync("/api/message/Receive");

            if (!string.IsNullOrEmpty(response))
            {
                Console.WriteLine($"Message received successfully: {response}");
                return response;
            }
            else
            {
                Console.WriteLine("No message available at the moment.");
                return string.Empty;
            }
        }
        catch (Exception err)
        {
            Console.WriteLine($"Failed to receive message. Error: {err.Message}");
            return string.Empty;
        }
    }
}

public class Person
{
public int Id { get; set; }
public string Name { get; set; }
public decimal Degree { get; set; }
}