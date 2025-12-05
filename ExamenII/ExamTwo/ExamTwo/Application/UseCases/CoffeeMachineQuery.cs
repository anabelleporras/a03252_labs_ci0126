using ExamTwo.Application.Ports;
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
    public Task<Dictionary<string, int>> GetCoffees()
    {
      return _repo.GetCoffees();
    }
    public Task<Dictionary<string, int>> GetCoffeePrices()
    {
      return _repo.GetCoffeePrices();
    }
    public Task<Dictionary<int, int>> GetCoinInventory()
    {
      return _repo.GetCoinInventory();
    }
  }
}
