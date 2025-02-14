using ConsumerLib;
using System.Reflection;
using System.Text.Json;


// Change the DLL path to match your actual file location.
string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "G:/Hamkaran_test/Log_Message_Broker/Example/ExampleConsumer.dll");

if (!File.Exists(dllPath))
{
    Console.WriteLine($"DLL not found: {dllPath}");
    return;
}

var assembly = Assembly.LoadFrom(dllPath);

string className = "ExampleConsumer";
var exampleConsumerType = assembly.GetType(className);

if (exampleConsumerType == null)
{
    Console.WriteLine($"Class '{className}' not found in the DLL.");
    return;
}

Type? messageType = exampleConsumerType
                           .GetInterfaces()
                           .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))
                           .GetGenericArguments()
                           .FirstOrDefault();

if (messageType == null)
{
    Console.WriteLine("message type is null");
    return;
}

var messageInstance = Activator.CreateInstance(messageType);

MethodInfo? receiveMethod = exampleConsumerType.GetMethod("RecieveMessage");

if (receiveMethod == null)
{
    Console.WriteLine("ReceiveMessage method not found!");
    return;
}

var rateLimitAttr = receiveMethod.GetCustomAttribute<RateLimitAttribute>();
if (rateLimitAttr == null)
{
    Console.WriteLine("RateLimitAttribute not found on ReceiveMessage method!");
    return;
}

var exampleConsumerInstance = Activator.CreateInstance(exampleConsumerType);

int messageCount = rateLimitAttr.RequestCount;


while (true)
{
    List<Task> tasks = new List<Task>();

    for (int i = 0; i < messageCount; i++)
    {
        tasks.Add(Task.Run(async () =>
        {
            var resultTask = (Task<string>)receiveMethod.Invoke(exampleConsumerInstance, new object[] { messageInstance });

            var result = await resultTask;

            if (result == string.Empty)
            {
                Console.WriteLine("No messages available.");
            }
            else
            {
                var message = DeserializeMessage(result, messageType);
                
                var properties = messageType.GetProperties()
                    .Select(property => $"{property.Name} : {property.GetValue(message)}");

                Console.WriteLine("Received Message: " + string.Join(", ", properties));
            }
        }));
    }
    await Task.WhenAll(tasks);
    Console.WriteLine("Batch completed.");
    await Task.Delay(rateLimitAttr.Delay);
}
    

static object? DeserializeMessage(string json, Type targetType)
{
    var deserializeMethod = typeof(JsonSerializer)
        .GetMethod("Deserialize", new[] { typeof(string), typeof(JsonSerializerOptions) })
        ?.MakeGenericMethod(targetType);

    if (deserializeMethod != null)
    {
        return deserializeMethod.Invoke(null, new object[] { json, null });
    }

    return null;
}