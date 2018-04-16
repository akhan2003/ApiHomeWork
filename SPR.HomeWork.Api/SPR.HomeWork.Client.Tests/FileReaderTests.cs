using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using SPR.HomeWork.Client;

namespace SPR.HomeWork.Client.Tests
{
    [TestClass]
    public class FileReaderTests
    {
        [TestMethod]
        public void TestParseLinePipeDelimited()
        {
            string line = "Tom | Smith | Male | Black | 01/01/1980";

            string[] words = FileReader.ParseLine(line);

            Assert.AreEqual(5, words.Length);
        }

        [TestMethod]
        public void TestParseLineCommaDelimited()
        {
            string line = "Tom,Smith,Male,Black,01/01/1980";

            string[] words = FileReader.ParseLine(line);

            Assert.AreEqual(5, words.Length);
        }

        [TestMethod]
        public void TestParseLineSpaceDelimited()
        {
            string line = "Tom Smith Male Black 01/01/1980";

            string[] words = FileReader.ParseLine(line);

            Assert.AreEqual(5, words.Length);
        }
    }
}
