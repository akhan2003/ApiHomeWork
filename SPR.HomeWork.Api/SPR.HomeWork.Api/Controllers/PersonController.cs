using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SPR.HomeWork.Models;
using SPR.HomeWork.Repository;
using SPR.HomeWork.Api.Constants;

namespace SPR.HomeWork.Api.Controllers
{
    public class PersonController : ApiController
    {
        List<Person> persons = new List<Person>();
        PersonRepository repository = new PersonRepository();

        public IEnumerable<Person> GetAllPersons()
        {
            var persons = repository.GetAll();

            return persons;
        }

        public Person GetPerson(int id)
        {
            Person item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        public IEnumerable<Person> GetAllPersonsSorted(string SortCriteria)
        {
            var persons = repository.GetAll();
            List<Person> sortedPersons = new List<Person>();

            //TO DO: change to switch
            if (SortCriteria == PersonEnums.SortCriteria.Gender.ToString())
                sortedPersons = persons.OrderBy(person => person.Gender).ToList();

            else if (SortCriteria == PersonEnums.SortCriteria.Name.ToString())
                sortedPersons = persons.OrderBy(person => person.LastName).ToList();

            else if (SortCriteria == PersonEnums.SortCriteria.DOB.ToString())
                sortedPersons = persons.OrderBy(person => person.DateOfBirth).ToList();


            return sortedPersons;
        }
      

        public HttpResponseMessage PostProduct(Person item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Person>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

       

    }
}
