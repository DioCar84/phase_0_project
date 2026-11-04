using TextAnalyser;

try
{
    // Open the text file using a stream reader.
    using StreamReader reader = new("TestFile.txt");

    // Read the stream as a string.
    string text = reader.ReadToEnd();

    // Write the text to the console.
    Console.WriteLine(text);

    // Count the number of words in the text.
    Console.WriteLine($"Word count: {WordCounter.CountWords(text)}");
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
}

