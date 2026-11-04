namespace TextAnalyser;

public static class LineCounter
{
    public static int CountLines(string text)
    {
        // Check if the text is null, empty, or consists only of white-space characters
        if (string.IsNullOrWhiteSpace(text))
        {
            return 0;
        }

        // Split the text on newline characters and count the resulting lines
        // Ensure a newline at the end doesn't add a line to the count
        var lines = text.Split(["\r\n", "\n", "\r"], StringSplitOptions.None);
        if (string.IsNullOrEmpty(lines[^1]))
        {
            return lines.Length - 1;
        }
        return lines.Length;
    }
}