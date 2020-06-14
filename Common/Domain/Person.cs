using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.Domain
{
    [Serializable]
    public class Patient
    {
        [XmlElement("PersonId")]
        public int PersonId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("Address")]
        public Address Address { get; set; }

        [XmlElement("StateWithCovid")]
        public CovidState StateWithCovid { get; set; }

    }

    [XmlRoot("PatientsCollection")]
    public class PatientsCollection
    {
        [XmlElement("Patient")]
        public List<Patient> Patients { get; set; }
    }
}
