using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SPR.HomeWork.Models;
using SPR.HomeWork.Repository;
using SPR.HomeWork.Api.Constants;
using System.Web.Http.Description;

namespace SPR.HomeWork.Api.Controllers
{
    [RoutePrefix("api")]
    public class PersonController : ApiController
    {
        //TO DO: Implement Dependency injection to simplify unit testing
        //TO DO: Implement async tasks

        List<Person> persons = new List<Person>();
        PersonRepository repository = new PersonRepository();


        [Route("persons")]
        [Route("persons/{sortcriteria?}")]
        //public IHttpActionResult Get(string sortcriteria = null)
        public IEnumerable<Person> Get(string sortcriteria = null)
        {
            try
            {
                var persons = repository.GetAll();
                List<Person> sortedPersons = new List<Person>();

               
                if (sortcriteria == null)
                    return persons;
                else
                {                    

                    //TO DO: change to switch
                    if (sortcriteria == PersonEnums.SortCriteria.Gender.ToString())
                        sortedPersons = persons.OrderBy(person => person.Gender).ThenBy(person => person.LastName).ToList();

                    else if (sortcriteria == PersonEnums.SortCriteria.Name.ToString())
                        sortedPersons = persons.OrderByDescending(person => person.LastName).ToList();

                    else if (sortcriteria == PersonEnums.SortCriteria.DOB.ToString())
                        sortedPersons = persons.OrderBy(person => person.DateOfBirth).ToList();



                    return sortedPersons;
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
                
            }
                        
        }


        [Route("persons/{id}")]
        public Person Get(int id)
        {
            Person item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }


        //[HttpPost]
        // public HttpResponseMessage Post(Person item)
        // {
        //     item = repository.Add(item);
        //     var response = Request.CreateResponse<Person>(HttpStatusCode.Created, item);

        //     string uri = Url.Link("DefaultApi", new { id = item.Id });
        //     response.Headers.Location = new Uri(uri);
        //     return response;
        // }

        [HttpPost]
        public IHttpActionResult Post(Person item)
        {
            try
            {
                if (item ==null)
                {
                    return BadRequest();
                }

                item = repository.Add(item);
                
                if (item != null)
                    return Created(Request.RequestUri + "/" + item.Id.ToString(), item);

                return BadRequest();                

            }
            catch (Exception)
            {
                return InternalServerError();
            }           
        }


    }
}
