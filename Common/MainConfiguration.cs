using System.Configuration;

namespace Common
{
    public class MainConfiguration
    {
        public Configuration Configuration { get; set; }
        public MainConfiguration()
        {
            Configuration = GetMainConfiguration();
        }
        /// <summary>
        /// Gets the configuration that the program needs in the App.config
        /// </summary>
        /// <returns>A Configuration object that have all the values needed</returns>
        private Configuration GetMainConfiguration()
        {
            Configuration configuration = new Configuration();

            configuration.DirectoryToSaveFiles = ConfigurationManager.AppSettings["DirectoryToSaveFile"];
            configuration.DirectoryImportPatients = ConfigurationManager.AppSettings["DirectoryImportPatients"];

            return configuration;
        }
    }

    public class Configuration
    {
        public string DirectoryToSaveFiles { get; set; }
        public string DirectoryImportPatients { get; set; }
    }
}
