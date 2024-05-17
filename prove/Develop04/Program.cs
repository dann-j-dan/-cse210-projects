using System;
using System.Threading;

// Base class for all activities
public class Activity
{
    protected int duration; // Duration of the activity in seconds

    // Constructor
    public Activity(int duration)
    {
        this.duration = duration;
    }

    // Method to display starting message and set duration
    public virtual void Start()
    {
        // Display starting message and set duration
        Console.WriteLine("Starting {0} activity...", GetType().Name);
        Console.WriteLine("Set duration: {0} seconds", duration);
        Console.WriteLine("Prepare to begin...");
        Thread.Sleep(3000); // Pause for 3 seconds
    }

    // Method to display ending message
    public virtual void End()
    {
        // Display ending message
        Console.WriteLine("Congratulations! You've completed the {0} activity.", GetType().Name);
        Console.WriteLine("Activity duration: {0} seconds", duration);
        Thread.Sleep(3000); // Pause for 3 seconds
    }
}

// Breathing activity class
public class BreathingActivity : Activity
{
    // Constructor
    public BreathingActivity(int duration) : base(duration) { }

    // Override Start method
    public override void Start()
    {
        base.Start();
        Console.WriteLine("This activity will help you relax by guiding you through deep breathing.");
        Console.WriteLine("Clear your mind and focus on your breathing.");
        // Implement breathing logic
    }
}

// Reflection activity class
public class ReflectionActivity : Activity
{
    // Constructor
    public ReflectionActivity(int duration) : base(duration) { }

    // Override Start method
    public override void Start()
    {
        base.Start();
        Console.WriteLine("This activity will help you reflect on times when you have shown strength and resilience.");
        Console.WriteLine("Reflect on the following prompt:");
        // Implement reflection logic
    }
}

// Listing activity class
public class ListingActivity : Activity
{
    // Constructor
    public ListingActivity(int duration) : base(duration) { }

    // Override Start method
    public override void Start()
    {
        base.Start();
        Console.WriteLine("This activity will help you reflect on the good things in your life by listing them.");
        Console.WriteLine("Prompt: ");
        // Implement listing logic
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Sample usage
        Activity activity = new BreathingActivity(300); // 5 minutes breathing activity
        activity.Start();
        // Perform activity logic
        activity.End();
    }
}
