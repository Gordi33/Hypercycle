using System;

namespace DistributionCycleRunner
{
    public static class DistributionCycleRun
    {
        public static DistributionEventOutcomeOutput[] ComputeNumOfCombinations(DistributionEventOutcomeInput[] events)
        {
            int numEvents = events.Length;
            int numOutcomes = events[0].Weights.Length;
            int[] combination = (true) ? InitializeFieldWithZeros(numEvents) : null;
            long cycleLength = (long)((true) ? Math.Pow(numOutcomes, numEvents) : 0);
            double probabilityOfCombination = 1;
            int hitsOfCombination = 0;
            bool cycleSmallerThan10bil = (cycleLength <= 10000000000);
            long combinationsIndex = 0;
            
            DistributionEventOutcomeOutput[] resultDistribution = InitializeArray<DistributionEventOutcomeOutput>(numEvents + 1);

            for(int i = 0; i < resultDistribution.Length; i++)
                resultDistribution[i].CorrectEventsCategory = i;

            var probabilitiesNormalised = NormaliseWeights(events);
            var maxOutcomeIndex = GetMaximumOutcomeOfEachEvent(probabilitiesNormalised);

            if (cycleSmallerThan10bil) // max 10 billion
            {
                do
                {
                    for (int currentEvent = 0; currentEvent < numEvents; currentEvent++)
                    {
                        probabilityOfCombination *= probabilitiesNormalised[currentEvent].Weights[combination[currentEvent]];

                        if( combination[currentEvent] == maxOutcomeIndex[currentEvent])
                            hitsOfCombination += 1;
                    }
                    resultDistribution[hitsOfCombination].Probability += probabilityOfCombination;
                    resultDistribution[hitsOfCombination].Hits += 1;
                    
                    probabilityOfCombination = 1;
                    hitsOfCombination = 0;

                    combination = ProcessNextCombination(numEvents, combination, 0, numOutcomes);
                    combinationsIndex++;
                } while (combinationsIndex < cycleLength);
            }

            return resultDistribution;
        }

        public static T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }
        
        public static DistributionEventOutcomeInput[] NormaliseWeights(DistributionEventOutcomeInput[] events)
        {
            double sum = 0.0;
            int numEvents = events.Length;
            int numOutcomes = events[0].Weights.Length;

            for (int i = 0; i < numEvents; i++)
                for (int j = 0; j < numOutcomes; j++)
                {
                    if(j == 0)
                    {
                        sum = 0;
                    }
                    
                    sum += events[i].Weights[j];

                    if(j == numOutcomes - 1)
                    {
                        for (int k = 0; k < numOutcomes; k++)
                        {
                            events[i].Weights[k] = events[i].Weights[k] / sum;
                        }
                    }
                }
            return events;
        }

        public static void PrintWeights(double[,] weights, int numEvents, int numOutcomes)
        {
            for (int i = 0; i < numEvents; i++)
                for (int j = 0; j < numOutcomes; j++)
                    Console.Write(Math.Round(100*weights[i, j], 2) + ((j == numOutcomes - 1) ? "\n" : "\t"));
        }

        public static double[] GetMaximumOutcomeOfEachEvent(DistributionEventOutcomeInput[] events)
        {
            int numEvents = events.Length;
            int numOutcomes = events[0].Weights.Length;
            double[] maxLiklihoodOutcomeOfEachEvent = new double[numEvents];
            double[] maxLiklihoodOutcomeOfEachEventIndex = new double[numEvents];

            for (int j = 0; j < numEvents; j++)
                maxLiklihoodOutcomeOfEachEvent[j] = 0;

            for (int i = 0; i < numEvents; i++)
                for (int j = 0; j < numOutcomes; j++)
                {
                    // max_liklihood_outcome_of_each_event[i] = Math.Max(max_liklihood_outcome_of_each_event[i], weights[i, j]);
                    if(maxLiklihoodOutcomeOfEachEvent[i] < events[i].Weights[j])
                    {
                        maxLiklihoodOutcomeOfEachEvent[i] = events[i].Weights[j];
                        maxLiklihoodOutcomeOfEachEventIndex[i] = j;
                    }                    
                }
                    
            return maxLiklihoodOutcomeOfEachEventIndex;   
        }

        private static int[] ProcessNextCombination(int numOfElements, int[] field, int level, int modulo)
        {
            field[level] = (field[level] + 1) % modulo;
            if (field[level] == 0 && level + 1 < numOfElements)
                ProcessNextCombination(numOfElements, field, level + 1, modulo);

            return field;
        }

        private static int[] InitializeFieldWithZeros(int numOfEvents)
        {
            int[] combination = new int[numOfEvents];

            for (int i = 0; i < numOfEvents; i++)
            {
                combination[i] = 0;
            }

            return combination;
        }
    }
}












