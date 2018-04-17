using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SPR.HomeWork.Models;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Globalization;

namespace SPR.HomeWork.Client
{
    
    public class FileReader
    {
        static HttpClient client = new HttpClient();
              
        static void Main()
        {

            List<Person> persons;
            List<Person> sortedPersons;
            string filePath1 = ConfigurationManager.AppSettings.Get("filePath-pipe");

            string currentDirectory = Directory.GetCurrentDirectory();            
            string currentDirectory2 = Directory.GetParent(Directory.GetParent(currentDirectory).ToString()).ToString();

            filePath1 = System.IO.Path.Combine(currentDirectory2, "SampleFiles", filePath1);

            persons = ReadFile(filePath1);

            System.Console.WriteLine("Sorted by Gender....");
            sortedPersons = SortList(persons, "Gender");
            ShowPersons(sortedPersons);

            System.Console.WriteLine("Sorted by Name....");
            sortedPersons = SortList(persons, "Name");
            ShowPersons(sortedPersons);

            System.Console.WriteLine("Sorted by DOB....");
            sortedPersons = SortList(persons, "DOB");
            ShowPersons(sortedPersons);
                       
            //Here is a complete example of how to call the HomeWork Api.
            System.Console.WriteLine("Calling Rest Service....");
            CallRestService();
            // Keep the console window open.
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();

        }

        public static List<Person> ReadFile(string filePath)
        {
            string[] lines;

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            else
            {
                lines = System.IO.File.ReadAllLines(filePath);
            }

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of file: " + filePath);

            var persons = new List<Person>();
            Person person;
            foreach (string line in lines)
            {
                string[] splittedLines = ParseLine(line);
                string format = "M/d/yyyy";
                CultureInfo provider = CultureInfo.InvariantCulture;

                person = new Person();
                person.FirstName = splittedLines[0];
                person.LastName = splittedLines.Length > 1 ? splittedLines[1] : null;
                person.Gender = splittedLines.Length > 2 ? splittedLines[2] : null;
                person.FavoriteColor = splittedLines.Length > 3 ? splittedLines[3] : null;
                person.DateOfBirth = DateTime.ParseExact(splittedLines[4], format, new CultureInfo("en-US"));
                             
                persons.Add(person);                       

            }

            return persons;

        }

        public static List<Person> SortList(List<Person> persons, string SortCriteria)
        {
            List<Person> sortedPersons = new List<Person>();

            if (SortCriteria == "Gender")
                sortedPersons = persons.OrderBy(person => person.Gender).ThenBy(person => person.LastName).ToList();

            else if (SortCriteria == "Name")
                sortedPersons = persons.OrderByDescending(person => person.LastName).ToList();

            else if (SortCriteria == "DOB")
                sortedPersons = persons.OrderBy(person => person.DateOfBirth).ToList();

            return sortedPersons;            
            
        }

        public static string[] ParseLine(string line)
        {
            string[] delimiters = { "|", ",", " " };
            string[] words = line.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

            return words;
        }
     
        public static void CallRestService()
        {
            string baseAddress = ConfigurationManager.AppSettings.Get("base-address");

            HttpClient conn = new HttpClient();
            //Provide the base address of tha API.  Move to config file
            conn.BaseAddress = new Uri(baseAddress);
            conn.DefaultRequestHeaders.Accept.Clear();
            //set Accept Header to send the data in JSON format
            conn.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                       
            GetPersonsAsync(conn).Wait();

            //GetPersonsAsync(conn).GetAwaiter().GetResult();
        }


        //static async Task GetPersonAsync(HttpClient conn)
        //{
        //    using (conn)
        //    {
        //        HttpResponseMessage response = await conn.GetAsync("api/Person/2");
        //        response.EnsureSuccessStatusCode();

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Person person = await response.Content.ReadAsAsync<Person>();
        //            ShowPerson(person);
        //            Console.ReadLine();
        //        }
        //    }
        //}
             
        public static async Task GetPersonsAsync(HttpClient conn)
        {
            List<Person> persons = new List<Person>();
            using (conn)
            {
                HttpResponseMessage response = await conn.GetAsync("api/Persons");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                   
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var objData = JsonConvert.DeserializeObject<List<Person>>(jsonString);

                    foreach (Person person in objData)
                        persons.Add(person);

                    ShowPersons(persons);
                    
                }
            }
        }

        public static void ShowPerson(Person person)
        {
            Console.Write($"FirstName: {person.FirstName}\tLastName: " +
                $"{person.LastName}\tGender: {person.Gender}\tFavoriteColor: " +
                $"{person.FavoriteColor}\tDateOfBirth: {person.DateOfBirth.ToShortDateString()}" + "\n");
        }

        public static void ShowPersons(List<Person> persons)
        {
            foreach (Person person in persons)
            {
                Console.Write($"FirstName: {person.FirstName}\tLastName: " +
                    $"{person.LastName}\tGender: {person.Gender}\tFavoriteColor: " +
                    $"{person.FavoriteColor}\tDateOfBirth: {person.DateOfBirth.ToShortDateString()}" + "\n");
                
            }
        }

       
    }


}
