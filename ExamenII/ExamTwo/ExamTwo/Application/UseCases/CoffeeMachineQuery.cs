using ExamTwo.Application.Ports;
using ExamTwo.Domain;
using ExamTwo.Infrastructure;

namespace ExamTwo.Application.UseCases
{
  public class CoffeeMachineQuery : ICoffeeMachineQuery
  {
    private readonly ICoffeeMachineRepository _repo;
    public CoffeeMachineQuery(ICoffeeMachineRepository repo)
    {
      _repo = repo;
    }
    public Task<Dictionary<string, CoffeeData>> GetCoffees()
    {
      return _repo.GetCoffees();
    }
    public Task<Dictionary<int, int>> GetCoinInventory()
    {
      return _repo.GetCoinInventory();
    }
  }
}
