using ExamTwo.Domain;

namespace ExamTwo.Application.UseCases
{
  public interface ICoffeeMachineCommand
  {
    Task<string> BuyCoffeeAsync(OrderRequest request);
  }
}
