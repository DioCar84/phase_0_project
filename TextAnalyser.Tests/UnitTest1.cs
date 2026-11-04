namespace TextAnalyser.Tests;

public class UnitTest1
{
    [Fact]
    public void HelloWorld_IsTwoWords_ReturnsTrue()
    {
        var text = "Hello World";
        var result = text.Split(' ').Length == 2;
        Assert.True(result);
    }

    [Fact]
    public void HelloWorld_IsElevenCharacters_ReturnsTrue()
    {
        var text = "Hello World";
        var result = text.Length == 11;
        Assert.True(result);
    }
}
