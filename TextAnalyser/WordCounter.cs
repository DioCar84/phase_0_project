namespace TextAnalyser;

public static class WordCounter
{
    public static int CountWords(string text)
    {
        // Split the text on any empty whitespace and count the resulting words
        return text.Split([' ', '\t', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Length;
    }
}