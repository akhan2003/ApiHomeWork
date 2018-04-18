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
using SPR.HomeWork.Api.Helper;

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
        public IHttpActionResult Get(string sort = null)
        {
            try
            {
                var returnValue = repository.GetAll();
               
                if (sort == null)                    
                    return Ok(returnValue);
                else
                {
                    return Ok(returnValue.AsQueryable().ApplySort(sort));             
                }
            }
            catch (Exception)
            {
                return InternalServerError();
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
