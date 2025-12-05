using ExamTwo.Domain.Entities;

namespace ExamTwo.Infrastructure.Data
{
  public class CoffeeMachineDataStore
  {
    public Dictionary<string, CoffeeData> coffees = new Dictionary<string, CoffeeData>
    {
      { "Americano", new CoffeeData(950, 10) },
      { "Cappuccino", new CoffeeData(1200, 8) },
      { "Latte", new CoffeeData(1350, 10) },
      { "Mocaccino", new CoffeeData(1500, 15) }
    };

    public Dictionary<int, int> coinInventory = new Dictionary<int, int>
    {
      { 500, 20 },
      { 100, 30 },
      { 50, 50 },
      { 25, 25 }
    };
  }
}
