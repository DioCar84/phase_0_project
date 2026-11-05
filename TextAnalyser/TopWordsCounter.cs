namespace TextAnalyser;

public static class TopWordsCounter
{
    public static IReadOnlyList<(string Word, int Count)> CountTopWords(string text, int topN)
    {
        if (topN <= 0)
        {
            throw new ArgumentException("topN must be greater than zero.", nameof(topN));
        }

        var wordCounts = new Dictionary<string, int>();
        var words = text.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            // Strip one punctuation character from each end of the word (inner punctuation is kept).
            // Known limitation: only one character per side is removed, see README.
            var trimmedWord = string.Empty;
            for (int i = 0; i < word.Length; i++)
            {
                if (i == 0 || i == word.Length - 1)
                {
                    if (!char.IsPunctuation(word[i]))
                    {
                        trimmedWord += word[i];
                    }
                }
                else
                {
                    trimmedWord += word[i];
                }
            }
            // Check if only punctuation is left
            if (trimmedWord.All(char.IsPunctuation))
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmedWord))
            {
                continue;
            }
            var lowerWord = trimmedWord.ToLowerInvariant();
            if (wordCounts.TryGetValue(lowerWord, out int value))
            {
                wordCounts[lowerWord] = ++value;
            }
            else
            {
                wordCounts[lowerWord] = 1;
            }
        }
        // Order the words by count descending, then alphabetically for ties and take the top N words
        return wordCounts.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key, StringComparer.Ordinal).Take(topN).Select(kv => (kv.Key, kv.Value)).ToList();
    }
}
