using Common;
using Common.Domain;
using CovidRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class PersonService
    {
        private readonly ScreenService screenService;
        private readonly LocalizationService locService;
        private readonly PersonValidationService personValidationService;
        private readonly PersonWithCovidRepository personWithCovidRepository;

        public PersonService(LocalizationService locService)
        {
            this.locService = locService;
            screenService = new ScreenService(locService);
            personValidationService = new PersonValidationService(locService);
            personWithCovidRepository = new PersonWithCovidRepository();
        }

        /// <summary>
        /// Adds a patient to the repository
        /// </summary>
        public void AddPatient()
        {
            Person personToAdd = new Person
            {
                Name = AddModifyPatientName(null),
                LastName = AddModifyPatientLastName(null),
                StateWithCovid = AddModifyStatusWithCovid(null),
                PersonId = SetPersonId(),
                Address = new Address()
                {
                    StreetName = AddModifyStreetName(null),
                    StreetNumber = AddModifyStreetNumber(null),
                    PostalCode = AddModifyPostalCode(null)
                }
            };

            personWithCovidRepository.AddPersonWithCovid(personToAdd);

            screenService.ShowPeopleAddedModifiedDeletedSuccesfully(AddModifyDeletePerson.Add);
        }

        /// <summary>
        /// Modifies a Patient
        /// </summary>
        public void ModifyPatient()
        {
            List<Person> persons = personWithCovidRepository.GetAllPeopleWithCovid();
            List<Error> errors;
            Person personToModify;
            string idOfPatient;
            List<int> personsIds = persons
                .Select(x => x.PersonId)
                .ToList();

            if (persons.Count != 0)
            {
                do
                {
                    screenService.ClearScreen();

                    screenService.ShowPatientsWithIds(persons);

                    screenService.ShowPatienToModifyDelete(AddModifyDeletePerson.Modify);

                    idOfPatient = Console.ReadLine();

                    errors = personValidationService.ValidateIdSelected(idOfPatient, personsIds);

                    if (errors.Count > 0)
                    {
                        screenService.ShowErrors(nameof(Person.PersonId), errors);
                    }

                } while (errors.Count > 0);

                personToModify = personWithCovidRepository.GetPersonById(int.Parse(idOfPatient));


                personToModify.Name = AddModifyPatientName(personToModify);
                personToModify.LastName = AddModifyPatientLastName(personToModify);
                personToModify.StateWithCovid = AddModifyStatusWithCovid(personToModify);
                personToModify.PersonId = SetPersonId();
                personToModify.Address.StreetName = AddModifyStreetName(personToModify);
                personToModify.Address.StreetNumber = AddModifyStreetNumber(personToModify);
                personToModify.Address.PostalCode = AddModifyPostalCode(personToModify);

                personWithCovidRepository.ModifyPersonWithCovid(personToModify);

                screenService.ShowPeopleAddedModifiedDeletedSuccesfully(AddModifyDeletePerson.Modify);
            }
            else
            {
                screenService.ShowNoPersonsInListToAddModifyDelete(AddModifyDeletePerson.Modify);
            }
        }

        /// <summary>
        /// Deletes a Patient
        /// </summary>
        public void DeletePatient()
        {
            List<Person> persons = personWithCovidRepository.GetAllPeopleWithCovid();
            List<Error> errors;
            string idOfPatient;
            List<int> personsIds = persons
                .Select(x => x.PersonId)
                .ToList();

            if (persons.Count != 0)
            {
                do
                {
                    screenService.ClearScreen();

                    screenService.ShowPatientsWithIds(persons);

                    screenService.ShowPatienToModifyDelete(AddModifyDeletePerson.Delete);

                    idOfPatient = Console.ReadLine();

                    errors = personValidationService.ValidateIdSelected(idOfPatient, personsIds);

                    if (errors.Count > 0)
                    {
                        screenService.ShowErrors(nameof(Person.PersonId), errors);
                    }

                } while (errors.Count > 0);

                personWithCovidRepository.RemovePersonWithCovid(int.Parse(idOfPatient));

                screenService.ShowPeopleAddedModifiedDeletedSuccesfully(AddModifyDeletePerson.Delete);
            }
            else
            {
                screenService.ShowNoPersonsInListToAddModifyDelete(AddModifyDeletePerson.Delete);
            }
        }

        /// <summary>
        /// Sets the person Id.
        /// </summary>
        /// <returns></returns>
        private int SetPersonId()
        {
            bool isAnyPersonAdded = personWithCovidRepository
                .GetAllPeopleWithCovid()
                .Any();

            if (isAnyPersonAdded == false)
            {
                return 1;
            }

            int lastId = personWithCovidRepository
                .GetAllPeopleWithCovid()
                .OrderByDescending(x => x.PersonId)
                .FirstOrDefault()
                .PersonId;

            return lastId + 1;
        }

        /// <summary>
        /// Adds or modifies the Person postal code
        /// </summary>
        /// <returns>The number of the postal code</returns>
        private int AddModifyPostalCode(Person person)
        {
            string postalCode;
            int postalCodeNumberified;
            Tuple<int, List<Error>> errorsAndint;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Person.Address.PostalCode));

                postalCode = Console.ReadLine();

                errorsAndint = personValidationService.ValidatePostalCode(postalCode);
                postalCodeNumberified = errorsAndint.Item1;

                if (errorsAndint.Item2.Count > 0)
                {
                    screenService.ShowErrors(nameof(Person.Address.PostalCode), errorsAndint.Item2);
                }

            } while (errorsAndint.Item2.Count > 0);

            return postalCodeNumberified;
        }

        /// <summary>
        /// Adds or modifies the street number of a person
        /// </summary>
        /// <returns>the number of the street</returns>
        private int AddModifyStreetNumber(Person person)
        {
            string streetNumber;
            int postalCodeNumberified;
            Tuple<int, List<Error>> errorsAndint;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Person.Address.StreetNumber));

                streetNumber = Console.ReadLine();

                errorsAndint = personValidationService.ValidatePostalCode(streetNumber);
                postalCodeNumberified = errorsAndint.Item1;

                if (errorsAndint.Item2.Count > 0)
                {
                    screenService.ShowErrors(nameof(Person.Address.StreetNumber), errorsAndint.Item2);
                }

            } while (errorsAndint.Item2.Count > 0);

            return postalCodeNumberified;
        }

        /// <summary>
        /// Shows all the patients
        /// </summary>
        public void ShowAllPatients()
        {
            List<Person> persons = personWithCovidRepository.GetAllPeopleWithCovid();

            if(persons.Count == 0)
            {
                screenService.NoPatientsToShow();
            }

            screenService.ShowPatients(persons);
        }

        /// <summary>
        /// Modifies the state of the covid in a patient
        /// </summary>
        /// <returns>The covid state</returns>
        /// <param name="person">The person that you are modifying</param>
        private CovidState AddModifyStatusWithCovid(Person person)
        {
            List<Error> errors;
            string covid;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Person.StateWithCovid));
                screenService.ShowCovidStates();

                covid = Console.ReadLine();

                errors = personValidationService.ValidateCovidState(covid);

                if (errors.Count > 0)
                {
                    screenService.ShowErrors(nameof(Person.StateWithCovid), errors);
                }

            } while (errors.Count > 0);

            return (CovidState)int.Parse(covid);
        }

        /// <summary>
        /// Shows all the patients of a certain covid state
        /// </summary>
        /// <param name="covidState">The patients with the Covid State you whish to see</param>
        public void ShowPeopleWithCovidState(CovidState covidState)
        {
            List<Person> persons = personWithCovidRepository.GetPeopleWithCovidState(covidState);

            screenService.ShowPatients(persons);
        }

        /// <summary>
        /// Adds or modifies a patient's name
        /// </summary>
        /// <returns>The patient's name</returns>
        private string AddModifyPatientName(Person person)
        {
            string name;
            List<Error> errors;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Person.Name));

                name = Console.ReadLine();

                errors = personValidationService.ValidateName(name);

                if (errors.Count > 0)
                {
                    screenService.ShowErrors(nameof(Person.Name), errors);
                }

            } while (errors.Count > 0);

            return name;
        }

        /// <summary>
        /// Adds or modifies the Name of the street
        /// </summary>
        /// <returns>the name of the street</returns>
        private string AddModifyStreetName(Person person)
        {
            List<Error> errors;
            string streetName;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Person.Address.StreetName));

                streetName = Console.ReadLine();

                errors = personValidationService.ValidateStreetName(streetName);

                if (errors.Count > 0)
                {
                    screenService.ShowErrors(nameof(Person.Address.StreetName), errors);
                }

            } while (errors.Count > 0);

            return streetName;
        }

        /// <summary>
        /// Adds or modifies the patient last name
        /// </summary>
        /// <returns>the patient last name</returns>
        private string AddModifyPatientLastName(Person person)
        {
            string name;
            List<Error> errors;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Person.LastName));

                name = Console.ReadLine();

                errors = personValidationService.ValidateName(name);

                if (errors.Count > 0)
                {
                    screenService.ShowErrors(nameof(Person.LastName), errors);
                }

            } while (errors.Count > 0);

            return name;
        }
    }
}
