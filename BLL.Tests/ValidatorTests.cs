using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;

namespace BLL.Tests
{
    [TestClass]
    public class ValidatorTests
    {
        private Validator _validator = null;

        [TestInitialize]
        public void TestInitialization()
        {
            _validator = new Validator();
        }

        [TestMethod]
        public void AcceptsValidConfiguration_NoPackages()
        {
            Assert.IsTrue(
                _validator.ValidateDependencies(
                    new Package[0],
                    new PackageDependency[0]));
        }

        [TestMethod]
        public void AcceptsValidConfiguration_1()
        {
            Assert.IsTrue(
                _validator.ValidateDependencies(
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
                    }));
        }


        [TestMethod]
        public void AcceptsValidConfiguration_2()
        {
            Assert.IsTrue(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("P1", "42")
                    },
                    new PackageDependency[]
                    {
                        new PackageDependency(new Package("P1", "42"), new [] { new Package("P2", "Beta-1") })
                    }
                ));
        }

        [TestMethod]
        public void AcceptsValidConfiguration_3()
        {
            Assert.IsTrue(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("B", "1")
                    },
                    new PackageDependency[]
                    {
                        new PackageDependency(new Package("B", "1"), new [] { new Package("B", "1") })
                    }
                ));
        }

        [TestMethod]
        public void AcceptsValidConfiguration_4()
        {
            Assert.IsTrue(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("A", "2"),
                        new Package("B", "2")
                    },
                    new PackageDependency[]
                    {
                        new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }),
                        new PackageDependency(new Package("A", "1"), new [] { new Package("B", "2") }),
                        new PackageDependency(new Package("A", "2"), new [] { new Package("C", "3") })
                    }
                ));
        }

        [TestMethod]
        public void AcceptsValidConfiguration_5()
        {
            Assert.IsTrue(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("B", "2")
                    },
                    new PackageDependency[]
                    {
                        new PackageDependency(new Package("A", "1"), new [] { new Package("B", "2") }),
                        new PackageDependency(new Package("B", "2"), new [] { new Package("A", "1") })
                    }
                ));
        }

        [TestMethod]
        public void AcceptsValidConfiguration_6()
        {
            Assert.IsTrue(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("A", "1")
                    },
                    new PackageDependency[0]));
        }

        [TestMethod]
        public void DoesNotAcceptInvalidConfiguration_1()
        {
            Assert.IsFalse(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("A", "1")
                    },
                    new PackageDependency[]
                    {
                        new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }),
                        new PackageDependency(new Package("A", "1"), new [] { new Package("B", "2") })
                    }));
        }

        [TestMethod]
        public void DoesNotAcceptInvalidConfiguration_2()
        {
            Assert.IsFalse(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("B", "2")
                    },
                    new PackageDependency[]
                    {
                        new PackageDependency(new Package("B", "2"), new [] { new Package("A", "1"), new Package("C", "1") }),
                        new PackageDependency(new Package("C", "1"), new [] { new Package("A", "2") })
                    }));
        }

        [TestMethod]
        public void DoesNotAcceptInvalidConfiguration_3()
        {
            Assert.IsFalse(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("A", "2"),
                        new Package("B", "2")
                    },
                    new PackageDependency[]
                    {
                        new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }),
                        new PackageDependency(new Package("A", "1"), new [] { new Package("B", "2") }),
                        new PackageDependency(new Package("A", "2"), new [] { new Package("C", "3") }),
                        new PackageDependency(new Package("C", "3"), new [] { new Package("D", "4") }),
                        new PackageDependency(new Package("D", "4"), new [] { new Package("B", "1") })
                    }));
        }

        [TestMethod]
        public void DoesNotAcceptInvalidConfiguration_4()
        {
            Assert.IsFalse(
                _validator.ValidateDependencies(
                    new Package[]
                    {
                        new Package("A", "1"),
                        new Package("C", "1")
                    },
                    new PackageDependency[]
                    {
                        new PackageDependency(new Package("A", "1"), new [] { new Package("B", "1") }),
                        new PackageDependency(new Package("C", "1"), new [] { new Package("B", "2") })
                    }));
        }
    }
}
