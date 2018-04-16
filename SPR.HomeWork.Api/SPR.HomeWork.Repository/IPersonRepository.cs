using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPR.HomeWork.Models;

namespace SPR.HomeWork.Repository
{
    interface IPersonRepository
    {
        IEnumerable<Person> GetAll();
        Person Get(int id);
        Person Add(Person person);
    }
}