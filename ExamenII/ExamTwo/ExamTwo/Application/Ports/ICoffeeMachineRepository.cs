using ExamTwo.Domain;

namespace ExamTwo.Application.Ports
{
  public interface ICoffeeMachineRepository
  {
    Task<Dictionary<string, int>> GetCoffees();
    Task<Dictionary<string, int>> GetCoffeePrices();
    Task<Dictionary<int, int>> GetCoinInventory();
    Task UpdateCoffeeQuantities(string coffee, int quantity);
    Task UpdateCoinInventory(int denomination, int count);
  }
}
