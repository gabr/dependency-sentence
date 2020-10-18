using System;
using System.IO;
using BLL;

namespace CLI
{
    class Program
    {
        private enum ExitCodes : int
        {
            OK             = 0,
            MissingOption  = 1,
            TooManyOptions = 1,
            EmptyOption    = 1,
            UnknownOption  = 1,
        };

        private const string HELP_MESSAGE = @"
CLI:
  Command line interface for packages dependencies validation.

Usage:
  CLI <file_path>

Returns on standard output:
    PASS - When packages dependencies configuration is valid.
    FAIL - When packages dependencies configuration is invalid.
           Fail details will be printed on standard error.

Options:
  <file_path>          Path to the file with dependencies description.
  -v, --version        Show version information
  /?, -?, -h, --help   Show help and usage information

";


        private static void Main(string[] args)
        {
            string option = HandleArgs(args);
            if (option == null)
                return;

            if (HandleHelpOption(option))
                return;

            if (HandleVersionOption(option))
                return;

            if (HandleFilePath(option))
                return;

            HandleUnknownOption(option);
        }

        private static string HandleArgs(string[] args)
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


            string option = args[0].Trim().ToLower();

            if (option == "")
            {
                Environment.ExitCode = (int)ExitCodes.EmptyOption;
                Console.Error.WriteLine("Given option is empty");
                Console.WriteLine(HELP_MESSAGE);
                return null;
            }

            return option;
        }

        private static bool HandleHelpOption(string option)
        {
            bool showHelp = option == "/?" ||
                            option == "-?" ||
                            option == "-h" ||
                            option == "--help";

            if (showHelp == false)
                return false;

            Console.WriteLine(HELP_MESSAGE);
            return true;
        }


        private static bool HandleVersionOption(string option)
        {
            bool showVersion = option == "-v" ||
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

        private static void HandleUnknownOption(string option)
        {
            Environment.ExitCode = (int)ExitCodes.UnknownOption;
            Console.Error.WriteLine($"Unknown option: '{option}'");
            Console.WriteLine(HELP_MESSAGE);
        }


        private static bool HandleFilePath(string option)
        {
            throw new NotImplementedException();
        }

    }
}

