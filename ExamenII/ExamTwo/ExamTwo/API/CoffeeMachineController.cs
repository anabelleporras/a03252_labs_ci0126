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
    public ActionResult<Dictionary<string, int>> GetCoffees()
    {
      return Ok(_query.GetCoffees());
    }

    [HttpGet("Price")]
    public ActionResult<Dictionary<string, int>> GetCoffeePrices()
    {
      return Ok(_query.GetCoffeePrices());
    }

    [HttpGet("Change")]
    public ActionResult<Dictionary<string, int>> GetChange()
    {
      return Ok(_query.GetCoinInventory());
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
