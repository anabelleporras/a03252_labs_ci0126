using ExamTwo.Application.Ports;

namespace ExamTwo.Infrastructure
{
  public class CoffeeMachineRepository : ICoffeeMachineRepository
  {
    CoffeeMachineDataStore _dataStore;
    public CoffeeMachineRepository(CoffeeMachineDataStore dataStore)
    {
      _dataStore = dataStore;
    }
    public Task<Dictionary<string, int>> GetCoffees()
    {
      return Task.FromResult(_dataStore.coffeeQuantities);
    }
    public Task<Dictionary<string, int>> GetCoffeePrices()
    {
      return Task.FromResult(_dataStore.coffeePrices);
    }
    public Task<Dictionary<int, int>> GetCoinInventory()
    {
      return Task.FromResult(_dataStore.coinInventory);
    }
    public Task UpdateCoffeeQuantities(string coffee, int quantity)
    {
      _dataStore.coffeeQuantities[coffee] -= quantity;
      return Task.CompletedTask;
    }
    public Task UpdateCoinInventory(int denomination, int count)
    {
      _dataStore.coinInventory[denomination] -= count;
      return Task.CompletedTask;
    }
  }
}
