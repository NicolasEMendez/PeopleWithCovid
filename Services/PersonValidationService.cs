using Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Services
{
    public class PersonValidationService
    {
        private readonly LocalizationService locService;

        public PersonValidationService(LocalizationService locService)
        {
            this.locService = locService;
        }
 

        /// <summary>
        /// Validates if a string is empty
        /// </summary>
        /// <param name="value">The string to be validated</param>
        /// <returns>An error, if it is null, no errors where encountered</returns>
        private Error ValidateEmptyString(string value)
        {
            if(value.Length == 0)
            {
                Error errorEmpty = new Error() { Description = locService.GetString("Error_Must_Not_Be_Empty") };
                return errorEmpty;
            }

            return null;
        }

        /// <summary>
        /// Validate if the string is a number
        /// </summary>
        /// <param name="value">The string value to be parsed</param>
        /// <returns>an Error if it isn't a number</returns>
        private Error ValidateIsNumber(string value)
        {
            var regexItem = new Regex("^([0-9]+)$");

            if (!regexItem.IsMatch(value))
            {
                return new Error() { Description = locService.GetString("Error_Only_Number") };
            }

            return null;
        }

        /// <summary>
        /// Validate if the string is a number
        /// </summary>
        /// <param name="value">The string value to be parsed</param>
        /// <returns>an Error if it isn't a number</returns>
        private Error ValidateIsString(string value)
        {
            var regexItem = new Regex("^[aA-zZ]*$");

            if (!regexItem.IsMatch(value))
            {
                return new Error() { Description = locService.GetString("Error_In_Name_OnlyLetters") };
            }

            return null;
        }

        /// <summary>
        /// Validates the name of the Person
        /// </summary>
        /// <param name="name">the string that has the name</param>
        /// <returns>A list of errors if any</returns>
        public List<Error> ValidateName(string name)
        {
            List<Error> errors = new List<Error>();

            Error errorEmptyString = ValidateEmptyString(name);

            Error nameIsString = ValidateIsString(name);

            if (errorEmptyString != null)
            {
                errors.Add(errorEmptyString);
            }

            if (nameIsString != null)
            {
                errors.Add(new Error() { Description = locService.GetString("Error_In_Name_OnlyLetters") });
            }

            if(name.Length > 20)
            {
                errors.Add(new Error() { Description = locService.GetString("Error_In_Name_Length") });
            }

            return errors;
        }

        /// <summary>
        /// Validates the id of the patient to modify/delete
        /// </summary>
        /// <param name="idOfPatient"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Error> ValidateIdSelected(string idOfPatient, List<int> ids)
        {
            List<Error> errors = new List<Error>();

            Error errorEmptyString = ValidateEmptyString(idOfPatient);

            if (errorEmptyString != null)
            {
                errors.Add(errorEmptyString);
            }

            Error validateIfIsNumber = ValidateIsNumber(idOfPatient);

            if (validateIfIsNumber != null)
            {
                errors.Add(validateIfIsNumber);
            }

            if (errorEmptyString == null && validateIfIsNumber == null)
            {
                int idOfPatientNumber = int.Parse(idOfPatient);

                if(!ids.Contains(idOfPatientNumber))
                {
                    errors.Add(new Error() { Description = locService.GetString("Error_Id_Must_Exists") });
                }
            }

            return errors;

        }

        /// <summary>
        /// Validates the number of state covid choosen by the user (Must be between the covid states)
        /// </summary>
        /// <param name="covid">the number of covid</param>
        /// <returns>a list of errors if any</returns>
        public List<Error> ValidateCovidState(string covid)
        {
            List<Error> errors = new List<Error>();

            Error errorEmptyString = ValidateEmptyString(covid);

            if (errorEmptyString != null)
            {
                errors.Add(errorEmptyString);
            }

            Error validateIfIsNumber = ValidateIsNumber(covid);

            if (validateIfIsNumber != null)
            {
                errors.Add(validateIfIsNumber);
            }

            if (!int.TryParse(covid,out int covidNumber) || covidNumber < 1 || covidNumber > Enum.GetValues(typeof(CovidState)).Length)
            {
                errors.Add(new Error() { Description = locService.GetString("Error_Between_1_5") });
            }

            return errors;
        }

        /// <summary>
        /// Validates the street name
        /// </summary>
        /// <param name="streetName">The string that means the street name</param>
        /// <returns>a list of errors if any</returns>
        public List<Error> ValidateStreetName(string streetName)
        {
            List<Error> errors = new List<Error>();

            Error errorEmptyString = ValidateEmptyString(streetName);

            Error nameIsString = ValidateIsString(streetName);

            if (nameIsString != null)
            {
                errors.Add(new Error() { Description = locService.GetString("Error_In_Name_OnlyLetters") });
            }

            if (errorEmptyString != null)
            {
                errors.Add(errorEmptyString);
            }

            if(streetName.Length < 10 || streetName.Length > 50)
            {
                errors.Add(new Error() { Description = locService.GetString("Error_Between_Ten_Fifty") });
            }

            return errors;
        }

        /// <summary>
        /// Returns a tuple of int, List<Error> in case the number could be converted to an int
        /// </summary>
        /// <param name="postalCode">the string representing the Postal Code</param>
        /// <returns>a tuple with errors if any, if not errors then the int part of the tuple will be filled with different than 0</returns>
        internal Tuple<int,List<Error>> ValidatePostalCode(string postalCode)
        {
            List<Error> errors = new List<Error>();

            Error errorEmptyString = ValidateEmptyString(postalCode);

            Error validateIfIsNumber = ValidateIsNumber(postalCode);

            if (errorEmptyString != null)
            {
                errors.Add(errorEmptyString);
            }

            if (validateIfIsNumber != null)
            {
                errors.Add(validateIfIsNumber);
            }

            if(!int.TryParse(postalCode, out int postalCodeNumberified) || postalCodeNumberified < 0 || postalCodeNumberified.ToString().Length > 5)
            {
                errors.Add(new Error() { Description = locService.GetString("Error_MoreThan_Zero_Five_Digits_Long") });
            }

            return new Tuple<int, List<Error>>(postalCodeNumberified, errors);
        }
    }

    public class Error
    {
        public string Description { get; set; }
    }
}
