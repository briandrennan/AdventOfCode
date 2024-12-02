namespace AdventOfCode.Y2024.Day1;

public static class ProblemFunctions
{
    public static async Task SolvePuzzles()
    {
        var (left, right, freq) = await GetLocations()
            .ConfigureAwait(false);

        Console.WriteLine("Left={0}, Right={1}, Frequencies={2}", left.Count, right.Count, freq.Count);

        if (left.Count != right.Count)
        {
            Console.Error.WriteLine("Error: the counts must be equal");
            return;
        }

        PrintPreview(left, right, 10);

        SolveFirstPuzzle(left, right);
        SolveSecondPuzzle(left, freq);
    }

    private static void SolveFirstPuzzle(List<int> left, List<int> right)
    {
        left.Sort();
        right.Sort();

        var distances = new int[left.Count];
        for (var i = 0; i < left.Count; i++)
        {
            distances[i] = Math.Abs(left[i] - right[i]);
        }

        var total = distances.Sum();
        Console.WriteLine("Day 1 Problem 1 Answer: {0}", total);
    }

    private static void SolveSecondPuzzle(List<int> locations, Dictionary<int, int> frequencies)
    {
        var similarityScore = locations
            .Select(l => frequencies.GetValueOrDefault(l) * l)
            .Sum();

        Console.WriteLine("Day 1 Problem 2 Answer: {0}", similarityScore);
    }

    private static async Task<(List<int> left, List<int> right, Dictionary<int, int> frequencies)> GetLocations()
    {
        using var file = File.Open("./data/day1.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
        using var streamReader = new StreamReader(file, detectEncodingFromByteOrderMarks: true);

        var left = new List<int>();
        var right = new List<int>();
        var frequencies = new Dictionary<int, int>();

        while (!streamReader.EndOfStream)
        {
            ReadOnlySpan<char> data = await streamReader
                .ReadLineAsync()
                .ConfigureAwait(false);

            var firstDelim = data.IndexOf(' ');
            var lastStart = data.LastIndexOf(' ');

            var l1 = int.Parse(data[..firstDelim]);
            var r1 = int.Parse(data[lastStart..]);

            left.Add(l1);
            right.Add(r1);

            if (!frequencies.TryAdd(r1, 1))
            {
                frequencies[r1]++;
            }
        }

        return (left, right, frequencies);
    }

    private static void PrintPreview(List<int> left, List<int> right, int lim)
    {
        var min = Math.Min(lim, left.Count);
        for (var i = 0; i < min; i++)
        {
            Console.WriteLine("{0}     {1}", left[i], right[i]);
        }
    }
}