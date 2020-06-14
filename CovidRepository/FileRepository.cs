using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Common;
using Common.Domain;
using Microsoft.Office.Interop.Excel;

namespace CovidRepository
{
    public class FileRepository
    {
        private readonly MainConfiguration mainConfiguration;

        public FileRepository()
        {
            mainConfiguration = new MainConfiguration();
        }

        /// <summary>
        /// Create an excel file
        /// </summary>
        /// <param name="completePath">The path where it will save</param>
        /// <returns></returns>
        public bool WritePersonToExcelFile(List<Patient> patients, string completePath)
        {
            Application application = new Application();
            Workbook xlWorkBook;
            Worksheet xlWorkSheet;

            xlWorkBook = application.Workbooks.Add(Type.Missing);
            xlWorkSheet = (Worksheet)xlWorkBook.ActiveSheet;

            if (application == null)
            {
                return false;
            }

            try
            {

                int i = 1;
                xlWorkSheet.Cells[i, 1] = nameof(Patient.Name);
                xlWorkSheet.Cells[i, 2] = nameof(Patient.LastName);
                xlWorkSheet.Cells[i, 3] = nameof(Patient.StateWithCovid);
                xlWorkSheet.Cells[i, 4] = nameof(Patient.Address.StreetName);
                xlWorkSheet.Cells[i, 5] = nameof(Patient.Address.StreetNumber);
                xlWorkSheet.Cells[i, 6] = nameof(Patient.Address.PostalCode);

                foreach (var person in patients)
                {
                    i++;
                    xlWorkSheet.Cells[i, 1] = person.Name;
                    xlWorkSheet.Cells[i, 2] = person.LastName;
                    xlWorkSheet.Cells[i, 3] = person.StateWithCovid.ToString();
                    xlWorkSheet.Cells[i, 4] = person.Address.StreetName;
                    xlWorkSheet.Cells[i, 5] = person.Address.StreetNumber;
                    xlWorkSheet.Cells[i, 6] = person.Address.PostalCode;
                }

                xlWorkBook.SaveAs(completePath, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                       false, false, XlSaveAsAccessMode.xlShared,
                       Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                xlWorkBook.Close(true, Type.Missing, Type.Missing);
                application.Quit();
                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(application);
            }

            return true;
        }

        /// <summary>
        /// Writes a txt file with all covid patients to a path.
        /// </summary>
        /// <param name="completePath">The path where it will create the file</param>
        public bool WritePersonsToTxtFile(List<Patient> persons, string completePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(completePath))
                {
                    foreach (var person in persons)
                    {
                        sw.WriteLine($"{nameof(person.Name)}: {person.Name}");
                        sw.WriteLine($"{nameof(person.LastName)}: {person.LastName}");
                        sw.WriteLine($"{nameof(person.StateWithCovid)}: {person.StateWithCovid}");
                        sw.WriteLine($"{nameof(person.Address.StreetName)}: {person.Address.StreetName}");
                        sw.WriteLine($"{nameof(person.Address.StreetNumber)}: {person.Address.StreetNumber}");
                        sw.WriteLine($"{nameof(person.Address.PostalCode)}: {person.Address.PostalCode}");
                        sw.WriteLine(string.Empty);
                        sw.WriteLine(string.Empty);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// Reads an xml and returns the deserialized object
        /// </summary>
        /// <param name="path">The path of the xml</param>
        /// <returns>A Collection of patients</returns>
        public PatientsCollection ReturnPatientsCollectionByReadingXml(string path)
        {
            PatientsCollection xmlDeserialized = null;
            XmlSerializer serializer = new XmlSerializer(typeof(PatientsCollection));
            using (StreamReader sw = new StreamReader(path))
            {
                xmlDeserialized = (PatientsCollection)serializer.Deserialize(sw);
            }

            return xmlDeserialized;
        }

        /// <summary>
        /// Return the initial covid patients path
        /// </summary>
        /// <returns>The directory</returns>
        public string ReturnInitialCovidPatientPath()
        {
            string solutiondir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())
                .Parent.Parent.FullName, mainConfiguration.Configuration.DirectoryImportPatients);

            return solutiondir;
        }

        /// <summary>
        /// Returns the complete path with the directory and filename
        /// </summary>
        /// <param name="directory">The directory structure</param>
        /// <param name="fileName">The file name</param>
        /// <returns></returns>
        public string ReturnCompletePath(string directory, string fileName)
        {
            string completePath = Path.Combine(directory, fileName);

            return completePath;
        }

        /// <summary>
        /// Creates the directory with the name
        /// </summary>
        /// <param name="completePath">the path of the directory</param>
        public void CreateDirectory(string completePath)
        {
            Directory.CreateDirectory(completePath);
        }

        /// <summary>
        /// Check if a directory Exists
        /// </summary>
        /// <param name="pathDirectory">The directory path</param>
        /// <returns></returns>
        public bool DirectoryExists(string pathDirectory)
        {
            return Directory.Exists(pathDirectory);
        }

        /// <summary>
        /// Creates the file name
        /// </summary>
        /// <param name="fileFormat">The format of the file</param>
        /// <returns></returns>
        public string CreateFileName(FileFormat fileFormat)
        {
            return DateTime.Now.ToString("dd-MM-yyyy") + "-" + Guid.NewGuid() + "." + fileFormat;
        }

        /// <summary>
        /// Check if a file exists
        /// </summary>
        /// <param name="completePath">The complete path to check</param>
        /// <returns></returns>
        public bool FileExists(string completePath)
        {
            return File.Exists(completePath);
        }

        /// <summary>
        /// Creates a file to the path specified
        /// </summary>
        /// <param name="completePath">The complete path to create the file</param>
        public void CreateFile(string completePath)
        {
            File.Create(completePath);
        }
    }
}
