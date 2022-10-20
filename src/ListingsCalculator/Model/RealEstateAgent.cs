namespace ListingsCalculator.Model;

public class RealEstateAgent
{
    public int Id { get; }
    public string Name { get;  }
    public int ListingsCount { get; private set; }

    public RealEstateAgent(int id, string name, int listingsCount)
    {
        Id = id;
        Name = name;
        ListingsCount = listingsCount;
    }

    public void IncrementListingsCount() => ListingsCount++;
};