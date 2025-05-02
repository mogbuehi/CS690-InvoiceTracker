using System;
using System.Collections.Generic;
using System.IO;

List<WorkSession> sessions = new();

Console.Write("Enter client name: ");
string client = Console.ReadLine();
string fileName = client + ".txt";

// Load sessions if they exist
if (File.Exists(fileName))
{
    foreach (var line in File.ReadAllLines(fileName))
        sessions.Add(WorkSession.FromString(client, line));
}

// Enter new session
Console.Write("Enter start time (yyyy-MM-dd HH:mm): ");
DateTime start = DateTime.Parse(Console.ReadLine());

Console.Write("Enter end time (yyyy-MM-dd HH:mm): ");
DateTime end = DateTime.Parse(Console.ReadLine());

Console.Write("Enter hourly rate: ");
decimal rate = decimal.Parse(Console.ReadLine());

var session = new WorkSession
{
    Client = client,
    Start = start,
    End = end,
    HourlyRate = rate
};

sessions.Add(session);

// Save all sessions
File.WriteAllLines(fileName, sessions.ConvertAll(s => s.ToString()));

Console.WriteLine($"\nSaved! Duration: {session.Duration.TotalHours:F2} hrs | Owed: ${session.AmountOwed:F2}");
