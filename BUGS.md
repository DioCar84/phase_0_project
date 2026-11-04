# Bug Diary

## Bug 1: Word count off by one

**Symptom:**

Two of the four unit tests failed after `CountWords` was rewritten as a loop:

| Test | Input | Expected | Actual |
|---|---|---|---|
| `CountWords_TakesInTwoWords_ReturnsCorrectCount` | `"Hello World"` | 2 | 1 |
| `CountWords_TakesInStringWithWhiteSpaceCharacters_ReturnsCorrectCount` | `"Hello\n\tWorld"` | 2 | 1 |

The two remaining tests, `CountWords_TakesInEmptyString_ReturnsZero` and `CountWords_TakesInWhitespaceOnlyString_ReturnsZero`, still passed. The bug only shows up when the text contains actual words.

**Reproduce:**

From the solution root:

- `dotnet test` - the two tests above fail
- `dotnet run --project TextAnalyser` - `TestFile.txt` contains `Hello World` and the program prints `Word count: 1`

**Hypothesis:**

Every failing case is short by exactly one, and inputs with no words are unaffected. That points to a single word being missed each time, most likely the first word, because of a boundary mistake in the loop that walks the characters.

**Investigation:**

The code under investigation:

```csharp
public static int CountWords(string text)
{
    int count = 0;
    for (int i = 1; i < text.Length; i++)
    {
        bool isWordStart = !char.IsWhiteSpace(text[i])
                           && (i == 0 || char.IsWhiteSpace(text[i - 1]));
        if (isWordStart) count++;
    }
    return count;
}
```

Debugged `CountWords_TakesInTwoWords_ReturnsCorrectCount` with **Debug Test** from the Test Explorer.

- **Breakpoint:** Set on the `for` line and stepped through with F10. On the first entry into the loop body `i` was already `1`, so the loop never ran with `i = 0`.
- **Watches:** Watched `i`, `text[i]` and `count`.
  - First iteration: `i = 1`, `text[i] = 'e'`. The first character, `'H'`, was never examined.
  - For `i = 1` to `4` (`e`, `l`, `l`, `o`), `isWordStart` was `false` because the previous character is a letter, so `count` stayed `0` for the whole of "Hello".
  - At `i = 6`, `text[i] = 'W'` and the previous character is a space, so `count` became `1`.
  - The loop ended at `i = 10` (`'d'`) and the method returned `1`.
  - The `i == 0` check in `isWordStart` can never be true, because the loop never starts at `0`. That's dead code, and a strong hint that the loop's start value is wrong.
- **Call stack:** While paused in `CountWords`, the frame above it was `UnitTest1.CountWords_TakesInTwoWords_ReturnsCorrectCount`. Selecting that frame showed `text = "Hello World"`. The input arriving from the test was correct, so the bug lies in `CountWords` itself, not in the test or in how the file is read.

**Root cause:**

The `for` loop starts at `i = 1` instead of `i = 0`. Index 0 is never checked, so a word starting at the first character is never counted. Any text that begins with a word is undercounted by exactly one. Empty and whitespace-only inputs have no words to miss, which is why those tests still passed.

**Fix:**

Changed the loop's starting value from `int i = 1` to `int i = 0`:

```csharp
public static int CountWords(string text)
{
    int count = 0;
    for (int i = 0; i < text.Length; i++)
    {
        bool isWordStart = !char.IsWhiteSpace(text[i])
                           && (i == 0 || char.IsWhiteSpace(text[i - 1]));
        if (isWordStart) count++;
    }
    return count;
}
```

**Verification:**

- `dotnet test` - all 4 tests pass
- `dotnet run --project TextAnalyser` - prints `Word count: 2` for `TestFile.txt`
