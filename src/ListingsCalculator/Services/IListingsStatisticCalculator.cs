namespace ListingsCalculator.Services;

public interface IListingsStatisticCalculator
{
    Task Calculate(string city, int count, CancellationToken cancellation);

}