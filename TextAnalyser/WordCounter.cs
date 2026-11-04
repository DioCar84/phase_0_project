namespace TextAnalyser;

public static class WordCounter
{
    public static int CountWords(string text)
    {
        // Check if the text is null, empty, or consists only of white-space characters
        if (string.IsNullOrWhiteSpace(text))
        {
            return 0;
        }

        // Split the text on any empty whitespace and count the resulting words
        return text.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Length;
    }
}