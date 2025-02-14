public interface IConsumer<T>
{
    Task<string> RecieveMessage(T message);
}