namespace ExamTwo.Application.Ports
{
  public interface ICoffeeMachineRepository
  {
    Task GetCoffees();
    Task GetCoffeePrices();
    Task GetChange();
    Task BuyCoffee();
  }
}
