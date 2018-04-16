using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using SPR.HomeWork.Client;
using System.Collections.Generic;
using SPR.HomeWork.Models;
using System.Linq;

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
    }
}
