using ExamTwo.Domain.Entities;

namespace ExamTwo.Application.Ports
{
  public interface ICoffeeMachineRepository
  {
    Task<Dictionary<string, CoffeeData>> GetCoffees();
    Task<Dictionary<int, int>> GetCoinInventory();
    Task UpdateCoffeeQuantities(string coffee, int quantity);
    Task UpdateCoinInventory(int denomination, int count);
  }
}
