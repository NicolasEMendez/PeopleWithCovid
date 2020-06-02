using Common;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Services
{
    public class LocalizationService
    {
        private ResourceManager resourceManager;
        private CultureInfo cultureInfo;

        public LocalizationService()
        {
            SetDefaultCultureAndResource();
        }

        private void SetDefaultCultureAndResource()
        {
            cultureInfo = CultureInfo.CreateSpecificCulture(Languages.es.ToString());
            resourceManager = new ResourceManager("Common.Resources.String", Assembly.GetAssembly(typeof(CollectionMenu)));
        }

        /// <summary>
        /// Changes the Culture Info for the language selected
        /// </summary>
        /// <param name="languages"></param>
        public void ChangeLanguage(Languages languages)
        {
            switch (languages)
            {
                case Languages.es:
                    cultureInfo = CultureInfo.CreateSpecificCulture(Languages.es.ToString());
                    resourceManager = new ResourceManager("Common.Resources.String", Assembly.GetAssembly(typeof(CollectionMenu)));
                    break;
                case Languages.en:
                    cultureInfo = CultureInfo.CreateSpecificCulture(Languages.en.ToString());
                    resourceManager = new ResourceManager("Common.Resources.String", Assembly.GetAssembly(typeof(CollectionMenu)));
                    break;
                default:
                    cultureInfo = CultureInfo.CreateSpecificCulture(Languages.en.ToString());
                    resourceManager = new ResourceManager("Common.Resources.String", Assembly.GetAssembly(typeof(CollectionMenu)));
                    break;
            }
        }

        /// <summary>
        /// Returns the localized string depending on the value in the resourece files
        /// </summary>
        /// <param name="value">the localization Name</param>
        /// <returns>The value of the localization</returns>
        public string GetString(string value)
        {
            return resourceManager.GetString(value, cultureInfo);
        }

    }

  
}
