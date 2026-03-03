namespace Company.Project.Data.Entities;

public class Account
{
    public int Id { get; set; }

    public int AccountId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public ICollection<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();
}
