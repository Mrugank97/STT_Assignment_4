using System;
using System.Threading;

class AlarmSystem
{
    public delegate void AlarmEventHandler();
    public static event AlarmEventHandler raiseAlarm;

    static void Ring_alarm()
    {
        Console.WriteLine("Alarm ringing! Time matched.");
    }

    static void Main()
    {
        Console.Write("Enter time (HH:MM:SS): ");
        string input = Console.ReadLine();
        DateTime targetTime = DateTime.Parse(input);

        raiseAlarm += Ring_alarm;

        Console.WriteLine("Waiting for the alarm...");

        while (true)
        {
            if (DateTime.Now.ToString("HH:mm:ss") == targetTime.ToString("HH:mm:ss"))
            {
                raiseAlarm?.Invoke();
                break;
            }
            Thread.Sleep(1000);
        }
    }
}
