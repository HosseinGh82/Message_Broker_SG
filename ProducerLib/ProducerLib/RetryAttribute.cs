namespace ProducerLib;

[AttributeUsage(AttributeTargets.Method)]
public class RetryAttribute : Attribute
{
    public int AttemptCount { get; }
    public int Delay { get; }

    public RetryAttribute(int attemptCount = 3, int delay = 3000)
    {
        AttemptCount = attemptCount;
        Delay = delay;
    }
}