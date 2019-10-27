using System;

namespace TalkingClock
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var clock = args.Length == 0 ? new Clock() : Clock.Parse(args[0]);
                Console.WriteLine(clock);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
