using ProducerLib;
using System.Net.Http.Json;

public class ExampleProducer : IProducer
{
    public List<Person> List { get; set; }

    private readonly HttpClient _httpClient;

    // Set the base URI for the API or service that will handle the message requests.
    // Replace with the appropriate URI based on your environment or configuration.
    private const string Uri = "http://localhost:5022";

    public ExampleProducer()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(Uri) };

        List = new List<Person>
        {
            new Person { Id = 1, Name = "Hossein", Degree = 90 },
            new Person { Id = 2, Name = "Sara", Degree = 88 },
            new Person { Id = 3, Name = "Yasi", Degree = 95 },
            new Person { Id = 4, Name = "Helia", Degree = 84 },
            new Person { Id = 5, Name = "Parsa", Degree = 90 },
            new Person { Id = 6, Name = "Shiva", Degree = 88 },
            new Person { Id = 7, Name = "Mobina", Degree = 95 },
            new Person { Id = 8, Name = "Ali", Degree = 84 },
            new Person { Id = 9, Name = "Hassan", Degree = 90 },
            new Person { Id = 10, Name = "Koroush", Degree = 88 },
            new Person { Id = 11, Name = "Farhad", Degree = 95 },
            new Person { Id = 12, Name = "Fateme", Degree = 84 },
            new Person { Id = 13, Name = "Maryam", Degree = 90 },
            new Person { Id = 14, Name = "Mohammad", Degree = 88 },
            new Person { Id = 15, Name = "Sina", Degree = 95 },
            new Person { Id = 16, Name = "Diana", Degree = 84 },
            new Person { Id = 17, Name = "Mehrdad", Degree = 90 },
            new Person { Id = 18, Name = "Mehran", Degree = 88 },
            new Person { Id = 19, Name = "Aria", Degree = 95 },
            new Person { Id = 20, Name = "Sarina", Degree = 84 },
            new Person { Id = 21, Name = "Hossein", Degree = 90 },
            new Person { Id = 22, Name = "Sara", Degree = 88 },
            new Person { Id = 23, Name = "Yasi", Degree = 95 },
            new Person { Id = 24, Name = "Helia", Degree = 84 },
            new Person { Id = 25, Name = "Parsa", Degree = 90 },
            new Person { Id = 26, Name = "Shiva", Degree = 88 },
            new Person { Id = 27, Name = "Mobina", Degree = 95 },
            new Person { Id = 28, Name = "Ali", Degree = 84 },
            new Person { Id = 29, Name = "Hassan", Degree = 90 },
            new Person { Id = 30, Name = "Koroush", Degree = 88 },
            new Person { Id = 31, Name = "Farhad", Degree = 95 },
            new Person { Id = 32, Name = "Fateme", Degree = 84 },
            new Person { Id = 33, Name = "Maryam", Degree = 90 },
            new Person { Id = 34, Name = "Mohammad", Degree = 88 },
            new Person { Id = 35, Name = "Sina", Degree = 95 },
            new Person { Id = 36, Name = "Diana", Degree = 84 },
            new Person { Id = 37, Name = "Mehrdad", Degree = 90 },
            new Person { Id = 38, Name = "Mehran", Degree = 88 },
            new Person { Id = 39, Name = "Aria", Degree = 95 },
            new Person { Id = 40, Name = "Sarina", Degree = 84 }
        };
    }


    [RateLimit(3, 5000)]
    [Retry(5, 3000)]
    public async Task<bool> SendMessage(string message)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/message/Send", message);
            Console.WriteLine($"Message sent successfully: {message}");
            return true;
        }
        catch (Exception err)
        {
            Console.WriteLine($"Message {message} did't send. Error: {err.Message}");
            return false;
        }
    }
}


public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Degree { get; set; }
}