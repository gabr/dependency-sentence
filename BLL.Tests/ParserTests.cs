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

        [TestMethod]
        public void ParsesValidPackagesDescription()
        {
            var parseResult = _parser.ParsePackagesDescription(@"
                    2
                    A,1
                    B,1
                    3
                    A,1,B,1
                    A,2,B,2
                    C,1,B,1
                ".Split('\n'));

            Assert.IsNull(parseResult.SyntaxError);
            Assert.IsNotNull(parseResult.PackagesToInstall);
            Assert.IsNotNull(parseResult.PackagesDependencies);

            Assert.AreEqual(2, parseResult.PackagesToInstall.Length);
            Assert.AreEqual(3, parseResult.PackagesDependencies.Length);


            Assert.AreEqual("A", parseResult.PackagesToInstall[0].Name);
            Assert.AreEqual("1", parseResult.PackagesToInstall[0].Version);

            Assert.AreEqual("B", parseResult.PackagesToInstall[1].Name);
            Assert.AreEqual("1", parseResult.PackagesToInstall[1].Version);


            Assert.AreEqual("A", parseResult.PackagesDependencies[0].Package.Name);
            Assert.AreEqual("1", parseResult.PackagesDependencies[0].Package.Version);
            Assert.AreEqual(1,   parseResult.PackagesDependencies[0].Dependencies.Length);
            Assert.AreEqual("B", parseResult.PackagesDependencies[0].Dependencies[0].Name);
            Assert.AreEqual("1", parseResult.PackagesDependencies[0].Dependencies[0].Version);

            Assert.AreEqual("A", parseResult.PackagesDependencies[0].Package.Name);
            Assert.AreEqual("2", parseResult.PackagesDependencies[0].Package.Version);
            Assert.AreEqual(1,   parseResult.PackagesDependencies[0].Dependencies.Length);
            Assert.AreEqual("B", parseResult.PackagesDependencies[0].Dependencies[0].Name);
            Assert.AreEqual("2", parseResult.PackagesDependencies[0].Dependencies[0].Version);

            Assert.AreEqual("C", parseResult.PackagesDependencies[0].Package.Name);
            Assert.AreEqual("1", parseResult.PackagesDependencies[0].Package.Version);
            Assert.AreEqual(1,   parseResult.PackagesDependencies[0].Dependencies.Length);
            Assert.AreEqual("B", parseResult.PackagesDependencies[0].Dependencies[0].Name);
            Assert.AreEqual("2", parseResult.PackagesDependencies[0].Dependencies[0].Version);
        }

    }
}
