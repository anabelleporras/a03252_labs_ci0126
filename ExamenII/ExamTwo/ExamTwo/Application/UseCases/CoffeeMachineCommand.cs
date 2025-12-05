using ExamTwo.Application.Ports;
using ExamTwo.Domain;
using ExamTwo.Infrastructure;

namespace ExamTwo.Application.UseCases
{
  public class CoffeeMachineCommand : ICoffeeMachineCommand
  {
    private readonly ICoffeeMachineRepository _repo;
    public CoffeeMachineCommand(ICoffeeMachineRepository repo)
    {
      _repo = repo;
    }
    public async Task<string> BuyCoffeeAsync(OrderRequest request)
    {
      if (request.Order == null || request.Order.Count == 0)
        throw new ArgumentException("Orden vacia.");

      if (request.Payment.TotalAmount <= 0)
        throw new ArgumentException("Dinero insuficiente");

      try
      {
        var availableCoffees = await _repo.GetCoffees();
        var coffeePrices = await _repo.GetCoffeePrices();
        var availableCoins = await _repo.GetCoinInventory();

        foreach (var coffee in request.Order)
        {
          if (!availableCoffees.ContainsKey(coffee.Key))
            throw new ArgumentException($"Café {coffee.Key} no está disponible.");

          if (coffee.Value > availableCoffees[coffee.Key])
            throw new ArgumentException($"No hay suficientes {coffee.Key} en la máquina.");
        }

        var costoTotal = request.Order.Sum(o => coffeePrices[o.Key] * o.Value);

        if (request.Payment.TotalAmount < costoTotal)
          throw new ArgumentException("Dinero ingresado es insuficiente");

        var change = request.Payment.TotalAmount - costoTotal;

        var usedCoins = new Dictionary<int, int>();
        string coinBreakdown = "";
        foreach (var coin in availableCoins.OrderByDescending(c => c.Key))
        {
          var count = Math.Min(change / coin.Key, coin.Value);
          if (count > 0)
          {
            coinBreakdown += $" {count} moneda de {coin.Key},  ";
            usedCoins[coin.Key] = count;
            change -= coin.Key * count;
          }
        }

        if (change > 0)
          throw new ArgumentException("No hay suficiente cambio en la máquina.");

        foreach (var coffee in request.Order)
        {
          await _repo.UpdateCoffeeQuantities(coffee.Key, coffee.Value);
        }

        foreach (var coin in usedCoins)
        {
          await _repo.UpdateCoinInventory(coin.Key, coin.Value);
        }

        string result = $"Su vuelto es de: {request.Payment.TotalAmount - costoTotal} colones. Desglose:{coinBreakdown}";

        return result;
      }
      catch (ArgumentException ex)
      {
        return ex.Message;
      }
    }
  }
}
