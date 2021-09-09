using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_Opgave_Journal
{
    public class Journal
    {
        private string patientName;

        /// <summary>
        /// The Name of the patient
        /// </summary>
        public string PatientName
        {
            get { return patientName; }
            set { patientName = value; }
        }

        private string patientCPR;

        /// <summary>
        /// The CPR number of the patient
        /// </summary>
        public string PatientCPR
        {
            get { return patientCPR; }
            set { patientCPR = value; }
        }

        private string patientAddress;

        /// <summary>
        /// The Address of the patient
        /// </summary>
        public string PatientAddress 
        {
            get { return patientAddress; }
            set { patientAddress = value; }
        }

        private string patientTelephone;

        /// <summary>
        /// The Phone numnber of the patient
        /// </summary>
        public string PatientTelephone
        {
            get { return patientTelephone; }
            set { patientTelephone = value; }
        }

        private string patientEmail;

        /// <summary>
        /// The Email of the patient
        /// </summary>
        public string PatientEmail
        {
            get { return patientEmail; }
            set { patientEmail = value; }
        }

        private string patientDoctor;

        /// <summary>
        /// The Doctor of the patient
        /// </summary>
        public string PatientDoctor
        {
            get { return patientDoctor; }
            set { patientDoctor = value; }
        }

        private List<JournalEntry> journalEntries;

        /// <summary>
        /// The entries
        /// </summary>
        public List<JournalEntry> PatientJournalEntries
        {
            get { return journalEntries; }
            set { journalEntries = value; }
        }


        public Journal(string patientName, string patientCPR, string patientAddress, string patientTelephone, string patientEmail, string patientDoctor)
        {
            this.patientName = patientName;
            this.patientCPR = patientCPR;
            this.patientAddress = patientAddress;
            this.patientTelephone = patientTelephone;
            this.patientEmail = patientEmail;
            this.patientDoctor = patientDoctor;

            journalEntries = new List<JournalEntry>();
        }




    }
}
