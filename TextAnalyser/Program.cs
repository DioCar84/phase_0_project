using TextAnalyser;

try
{
    const string UsageMessage = "Usage: analyse <file> [--top N]";
    const int DefaultTopN = 10;
    int topN = DefaultTopN;

    // check if a file path argument is provided
    if (args.Length == 0)
    {
        Console.Error.WriteLine("Argument cannot be empty. Please provide a file path as an argument.");
        Console.Error.WriteLine(UsageMessage);
        return 1;
    }

    if (args.Length > 1 && args[1] != "--top")
    {
        Console.Error.WriteLine("Invalid argument. Only --top is supported as the second argument.");
        Console.Error.WriteLine(UsageMessage);
        return 1;
    }

    // From here on, args[1] (if present) is known to be "--top"
    if (args.Length == 2)
    {
        Console.Error.WriteLine("Missing value for --top argument. Please provide a valid number greater than 0.");
        Console.Error.WriteLine(UsageMessage);
        return 1;
    }

    // Check the value provided for the --top argument is a whole number greater than 0
    if (args.Length > 2)
    {
        if (int.TryParse(args[2], out int parsedTopN) && parsedTopN > 0)
        {
            topN = parsedTopN;
        }
        else
        {
            Console.Error.WriteLine("Invalid value for --top argument. Please provide a valid number greater than 0.");
            Console.Error.WriteLine(UsageMessage);
            return 1;
        }
    }

    if (!File.Exists(args[0]))
    {
        Console.Error.WriteLine($"File not found: {args[0]}. Please provide a valid file path.");
        return 1;
    }

    // Read the file content as a string.
    string text = File.ReadAllText(args[0]);
    var wordCount = WordCounter.CountWords(text);

    if (wordCount == 0)
    {
        Console.Error.WriteLine("The file contains no words.");
        return 1;
    }

    var lineCount = LineCounter.CountLines(text);
    var charCount = CharCounter.CountChars(text);
    var topWords = TopWordsCounter.CountTopWords(text, topN);

    // Print the number of words, lines, characters and the top words in the text.
    Console.WriteLine($"Word count: {wordCount}");
    Console.WriteLine($"Line count: {lineCount}");
    Console.WriteLine($"Character count: {charCount}");
    Console.WriteLine("Top words:");
    foreach (var (word, count) in topWords)
    {
        Console.WriteLine($"{word}: {count}");
    }
    return 0;
}
catch (IOException e)
{
    Console.Error.WriteLine("The file could not be read:");
    Console.Error.WriteLine(e.Message);
    return 1;
}

