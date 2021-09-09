using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ZBC_Opgave_Journal
{
    public class JournalsDB
    {
        private string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JournalDB.json");

        public JournalsDB()
        {
            if (!File.Exists(dbFilePath))
            {
                var f = File.Create(dbFilePath);
                f.Close();
            }
        }

        /// <summary>
        /// Grabs all the journals from the file 
        /// </summary>
        /// <returns></returns>
        public List<Journal> GetJournals()
        {
            string json = File.ReadAllText(dbFilePath);

            if (json.Length <= 0)
            {
                return new List<Journal>();
            }

            List<Journal> list = JsonSerializer.Deserialize<List<Journal>>(json);

            return list;
        }

        /// <summary>
        /// Loads the journals, adds an entry to one of them, saves again
        /// </summary>
        /// <param name="journal"></param>
        /// <param name="newEntry"></param>
        public void AddEntryToJournal(Journal journal, JournalEntry newEntry)
        {
            List<Journal> list = GetJournals();

            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].PatientName == journal.PatientName)
                {
                    list[i].PatientJournalEntries.Add(newEntry);
                }
            }

            SaveToFile(list);
        }

        /// <summary>
        /// Gets a specific journal out of the DB
        /// </summary>
        /// <param name="patientName"></param>
        /// <returns></returns>
        public Journal GetJournal(string patientName)
        {
            string json = File.ReadAllText(dbFilePath);
            List<Journal> list = JsonSerializer.Deserialize<List<Journal>>(json);

            return list.Find(p => p.PatientName == patientName);
        }

        /// <summary>
        /// Adds a journal and saves
        /// </summary>
        /// <param name="journal"></param>
        public void AddJournalToDB(Journal journal)
        {
            List<Journal> list = GetJournals();
            list.Add(journal);

            SaveToFile(list);
        }

        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="journals"></param>
        public void SaveToFile(List<Journal> journals)
        {
            var json = JsonSerializer.Serialize(journals);
           
            File.WriteAllText(dbFilePath, json);

        }
    }
}
