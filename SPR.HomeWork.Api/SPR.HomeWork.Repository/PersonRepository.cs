﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPR.HomeWork.Models;

namespace SPR.HomeWork.Repository
{
    public class PersonRepository: IPersonRepository
    {
        private List<Person> persons = new List<Person>();
        private int _personId = 1;

        public PersonRepository()
        {
            Add(new Person { FirstName = "Joe", LastName = "Schmoe", Gender = "Male", FavoriteColor = "Red", DateOfBirth = new DateTime(1980, 1, 18)});
            Add(new Person { FirstName = "Jane", LastName = "Schmoe", Gender = "Female", FavoriteColor = "Blue", DateOfBirth = new DateTime(1982, 1, 18)});
            Add(new Person { FirstName = "Steve", LastName = "Jobs", Gender = "Male", FavoriteColor = "Blue", DateOfBirth = new DateTime(1983, 1, 18) });
        }

        public IEnumerable<Person> GetAll()
        {
            return persons;
        }

        public Person Get(int id)
        {
            return persons.Find(p => p.Id == id);
        }


        public Person Add(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException("person");
            }

            person.Id = _personId++;
            persons.Add(person);
            return person;
        }

    }
}