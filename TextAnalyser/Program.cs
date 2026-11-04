using TextAnalyser;

try
{
    // check if a file path argument is provided
    if (args.Length == 0)
    {
        Console.WriteLine("Argument cannot be empty. Please provide a file path as an argument.");
        return 1;
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

    // Count the number of words and lines in the text.
    Console.WriteLine($"Word count: {wordCount}");
    Console.WriteLine($"Line count: {lineCount}");
    return 0;
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
    return 1;
}

