# Phase 0 Project - Text Analyser

## Prerequisites

- .NET 10 SDK

## .NET CLI Commands used to generate the project

`dotnet new sln -n TextAnalyser`

This creates the solution file for the project and names it "TextAnalyser". On the .NET 10 SDK this produces `TextAnalyser.slnx` (older SDKs produce `TextAnalyser.sln`)

`dotnet new console -n TextAnalyser`

This creates a new console app using the template and places it in the directory TextAnalyser

`dotnet new xunit -n TextAnalyser.Tests`

This creates the testing project, using the xUnit library and places it inside the TextAnalyser.Tests directory

`dotnet sln add TextAnalyser TextAnalyser.Tests`

Adds both the console app and the test project to the solution file

`dotnet add TextAnalyser.Tests reference TextAnalyser`

Has the test project reference the console app so the tests can run against this source code

`dotnet new gitignore`

Creates a `.gitignore` file with the most common .NET files that should not be pushed upstream

## .NET CLI commands to use to build/run/restore/test project

Run these from the solution root (the directory containing `TextAnalyser.slnx`)

`dotnet restore`

Restores the NuGet packages being used by this solution

`dotnet build`

Restores the NuGet packages and then builds the console app and the test project .dll files

`dotnet run --project TextAnalyser`

Restores the NuGet packages, builds the console app and then executes the program. `--project` is required because `dotnet run` cannot run a solution file. Arguments for the program go after `--`, e.g. `dotnet run --project TextAnalyser -- sample.txt --top 5`

`dotnet test`

Restores the NuGet packages, builds both console app and test projects and runs the tests in the test project
