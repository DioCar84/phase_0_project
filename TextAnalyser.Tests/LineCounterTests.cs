namespace TextAnalyser.Tests;
using TextAnalyser;

public class LineCounterTests
{
    [Fact]
    public void CountLines_TakesInOneLine_ReturnsCorrectCount()
    {
        var text = "Hello World";
        var count = LineCounter.CountLines(text);
        Assert.Equal(1, count);
    }

    [Fact]
    public void CountLines_TakesInTwoLines_ReturnsCorrectCount()
    {
        var text = "Hello\nWorld";
        var count = LineCounter.CountLines(text);
        Assert.Equal(2, count);
    }

    [Fact]
    public void CountLines_TakesInEmptyString_ReturnsZero()
    {
        var text = "";
        var count = LineCounter.CountLines(text);
        Assert.Equal(0, count);
    }

    [Fact]
    public void CountLines_TakesInWhitespaceOnlyString_ReturnsZero()
    {
        var text = "   ";
        var count = LineCounter.CountLines(text);
        Assert.Equal(0, count);
    }

    [Fact]
    public void CountLines_BlankLineBetweenLines_IsCounted()
    {
        var text = "Hello\n\nWorld";
        var count = LineCounter.CountLines(text);
        Assert.Equal(3, count);
    }

    [Fact]
    public void CountLines_TakesInStringWithTrailingNewline_ReturnsCorrectCount()
    {
        var text = "Hello World\n";
        var count = LineCounter.CountLines(text);
        Assert.Equal(1, count);
    }

    [Fact]
    public void CountLines_TakesInStringWithWindowsLineEndings_ReturnsCorrectCount()
    {
        var text = "Hello World\r\n";
        var count = LineCounter.CountLines(text);
        Assert.Equal(1, count);
    }

    [Fact]
    public void CountLines_TakesInStringWithWindowsLineEndingInBetween_ReturnsCorrectCount()
    {
        var text = "Hello\r\nWorld";
        var count = LineCounter.CountLines(text);
        Assert.Equal(2, count);
    }

    [Fact]
    public void CountLines_TakesInStringWithWhitespaceOnlyLineInBetween_ReturnsCorrectCount()
    {
        var text = "Hello\n \nWorld";
        var count = LineCounter.CountLines(text);
        Assert.Equal(3, count);
    }

    [Fact]
    public void CountLines_TakesInStringWithALineContainingOnlyWhitespace_ReturnsCorrectCount()
    {
        var text = "Hello\n ";
        var count = LineCounter.CountLines(text);
        Assert.Equal(2, count);
    }

    [Fact]
    public void CountLines_TakesInStringWithATrailingBlankLineFollowedByAFinalNewline_ReturnsCorrectCount()
    {
        var text = "Hello\n\n";
        var count = LineCounter.CountLines(text);
        Assert.Equal(2, count);
    }
}