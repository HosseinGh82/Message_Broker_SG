namespace ProducerLib;

[AttributeUsage(AttributeTargets.Method)]
public class RateLimitAttribute : Attribute
{
    public int RequestCount { get; }
    public int Delay { get; }

    public RateLimitAttribute(int requestCount = 3, int delay = 3000)
    {
        RequestCount = requestCount;
        Delay = delay;
    }
}