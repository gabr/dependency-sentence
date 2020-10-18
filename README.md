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

TODO

[system requirements details]: https://github.com/dotnet/core/blob/master/release-notes/3.1/3.1-supported-os.md
[.NET Core 3.1 SDK Download]: https://dotnet.microsoft.com/download/dotnet-core/3.1

