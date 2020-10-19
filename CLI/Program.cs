using System;
using System.IO;
using System.Linq;
using BLL;

namespace CLI
{
    class Program
    {
        private enum ExitCodes : int
        {
            OK             = 0,
            MissingOption  = 1,
            TooManyOptions = 2,
            EmptyOption    = 3,
            FileNotFound   = 4,
            EmptyFile      = 5,
            ParseError     = 6,
        };

        private const string HELP_MESSAGE = @"
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

";


        private static void Main(string[] args)
        {
            var meabyOptions = HandleArgs(args);
            if (meabyOptions == null)
                return;

            var options = meabyOptions.Value;

            if (HandleHelpOption(options.option))
                return;

            if (HandleVersionOption(options.option))
                return;

            HandleFilePath(options.filePath);
        }

        private static (string option, string filePath)? HandleArgs(string[] args)
        {
            // we are expecting only one argument
            if (args.Length == 0)
            {
                Environment.ExitCode = (int)ExitCodes.MissingOption;
                Console.Error.WriteLine("No option specified");
                Console.WriteLine(HELP_MESSAGE);
                return null;
            }

            if (args.Length > 1)
            {
                Environment.ExitCode = (int)ExitCodes.TooManyOptions;
                Console.Error.WriteLine($"Too many options specified: {args.Length}");
                Console.WriteLine(HELP_MESSAGE);
                return null;
            }


            string option = args[0].Trim();

            if (option == "")
            {
                Environment.ExitCode = (int)ExitCodes.EmptyOption;
                Console.Error.WriteLine("Given option is empty");
                Console.WriteLine(HELP_MESSAGE);
                return null;
            }

            return (
                option: option.ToLower(),
                // file path is just an option but without ToLower
                filePath: option
            );
        }

        private static bool HandleHelpOption(string option)
        {
            bool showHelp = option == "/?"  ||
                            option == "-?"  ||
                            option == "-h"  ||
                            // this is common error so we will handle it
                            option == "--h" ||
                            option == "--help";

            if (showHelp == false)
                return false;

            Console.WriteLine(HELP_MESSAGE);
            return true;
        }


        private static bool HandleVersionOption(string option)
        {
            bool showVersion = option == "-v"  ||
                               // this is common error so we will handle it
                               option == "--v" ||
                               option == "--version";

            if (showVersion == false)
                return false;

            var assemblyName = typeof(Program).Assembly.GetName();
            var version = assemblyName.Version;
            var versionString = $"{version.Major}." +
                                $"{version.Minor}." +
                                $"{version.Build}." +
                                $"{version.MinorRevision}";

            Console.WriteLine(versionString);
            return true;
        }

        private static void HandleFilePath(string path)
        {
            if (File.Exists(path) == false)
            {
                Environment.ExitCode = (int)ExitCodes.FileNotFound;
                Console.Error.WriteLine($"File not found: '{path}'");
                return;
            }

            // Prepare file lines, but only trim them - do not remove any!
            // We want to have accurate line count number in the potential
            // parsing error message.
            string[] lines = File.ReadAllLines(path)
                .Select(l => l.Trim())
                .ToArray();

            if (lines.Length == 0)
            {
                Environment.ExitCode = (int)ExitCodes.EmptyFile;
                Console.Error.WriteLine($"Given file is empty: '{path}'");
                return;
            }

            try
            {
                var parser = new Parser();
                var parserResult = parser.ParsePackagesDescription(lines);

                var validator = new Validator();
                var areValid = validator.ValidateDependencies(
                    parserResult.PackagesToInstall,
                    parserResult.PackagesDependencies);

                Console.WriteLine(areValid ? "PASS" : "FAIL");
            }
            catch (FormatException ex)
            {
                Environment.ExitCode = (int)ExitCodes.ParseError;
                Console.Error.WriteLine($"Error while parsing file: '{path}'\n" + ex.Message);
            }
        }

    }
}

