using System;

public class WorkSession
{
    public string Client { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal HourlyRate { get; set; }

    public TimeSpan Duration => End - Start;
    public decimal AmountOwed => (decimal)Duration.TotalHours * HourlyRate;

    public override string ToString() =>
        $"{Start},{End},{HourlyRate}";

    public static WorkSession FromString(string client, string line)
    {
        var parts = line.Split(',');
        return new WorkSession
        {
            Client = client,
            Start = DateTime.Parse(parts[0]),
            End = DateTime.Parse(parts[1]),
            HourlyRate = decimal.Parse(parts[2])
        };
    }
}
