using Common;
using Common.Domain;
using System;
using System.Collections.Generic;

namespace Services
{
    public class ScreenService
    {
        private readonly LocalizationService locService;

        public ScreenService(LocalizationService locService)
        {
            this.locService = locService;
        }

        /// <summary>
        /// Shows an "Enter Data" message with the name of the data that needs completion
        /// </summary>
        /// <param name="dataToComplete">The data that needs to be completed</param>
        public void ShowPleaseEnterData(string dataToComplete)
        {
            Console.WriteLine($"{locService.GetString("Please_Enter_Data")}: {locService.GetString(dataToComplete)}");
        }

        /// <summary>
        /// Shows all the errors that a validation captured
        /// </summary>
        /// <param name="dataToCorrect">The data that needs correction</param>
        /// <param name="errors">The list of error that has</param>
        public void ShowErrors(string dataToCorrect, List<Error> errors)
        {
            ClearScreen();
            MakeSpaces(1);

            Console.WriteLine($"{locService.GetString("Error_In_Data")}: {locService.GetString(dataToCorrect)}");
            Console.Clear();

            int i = 0;
            foreach (var error in errors)
            {
                i++;
                Console.WriteLine($"{i}) {error.Description}");
            }

            Console.ReadKey(true);
        }

        /// <summary>
        /// Shows all the patients but with their Ids
        /// </summary>
        /// <param name="persons"></param>
        public void ShowPatientsWithIds(List<Patient> persons)
        {
            ClearScreen();

            foreach (var person in persons)
            {
                Console.WriteLine($"{locService.GetString("Person_Id")}: {person.PersonId}");
                Console.WriteLine($"{locService.GetString("Person_Name")}: {person.Name}");
                Console.WriteLine($"{locService.GetString("Person_LastName")}: {person.LastName}");
                Console.WriteLine($"{locService.GetString("Person_Covid")}: { SetCovidStatusName(person.StateWithCovid) }");
                Console.WriteLine($"{locService.GetString("Person_Address_StreetName")}: {person.Address.StreetName}");
                Console.WriteLine($"{locService.GetString("Person_Address_StreetNumber")}: {person.Address.StreetNumber}");
                Console.WriteLine($"{locService.GetString("Person_Address_PostalCode")}: {person.Address.PostalCode}");

                MakeSpaces(1);
            }

            ShowPressAnyKeyToContinue();
        }

        /// <summary>
        /// Make enter spaces for x amount of times
        /// </summary>
        /// <param name="amountOfSpaces">the amount of spaces</param>
        private void MakeSpaces(int amountOfSpaces)
        {
            string repetitionEscape = string.Empty;

            for (int i = 0; i < amountOfSpaces; i++)
            {
                repetitionEscape += "\n";
            }

            Console.WriteLine(repetitionEscape);
        }

        /// <summary>
        /// Shows a Patient
        /// </summary>
        /// <param name="personToModify"></param>
        public void ShowPatient(Patient personToModify)
        {
            ClearScreen();

            Console.WriteLine($"{locService.GetString("Person_Id")}: {personToModify.PersonId}");
            Console.WriteLine($"{locService.GetString("Person_Name")}: {personToModify.Name}");
            Console.WriteLine($"{locService.GetString("Person_LastName")}: {personToModify.LastName}");
            Console.WriteLine($"{locService.GetString("Person_Covid")}: { SetCovidStatusName(personToModify.StateWithCovid) }");
            Console.WriteLine($"{locService.GetString("Person_Address_StreetName")}: {personToModify.Address.StreetName}");
            Console.WriteLine($"{locService.GetString("Person_Address_StreetNumber")}: {personToModify.Address.StreetNumber}");
            Console.WriteLine($"{locService.GetString("Person_Address_PostalCode")}: {personToModify.Address.PostalCode}");

            MakeSpaces(1);
        }

        /// <summary>
        /// Shows a message that a certain action could not be done
        /// </summary>
        /// <param name="action">The action (Add, Delete, Modify)</param>
        internal void ShowNoPersonsInListToAddModifyDelete(AddModifyDeletePerson action)
        {
            ClearScreen();

            Console.WriteLine($"{locService.GetString("No_Patients_To")} {locService.GetString(action.ToString())}");

            Console.ReadKey(true);
        }

