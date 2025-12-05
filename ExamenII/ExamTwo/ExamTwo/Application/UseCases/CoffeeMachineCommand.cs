using ExamTwo.Domain;
using ExamTwo.Infrastructure;

namespace ExamTwo.Application.UseCases
{
  public class CoffeeMachineCommand : ICoffeeMachineCommand
  {
    private readonly CoffeeMachineDataStore _db;
    public CoffeeMachineCommand(CoffeeMachineDataStore db)
    {
      _db = db;
    }
    public Task<string> BuyCoffeeAsync(OrderRequest request)
    {
      if (request.Order == null || request.Order.Count == 0)
        return Task.FromResult("Orden vacia.");

      if (request.Payment.TotalAmount <= 0)
        return Task.FromResult("Dinero insuficiente ");

      try
      {
        var costoTotal = request.Order.Sum(o => _db.coffeePrices.First(c => c.Key == o.Key).Value * o.Value);

        if (request.Payment.TotalAmount < costoTotal)
        {
          return Task.FromResult("Dinero insuficiente ");
        }

        foreach (var cafe in request.Order)
        {
          var selected = _db.coffeeQuantities.First(c => c.Key == cafe.Key).Key;
          if (cafe.Value > _db.coffeeQuantities[selected])
          {
            return Task.FromResult($"No hay suficientes {selected} en la máquina.");
          }
          _db.coffeeQuantities[selected] -= cafe.Value;
        }

        var change = request.Payment.TotalAmount - costoTotal;
        String result = $"Su vuelto es de: {change} colones. Desglose:";

        foreach (var coin in _db.change.Keys.OrderByDescending(c => c))
        {
          var count = Math.Min(change / coin, _db.change[coin]);
          if (count > 0)
          {
            result += $" {count} moneda de {coin},  ";
            change -= coin * count;
          }
        }

        if (change > 0)
        {
          return Task.FromResult("No hay suficiente cambio en la máquina.");
        }

        return Task.FromResult(result);
      }
      catch (ArgumentException ex)
      {
        return Task.FromResult(ex.Message);
      }
    }
  }
}
