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

        return text.Trim().Split(' ').Length;
    }
}