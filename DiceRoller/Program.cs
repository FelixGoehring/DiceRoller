using System.Diagnostics;
class DiceRoller
{
    static void Main(string[] args)
    {
        Console.WriteLine("DiceRoller    by KKLG \nVersion 1.1\n\n\n");

        while (true)
        {
            // Get user input with validation
            int numberOfDice = GetValidatedInput("Enter the number of dice: ", minValue: 1);
            int sidesPerDie = GetValidatedInput("Enter the number of sides on each die: ");
            int numberOfDiceToSum = GetValidatedInput($"Enter the number of dice values to sum (always takes the highest values, must be between 1 and {numberOfDice}): ", minValue: 1, maxValue: numberOfDice);
            int numberOfRounds = GetValidatedInput("Enter the number of rounds to test: ");

            // Perform calculations with timer and progress bar
            PerformCalculations(numberOfDice, sidesPerDie, numberOfDiceToSum, numberOfRounds);

            // Ask user if they want to test again
            Console.WriteLine("\n\nDone!\n\nPress ENTER to start again\n\n\n");
            Console.ReadLine();
        }
    }

    static int GetValidatedInput(string prompt, int minValue = 1, int? maxValue = null)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (int.TryParse(input, out result) && result >= minValue && (!maxValue.HasValue || result <= maxValue.Value))
            {
                break;
            }
            string maxInfo = maxValue.HasValue ? $" and less than or equal to {maxValue.Value}" : "";
            Console.WriteLine($"Invalid input. Please enter an integer greater than or equal to {minValue}{maxInfo}.");
        }
        return result;
    }

    static void PerformCalculations(int numberOfDice, int sidesPerDie, int numberOfDiceToSum, int numberOfRounds)
    {
        Random random = new Random();
        List<int> results = new List<int>();

        Stopwatch stopwatch = Stopwatch.StartNew();

        int progressUpdateFrequency = Math.Max(1, numberOfRounds / 100);
        int nextUpdate = progressUpdateFrequency;
        Console.WriteLine("\nCalculation progress:");
        for (int i = 0; i < numberOfRounds; i++)
        {
            results.Add(RollDice(numberOfDice, sidesPerDie, numberOfDiceToSum, random));
            if (i >= nextUpdate || i == numberOfRounds - 1)
            {
                PrintProgress(i + 1, numberOfRounds);
                nextUpdate += random.Next(progressUpdateFrequency, progressUpdateFrequency * 2);
            }
        }

        stopwatch.Stop();
        Console.WriteLine("\nLoading results....");

        float averageResult = (float)results.Average();


        int minRoll = results.Min();
        int maxRoll = results.Max();
        double medianRoll = CalculateMedian(results);
        double stdDeviation = CalculateStandardDeviation(results, averageResult);
        (double lower, double upper) confidenceInterval = CalculateConfidenceInterval(results, averageResult, stdDeviation, 0.95);

        Console.WriteLine($"\n\n\u001b[38;5;156mAverage result after {numberOfRounds} rounds: {averageResult}\u001b[0m");
        Console.WriteLine($"Minimum roll: {minRoll}");
        Console.WriteLine($"Maximum roll: {maxRoll}");
        Console.WriteLine($"Median roll: {medianRoll}");
        Console.WriteLine($"Standard deviation: {stdDeviation}");
        Console.WriteLine($"95% Confidence Interval: [{confidenceInterval.lower}, {confidenceInterval.upper}]");
        Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");

        PrintFrequencyDistributionAndGraph(results, numberOfDiceToSum, sidesPerDie);
    }

    static int RollDice(int numberOfDice, int sidesPerDie, int numberOfDiceToSum, Random random)
    {
        int[] diceValues = new int[numberOfDice];

        for (int i = 0; i < numberOfDice; i++)
        {
            diceValues[i] = random.Next(1, sidesPerDie + 1);
        }

        return diceValues.OrderByDescending(v => v).Take(numberOfDiceToSum).Sum();
    }

    static double CalculateMedian(List<int> results)
    {
        var sortedResults = results.OrderBy(n => n).ToList();
        int count = sortedResults.Count;
        if (count % 2 == 0)
        {
            return (sortedResults[count / 2 - 1] + sortedResults[count / 2]) / 2.0;
        }
        else
        {
            return sortedResults[count / 2];
        }
    }

    static double CalculateStandardDeviation(List<int> results, double average)
    {
        double sumOfSquares = results.Sum(result => Math.Pow(result - average, 2));
        return Math.Sqrt(sumOfSquares / results.Count);
    }

    static (double lower, double upper) CalculateConfidenceInterval(List<int> results, double mean, double stdDev, double confidenceLevel)
    {
        double z = 1.96; // For 95% confidence
        double marginOfError = z * stdDev / Math.Sqrt(results.Count);
        return (mean - marginOfError, mean + marginOfError);
    }

    static void PrintFrequencyDistributionAndGraph(List<int> results, int numberOfDiceToSum, int sidesPerDie)
    {
        var frequencyDistribution = results.GroupBy(result => result)
                                           .OrderBy(group => group.Key)
                                           .ToDictionary(group => group.Key, group => group.Count());

        int maxGraphWidth = 100;
        int minGraphWidth = 5;
        int numberOfEntries = frequencyDistribution.Count;
        int graphWidth = minGraphWidth + (maxGraphWidth - minGraphWidth) * numberOfEntries / 100;
        if (graphWidth > maxGraphWidth) graphWidth = maxGraphWidth;
        if (graphWidth < minGraphWidth) graphWidth = minGraphWidth;

        int maxKeyLength = frequencyDistribution.Keys.Max().ToString().Length;
        int maxValueLength = frequencyDistribution.Values.Max().ToString().Length;
        int maxPercentageLength = "100.00%".Length; // Length of percentage format "100.00%"

        double maxPercentage = frequencyDistribution.Values.Max() / (double)results.Count * 100;
        var maxEntry = frequencyDistribution.OrderByDescending(entry => entry.Value).First();

        Console.WriteLine("\nFrequency Distribution and Graph:");
        foreach (var entry in frequencyDistribution)
        {
            double percentage = (double)entry.Value / results.Count * 100;
            int barLength = (int)(percentage / maxPercentage * graphWidth);
            string bar = new string('#', barLength);

            if (entry.Equals(maxEntry))
            {
                Console.WriteLine($"\u001b[38;5;156m{entry.Key.ToString().PadLeft(maxKeyLength)}: {bar} {entry.Value.ToString().PadLeft(maxValueLength)} ({percentage:F2}%)\u001b[0m");
            }
            else
            {
                Console.WriteLine($"{entry.Key.ToString().PadLeft(maxKeyLength)}: {bar} {entry.Value.ToString().PadLeft(maxValueLength)} ({percentage:F2}%)");
            }
        }
    }

    static void PrintProgress(int current, int total)
    {
        int progressBarWidth = 50;
        float progress = (float)current / total;
        int filledWidth = (int)(progressBarWidth * progress);

        Console.CursorLeft = 0;
        Console.Write("[");
        Console.Write(new string('=', filledWidth));
        Console.Write(new string(' ', progressBarWidth - filledWidth));
        Console.Write($"] {current}/{total}");
    }
}
