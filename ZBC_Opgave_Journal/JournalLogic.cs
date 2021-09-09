using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_Opgave_Journal
{
    public class JournalLogic
    {
        JournalsDB _db;

        public JournalLogic()
        {
            _db = new JournalsDB();
        }

        /// <summary>
        /// Creates a new journal
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pCpr"></param>
        /// <param name="pAddress"></param>
        /// <param name="pEmail"></param>
        /// <param name="pTelephone"></param>
        /// <param name="pDoctor"></param>
        public void CreateNewJournal(string pName, string pCpr, string pAddress, string pEmail, string pTelephone, string pDoctor)
        {
            Journal j = new Journal(pName, pCpr, pAddress, pEmail, pTelephone, pDoctor);
            _db.AddJournalToDB(j);
        }

        /// <summary>
        /// Calculates and formats the age of the person from the CPR number
        /// </summary>
        /// <param name="journal"></param>
        /// <returns></returns>
        public string GetAge(Journal journal)
        {
            string cpr = journal.PatientCPR;

            int year = GetCPRCentury(cpr) + int.Parse(cpr.Substring(4, 2));
                                        // dd-mm-yyyy
            string formattedDate = $"{cpr.Substring(0, 2)}-{cpr.Substring(2, 2)}-{year}";
            
            DateTime birthDay = DateTime.ParseExact(formattedDate, "dd-mm-yyyy", null);

            DateTime today = DateTime.Today;

            // Calculate age
            int age = today.Year - birthDay.Year;

            if(birthDay > today.AddYears(-age))
            {
                age--;
            }

            // Calculate days

            int days = CalculateDays(birthDay, today);

            return $"{age} years and {days} days old.";
        }

        /// <summary>
        /// Gets the century of the CPR birtday
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int GetCPRCentury(string number)
        {
            int year = int.Parse(number.Substring(4, 2));

            switch (number[7])
            {
                case '0':
                case '1':
                case '2':
                case '3':
                    return 1900;

                case '4':
                case '9':
                    if (year < 37)
                        return 2000;
                    else
                        return 1900;
                case '5':
                case '6':
                case '7':
                case '8':
                    if (year < 37)
                    {
                        return 2000;
                    } 
                    else if (year > 57)
                    {
                        return 1800;
                    }
                    else
                    {
                        return 0;
                    }

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Calculates the days leftover from the age
        /// </summary>
        /// <param name="birthDay"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        public int CalculateDays(DateTime birthDay, DateTime today)
        {
            int DaysInBdayMonth = DateTime.DaysInMonth(birthDay.Year, birthDay.Month);
            int DaysRemain = today.Day + (DaysInBdayMonth - birthDay.Day);

            int days;

            if (today.Month > birthDay.Month)
            {
                days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
            }
            else if (today.Month == birthDay.Month)
            {
                if (today.Day >= birthDay.Day)
                {
                    days = today.Day - birthDay.Day;
                }
                else
                {
                    days = DateTime.DaysInMonth(birthDay.Year, birthDay.Month) - (birthDay.Day - today.Day);
                }
            }
            else
            {
                days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
            }

            return days;
        }

        /// <summary>
        /// Adds a entry to a journal and saves
        /// </summary>
        /// <param name="journal"></param>
        /// <param name="text"></param>
        public void AddJournalEntry(Journal journal, string text)
        {
            JournalEntry entry = new JournalEntry(text);
            entry.Date = DateTime.Now;
            journal.PatientJournalEntries.Add(entry);
            _db.AddEntryToJournal(journal, entry);


        }

        /// <summary>
        /// Returns the patient's journal
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Journal GetJournal(string name)
        {
            return _db.GetJournal(name);
        }

        /// <summary>
        /// Returns a list of all the journals
        /// </summary>
        /// <returns></returns>
        public List<Journal> GetJournalsFromDB()
        {
            return _db.GetJournals();
        }
    }
}
