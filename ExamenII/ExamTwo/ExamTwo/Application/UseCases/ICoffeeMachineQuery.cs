using ExamTwo.Domain;

namespace ExamTwo.Application.UseCases
{
  public interface ICoffeeMachineQuery
  {
    Task<Dictionary<string, CoffeeData>> GetCoffees();
    Task<Dictionary<int, int>> GetCoinInventory();
  }
}
