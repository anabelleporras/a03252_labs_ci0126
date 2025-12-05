using ExamTwo.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExamTwo.Application.UseCases;
using ExamTwo.Domain.Entities;
using ExamTwo.Domain.DTOs;

namespace ExamTwo.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CoffeeMachineController : Controller
  {
    private readonly ICoffeeMachineQuery _query;
    private readonly ICoffeeMachineCommand _command;

    public CoffeeMachineController(ICoffeeMachineQuery query, ICoffeeMachineCommand command)
    {
      _query = query;
      _command = command;
    }

    [HttpGet()]
    public ActionResult<Dictionary<string, CoffeeData>> GetCoffees()
    {
      return Ok(_query.GetCoffees());
    }

    [HttpPost()]
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
