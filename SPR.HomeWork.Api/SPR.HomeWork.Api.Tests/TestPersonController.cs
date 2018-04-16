﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPR.HomeWork.Api.Controllers;
using SPR.HomeWork.Models;
using SPR.HomeWork.Repository;
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
            testPersons.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Schmoe", Gender = "Male", FavoriteColor = "Red", DateOfBirth = new DateTime(1983, 1, 18) });
            testPersons.Add(new Person { Id = 1, FirstName = "Jane", LastName = "Schmoe", Gender = "Female", FavoriteColor = "Blue", DateOfBirth = new DateTime(1982, 1, 18) });
            testPersons.Add(new Person { Id = 1, FirstName = "Steve", LastName = "Jobs", Gender = "Male", FavoriteColor = "Blue", DateOfBirth = new DateTime(1980, 1, 18)});

            return testPersons;
        }

    }
}
