using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using SPR.HomeWork.Client;
using System.Collections.Generic;
using SPR.HomeWork.Models;
using System.Linq;
using System.IO;
using System.Net.Http;

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

        [TestMethod]
        public void TestSortListByGenderAndLastName()
        {
            var testPersons = new List<Person>();
            testPersons.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Adams", Gender = "Male", FavoriteColor = "Red", DateOfBirth = new DateTime(1983, 1, 18) });
            testPersons.Add(new Person { Id = 1, FirstName = "Jane", LastName = "Black", Gender = "Female", FavoriteColor = "Blue", DateOfBirth = new DateTime(1982, 1, 18) });
            testPersons.Add(new Person { Id = 1, FirstName = "Steve", LastName = "Charlie", Gender = "Male", FavoriteColor = "Blue", DateOfBirth = new DateTime(1980, 1, 18) });

            var actual = FileReader.SortList(testPersons, "Gender");

            var expected = testPersons.OrderBy(person => person.Gender).ThenBy(person => person.LastName).ToList();

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void TestSortListByLastNameDescending()
        {
            var testPersons = new List<Person>();
            testPersons.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Adams", Gender = "Male", FavoriteColor = "Red", DateOfBirth = new DateTime(1983, 1, 18) });
            testPersons.Add(new Person { Id = 1, FirstName = "Jane", LastName = "Black", Gender = "Female", FavoriteColor = "Blue", DateOfBirth = new DateTime(1982, 1, 18) });
            testPersons.Add(new Person { Id = 1, FirstName = "Steve", LastName = "Charlie", Gender = "Male", FavoriteColor = "Blue", DateOfBirth = new DateTime(1980, 1, 18) });

            var actual = FileReader.SortList(testPersons, "Name");

            var expected = testPersons.OrderByDescending(person => person.LastName).ToList();

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void TestSortListByDOBAcsending()
        {
            var testPersons = new List<Person>();
            testPersons.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Adams", Gender = "Male", FavoriteColor = "Red", DateOfBirth = new DateTime(1983, 1, 18) });
            testPersons.Add(new Person { Id = 1, FirstName = "Jane", LastName = "Black", Gender = "Female", FavoriteColor = "Blue", DateOfBirth = new DateTime(1982, 1, 18) });
            testPersons.Add(new Person { Id = 1, FirstName = "Steve", LastName = "Charlie", Gender = "Male", FavoriteColor = "Blue", DateOfBirth = new DateTime(1980, 1, 18) });

            var actual = FileReader.SortList(testPersons, "DOB");

            var expected = testPersons.OrderBy(person => person.DateOfBirth).ToList();

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void TestShowPerson()
        {

            var currentConsoleOut = Console.Out;
            Person person = new Person { Id = 1, FirstName = "Jane", LastName = "Blake", Gender = "Female", FavoriteColor = "Blue", DateOfBirth = new DateTime(1989, 1, 18) };
            string expected = "FirstName: Jane\tLastName: Blake\tGender: Female\tFavoriteColor: Blue\tDateOfBirth: 1/18/1989\n";

            using (var consoleOutput = new ConsoleOutput())
            {
                FileReader.ShowPerson(person);
                Assert.AreEqual(expected, consoleOutput.GetOuput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);

        }

        [TestMethod]
        public void TestShowPersons()
        {

            var currentConsoleOut = Console.Out;

            var testPersons = new List<Person>();

            testPersons.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Adams", Gender = "Male", FavoriteColor = "Red", DateOfBirth = new DateTime(1983, 1, 18) });
            testPersons.Add(new Person { Id = 2, FirstName = "Jane", LastName = "Blake", Gender = "Female", FavoriteColor = "Blue", DateOfBirth = new DateTime(1982, 1, 18) });


            string expected = "FirstName: Joe\tLastName: Adams\tGender: Male\tFavoriteColor: Red\tDateOfBirth: 1/18/1983\n" + 
                                "FirstName: Jane\tLastName: Blake\tGender: Female\tFavoriteColor: Blue\tDateOfBirth: 1/18/1982\n";

            using (var consoleOutput = new ConsoleOutput())
            {
                FileReader.ShowPersons(testPersons);
                Assert.AreEqual(expected, consoleOutput.GetOuput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);

        }

        [TestMethod]
        public void TestReadFile()
        {
            var testPersons = new List<Person>();

            testPersons = FileReader.ReadFile("D:\\HomeWork\\personfile-pipedelimited.txt");

            Assert.AreEqual(testPersons.Count, 3);
        }

        [TestMethod]
        public void TestGetPersonApi()
        {
            string baseAddress = "http://localhost:52888/";

            HttpClient conn = new HttpClient();
            //Provide the base address of tha API.  Move to config file
            conn.BaseAddress = new Uri(baseAddress);
            conn.DefaultRequestHeaders.Accept.Clear();
            //set Accept Header to send the data in JSON format
            conn.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var currentConsoleOut = Console.Out;
          
            string expected = "FirstName: Joe\tLastName: Schmoe\tGender: Male\tFavoriteColor: Red\tDateOfBirth: 1/18/1980\n" +
                                "FirstName: Jane\tLastName: Schmoe\tGender: Female\tFavoriteColor: Blue\tDateOfBirth: 1/18/1982\n" +
                                "FirstName: Steve\tLastName: Jobs\tGender: Male\tFavoriteColor: Blue\tDateOfBirth: 1/18/1983\n";


            using (var consoleOutput = new ConsoleOutput())
            {
                FileReader.GetPersonsAsync(conn).Wait();
                Assert.AreEqual(expected, consoleOutput.GetOuput());
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);

        }

        //reference:http://www.vtrifonov.com/2012/11/getting-console-output-within-unit-test.html
        public class ConsoleOutput : IDisposable
        {
            private StringWriter stringWriter;
            private TextWriter originalOutput;
            public ConsoleOutput()
            {
                stringWriter = new StringWriter();
                originalOutput = Console.Out;
                Console.SetOut(stringWriter);
            }
            public string GetOuput()
            {
                return stringWriter.ToString();
            }
            public void Dispose()
            {
                Console.SetOut(originalOutput);
                stringWriter.Dispose();
            }
        }
    }
}
