using System.Text.Json;
using System.Reflection;
using ProducerLib;
using System.Collections;


// Change the DLL path to match your actual file location.
string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "G:/Hamkaran_test/Log_Message_Broker/Example/ExampleProducer.dll");

if (!File.Exists(dllPath))
{
    Console.WriteLine($"DLL not found: {dllPath}");
    return;
}

var assembly = Assembly.LoadFrom(dllPath);

string className = "ExampleProducer";
var exampleProducerType = assembly.GetType(className);

if (exampleProducerType == null)
{
    Console.WriteLine($"Class '{className}' not found in the DLL.");
    return;
}


MethodInfo? sendMethod = exampleProducerType.GetMethod("SendMessage");

if (sendMethod == null)
{
    Console.WriteLine("SendMessage method not found!");
    return;
}


var retryAttr = sendMethod.GetCustomAttribute<RetryAttribute>();
if (retryAttr == null)
{
    Console.WriteLine("RetryAttribute not found on SendMessage method!");
    return;
}

var rateLimitAttr = sendMethod.GetCustomAttribute<RateLimitAttribute>();
if (rateLimitAttr == null)
{
    Console.WriteLine("RateLimitAttribute not found on SendMessage method!");
    return;
}


var exampleProducerInstance = Activator.CreateInstance(exampleProducerType);


var listOfObject = exampleProducerType
    .GetProperty("List")
    .GetValue(exampleProducerInstance) as IList;

if (listOfObject == null)
{
    Console.WriteLine("List of message is null or not found!");
    return;
}


int messageCount = rateLimitAttr.RequestCount;

if (messageCount <= 0)
{
    Console.WriteLine("Invalid message count!");
    return;
}

int batch = (int)Math.Ceiling((double)listOfObject.Count / messageCount);


for (int i = 0; i < batch; i++)
{
    List<Task> tasks = new List<Task>();

    for (int j = 0; j < messageCount && (i * messageCount + j) < listOfObject.Count; j++)
    {
        int index = i * messageCount + j;
        tasks.Add(Task.Run(async () =>
        {
            string message = JsonSerializer.Serialize(listOfObject[index]);

            for (int attempt = 1; attempt <= retryAttr.AttemptCount; attempt++)
            {
                var resultTask = (Task<bool>)sendMethod.Invoke(exampleProducerInstance, new object[] { message });

                if (resultTask == null)
                {
                    Console.WriteLine("sendMethod.Invoke returned an unexpected type.");
                    continue;
                }

                bool result = await resultTask;
                if (result == true)
                {
                    Console.WriteLine($"Message {index + 1} processed, Result: {result}");
                    break;
                }
                else
                {
                    Console.WriteLine($"Failed to send message (Attempt {attempt})");
                    await Task.Delay(retryAttr.Delay / messageCount);
                }
            }
        }));
    }
    await Task.WhenAll(tasks);
    Console.WriteLine("Batch completed.");
    await Task.Delay(rateLimitAttr.Delay);
}