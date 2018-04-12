using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPR.HomeWork.Api.Controllers;
using SPR.HomeWork.Api.Models;
using SPR.HomeWork.Api.Repository;
using SPR.HomeWork.Api.Constants;

namespace SPR.HomeWork.Api.Tests
{
    [TestClass]
    public class TestPersonController
    {
        [TestMethod]
        public void GetAllPersons()
        {            
            var controller = new PersonController();

            var testPersons = GetTestPersons();
            var result = controller.GetAllPersons() as List<Person>;
            Assert.AreEqual(testPersons.Count, result.Count);
        }

        public List<Person> GetTestPersons()
        {
            var testPersons = new List<Person>();
            testPersons.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Schmoe", Gender = "Male", FavoriteColor = "Red", DateOfBirth = "10/1/1980" });
            testPersons.Add(new Person { Id = 1, FirstName = "Jane", LastName = "Schmoe", Gender = "Female", FavoriteColor = "Blue", DateOfBirth = "10/1/1982" });
            testPersons.Add(new Person { Id = 1, FirstName = "Steve", LastName = "Jobs", Gender = "Male", FavoriteColor = "Blue", DateOfBirth = "10/1/1960" });

            return testPersons;
        }

    }
}
