using ExamTwo.Application.Ports;
using ExamTwo.Domain.Entities;
using ExamTwo.Infrastructure.Data;

namespace ExamTwo.Infrastructure
{
  public class CoffeeMachineRepository : ICoffeeMachineRepository
  {
    CoffeeMachineDataStore _dataStore;
    public CoffeeMachineRepository(CoffeeMachineDataStore dataStore)
    {
      _dataStore = dataStore;
    }
    public Task<Dictionary<string, CoffeeData>> GetCoffees()
    {
      return Task.FromResult(_dataStore.coffees);
    }
    public Task<Dictionary<int, int>> GetCoinInventory()
    {
      return Task.FromResult(_dataStore.coinInventory);
    }
    public Task UpdateCoffeeQuantities(string coffee, int quantity)
    {
      _dataStore.coffees[coffee].Quantity -= quantity;
      return Task.CompletedTask;
    }
    public Task UpdateCoinInventory(int denomination, int count)
    {
      _dataStore.coinInventory[denomination] -= count;
      return Task.CompletedTask;
    }
  }
}
