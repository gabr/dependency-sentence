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
        public void Test1()
        {
        }

    }
}
