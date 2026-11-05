# What I Learned From This Project

## Summary

Text Analyser is a .NET 10 console app. Run as `analyse <file> [--top N]`, it reports a file's word, line and character counts and its N most frequent words (10 by default). Bad input gets a clear message on stderr and exit code 1: no arguments, a missing file, an empty file, or an invalid `--top` value.

The program itself was never the point. Phase 0 is about practising the basic development loop until it's automatic:

- creating a solution and projects with the `dotnet` CLI
- writing unit tests with xUnit
- debugging with breakpoints, watches and the call stack
- working in Git with feature branches, small commits, pull requests and a merge conflict
- documenting all of it

The finished project has four counter classes, 40 unit tests, 12 merged pull requests, a bug diary (BUGS.md), and a README that covers setup, the errors I hit and the known limitations.

## How it went

*The order things were built in (reading a file → word count → errors → bug diary → line count → character count → top words → `--top` → error output) and anything notable about each step.*

## What went well

*What worked: habits, tools or decisions you'd repeat.*

## What was harder than expected

*Where you got stuck, what took longer than planned, and why.*

## Scope

The project grew well beyond the Phase 0 brief. The roadmap says "the program barely matters" and asks for only one or two trivial tests, but by the top-words feature I had 40 tests and was on a third review round about punctuation edge cases. Inputs like `"word,"` and `...word...` had nothing to do with the CLI, testing, debugging or Git skills the phase is about.

The turning point was stepping back and asking what the project was *for*. From then on I used a simple rule:

- **Must fix:** anything that breaks the build or the tests, anything where the code contradicts its own comments, tests or docs, and anything the Phase 0 checklist requires.
- **Everything else:** either a quick fix or a known limitation, written up in the README.

Using that rule, I skipped detecting folders, catching `UnauthorizedAccessException`, and full punctuation trimming, and documented each one as a known limitation instead. I also dropped the "check every counter is fully tested" item, because test design is covered in Phase 1.

The lesson: decide what "done" means *before* starting a feature, based on the goal rather than on every edge case I can think of. A documented limitation is a legitimate outcome, not a failure.

## Debugging

**With the debugger.** For the planted off-by-one bug in `CountWords` (written up in BUGS.md), I used **Debug Test** from the Test Explorer instead of guessing:

- **The hypothesis came first.** Every failing case was short by exactly one, and empty input still passed. That pointed to a boundary problem before I set a single breakpoint.
- **Breakpoint on the loop:** stepping in showed `i` was already `1` the first time through, so index 0 was never checked.
- **Watches** on `i`, `text[i]` and `count` showed exactly where the count should have gone up and didn't.
- **Call stack:** selecting the test's frame showed that the input was correct, so the bug was in `CountWords`, not in the test or in how the file was read.
- **Dead code was a clue:** the `i == 0` check could never be true, which pointed straight at the loop's start value.

**Without the debugger.** Some bugs passed every test and only showed up when I ran the code against awkward input: `it's`, `3.14`, `e.g.`, `"word,"`, an em-dash, a non-breaking space. A quick table of input → actual output found the punctuation bugs faster than stepping through would have. For Program.cs, which has no unit tests, I checked behaviour by running every argument case and looking at the exit code (`echo $?`). Then I redirected each stream separately (`2>/dev/null` and `1>/dev/null`) to prove that errors went to stderr and the report went to stdout.

**Build warnings:** an incremental `dotnet build` can skip projects that haven't changed and hide their warnings. `dotnet build --no-incremental` shows everything.

## Testing

Each counter has its own test class. Tests follow the `Method_Scenario_Expected` naming pattern and use `Assert.Equal`. The most useful lessons came from tests that were wrong:

- **A passing test can prove nothing.** `CountTopWords_OrdersByCountDescending` still passed with the sort removed, because the most frequent word also came first in the input. If the input is already in the expected order, the test can't tell whether the code sorted it. Now I ask: *would this test fail if I deleted the code it's meant to test?*
- **Check the result, not a stand-in for it.** `CountTopWords_InnerApostrophe_IsKept` only checked the count, so it passed while `it's` was being turned into `its`. Asserting the actual word caught the bug straight away.
- **The expected value can be the bug.** `CountTopWords_TrailingAndLeadingPunctuation_IsStripped` failed with `Expected: 3, Actual: 6`. The input had six `apple`s. The code was right and the test was wrong, so when a test fails, check the test as well as the code.
- **The test name is a claim.** A test called `..._TrailingAndLeading...` that only checks trailing punctuation lets a bug through. The name and the assertions need to match.
- **`Assert.Throws<T>` checks for that exact type.** A subclass doesn't count: `Assert.Throws<ArgumentException>` fails if the code throws `ArgumentOutOfRangeException`.
- **Not everything can be unit-tested yet.** Program.cs uses top-level statements, so its argument handling can't be called from a test. I verified it by running the program. Pulling that logic into a class that can be tested is a Phase 1 topic.

