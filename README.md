# Dependency Sentence

A simple software packages dependencies checker.

Table of contents:
1. [Dependency Sentence](#dependency-sentence)
   * [What does it do?](#what-does-it-do)
   * [Using the CLI](#using-the-cli)
2. [System requirements](#system-requirements)
   * [Supported operating systems](#supported-operating-systems)
   * [.NET Core 3.0.1 SDK](#net-core-301-sdk)
3. [Project setup and startup](#project-setup-and-startup)
4. [Design](#design)


## What does it do

The implemented library allows to analyze software package dependencies.

**The main purpose is to check if given dependencies description is valid.**

A system with package dependencies is described like this:

    2
    A,1
    B,1
    3
    A,1,B,1
    A,2,B,2
    C,1,B,1

* The first line is the number `N` of packages to install.
* The next `N` lines are packages to install. These are in the form `p,v` where `p` is a package name and `v` is a version that needs to be installed.
* The next line is the number `M` of dependencies
* The following `M` lines are of the form `p1,v1,p2,v2` indicating that package `p1` in version `v1` depends on `p2` in version `v2`.
* Packages and version are guaranteed to not contain `,` characters.

If more than one version of a package is required the installation is **invalid**.

The sample input above is valid, but the sample input below is invalid as `A,1` requires `B,1` and we are trying to install `B,2`:

    2
    A,1
    B,2
    3
    A,1,B,1
    A,2,B,2
    C,1,B,1


## Using the CLI

The dependencies checker is accessible via a Command Line Interface CLI.

The program `CLI.exe` can be run in command line to verify given software package dependencies file description.

### Help

When run without options will output help information:

    > .\publish\CLI.exe
    No option specified

    CLI:
      Command line interface for packages dependencies validation.

    Usage:
      CLI <file_path>

    Returns on standard output:
        PASS - When packages dependencies configuration is valid.
        FAIL - When packages dependencies configuration is invalid.

    Errors:
        Parsing errors and others are printed to standard error.

    Options:
      <file_path>          Path to the file with dependencies description.
      -v, --version        Show version information
      /?, -?, -h, --help   Show help and usage information

### Testing configuration

To verify a configuration file simply specify a path to it:

    > .\publish\CLI.exe .\testdata\input000.txt
    PASS
    > .\publish\CLI.exe .\testdata\input001.txt
    FAIL

The program will output `PASS` or `FAIL` messages to indicate if given configuration is valid or no.

### Syntax errors

In case of invalid syntax in dependencies description file the program will output message with description about the error on standard error:

    > .\publish\CLI.exe .\testdata\syntaxError.txt
    Error while parsing file: '.\testdata\syntaxerror.txt'
    Parse error at line 2: 'Wrong number of delimiters ',': 2', line: 'P1,42,'


# System requirements

## Supported operating systems

| OS               | Version       | Architectures |
|------------------|---------------|---------------|
|Windows Client    | 7 SP1+, 8.1   | x64, x86      |
|Windows 10 Client | Version 1607+ | x64, x86      |
|Windows Server    | 2012 R2+      | x64, x86      |
|Nano Server       | Version 1803+ | x64, ARM32    |

More at: [system requirements details]

## .NET Core 3.1 SDK

The .NET Core in version 3.1 is required to compile and run the project.

[.NET Core 3.1 SDK Download]

Instal the SDK and check if it works by typing in the console:

    dotnet --version

The result of the command should be value `3.1.201` or higher.


# Project setup and startup

After installing .NET Core 3.1 SDK go into project directory and run the `publish.bat` script.

Now in the newly created `publish` directory will be `CLI.exe` executable which can be used according to [Using the CLI](#using-the-cli) section.

# Design

## Project structure

    │   build.bat    # builds whole solution
    │   test.bat     # runs tests
    │   clean.ps1    # removes build files
    │   publish.bat  # publishes CLI.exe
    |
    │   dependency-sentence.sln
    │   README.md
    |
    |   # Business Logic Layer
    |   # Contains all the logic about parsing and
    |   # validating the packages dependencies files.
    ├───BLL
    │       BLL.csproj
    │       Package.cs
    │       PackageDependency.cs
    │       Parser.cs
    │       ParseResult.cs
    │       Validator.cs
    │
    |   # Unit tests for BLL
    ├───BLL.Tests
    │       BLL.Tests.csproj
    │       ParserTests.cs
    │       ValidatorTests.cs
    │
    |   # The simple command line "front-end" to the BLL library
    └───CLI
            CLI.csproj
            Program.cs


## Design questions & decisions

1. Why `.bat` scripts?  
   As a simple form of documentation how to perform certain actions in the project.  
   They are also used by my editor to quickly build and test the projects.  
   In case of more complex automation tasks I use PowerShell scripts instead of `.bat` ones.
2. Why `string` type for storing version of a package?  
   In my experience the software version number are rarely **numbers**.  
   Often they contain more then single dot or an suffix like `-alpha` etc.
3. Why name of the package is not forced to be lower case or upper case?  
   In my opinion the name `package` and `PACKage` are different names therefore my solution is case sensitive for packages names.
4. Why there are no documentation comments for the methods of `Package.cs`, `PackageDependency.cs` and `ParseResult.cs`?  
   Those classes are simple value objects and contains only well known overloaded methods and operators.
5. Why `Parser.cs` and `Validator.cs` are classes if they do not contain any state?  Why not static classes or different solution?  
   For now it is very simple system but when they grow there is always need to add come kind of configuration or additional dependencies to the classes.  
   Having them as regular classes which have to be instantiated before use we have the ability to use them in dependency injection system or to add them configuration parameters like i.e. parameter to change the delimiters from commas `,` into something else in the parser.  The `Validator.cs` could have a configuration specifying which validation rules to use etc.  
   It is easy to create static class/methods but hard to go back after system grows and multiple different instances are necessary.
6. Why validator method `ValidateDependencies` returns simple `bool` instead of a `ValidationResult` object?  
   I would do it like so if there was a requirement for an error message explaining why given configuration is invalid.
   But in this current system simple `bool` is enough.  
   I left `TODO` comment about that in the `Validator.cs` class.
7. Why `DetermineAllPackagesToInstall()` is a function and not a separate system/class?  
   I would for sure separate validation from determining packages to install if there was a need to actually install the packages.
   In that case a different system would need to have that ability so I would separate those functionalities and validator would receive this functionality as a dependency in constructor.
8. Why allowing for empty lines in files?  
   Because you left a few in your test files.  
   Also I think it would be to strict of a requirement to enforce no empty lines in real system.
9. Why comma delimiter is hardcoded?  
   I added the possibility to configure the delimiter but not commited it.  
   The syntax given in requirements is weird because the delimiter between packages is the same as delimiter between package name and version.
   If the format would be specified as: `A,1;B,1` I would for sure add the possibility to change to specify the separator in the parser.  
   But the situation when the separator between packages and package name and version can be the same the completely different algorithm is required and it was much less readable therefore I reverted it to the current simpler version.  
10. Why using `FormatException` instead of custom one?  
    It fits perfectly the given scenario and it is always better to use well known framework exceptions then to create new ones just to have a different name in stack trace.  
    I acctualy started with the custom parse error class but removed it (see commit dafe2df).
11. Why merge with `squash`?  
    So you can easily see the branches structure - command `git log --all --graph --oneline`.  
    Also that is just my style, ofc I can addapt to Your specification.  
12. Why this project took 2 days?  
    It did not.  I was working on it in the span of 2 days, but only a few hours each day.
13. Why do you handle command line arguments by hand instead of using library?  
    The current state of libraries for command line arguments handling in .NET Core is IMO a mess.  
    Therefore for such a simple example I handled them myself.


[system requirements details]: https://github.com/dotnet/core/blob/master/release-notes/3.1/3.1-supported-os.md
[.NET Core 3.1 SDK Download]: https://dotnet.microsoft.com/download/dotnet-core/3.1

