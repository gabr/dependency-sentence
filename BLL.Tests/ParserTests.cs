using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;

namespace BLL.Tests
{
    [TestClass]
    public class ParserTests
    {
        // TODO: Add tests for FormatException messages content
        // NOTE(Arek): I could spend all day adding tests but I think it is not
        //             necessary for recruitment process ;)  I left this TODO
        //             comment just FYI so you know that I would test the
        //             messages as well.

        private Parser _parser = null;

        [TestInitialize]
        public void TestInitialization()
        {
            _parser = new Parser();
        }

        private string[] PrepareInputLines(string text)
        {
            return text
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => l != "")
                .ToArray();
        }

        [TestMethod]
        public void ParsesValidPackagesDescription()
        {
            var lines = PrepareInputLines(
            @"
                2
                A,1
                B,1
                3
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");

            var parseResult = _parser.ParsePackagesDescription(lines);

            Assert.IsNotNull(parseResult);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            var expectedParseResult = new ParseResult(
                new Package[]
                {
                    new Package("A", "1"),
                    new Package("B", "1")
                },
                new PackageDependency[]
                {
                    new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }),
                    new PackageDependency(new Package("A", "2"), new [] { new Package("B", "2") }),
                    new PackageDependency(new Package("C", "1"), new [] { new Package("B", "1") })
                });

