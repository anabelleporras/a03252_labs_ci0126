using ExamTwo.Infrastructure;

namespace ExamTwo.Application.UseCases
{
  public class CoffeeMachineQuery : ICoffeeMachineQuery
  {
    private readonly CoffeeMachineDataStore _db;
    public CoffeeMachineQuery(CoffeeMachineDataStore db)
    {
      _db = db;
    }
    public Task<Dictionary<string, int>> GetCoffeesAsync()
    {
      return Task.FromResult(_db.coffeeQuantities);
    }
    public Task<Dictionary<string, int>> GetCoffeePricesAsync()
    {
      return Task.FromResult(_db.coffeePrices);
    }
    public Task<Dictionary<int, int>> GetChangeAsync()
    {
      return Task.FromResult(_db.change);
    }
  }
}
