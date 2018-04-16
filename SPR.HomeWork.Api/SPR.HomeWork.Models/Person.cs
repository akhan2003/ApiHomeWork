using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPR.HomeWork.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string FavoriteColor { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}