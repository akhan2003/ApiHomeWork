using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPR.HomeWork.Api.Models;

namespace SPR.HomeWork.Api.Repository
{
    interface IPersonRepository
    {
        IEnumerable<Person> GetAll();
        Person Add(Person person);
    }
}