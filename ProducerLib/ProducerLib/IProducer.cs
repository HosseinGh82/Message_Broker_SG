public interface IProducer
{
    Task<bool> SendMessage(string message);
}