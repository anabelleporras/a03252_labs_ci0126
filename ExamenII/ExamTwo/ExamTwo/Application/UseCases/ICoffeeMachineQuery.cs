namespace ExamTwo.Application.UseCases
{
  public interface ICoffeeMachineQuery
  {
    Task<Dictionary<string, int>> GetCoffees();
    Task<Dictionary<string, int>> GetCoffeePrices();
    Task<Dictionary<int, int>> GetCoinInventory();
  }
}
