using System;
using System.Linq;
using System.Collections.Generic;

namespace BLL
{
    /// <summary>
    /// Allows to parse packages description from strings to objects representation.
    /// </summary>
    public class Parser
    {
        public Parser() { }

        /// <summary>
        /// Parses given strings into objects.
        ///
        /// Given strings should contain two chunks of data about packages.
        /// Each chunk should be preceded with a number telling how many of
        /// packages it contains.  First chunk should list packages to install
        /// and the second one should describe those packages dependencies.
        ///
        /// The second chunk with packages dependencies is not required.
        ///
        /// Example lines:
        /// <para>
        /// 2
        /// A,1
        /// B,1
        /// 3
        /// A,1,B,1
        /// A,2,B,2
        /// C,1,B,1
        /// </para>
        ///
        /// This method does not allow for empty lines.
        /// </summary>
        /// <exception cref="FormatException">thrown when format is invalid</exception>
        public ParseResult ParsePackagesDescription(string[] lines)
        {
            if (lines == null) throw new ArgumentNullException(nameof(lines));

            var linesQueue = new Queue<string>(lines);

            try
            {
                // parse packages to install chunk - it is required and should always be present
                var packagesToInstall = ParsePackagesToInstall(linesQueue);

                var packagesDependencies = new PackageDependency[0];
                // do we have packages dependencies chunk?
                if (linesQueue.Count > 0)
                    packagesDependencies = ParsePackagesDependencies(linesQueue);

                if (linesQueue.Count > 0)
                    throw new FormatException($"{linesQueue.Count} lines left after parsing - probably wrong chunks sizes");

                return new ParseResult(packagesToInstall, packagesDependencies);
            }
            // we expect that exception if we exceed the queue
            catch (InvalidOperationException ioex)
            {
                throw new FormatException($"Expected next line but found nothing after {lines.Length} line", ioex);
            }
            catch (FormatException fex)
            {
                int errorLine = lines.Length - linesQueue.Count;
                string line = lines[errorLine - 1];
                throw new FormatException($"Parse error at line {errorLine}: '{fex.Message}', line: '{line}'", fex);
            }
        }

        /// <summary>
        /// Parses chunk of data with packages to install.
        /// </summary>
        /// <exception cref="FormatException">thrown when format is invalid</exception>
        private Package[] ParsePackagesToInstall(Queue<string> linesQueue)
        {
            return ParseNextPackagesChunk(linesQueue)
                .Select(packages =>
                {
                    if (packages.Length != 1)
                        throw new FormatException($"Expected 1 package description but found: {packages.Length}");

                    return packages[0];
                })
                .ToArray();
        }

        /// <summary>
        /// Parses chunk of data with packages dependencies.
        /// </summary>
        /// <exception cref="FormatException">thrown when format is invalid</exception>
        private PackageDependency[] ParsePackagesDependencies(Queue<string> linesQueue)
        {
            return ParseNextPackagesChunk(linesQueue)
                .Select(packages =>
                {
                    if (packages.Length < 2)
                        throw new FormatException($"Expected at least 2 packages descriptions but found: {packages.Length}");

                    return new PackageDependency(
                        packages[0],
                        packages.Skip(1).ToArray());
                })
                .ToArray();
        }

        /// <summary>
        /// Tries to get next chunk of data as list of Packages arrays.
        /// First line should contain number representing chunk size.
        /// Then each next line is converted into array of Packages and yielded.
        /// </summary>
        /// <exception cref="FormatException">thrown when format is invalid</exception>
        private IEnumerable<Package[]> ParseNextPackagesChunk(Queue<string> linesQueue)
        {
            // first we get the number of lines in next chunk of data
            string line = linesQueue.Dequeue();
            int chunkSize;

            if (int.TryParse(line, out chunkSize) == false)
                throw new FormatException("Expected number");

            if (chunkSize < 0)
                throw new FormatException($"Given chunk size is negative: {chunkSize}");

            // if we got it correctly then we parse and
            // return each line as array of packages
            for (int i = 0; i < chunkSize; i++)
            {
                line = linesQueue.Dequeue();
                yield return ParsePackages(line);
            }
        }

        /// <summary>
        /// Extracts Packages objects from given string.
        /// </summary>
        /// <exception cref="FormatException">thrown when format is invalid</exception>
        private Package[] ParsePackages(string packagesString)
        {
            // empty lines are not accepted
            if (string.IsNullOrWhiteSpace(packagesString))
                throw new FormatException($"Expected packages description but line is empty");

            // example line: A,1,B,1
            const char delimiter = ',';
            var split = packagesString.Split(delimiter);

            // we expect even number of elements and odd number of delimiters
            if (split.Length % 2 != 0)
                throw new FormatException($"Wrong number of delimiters '{delimiter}': {split.Length - 1}");

            var packages = new Package[split.Length / 2];
            for (int i = 0; i < packages.Length; i++)
                packages[i] = new Package(
                    split[i * 2],
                    split[i * 2 + 1]);

            return packages;
        }
    }
}

