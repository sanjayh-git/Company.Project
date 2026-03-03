namespace Company.Project.Data.Entities;

public class MeterReading
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public DateTime ReadingDateTime { get; set; }
    public string ReadingValue { get; set; } = string.Empty;
}
