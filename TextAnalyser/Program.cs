using TextAnalyser;

try
{
    const int DefaultTopN = 10;
    int topN = DefaultTopN;

    // check if a file path argument is provided
    if (args.Length == 0)
    {
        Console.WriteLine("Argument cannot be empty. Please provide a file path as an argument.");
        return 1;
    }

    if (args.Length > 1 && args[1] != "--top")
    {
        Console.WriteLine("Invalid argument. Only --top is supported as the second argument.");
        return 1;
    }

    // From here on, args[1] (if present) is known to be "--top"
    if (args.Length == 2)
    {
        Console.WriteLine("Missing value for --top argument. Please provide a valid number greater than 0.");
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
            Console.WriteLine("Invalid value for --top argument. Please provide a valid number greater than 0.");
            return 1;
        }
    }

    if (!File.Exists(args[0]))
    {
        Console.WriteLine("The specified file does not exist. Please provide a valid file path.");
        return 1;
    }

    // Read the file content as a string.
    string text = File.ReadAllText(args[0]);
    var wordCount = WordCounter.CountWords(text);

    if (wordCount == 0)
    {
        Console.WriteLine("The file contains no words.");
        return 1;
    }

    var lineCount = LineCounter.CountLines(text);
    var charCount = CharCounter.CountChars(text);
    var topWords = TopWordsCounter.CountTopWords(text, topN);

    // Print the number of words, lines, and characters in the text.
    Console.WriteLine($"Word count: {wordCount}");
    Console.WriteLine($"Line count: {lineCount}");
    Console.WriteLine($"Character count: {charCount}");
    Console.WriteLine("Top words:");
    foreach (var (Word, Count) in topWords)
    {
        Console.WriteLine($"{Word}: {Count}");
    }
    return 0;
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
    return 1;
}

