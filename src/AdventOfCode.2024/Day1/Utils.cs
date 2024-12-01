namespace AdventOfCode.Y2024.Day1;

public static class ProblemFunctions
{
    public static async Task SolvePuzzles()
    {
        await SolveFirstPuzzle();
        await SolveSecondPuzzle();
    }

    public static async Task SolveFirstPuzzle()
    {
        var (left, right) = await GetLocations().ConfigureAwait(false);
        Console.WriteLine("Left={0}, Right={1}", left.Count, right.Count);
        if (left.Count != right.Count)
        {
            Console.Error.WriteLine("Error: the counts must be equal");
            return;
        }

        PrintPreview(left, right, 10);

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

    public static async Task SolveSecondPuzzle()
    {
        var (locations, frequencies) = await GetLocationsAndFrequencies()
            .ConfigureAwait(false);

        Console.WriteLine("Total Locations={0}, Frequency Count={1}", locations.Count, frequencies.Count);

        var similarityScore = locations
            .Select(l => frequencies.GetValueOrDefault(l) * l)
            .Sum();

        Console.WriteLine("Day 1 Problem 2 Answer: {0}", similarityScore);
    }

    private static async Task<(List<int> left, List<int> right)> GetLocations()
    {
        using var file = File.Open("./data/day1.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
        using var streamReader = new StreamReader(file, detectEncodingFromByteOrderMarks: true);

        var left = new List<int>();
        var right = new List<int>();
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
        }

        return (left, right);
    }

    private static async Task<(List<int> locations, Dictionary<int, int> frequencies)> GetLocationsAndFrequencies()
    {
        using var file = File.Open("./data/day1.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
        using var streamReader = new StreamReader(file, detectEncodingFromByteOrderMarks: true);

        var left = new List<int>();
        var right = new Dictionary<int, int>();
        while (!streamReader.EndOfStream)
        {
            ReadOnlySpan<char> data = await streamReader.ReadLineAsync().ConfigureAwait(false);
            var firstDelim = data.IndexOf(' ');
            var lastStart = data.LastIndexOf(' ');

            var l1 = int.Parse(data[..firstDelim]);
            var r1 = int.Parse(data[lastStart..]);

            left.Add(l1);

            if (!right.TryAdd(r1, 1))
            {
                right[r1]++;
            }
        }

        return (left, right);
    }

    private static void PrintPreview(IList<int> left, IList<int> right, int lim)
    {
        var min = Math.Min(lim, left.Count);
        for (var i = 0; i < min; i++)
        {
            Console.WriteLine("{0}     {1}", left[i], right[i]);
        }
    }
}