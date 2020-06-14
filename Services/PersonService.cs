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
        private readonly FileRepository fileRepository;
        private readonly MainConfiguration mainConfiguration;

        public PersonService(LocalizationService locService)
        {
            this.locService = locService;
            screenService = new ScreenService(locService);
            personValidationService = new PersonValidationService(locService);
            personWithCovidRepository = new PersonWithCovidRepository();
            fileRepository = new FileRepository();
            mainConfiguration = new MainConfiguration();
        }

        /// <summary>
        /// Adds a patient to the repository
        /// </summary>
        public void AddPatient()
        {
            Patient personToAdd = new Patient
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
            List<Patient> persons = personWithCovidRepository.GetAllPeopleWithCovid();
            List<Error> errors;
            Patient personToModify;
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
                        screenService.ShowErrors(nameof(Patient.PersonId), errors);
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
            List<Patient> persons = personWithCovidRepository.GetAllPeopleWithCovid();
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
                        screenService.ShowErrors(nameof(Patient.PersonId), errors);
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
        private int AddModifyPostalCode(Patient person)
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

                screenService.ShowPleaseEnterData(nameof(Patient.Address.PostalCode));

                postalCode = Console.ReadLine();

                errorsAndint = personValidationService.ValidatePostalCode(postalCode);
                postalCodeNumberified = errorsAndint.Item1;

                if (errorsAndint.Item2.Count > 0)
                {
                    screenService.ShowErrors(nameof(Patient.Address.PostalCode), errorsAndint.Item2);
                }

            } while (errorsAndint.Item2.Count > 0);

            return postalCodeNumberified;
        }

        /// <summary>
        /// Adds or modifies the street number of a person
        /// </summary>
        /// <returns>the number of the street</returns>
        private int AddModifyStreetNumber(Patient person)
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

                screenService.ShowPleaseEnterData(nameof(Patient.Address.StreetNumber));

                streetNumber = Console.ReadLine();

                errorsAndint = personValidationService.ValidatePostalCode(streetNumber);
                postalCodeNumberified = errorsAndint.Item1;

                if (errorsAndint.Item2.Count > 0)
                {
                    screenService.ShowErrors(nameof(Patient.Address.StreetNumber), errorsAndint.Item2);
                }

            } while (errorsAndint.Item2.Count > 0);

            return postalCodeNumberified;
        }

        /// <summary>
        /// Shows all the patients
        /// </summary>
        public void ShowAllPatients()
        {
            List<Patient> persons = personWithCovidRepository.GetAllPeopleWithCovid();

            if(persons.Count == 0)
            {
                screenService.NoPatientsToShow();

                return;
            }

            screenService.ShowPatients(persons);
        }

        /// <summary>
        /// Modifies the state of the covid in a patient
        /// </summary>
        /// <returns>The covid state</returns>
        /// <param name="person">The person that you are modifying</param>
        private CovidState AddModifyStatusWithCovid(Patient person)
        {
            List<Error> errors;
            string covid;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Patient.StateWithCovid));
                screenService.ShowCovidStates();

                covid = Console.ReadLine();

                errors = personValidationService.ValidateCovidState(covid);

                if (errors.Count > 0)
                {
                    screenService.ShowErrors(nameof(Patient.StateWithCovid), errors);
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
            List<Patient> persons = personWithCovidRepository.GetPeopleWithCovidState(covidState);

            if(persons.Count == 0)
            {
                screenService.NoPatientsToShow();

                return;
            }

            screenService.ShowPatients(persons);
        }

        /// <summary>
        /// Adds or modifies a patient's name
        /// </summary>
        /// <returns>The patient's name</returns>
        private string AddModifyPatientName(Patient person)
        {
            string name;
            List<Error> errors;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Patient.Name));

                name = Console.ReadLine();

                errors = personValidationService.ValidateName(name);

                if (errors.Count > 0)
                {
                    screenService.ShowErrors(nameof(Patient.Name), errors);
                }

            } while (errors.Count > 0);

            return name;
        }

        /// <summary>
        /// Adds or modifies the Name of the street
        /// </summary>
        /// <returns>the name of the street</returns>
        private string AddModifyStreetName(Patient person)
        {
            List<Error> errors;
            string streetName;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Patient.Address.StreetName));

                streetName = Console.ReadLine();

                errors = personValidationService.ValidateStreetName(streetName);

                if (errors.Count > 0)
                {
                    screenService.ShowErrors(nameof(Patient.Address.StreetName), errors);
                }

            } while (errors.Count > 0);

            return streetName;
        }

        /// <summary>
        /// Export the patients to an excel file
        /// </summary>
        public void ExportPatients(FileFormat fileFormat)
        {
            var patients = personWithCovidRepository.GetAllPeopleWithCovid();

            string fileName = fileRepository.CreateFileName(fileFormat);

            string completePath = fileRepository.ReturnCompletePath(mainConfiguration.Configuration.DirectoryToSaveFiles, fileName);

            if (!fileRepository.DirectoryExists(mainConfiguration.Configuration.DirectoryToSaveFiles))
            {
                fileRepository.CreateDirectory(mainConfiguration.Configuration.DirectoryToSaveFiles);
            }

            switch (fileFormat)
            {
                case FileFormat.xlsx:
                    CreateXlsxFile(patients, completePath);
                    break;
                case FileFormat.txt:
                    CreateTxtFile(patients, completePath);
                    break;
                default:
                    screenService.ShowErrorCreatingFile(completePath);
                    break;
            }

        }

        /// <summary>
        /// Calls the file repository to create an excel file
        /// </summary>
        /// <param name="completePath">The complete path where it was created</param
        /// <param name="persons">The patients you want to show</param>
        private void CreateXlsxFile(List<Patient> persons, string completePath)
        {
            bool excelFileCreatedSuccessfully = fileRepository.WritePersonToExcelFile(persons, completePath);

            if(excelFileCreatedSuccessfully)
            {
                screenService.ShowFileCreatedSuccessFully(completePath);
            }
            else
            {
                screenService.ShowErrorCreatingFile(completePath);
            }
        }

        /// <summary>
        /// Calls the file repository to create a txt file of all patients
        /// </summary>
        /// <param name="completePath"></param>
        /// <param name="persons">The patients you want to show</param>
        private void CreateTxtFile(List<Patient> persons, string completePath)
        {
            bool wroteSuccessfully = fileRepository.WritePersonsToTxtFile(persons, completePath);

            if (wroteSuccessfully)
            {
                screenService.ShowFileCreatedSuccessFully(completePath);
            }
            else
            {
                screenService.ShowErrorCreatingFile(completePath);
            }
        }

        /// <summary>
        /// Adds or modifies the patient last name
        /// </summary>
        /// <returns>the patient last name</returns>
        private string AddModifyPatientLastName(Patient person)
        {
            string name;
            List<Error> errors;

            do
            {
                if (person != null)
                {
                    screenService.ShowPatient(person);
                }

                screenService.ShowPleaseEnterData(nameof(Patient.LastName));

                name = Console.ReadLine();

                errors = personValidationService.ValidateName(name);

                if (errors.Count > 0)
                {
                    screenService.ShowErrors(nameof(Patient.LastName), errors);
                }

            } while (errors.Count > 0);

            return name;
        }
    }
}
