using DistributionCycleRunner;
using System;

namespace DistributionConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();

            DistributionEventOutcomeInput[] _events = new DistributionEventOutcomeInput[]
            {
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,10,10,10,10,10,10,10,10 } },
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,20,10,10,10,10,10,10,10 } },
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,30,10,10,10,10,10,10,10 } },
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,40,10,10,10,10,10,10,10 } },
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,50,10,10,10,10,10,10,10 } },
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,60,10,10,10,10,10,10,10 } },
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,70,10,10,10,10,10,10,10 } },
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,50,10,10,10,10,10,10,10 } },
                new DistributionEventOutcomeInput { Weights = new[] {10.0,10,10,10,10,10,10,10,10,10 } }
            };

            s.Start();
            DistributionEventOutcomeOutput[] temp = DistributionCycleRun.ComputeNumOfCombinations(_events);
            s.Stop();

            for(int i = 0; i < temp.Length; i++)
            {
                Console.Write(temp[i].CorrectEventsCategory);
                Console.Write("\t");
                Console.Write(temp[i].Probability);
                Console.Write("\t");
                Console.Write(temp[i].Hits);
                Console.Write("\n");
            }

            
            TimeSpan timeSpan = s.Elapsed;
            //Console.WriteLine("Time: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            Console.WriteLine("Time in seconds: {0}s", timeSpan.Seconds);

            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }
    }

}
