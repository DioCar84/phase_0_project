namespace TextAnalyser.Tests;
using TextAnalyser;

public class WordCounterTests
{
    [Fact]
    public void CountWords_TakesInTwoWords_ReturnsCorrectCount()
    {
        var text = "Hello World";
        var count = WordCounter.CountWords(text);
        Assert.Equal(2, count);
    }

    [Fact]
    public void CountWords_TakesInEmptyString_ReturnsZero()
    {
        var text = "";
        var count = WordCounter.CountWords(text);
        Assert.Equal(0, count);
    }

    [Fact]
    public void CountWords_TakesInWhitespaceOnlyString_ReturnsZero()
    {
        var text = "   ";
        var count = WordCounter.CountWords(text);
        Assert.Equal(0, count);
    }

    [Fact]
    public void CountWords_TakesInStringWithWhiteSpaceCharacters_ReturnsCorrectCount()
    {
        var text = "Hello\n\tWorld";
        var count = WordCounter.CountWords(text);
        Assert.Equal(2, count);
    }
}
