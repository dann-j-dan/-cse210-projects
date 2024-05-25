using System;
using System.Collections.Generic;
using System.IO;

public abstract class Goal
{
    public string Name { get; set; }
    public int Points { get; protected set; }
    public bool IsComplete { get; protected set; }

    public Goal(string name, int points)
    {
        Name = name;
        Points = points;
        IsComplete = false;
    }

    public abstract void RecordEvent();
    public abstract string GetGoalStatus();
    public abstract string Serialize();
    public static Goal Deserialize(string data);
}

public class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points) : base(name, points) { }

    public override void RecordEvent()
    {
        IsComplete = true;
        Points += 1000;
    }

    public override string GetGoalStatus()
    {
        return IsComplete ? "[X] " + Name : "[ ] " + Name;
    }

    public override string Serialize()
    {
        return $"SimpleGoal:{Name},{Points},{IsComplete}";
    }

    public static SimpleGoal Deserialize(string data)
    {
        var parts = data.Split(',');
        var goal = new SimpleGoal(parts[0], int.Parse(parts[1]));
        goal.IsComplete = bool.Parse(parts[2]);
        return goal;
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, int points) : base(name, points) { }

    public override void RecordEvent()
    {
        Points += 100;
    }

    public override string GetGoalStatus()
    {
        return "[ ] " + Name + " (eternal)";
    }

    public override string Serialize()
    {
        return $"EternalGoal:{Name},{Points}";
    }

    public static EternalGoal Deserialize(string data)
    {
        var parts = data.Split(',');
        return new EternalGoal(parts[0], int.Parse(parts[1]));
    }
}

public class ChecklistGoal : Goal
{
    public int TargetCount { get; set; }
    public int CurrentCount { get; set; }

    public ChecklistGoal(string name, int points, int targetCount) : base(name, points)
    {
        TargetCount = targetCount;
        CurrentCount = 0;
    }

    public override void RecordEvent()
    {
        if (CurrentCount < TargetCount)
        {
            CurrentCount++;
            Points += 50;
            if (CurrentCount == TargetCount)
            {
                Points += 500;
                IsComplete = true;
            }
        }
    }

    public override string GetGoalStatus()
    {
        return IsComplete ? "[X] " + Name + $" (Completed {CurrentCount}/{TargetCount} times)" 
                          : "[ ] " + Name + $" (Completed {CurrentCount}/{TargetCount} times)";
    }

    public override string Serialize()
    {
        return $"ChecklistGoal:{Name},{Points},{CurrentCount},{TargetCount},{IsComplete}";
    }

    public static ChecklistGoal Deserialize(string data)
    {
        var parts = data.Split(',');
        var goal = new ChecklistGoal(parts[0], int.Parse(parts[1]), int.Parse(parts[3]));
        goal.CurrentCount = int.Parse(parts[2]);
        goal.IsComplete = bool.Parse(parts[4]);
        return goal;
    }
}

public class EternalQuestProgram
{
    private List<Goal> goals = new List<Goal>();
    private int totalScore = 0;

    public void Run()
    {
        LoadGoals();
        while (true)
        {
            DisplayMenu();
            string choice = Console.ReadLine();
            if (choice == "1") CreateGoal();
            else if (choice == "2") RecordEvent();
            else if (choice == "3") ListGoals();
            else if (choice == "4") SaveGoals();
            else if (choice == "5") break;
            Console.Clear();
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("Eternal Quest Program");
        Console.WriteLine("1. Create Goal");
        Console.WriteLine("2. Record Event");
        Console.WriteLine("3. List Goals");
        Console.WriteLine("4. Save Goals");
        Console.WriteLine("5. Quit");
        Console.Write("Select an option: ");
    }

    private void CreateGoal()
    {
        Console.WriteLine("Enter goal type (Simple, Eternal, Checklist): ");
        string type = Console.ReadLine().ToLower();
        Console.WriteLine("Enter goal name: ");
        string name = Console.ReadLine();
        Console.WriteLine("Enter goal points: ");
        int points = int.Parse(Console.ReadLine());

        if (type == "simple")
        {
            goals.Add(new SimpleGoal(name, points));
        }
        else if (type == "eternal")
        {
            goals.Add(new EternalGoal(name, points));
        }
        else if (type == "checklist")
        {
            Console.WriteLine("Enter target count: ");
            int targetCount = int.Parse(Console.ReadLine());
            goals.Add(new ChecklistGoal(name, points, targetCount));
        }
    }

    private void RecordEvent()
    {
        ListGoals();
        Console.WriteLine("Enter goal number to record: ");
        int index = int.Parse(Console.ReadLine()) - 1;
        if (index >= 0 && index < goals.Count)
        {
            goals[index].RecordEvent();
            totalScore += goals[index].Points;
        }
    }

    private void ListGoals()
    {
        Console.WriteLine("Goals:");
        for (int i = 0; i < goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {goals[i].GetGoalStatus()}");
        }
        Console.WriteLine($"Total Score: {totalScore}");
    }

    private void SaveGoals()
    {
        using (StreamWriter outputFile = new StreamWriter("goals.txt"))
        {
            foreach (var goal in goals)
            {
                outputFile.WriteLine(goal.Serialize());
            }
            outputFile.WriteLine(totalScore);
        }
    }

    private void LoadGoals()
    {
        if (File.Exists("goals.txt"))
        {
            string[] lines = File.ReadAllLines("goals.txt");
            foreach (string line in lines)
            {
                if (int.TryParse(line, out int score))
                {
                    totalScore = score;
                }
                else
                {
                    goals.Add(Goal.Deserialize(line));
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        EternalQuestProgram program = new EternalQuestProgram();
        program.Run();
    }
}