`[Theory]` with `[InlineData]` would have shortened the many tests that differ only in their input. I'm leaving that for Phase 1, where test design is covered.

## Git

Every change went through its own branch and a GitHub pull request, 12 so far, and nothing was pushed straight to `main`. After each merge I ran `git switch main`, then `git pull`, then created the next branch. Most features were two commits: the logic and its tests, then calling it from Program.cs. That kept each commit easy to review, and easy to revert if needed.

- **The merge conflict** (commit `7bbe3ec`, written up in the README) came from two branches rewriting the same `return` line in `CountWords`. Resolving it meant understanding *why* each side made its change, not just choosing one. I kept the empty-input check and the whitespace split, dropped the `Trim()` because it was redundant, and ran the tests afterwards to confirm the result.
- **Stage files by name.** I kept two files out of every commit until the end: a csproj change and this file. That meant never running `git add .` or `git commit -a`, and adding each file by name. It's a good habit anyway, because it stops unrelated files sneaking into a commit.
- **Check where a branch's upstream points.** Several of my local branches had `origin/main` as their upstream, instead of their own remote branch. Pushing the first time with `git push -u origin <branch>` fixes this.
- **Run `git fetch --prune` now and then.** Branches deleted on GitHub after merging stay in the local list until you prune them.
- **One commit wasn't split.** The `--top` feature went in as one commit containing both the Program.cs and README changes. Two commits would have been cleaner, but it wasn't worth rewriting history for.

### Git Commit Messages

Resolve merge conflict doesn't say what conflicted or how you resolved it, and it's the one commit in this checklist where that matters most. A better message would be something like "Resolve CountWords conflict: keep empty-input guard and whitespace split; drop redundant Trim".

Remove any empty whitespace from CountWords function doesn't describe what the change does. The change splits on all whitespace (tabs, newlines, repeated spaces), and the message should say so.

## C# compared with TypeScript

| Topic | TypeScript | C# | What caught me out |
|---|---|---|---|
| Null checks | `strictNullChecks` gives compile errors | Nullable reference types give compile *warnings* (CS8600, CS8604) | A non-nullable `string` can still be `null` at runtime. The check is only a compiler warning. |
| Parsing numbers | `parseInt("12abc")` returns `12` and `Number("")` returns `0`, without complaint | `int.TryParse(s, out var n)` returns `false` for both | `out` parameters return a second value through an argument. `int.Parse` throws instead, so `TryParse` is the right choice for user input. |
| Map lookup | `map.get(k)` returns `undefined` if the key is missing | `dict[k]` throws `KeyNotFoundException` | Use `TryGetValue` or `GetValueOrDefault`. |
| Map order | `Map` keeps insertion order, and the spec guarantees it | `Dictionary` order is **not** guaranteed | My first top-words version relied on it. Ordered results need a list. |
| Array methods vs LINQ | `map`/`filter`/`sort` run straight away, and `sort` changes the array in place | LINQ is lazy and returns new sequences | Finish with `.ToList()`, or the query runs again every time something loops over it. |
| Sorting strings | `localeCompare` is opt-in | `ThenBy(x => x)` is culture-sensitive by default | Pass `StringComparer.Ordinal` for plain character order. |
| Lowercasing | `toLowerCase()` ignores locale | `ToLower()` uses the machine's culture (the Turkish `I` problem) | Use `ToLowerInvariant()`. |
| String length | UTF-16 code units | UTF-16 code units | The same in both: an emoji counts as 2. |
| Building strings | Immutable | Immutable | `+=` in a loop creates a new string every time. `StringBuilder` is the C# fix. |
| Errors | `catch (e)` catches anything, then you check with `instanceof` | `catch (IOException e)` matches by type, and `when` filters | `UnauthorizedAccessException` is **not** an `IOException`, so it isn't caught. |
| Console output | `console.log` / `console.error` | `Console.WriteLine` / `Console.Error.WriteLine` | Errors on stderr stay visible when the report is redirected to a file. |
| Program structure | Functions exported from a module | `static class` with `static` methods. Top-level statements make Program.cs read like a script. | Top-level statements can't be called from a unit test. |
| Small result types | `{ word, count }` objects | Named value tuples, `(string Word, int Count)` | Tuples compare by value, so `Assert.Equal` works on a whole list. |

## What I'd do differently

*If you started this project again tomorrow.*

## Takeaways for Phase 1

*What you'll carry forward, and the gaps you want to close next.*
