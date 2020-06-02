using Common;
using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace CovidRepository
{
    public class PersonWithCovidRepository
    {
        private static List<Person> persons = new List<Person>();

        public PersonWithCovidRepository()
        {
            persons = AddPeopleWithCovidAlready();
        }

        /// <summary>
        /// Adds a person with covid
        /// </summary>
        /// <param name="person"></param>
        public void AddPersonWithCovid(Person person)
        {
            persons.Add(person);
        }

        public Person GetPersonById(int id)
        {
            return persons.Where(x => x.PersonId == id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Remove a person of the list with the id
        /// </summary>
        /// <param name="person">The person that wants to be removed</param>
        public void RemovePersonWithCovid(int personId)
        {
            int index = persons
                .FindIndex(x => x.PersonId == personId);

            persons.RemoveAt(index);
        }

        /// <summary>
        /// Modify a person
        /// </summary>
        /// <param name="personModified">The person modifieds that will replace the older one</param>
        public void ModifyPersonWithCovid(Person personModified)
        {
            int index = persons
                .FindIndex(x => x.PersonId == personModified.PersonId);

            persons[index] = personModified;
        }

        /// <summary>
        /// Returns a list of people with covid depending on the state of the patient
        /// </summary>
        /// <param name="state">The state (CovidState) you want to search</param>
        /// <returns>a list of persons in that state</returns>
        public List<Person> GetPeopleWithCovidState(CovidState state)
        {
            return persons
                .Where(x => x.StateWithCovid == state)
                .ToList();
        }

        /// <summary>
        /// Gets all the people with covid
        /// </summary>
        /// <returns>Returns a list of people</returns>
        public List<Person> GetAllPeopleWithCovid()
        {
            return persons;
        }

        /// <summary>
        /// Add initial patients with covid
        /// </summary>
        private List<Person> AddPeopleWithCovidAlready()
        {
            List<Person> personsWithCovid = new List<Person>()
            {
                new Person()
                {
                    LastName = "Casals",
                    Name = "Julio Cesar",
                    PersonId = 1,
                    StateWithCovid = CovidState.Deceased,
                    Address = new Address()
                    {
                        PostalCode = 1100,
                        StreetName = "Garalina",
                        StreetNumber = 1600
                    }
                },
                new Person()
                {
                    LastName = "Galindo",
                    Name = "Gaspar",
                    PersonId = 2,
                    StateWithCovid = CovidState.UnderEvaluation,
                    Address = new Address()
                    {
                        PostalCode = 1100,
                        StreetName = "Portia",
                        StreetNumber = 2504
                    }
                },
                new Person()
                {
                    LastName = "Valcarcel",
                    Name = "Luis",
                    PersonId = 3,
                    StateWithCovid = CovidState.Critical,
                    Address = new Address()
                    {
                        PostalCode = 1100,
                        StreetName = "Grim Dawn",
                        StreetNumber = 3321
                    }
                },
                new Person()
                {
                    LastName = "Arenas",
                    Name = "Lazaro",
                    PersonId = 4,
                    StateWithCovid = CovidState.AdmittedInHospital,
                    Address = new Address()
                    {
                        PostalCode = 1100,
                        StreetName = "Rust",
                        StreetNumber = 453
                    }
                },
                new Person()
                {
                    LastName = "Pazos",
                    Name = "Ramiel",
                    PersonId = 5,
                    StateWithCovid = CovidState.Recuperated,
                    Address = new Address()
                    {
                        PostalCode = 4421,
                        StreetName = "Streets Of Rogue",
                        StreetNumber = 5221
                    }
                }
            };

            return personsWithCovid;
        }
    }
}
