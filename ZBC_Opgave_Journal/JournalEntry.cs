using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_Opgave_Journal
{
    public class JournalEntry
    {
        private DateTime date;

        /// <summary>
        /// The date when this entry was created
        /// </summary>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private string text;

        /// <summary>
        /// The content
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


        public JournalEntry(string text)
        {
            this.text = text;
            DateTime date = DateTime.Now;
        }

    }
}
