namespace ExamTwo.Application.UseCases
{
  public interface ICoffeeMachineQuery
  {
    Task<Dictionary<string, int>> GetCoffeesAsync();
    Task<Dictionary<string, int>> GetCoffeePricesAsync();
    Task<Dictionary<int, int>> GetChangeAsync();
  }
}