            Assert.AreEqual(expectedParseResult, parseResult);
        }

        [TestMethod]
        public void ParsesValidPackagesDescription_WithoutDependencies()
        {
            var lines = PrepareInputLines(
            @"
                1
                A,1
            ");

            var parseResult = _parser.ParsePackagesDescription(lines);

            Assert.IsNotNull(parseResult);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            var expectedParseResult = new ParseResult(
                new Package[]
                {
                    new Package("A", "1")
                },
                new PackageDependency[0]);

            Assert.AreEqual(expectedParseResult, parseResult);
        }

        [TestMethod]
        public void ParsesValidPackagesDescription_ZeroPackages()
        {
            var lines = PrepareInputLines(
            @"
                0
            ");

            var parseResult = _parser.ParsePackagesDescription(lines);

            Assert.IsNotNull(parseResult);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            var expectedParseResult = new ParseResult(
                new Package[0],
                new PackageDependency[0]);

            Assert.AreEqual(expectedParseResult, parseResult);
        }

        [TestMethod]
        public void ParsesValidPackagesDescription_Big()
        {
            // build a very large list of packages and dependencies
            int count = 100000;
            var lines = new List<string>();
            var packages = new List<Package>();
            var packagesDependencies = new List<PackageDependency>();

            lines.Add(count.ToString());
            for (int i = 0; i < count; i++)
            {
                lines.Add("A,1");
                packages.Add(new Package("A", "1"));
            }

            lines.Add(count.ToString());
            for (int i = 0; i < count; i++)
            {
                lines.Add("A,1,B,1");
                packagesDependencies.Add(new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }));
            }

            var parseResult = _parser.ParsePackagesDescription(lines.ToArray());

            Assert.IsNotNull(parseResult);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            var expectedParseResult = new ParseResult(
                packages.ToArray(),
                packagesDependencies.ToArray());

            Assert.AreEqual(expectedParseResult, parseResult);
        }

        [TestMethod]
        public void ParsesValidPackagesDescription_LongLines()
        {
            // build a input with very long lines
            int linesCount = 10;
            int dependenciesCount = 100000;
            var lines = new List<string>();
            var packages = new List<Package>();
            var packagesDependencies = new List<PackageDependency>();

            lines.Add(linesCount.ToString());
            for (int i = 0; i < linesCount; i++)
            {
                lines.Add("A,1");
                packages.Add(new Package("A", "1"));
            }

            var sb = new StringBuilder();
            var dependencyPackages = new List<Package>();
            sb.Append("A,1,");
            for (int i = 0; i < dependenciesCount; i++)
            {
                sb.Append("A,1,");
                dependencyPackages.Add(new Package("A", "1"));
            }
            sb.Remove(sb.Length - 1, 1);

            var dependenciesLine = sb.ToString();
            var packageDependency = new PackageDependency(new Package("A", "1"), dependencyPackages.ToArray());

            lines.Add(linesCount.ToString());
            for (int i = 0; i < linesCount; i++)
            {
                lines.Add(dependenciesLine);
                packagesDependencies.Add(packageDependency);
            }

            var parseResult = _parser.ParsePackagesDescription(lines.ToArray());

            Assert.IsNotNull(parseResult);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            var expectedParseResult = new ParseResult(
                packages.ToArray(),
                packagesDependencies.ToArray());

            Assert.AreEqual(expectedParseResult, parseResult);
        }

        [TestMethod]
        public void ParsesValidPackagesDescription_EmptyLinesAtTheBeginning()
        {
            var lines =
            @"


                2
                A,1
                B,1
                3
                A,1,B,1
                A,2,B,2
                C,1,B,1"
                .Split('\n')
                .Select(l => l.Trim())
                .ToArray();

            var parseResult = _parser.ParsePackagesDescription(lines);

            Assert.IsNotNull(parseResult);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            var expectedParseResult = new ParseResult(
                new Package[]
                {
                    new Package("A", "1"),
                    new Package("B", "1")
                },
                new PackageDependency[]
                {
                    new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }),
                    new PackageDependency(new Package("A", "2"), new [] { new Package("B", "2") }),
                    new PackageDependency(new Package("C", "1"), new [] { new Package("B", "1") })
                });

            Assert.AreEqual(expectedParseResult, parseResult);
        }

        [TestMethod]
        public void ParsesValidPackagesDescription_EmptyLinesAtTheEnd()
        {
            var lines =
            @"  2
                A,1
                B,1
                3
                A,1,B,1
                A,2,B,2
                C,1,B,1



            "
                .Split('\n')
                .Select(l => l.Trim())
                .ToArray();

            var parseResult = _parser.ParsePackagesDescription(lines);

            Assert.IsNotNull(parseResult);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            var expectedParseResult = new ParseResult(
                new Package[]
                {
                    new Package("A", "1"),
                    new Package("B", "1")
                },
                new PackageDependency[]
                {
                    new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }),
                    new PackageDependency(new Package("A", "2"), new [] { new Package("B", "2") }),
                    new PackageDependency(new Package("C", "1"), new [] { new Package("B", "1") })
                });

            Assert.AreEqual(expectedParseResult, parseResult);
        }

        [TestMethod]
        public void ParsesValidPackagesDescription_EmptyLinesBetweenChunks()
        {
            var lines =
            @"  2
                A,1
                B,1


                3
                A,1,B,1
                A,2,B,2
                C,1,B,1"
                .Split('\n')
                .Select(l => l.Trim())
                .ToArray();

            var parseResult = _parser.ParsePackagesDescription(lines);

            Assert.IsNotNull(parseResult);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            var expectedParseResult = new ParseResult(
                new Package[]
                {
                    new Package("A", "1"),
                    new Package("B", "1")
                },
                new PackageDependency[]
                {
                    new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }),
                    new PackageDependency(new Package("A", "2"), new [] { new Package("B", "2") }),
                    new PackageDependency(new Package("C", "1"), new [] { new Package("B", "1") })
                });

            Assert.AreEqual(expectedParseResult, parseResult);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_NegativeChunkSize()
        {
            var lines = PrepareInputLines(
            @"
                -1
                A,1
                B,1
                3
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_FirstSeciont_ChungSizeIsNotANumber()
        {
            var lines = PrepareInputLines(
            @"
                N
                A,1
                B,1
                3
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_FirstSeciont_WrongChunkSize_TooSmall()
        {
            var lines = PrepareInputLines(
            @"
                0
                A,1
                B,1
                3
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_FirstSeciont_WrongChunkSize_LargerThenLinesCount()
        {
            var lines = PrepareInputLines(
            @"
                100
                A,1
                B,1
                3
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_FirstSeciont_WrongChunkSize_LargerThenChunkSize()
        {
            var lines = PrepareInputLines(
            @"
                4
                A,1
                B,1
                3
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_SecondSeciont_ChungSizeIsNotANumber()
        {
            var lines = PrepareInputLines(
            @"
                2
                A,1
                B,1
                N
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_SecondSeciont_WrongChunkSize_TooSmall()
        {
            var lines = PrepareInputLines(
            @"
                2
                A,1
                B,1
                0
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_SecondSeciont_WrongChunkSize_TooLarge()
        {
            var lines = PrepareInputLines(
            @"
                2
                A,1
                B,1
                4
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_WrongPackageFormat()
        {
            var lines = PrepareInputLines(
            @"
                2
                A,1,C,1
                B,1
                4
                A,1,B,1
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_WrongNumberOfCommas()
        {
            var lines = PrepareInputLines(
            @"
                2
                A,1
                B,1
                3
                A,1,B,1,
                A,2,B,2
                C,1,B,1
            ");
            var parseResult = _parser.ParsePackagesDescription(lines);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ThrowsOnInvalidPackagesDescriptionFormat_EmptyLine()
        {
            var lines =
                @"
                    2
                    A,1
                    B,1
                    4
                    A,1,B,1,

                    A,2,B,2
                    C,1,B,1
                "
                .Split('\n')
                .Select(l => l.Trim())
                .ToArray();

            var parseResult = _parser.ParsePackagesDescription(lines);
        }


    }
}
