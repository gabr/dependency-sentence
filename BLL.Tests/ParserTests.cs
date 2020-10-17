using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;

namespace BLL.Tests
{
    [TestClass]
    public class ParserTests
    {
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
    }
}
