using ExamTwo.Domain.DTOs;

namespace ExamTwo.Application.UseCases
{
  public interface ICoffeeMachineCommand
  {
    Task<string> BuyCoffeeAsync(OrderRequest request);
  }
}
