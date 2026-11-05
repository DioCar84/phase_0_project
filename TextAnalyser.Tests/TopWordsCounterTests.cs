namespace TextAnalyser.Tests;
using TextAnalyser;

public class TopWordsCounterTests
{
    [Fact]
    public void CountTopWords_EmptyText_ReturnsEmptyList()
    {
        string text = "";
        int topN = 2;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Empty(result);
    }

    [Fact]
    public void CountTopWords_WhitespaceOnly_ReturnsEmptyList()
    {
        string text = "   \t\n";
        int topN = 2;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Empty(result);
    }

    [Fact]
    public void CountTopWords_RepeatedWord_ReturnsWordWithCount()
    {
        string text = "apple apple apple";
        int topN = 1;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Single(result);
        Assert.Equal("apple", result[0].Word);
        Assert.Equal(3, result[0].Count);
    }

    [Fact]
    public void CountTopWords_MixedCase_CountsAsSameWord()
    {
        string text = "Apple apple APPLE";
        int topN = 1;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Single(result);
        Assert.Equal("apple", result[0].Word);
        Assert.Equal(3, result[0].Count);
    }

    [Fact]
    public void CountTopWords_TrailingAndLeadingPunctuation_IsStripped()
    {
        string text = "apple, apple! apple. (apple !apple .apple";
        int topN = 1;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Single(result);
        Assert.Equal("apple", result[0].Word);
        Assert.Equal(6, result[0].Count);
    }

    [Fact]
    public void CountTopWords_InnerApostrophe_IsKept()
    {
        string text = "it's its it's";
        int topN = 3;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Equal(2, result.First(r => r.Word == "it's").Count);
        Assert.Equal(1, result.First(r => r.Word == "its").Count);
    }

    [Fact]
    public void CountTopWords_PunctuationOnlyToken_IsSkipped()
    {
        string text = "!!! ??? ,,,";
        int topN = 1;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Empty(result);
    }

    [Fact]
    public void CountTopWords_MixedWhitespace_SplitsLikeWordCounter()
    {
        string text = "apple\tbanana  apple\norange  apple\u00A0banana";
        int topN = 3;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Equal(3, result.Count);
        Assert.Equal(3, result.First(r => r.Word == "apple").Count);
        Assert.Equal(2, result.First(r => r.Word == "banana").Count);
        Assert.Equal(1, result.First(r => r.Word == "orange").Count);
    }

    [Fact]
    public void CountTopWords_OrdersByCountDescending()
    {
        string text = "orange banana apple apple banana apple";
        int topN = 3;
        var result = TopWordsCounter.CountTopWords(text, topN);
        var keys = result.Select(r => r.Word).ToList();
        Assert.Equal("apple", keys[0]);
        Assert.Equal("banana", keys[1]);
        Assert.Equal("orange", keys[2]);
    }

    [Fact]
    public void CountTopWords_TiedCounts_OrdersAlphabetically()
    {
        string text = "banana apple cherry";
        int topN = 3;
        var result = TopWordsCounter.CountTopWords(text, topN);
        var keys = result.Select(r => r.Word).ToList();
        Assert.Equal("apple", keys[0]);
        Assert.Equal("banana", keys[1]);
        Assert.Equal("cherry", keys[2]);
    }

    [Fact]
    public void CountTopWords_TieAtCutOff_ReturnsExactlyN()
    {
        string text = "apple banana cherry";
        int topN = 2;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Equal(topN, result.Count);
        Assert.Equal("apple", result[0].Word);
        Assert.Equal("banana", result[1].Word);
    }

    [Fact]
    public void CountTopWords_NLargerThanDistinctWords_ReturnsAll()
    {
        string text = "apple banana cherry";
        int topN = 5;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Equal(3, result.Count);
        Assert.Equal("apple", result[0].Word);
        Assert.Equal("banana", result[1].Word);
        Assert.Equal("cherry", result[2].Word);
    }

    [Fact]
    public void CountTopWords_NIsZero_ThrowsArgumentException()
    {
        string text = "apple banana cherry";
        int topN = 0;
        Assert.Throws<ArgumentException>(() => TopWordsCounter.CountTopWords(text, topN));
    }

    [Fact]
    public void CountTopWords_NegativeN_ThrowsArgumentException()
    {
        string text = "apple banana cherry";
        int topN = -1;
        Assert.Throws<ArgumentException>(() => TopWordsCounter.CountTopWords(text, topN));
    }

    [Fact]
    public void CountTopWords_SymbolsCountAsWords_ReturnsCorrectCounts()
    {
        string text = "$ + £";
        int topN = 3;
        var result = TopWordsCounter.CountTopWords(text, topN);
        Assert.Equal(3, result.Count);
        Assert.Equal("$", result[0].Word);
        Assert.Equal(1, result[0].Count);
        Assert.Equal("+", result[1].Word);
        Assert.Equal(1, result[1].Count);
        Assert.Equal("£", result[2].Word);
        Assert.Equal(1, result[2].Count);
    }
}