        /// <summary>
        /// Clears the screen
        /// </summary>
        public void ClearScreen()
        {
            Console.Clear();
        }

        /// <summary>
        /// Shows a message to select which patient to modify
        /// </summary>
        public void ShowPatienToModifyDelete(AddModifyDeletePerson action)
        {
            MakeSpaces(1);

            if(action == AddModifyDeletePerson.Modify)
            {
                Console.WriteLine($"{locService.GetString("Modify_Which_Patient")}");
            }
            else
            {
                Console.WriteLine($"{locService.GetString("Delete_Which_Patient")}");
            }  
        }

        /// <summary>
        /// Shows all the states of covid
        /// </summary>
        public void ShowCovidStates()
        {
            Array covidStates = Enum.GetValues(typeof(CovidState));
            int i = 0;

            MakeSpaces(1);

            foreach (var item in covidStates)
            {
                i++;
                Console.WriteLine($"{i}) {item.ToString()}");
            }

            MakeSpaces(1);
        }

        /// <summary>
        /// Shows a message that the person has been sucessfully Added/Modified
        /// </summary>
        /// <param name="action">The type of Action (Add/Modify)</param>
        public void ShowPeopleAddedModifiedDeletedSuccesfully(AddModifyDeletePerson action)
        {
            ClearScreen();

            string suffix = string.Empty;

            switch (action)
            {
                case AddModifyDeletePerson.Add:
                    suffix = locService.GetString("Person_Added");
                    break;
                case AddModifyDeletePerson.Modify:
                    suffix = locService.GetString("Person_Modified");
                    break;
                case AddModifyDeletePerson.Delete:
                    suffix = locService.GetString("Person_Deleted");
                    break;
                default:
                    break;
            }

            Console.WriteLine($"{locService.GetString("Person_AddedModified_Sucessfully")} {suffix} ");

            Console.ReadKey(true);
        }

        /// <summary>
        /// Shows a message to press a button and reads a key
        /// </summary>
        public void ShowPressAnyKeyToContinue()
        {
            MakeSpaces(1);

            Console.WriteLine(locService.GetString("Menu_Press_Any_Button"));

            Console.ReadKey(true);
        }

        /// <summary>
        /// Shows all Patients
        /// </summary>
        public void ShowPatients(List<Patient> persons)
        {
            ClearScreen();

            foreach (var person in persons)
            {
                Console.WriteLine($"{locService.GetString("Person_Name")}: {person.Name}");
                Console.WriteLine($"{locService.GetString("Person_LastName")}: {person.LastName}"); 
                Console.WriteLine($"{locService.GetString("Person_Covid")}: { SetCovidStatusName(person.StateWithCovid) }");
                Console.WriteLine($"{locService.GetString("Person_Address_StreetName")}: {person.Address.StreetName}");
                Console.WriteLine($"{locService.GetString("Person_Address_StreetNumber")}: {person.Address.StreetNumber}");
                Console.WriteLine($"{locService.GetString("Person_Address_PostalCode")}: {person.Address.PostalCode}");

                MakeSpaces(1);
            }

            ShowPressAnyKeyToContinue();
        }

        /// <summary>
        /// Shows a Message that there is no patients to show
        /// </summary>
        public void NoPatientsToShow()
        {
            ClearScreen();

            Console.WriteLine(locService.GetString("No_Patients_To_Show"));

            ShowPressAnyKeyToContinue();
        }

        /// <summary>
        /// Returns the localization of the covid status
        /// </summary>
        /// <param name="stateWithCovid">The Covid Status</param>
        /// <returns>a localized string</returns>
        private string SetCovidStatusName(CovidState stateWithCovid)
        {
            string prefix = locService.GetString("Covid_Status");

            string locStateWithCovid = locService.GetString($"{prefix}" + stateWithCovid);
            
            return locStateWithCovid;
        }

    }
}
