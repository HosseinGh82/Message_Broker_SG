using System.Collections.Concurrent;

namespace MessageBroker.Services;

public class MessageService
{
    private const string FilePath = "Messages.json";
    private readonly ConcurrentQueue<string> _messages = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public MessageService()
    {
        LoadMessages();
    }

    private async Task LoadMessages()
    {
        if (File.Exists(FilePath))
        {
            foreach (string line in File.ReadLines(FilePath))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    _messages.Enqueue(line);
                }
            }
            LogService.Info("Messages loaded from file.");
        }
        else
        {
            LogService.Error($"File path is not exist! {FilePath}");
        }
    }

    private async Task SaveMessageToFile(string message)
    {
        await _semaphore.WaitAsync();

        try
        {
            await File.AppendAllTextAsync(FilePath, message + Environment.NewLine);
            LogService.Info($"Message saved to file. Message: {message}");
        }
        catch(Exception ex)
        {
            LogService.Warning($"Message did not save to file! Message: {message}, Error: {ex.Message}");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task SaveMessagesToFile(string message)
    {
        await _semaphore.WaitAsync();
        try
        {
            await File.WriteAllLinesAsync(FilePath, _messages);
            LogService.Info($"Message deleted from file. Message: {message}");
        }
        catch (Exception ex)
        {
            LogService.Warning($"Message did not delete from file! Message: {message}, Error: {ex.Message}");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task AddMessage(string message)
    {
        _messages.Enqueue(message);
        LogService.Info($"Message added to queue. Message: {message}");
        await SaveMessageToFile(message);
    }

    public async Task<string?> GetMessage()
    {
        if (_messages.TryDequeue(out var message))
        {
            LogService.Info($"Message deleted from queue. Message: {message}");
            await SaveMessagesToFile(message);
            return message;
        }
        LogService.Warning("No messages in queue!");
        return null;
    }
}