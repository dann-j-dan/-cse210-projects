using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Exit");
            Console.WriteLine("Enter your choice:");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    journal.WriteNewEntry();
                    break;
                case 2:
                    journal.DisplayJournal();
                    break;
                case 3:
                    journal.SaveJournal();
                    break;
                case 4:
                    journal.LoadJournal();
                    break;
                case 5:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}

public class Journal
{
    private List<Entry> entries;

    public Journal()
    {
        entries = new List<Entry>();
    }

    public void WriteNewEntry()
    {
        // Logic to generate a random prompt and get user response
        // Then create a new Entry object and add it to the journal
        Entry entry = new Entry();
        entries.Add(entry);
    }

    public void DisplayJournal()
    {
        foreach (Entry entry in entries)
        {
            Console.WriteLine(entry.ToString());
        }
    }

    public void SaveJournal()
    {
        Console.WriteLine("Enter filename to save:");
        string filename = Console.ReadLine();

        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (Entry entry in entries)
            {
                writer.WriteLine(entry.ToString());
            }
        }

        Console.WriteLine("Journal saved successfully.");
    }

    public void LoadJournal()
    {
        Console.WriteLine("Enter filename to load:");
        string filename = Console.ReadLine();

        entries.Clear();

        using (StreamReader reader = new StreamReader(filename))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // Parse each line and create Entry objects
                // Then add them to the journal
                Entry entry = new Entry();
                // Populate entry from line...
                entries.Add(entry);
            }
        }

        Console.WriteLine("Journal loaded successfully.");
    }
}

public class Entry
{
    public string Prompt { get; set; }
    public string Response { get; set; }
    public DateTime Date { get; set; }

    public Entry()
    {
        // Generate a random prompt and set the date
        Random random = new Random();
        List<string> prompts = new List<string>
        {
            // Add your list of prompts here
        };
        Prompt = prompts[random.Next(prompts.Count)];
        Response = ""; // Allow the user to input their response
        Date = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Date}: {Prompt}\n{Response}\n";
    }
}
