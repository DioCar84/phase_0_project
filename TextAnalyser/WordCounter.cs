namespace TextAnalyser;

public static class WordCounter
{
    public static int CountWords(string text)
    {
        return text.Split(' ').Length;
    }
}