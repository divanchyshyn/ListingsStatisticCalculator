namespace ListingsCalculator.Services;

public class ConsoleMessageSync : IMessageSync
{
    public void Send(string message) => Console.WriteLine(message);
}