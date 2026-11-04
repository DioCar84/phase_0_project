namespace TextAnalyser.Tests;
using TextAnalyser;

public class UnitTest1
{
    [Fact]
    public void HelloWorld_IsTwoWords_ReturnsTrue()
    {
        var text = "Hello World";
        var count = WordCounter.CountWords(text);
        Assert.Equal(2, count);
    }

    [Fact]
    public void HelloWorld_IsElevenCharacters_ReturnsTrue()
    {
        var text = "Hello World";
        var result = text.Length == 11;
        Assert.True(result);
    }
}
