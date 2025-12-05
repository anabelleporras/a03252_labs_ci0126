using ExamTwo.Application.Ports;
using ExamTwo.Application.UseCases;
using ExamTwo.Domain.DTOs;
using ExamTwo.Domain.Entities;
using Moq;

namespace Tests
{
  public class CoffeeMachineCommand_Test
  {
    private Mock<ICoffeeMachineRepository> _repoMock;
    private CoffeeMachineCommand _sut;

    [SetUp]
    public void Setup()
    {
      _repoMock = new Mock<ICoffeeMachineRepository>();
      _sut = new CoffeeMachineCommand(_repoMock.Object);
    }

    private OrderRequest BuildOrderRequest(Dictionary<string, int> order, int totalAmount)
    {
      return new OrderRequest
      {
        Order = order,
        Payment = new Payment { TotalAmount = totalAmount }
      };
    }

    private Dictionary<string, CoffeeData> GetCoffees()
    {
      return new Dictionary<string, CoffeeData>
      {
        { "Americano", new CoffeeData(950, 10) },
        { "Cappuccino", new CoffeeData(1200, 8) },
      };
    }

    private Dictionary<int, int> GetCoinInventory()
    {
      return new Dictionary<int, int>
      {
        { 500, 10 },
        { 100, 10 },
        { 50, 10 },
        { 25, 10 }
      };
    }

    [Test]
    public void BuyCoffeeAsync_EmptyOrder_ThrowsArgumentException()
    {
      // Arrange
      var request = new OrderRequest
      {
        Order = new Dictionary<string, int>(),
        Payment = new Payment { TotalAmount = 1000 }
      };

      // Act & Assert
      var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.BuyCoffeeAsync(request));

      Assert.That(ex!.Message, Is.EqualTo("Orden vacia."));
    }

    [Test]
    public void BuyCoffeeAsync_PaymentLessOrEqualZero_ThrowsArgumentException()
    {
      // Arrange
      var request = BuildOrderRequest(
        new Dictionary<string, int> { { "Americano", 1 } },
        totalAmount: 0);

      // Act & Assert
      var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.BuyCoffeeAsync(request));

      Assert.That(ex!.Message, Is.EqualTo("Dinero insuficiente"));
    }

    [Test]
    public async Task BuyCoffeeAsync_CoffeeNotAvailable_ReturnsErrorMessage()
    {
      // Arrange
      var coffees = GetCoffees();
      var coins = GetCoinInventory();

      _repoMock.Setup(r => r.GetCoffees()).ReturnsAsync(coffees);

      _repoMock.Setup(r => r.GetCoinInventory()).ReturnsAsync(coins);

      var request = BuildOrderRequest(new Dictionary<string, int> { { "Latte", 1 } }, totalAmount: 2000);

      // Act
      var result = await _sut.BuyCoffeeAsync(request);

      // Assert
      Assert.That(result, Is.EqualTo("Café Latte no está disponible."));
    }

    [Test]
    public async Task BuyCoffeeAsync_QuantityNotAvailable_ReturnsErrorMessage()
    {
      // Arrange
      var coffees = new Dictionary<string, CoffeeData>
      {
        { "Americano", new CoffeeData (950, 1) }
      };
      var coins = GetCoinInventory();

      _repoMock.Setup(r => r.GetCoffees()).ReturnsAsync(coffees);

      _repoMock.Setup(r => r.GetCoinInventory()).ReturnsAsync(coins);

      var request = BuildOrderRequest(new Dictionary<string, int> { { "Americano", 2 } }, totalAmount: 5000);

      // Act
      var result = await _sut.BuyCoffeeAsync(request);

      // Assert
      Assert.That(result, Is.EqualTo("No hay suficientes Americano en la máquina."));
    }

    [Test]
    public async Task BuyCoffeeAsync_PaymentLessThanTotalCost_ReturnsErrorMessage()
    {
      // Arrange
      var coffees = GetCoffees();
      var coins = GetCoinInventory();

      _repoMock.Setup(r => r.GetCoffees()).ReturnsAsync(coffees);

      _repoMock.Setup(r => r.GetCoinInventory()).ReturnsAsync(coins);

      var request = BuildOrderRequest(new Dictionary<string, int> { { "Americano", 1 } }, totalAmount: 500);

      // Act
      var result = await _sut.BuyCoffeeAsync(request);

      // Assert
      Assert.That(result, Is.EqualTo("Dinero ingresado es insuficiente"));
    }

    [Test]
    public async Task BuyCoffeeAsync_NotEnoughChange_ReturnsErrorMessage()
    {
      // Arrange
      var coffees = GetCoffees();

      var coins = new Dictionary<int, int>
      {
        { 500, 0 },
        { 100, 0 },
        { 50, 0 }
      };

      _repoMock.Setup(r => r.GetCoffees()).ReturnsAsync(coffees);

      _repoMock.Setup(r => r.GetCoinInventory()).ReturnsAsync(coins);

      var request = BuildOrderRequest(new Dictionary<string, int> { { "Americano", 1 } }, totalAmount: 1000);

      // Act
      var result = await _sut.BuyCoffeeAsync(request);

      // Assert
      Assert.That(result, Is.EqualTo("No hay suficiente cambio en la máquina."));
    }

    [Test]
    public async Task BuyCoffeeAsync_ValidOrderPaymentAndChangeSuccess()
    {
      // Arrange
      var coffees = GetCoffees();
      var coins = GetCoinInventory();

      _repoMock.Setup(r => r.GetCoffees()).ReturnsAsync(coffees);

      _repoMock.Setup(r => r.GetCoinInventory()).ReturnsAsync(coins);

      _repoMock.Setup(r => r.UpdateCoffeeQuantities(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.CompletedTask);

      _repoMock.Setup(r => r.UpdateCoinInventory(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

      var request = BuildOrderRequest(new Dictionary<string, int> { { "Americano", 1 } }, totalAmount: 1000);

      // Act
      var result = await _sut.BuyCoffeeAsync(request);

      // Assert
      Assert.That(result, Does.Contain("Su vuelto es de: 50 colones"));
      Assert.That(result, Does.Contain("1 moneda de 50"));

      _repoMock.Verify(r => r.UpdateCoffeeQuantities("Americano", 1), Times.Once);
      _repoMock.Verify(r => r.UpdateCoinInventory(50, 1), Times.Once);
    }

  } // end class
}
