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

bool running = true;

while (running)
{
    Console.WriteLine("Choose one of the three modes by typing it below");
    Console.WriteLine("Choose a mode: log | view | exit");
    string mode = Console.ReadLine().Trim().ToLower();

    if (mode == "log")
    {
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
        File.WriteAllLines(fileName, sessions.ConvertAll(s => s.ToString()));
        Console.WriteLine($"\nSaved. Duration: {session.Duration.TotalHours:F2} hrs | Owed: ${session.AmountOwed:F2}");
    }
    else if (mode == "view")
    {
        Console.WriteLine($"\nSessions for {client}:");

        decimal total = 0;
        foreach (var s in sessions)
        {
            Console.WriteLine($"{s.Start} → {s.End} | {s.Duration.TotalHours:F2} hrs | Rate: ${s.HourlyRate} | Owed: ${s.AmountOwed:F2}");
            total += s.AmountOwed;
        }

        Console.WriteLine($"\nTotal owed: ${total:F2}");

        // Ask if user wants to generate invoice
        Console.Write("\nGenerate invoice file? (Y/N): ");
        string answer = Console.ReadLine().Trim().ToUpper();

        if (answer == "Y")
        {
            string invoiceFile = client + "_invoice.txt";
            using (StreamWriter writer = new StreamWriter(invoiceFile))
            {
                writer.WriteLine($"Invoice for {client}");
                writer.WriteLine("------------------------");

                foreach (var s in sessions)
                {
                    writer.WriteLine($"{s.Start} → {s.End} | {s.Duration.TotalHours:F2} hrs | Rate: ${s.HourlyRate} | Owed: ${s.AmountOwed:F2}");
                }

                writer.WriteLine("\n------------------------");
                writer.WriteLine($"Total owed: ${total:F2}");
            }

            Console.WriteLine($"Invoice saved to {invoiceFile}");
        }
        else
        {
            Console.WriteLine("Invoice not generated.");
        }
    }
    else if (mode == "exit")
    {
        running = false;
        Console.WriteLine("Goodbye!");
    }
    else
    {
        Console.WriteLine("Invalid choice. Try: log, view, or exit. Be sure to type your choice correctly and press Enter");
    }
}
