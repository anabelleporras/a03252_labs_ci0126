namespace ExamTwo.Domain
{
  public class CoffeeData
  {
    public int Price { get; set; }
    public int Quantity { get; set; }
    public CoffeeData(int price, int quantity)
    {
      Price = price;
      Quantity = quantity;
    }
  }
}

