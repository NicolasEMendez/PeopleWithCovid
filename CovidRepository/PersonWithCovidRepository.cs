using Common;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace CovidRepository
{
    public class PersonWithCovidRepository
    {
        private readonly FileRepository fileRepository;
        private readonly MainConfiguration mainConfiguration;
        private List<Patient> persons = new List<Patient>();

        public PersonWithCovidRepository()
        {
            mainConfiguration = new MainConfiguration();
            fileRepository = new FileRepository();
            persons = AddPeopleWithCovidAlready();
        }

        /// <summary>
        /// Adds a person with covid
        /// </summary>
        /// <param name="person"></param>
        public void AddPersonWithCovid(Patient person)
        {
            persons.Add(person);
        }

        public Patient GetPersonById(int id)
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
        public void ModifyPersonWithCovid(Patient personModified)
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
        public List<Patient> GetPeopleWithCovidState(CovidState state)
        {
            return persons
                .Where(x => x.StateWithCovid == state)
                .ToList();
        }

        /// <summary>
        /// Gets all the people with covid
        /// </summary>
        /// <returns>Returns a list of people</returns>
        public List<Patient> GetAllPeopleWithCovid()
        {
            return persons;
        }

        /// <summary>
        /// Add initial patients with covid
        /// </summary>
        private List<Patient> AddPeopleWithCovidAlready()
        {
            string solutionDir = fileRepository.ReturnInitialCovidPatientPath();

            PatientsCollection patientsDeserialized = fileRepository.ReturnPatientsCollectionByReadingXml(solutionDir);

            return patientsDeserialized.Patients;
        }
    }
}
