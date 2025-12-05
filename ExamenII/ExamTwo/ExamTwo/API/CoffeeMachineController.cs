using ExamTwo.Infrastructure;
using ExamTwo.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExamTwo.Application.UseCases;

namespace ExamTwo.Controllers
{
  public class CoffeeMachineController : Controller
  {
    private readonly ICoffeeMachineQuery _query;
    private readonly ICoffeeMachineCommand _command;

    public CoffeeMachineController(ICoffeeMachineQuery query, ICoffeeMachineCommand command)
    {
      _query = query;
      _command = command;
    }

    [HttpGet("Coffee")]
    public ActionResult<Dictionary<string, int>> GetCoffeePrices()
    {
      return Ok(_query.GetCoffeesAsync());
    }

    [HttpGet("Price")]
    public ActionResult<Dictionary<string, int>> GetCoffeePricesInCents()
    {
      return Ok(_query.GetCoffeePricesAsync());
    }

    [HttpGet("Change")]
    public ActionResult<Dictionary<string, int>> GetQuantity()
    {
      return Ok(_query.GetChangeAsync());
    }

    [HttpPost("Coffee")]
    public ActionResult<string> BuyCoffee([FromBody] OrderRequest request)
    {
      try
      {
        var result = _command.BuyCoffeeAsync(request);
        return Ok(result);
      }
      catch (System.Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
