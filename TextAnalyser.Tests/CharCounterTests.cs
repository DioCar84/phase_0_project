namespace TextAnalyser.Tests;
using TextAnalyser;

public class CharCounterTests
{
    [Fact]
    public void CountChars_TakesInString_ReturnsCorrectCount()
    {
        var text = "Hello";
        var count = CharCounter.CountChars(text);
        Assert.Equal(5, count);
    }

    [Fact]
    public void CountChars_TakesInEmptyString_ReturnsZero()
    {
        var text = "";
        var count = CharCounter.CountChars(text);
        Assert.Equal(0, count);
    }

    [Fact]
    public void CountChars_TakesInWhitespaceString_ReturnsCorrectCount()
    {
        var text = "   ";
        var count = CharCounter.CountChars(text);
        Assert.Equal(3, count);
    }

    [Fact]
    public void CountChars_TakesInStringWithSpecialCharacters_ReturnsCorrectCount()
    {
        var text = "!@#";
        var count = CharCounter.CountChars(text);
        Assert.Equal(3, count);
    }

    [Fact]
    public void CountChars_BlankLinesBetweenChars_IsCounted()
    {
        var text = "Hello\n\nWorld";
        var count = CharCounter.CountChars(text);
        Assert.Equal(12, count);
    }

    [Fact]
    public void CountChars_TakesInStringWithUnixLineEndingInBetween_ReturnsCorrectCount()
    {
        var text = "Hello\nWorld";
        var count = CharCounter.CountChars(text);
        Assert.Equal(11, count);
    }

    [Fact]
    public void CountChars_TakesInStringWithWindowsLineEndingInBetween_ReturnsCorrectCount()
    {
        var text = "Hello\r\nWorld";
        var count = CharCounter.CountChars(text);
        Assert.Equal(12, count);
    }

    [Fact]
    public void CountChars_TabBetweenWords_IsCounted()
    {
        var text = "Hello\tWorld";
        var count = CharCounter.CountChars(text);
        Assert.Equal(11, count);
    }

    [Fact]
    // .Length counts UTF-16 code units; this emoji is a surrogate pair, so 2 not 1
    public void CountChars_TakesInStringWithUnicodeCharacters_ReturnsCorrectCount()
    {
        var text = "😀";
        var count = CharCounter.CountChars(text);
        Assert.Equal(2, count);
    }
}