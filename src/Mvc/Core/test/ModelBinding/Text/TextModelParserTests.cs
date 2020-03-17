using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NLib.AtpNetCore.Mvc.ModelBinding.Text.Tests
{
    [TestClass()]
    public class TextModelParserTests
    {
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void ParseTextTest()
        {
            TextModelParser.ParseText(null);
        }

        [TestMethod()]
        public void ParseTextTest1()
        {
            var input = "Example";
            var output = TextModelParser.ParseText(input);

            Assert.AreEqual(input, output.Format);
            Assert.IsNull(output.Inserts);
        }

        [TestMethod()]
        public void ParseTextTest2()
        {
            var input = "<Example>";
            var output = TextModelParser.ParseText(input);

            Assert.AreEqual("{0}", output.Format);
            Assert.IsNotNull(output.Inserts);
            Assert.AreEqual(1, output.Inserts.Length);
            Assert.AreEqual("Example", output.Inserts[0]);
        }



        [TestMethod()]
        public void ParseTextTest3()
        {
            var input = "1<Example>";
            var output = TextModelParser.ParseText(input);

            Assert.AreEqual("1{0}", output.Format);
            Assert.IsNotNull(output.Inserts);
            Assert.AreEqual(1, output.Inserts.Length);
            Assert.AreEqual("Example", output.Inserts[0]);
        }

        [TestMethod()]
        public void ParseTextTest4()
        {
            var input = "<Example>1";
            var output = TextModelParser.ParseText(input);

            Assert.AreEqual("{0}1", output.Format);
            Assert.IsNotNull(output.Inserts);
            Assert.AreEqual(1, output.Inserts.Length);
            Assert.AreEqual("Example", output.Inserts[0]);
        }
    }
}