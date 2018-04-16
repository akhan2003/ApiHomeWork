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

namespace SPR.HomeWork.Client
{
    
    public class FileReader
    {
        static HttpClient client = new HttpClient();
              
        static void Main()
        {

            List<Person> persons;
            string filePath1 = ConfigurationManager.AppSettings.Get("filePath-pipe");

            persons = ReadFile(filePath1);

            System.Console.WriteLine("Sorted by Gender....");
            DisplayPersonSorted(persons, "Gender");

            System.Console.WriteLine("Sorted by Name....");
            DisplayPersonSorted(persons, "Name");

            System.Console.WriteLine("Sorted by DOB....");
            DisplayPersonSorted(persons, "DOB");


            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();

            //Here is a complete example of how to call the HomeWork Api.
            //CallRestService();


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

            foreach (string line in lines)
            {
                string[] splittedLines = ParseLine(line);

                persons.Add(new Person
                {
                    FirstName = splittedLines[0],
                    LastName = splittedLines.Length > 1 ? splittedLines[1] : null,
                    Gender = splittedLines.Length > 2 ? splittedLines[2] : null,
                    FavoriteColor = splittedLines.Length > 3 ? splittedLines[3] : null,
                    DateOfBirth = splittedLines.Length > 4 ? splittedLines[4] : null

                });              

            }

            return persons;

        }

        public static void DisplayPersonSorted(List<Person> persons, string SortCriteria)
        {
            List<Person> sortedPersons = new List<Person>();

            if (SortCriteria == "Gender")
                sortedPersons = persons.OrderBy(person => person.Gender).ToList();

            else if (SortCriteria == "Name")
                sortedPersons = persons.OrderBy(person => person.LastName).ToList();

            else if (SortCriteria == "DOB")
                sortedPersons = persons.OrderBy(person => person.DateOfBirth).ToList();

          
            ShowPersons(sortedPersons);
            
        }

        public static string[] ParseLine(string line)
        {
            string[] delimiters = { "|", ",", " " };
            string[] words = line.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);

            return words;
        }
     
        private static void CallRestService()
        {
            string baseAddress = ConfigurationManager.AppSettings.Get("base-adddress");

            HttpClient conn = new HttpClient();
            //Provide the base address of tha API.  Move to config file
            conn.BaseAddress = new Uri(baseAddress);
            conn.DefaultRequestHeaders.Accept.Clear();
            //set Accept Header to send the data in JSON format
            conn.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //GetPersonAsync(conn).Wait();

            GetPersonsAsync(conn).Wait();
        }


        static async Task GetPersonAsync(HttpClient conn)
        {
            using (conn)
            {
                HttpResponseMessage response = await conn.GetAsync("api/Person/2");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    Person person = await response.Content.ReadAsAsync<Person>();
                    ShowPerson(person);
                    Console.ReadLine();
                }
            }
        }
             
        static async Task GetPersonsAsync(HttpClient conn)
        {
            using (conn)
            {
                HttpResponseMessage response = await conn.GetAsync("api/Person");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                   
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var objData = JsonConvert.DeserializeObject<List<Person>>(jsonString);

                    foreach (Person person in objData)
                        ShowPerson(person);
                    Console.ReadLine();
                }
            }
        }

        static void ShowPerson(Person person)
        {
            Console.WriteLine($"FirstName: {person.FirstName}\tLastName: " +
                $"{person.LastName}\tGender: {person.Gender}\tFavoriteColor: " +
                $"{person.FavoriteColor}\tDateOfBirth: {person.DateOfBirth}");
        }

        static void ShowPersons(List<Person> persons)
        {
            foreach (Person person in persons)
            {
                Console.WriteLine($"FirstName: {person.FirstName}\tLastName: " +
                    $"{person.LastName}\tGender: {person.Gender}\tFavoriteColor: " +
                    $"{person.FavoriteColor}\tDateOfBirth: {person.DateOfBirth}" + "\n");
            }
        }

    }


}
