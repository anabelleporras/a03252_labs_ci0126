namespace ExamTwo.Domain.DTOs
{
  public class Payment
  {
    public int TotalAmount { get; set; }
    public List<int> Bills { get; set; }
    public Dictionary<int, int> Coins { get; set; }
  }
}
