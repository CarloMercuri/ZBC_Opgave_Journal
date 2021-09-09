using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZBC_Opgave_Journal
{
    public class GUI
    {
        // Console size hack, makes it so you cannot resize it

        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();


        private JournalLogic _logic;
        private int separatorY = 10;
        private int entryY = 11;



        public GUI(JournalLogic logic)
        {
            Console.SetWindowSize(110, 30);
            Console.SetBufferSize(110, 30);
            LockConsole();

            _logic = logic;
        }

        /// <summary>
        /// The main menu
        /// </summary>
        public void MainMenu()
        {
            Console.Clear();

            Console.WriteLine("1. Opret Journal");
            Console.WriteLine("2. Se Journal");

            // Get input
            char key = GetValidKeyInput().KeyChar;

            switch (key)
            {
                case '1':
                    CreateNewJournalUI();
                    break;

                case '2':
                    LoadJournalsUI();
                    break;
            }
        }

        /// <summary>
        /// Page where you can choose which journal to display
        /// </summary>
        private void LoadJournalsUI()
        {
            // Always clear
            Console.Clear();

            // grab the list
            List<Journal> list = _logic.GetJournalsFromDB();

            Console.WriteLine("Patients: \n\r");

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($" {i}: {list[i].PatientName}");
            }

            int selection = -1;

            // Get the user's input
            while (true)
            {
                Console.Write("\n\rChoose which journal you want to see (id): ");

                selection = GetUserInputInteger();

                if (selection >= list.Count)
                {
                    Console.WriteLine("Invalid selection!");
                }
                else
                {
                    break;
                }
            }

            DisplayJournal(list, selection);

        }

        /// <summary>
        /// Displays a journal
        /// </summary>
        /// <param name="list"></param>
        /// <param name="selection"></param>
        private void DisplayJournal(List<Journal> list, int selection)
        {
            Journal journal = list[selection];

            Console.Clear();

            // TOP SECTION
            Console.SetCursorPosition(10, 1);
            Console.Write($"Patient Name: {journal.PatientName}");

            Console.SetCursorPosition(10, 3);
            Console.Write($"Address: {journal.PatientAddress}");

            Console.SetCursorPosition(10, 5);
            Console.Write($"Telefon: {journal.PatientTelephone}");

            Console.SetCursorPosition(10, 7);
            Console.Write($"Email: {journal.PatientEmail}");

            Console.SetCursorPosition(45, 1);
            Console.Write($"CPR Number: {journal.PatientCPR}");

            Console.SetCursorPosition(45, 3);
            Console.Write($"Doctor: {journal.PatientDoctor}");

            Console.SetCursorPosition(45, 5);
            Console.Write($"Age: {_logic.GetAge(journal)}");

            // Separator
            Console.SetCursorPosition(0, separatorY);
            Console.Write(new string('=', Console.WindowWidth));

            // Journal entry section

            int currentEntry = 0;


            if(journal.PatientJournalEntries.Count <= 0) // If there's no entries
            {
                Console.SetCursorPosition(2, entryY);
                Console.Write("No journal entries");
                Console.SetCursorPosition(60, Console.WindowHeight - 2);
                Console.Write("K - New Entry");
            }
            else
            {
                ShowJournalEntry(journal.PatientJournalEntries, 0); // If there are
            }

            // It's always gonna come down to this, so we just keep it in a loop
            while (true)
            {
                char key = Console.ReadKey(false).KeyChar;
                key = char.ToUpper(key);


                switch (key)
                {
                    case 'P':
                        if (currentEntry > 0)
                        {
                            currentEntry--;
                            ShowJournalEntry(list[selection].PatientJournalEntries, currentEntry);
                        }
                        break;

                    case 'N':
                        if (currentEntry < list[selection].PatientJournalEntries.Count - 1)
                        {
                            currentEntry++;
                            ShowJournalEntry(list[selection].PatientJournalEntries, currentEntry);
                        }
                        break;

                    case 'K':
                        CreateNewEntryUI(list, selection);
                        break;
                }
            }
            


        }

        /// <summary>
        /// The UI page to create a new entry
        /// </summary>
        /// <param name="jList"></param>
        /// <param name="selection"></param>
        private void CreateNewEntryUI(List<Journal> jList, int selection)
        {
            Console.Clear();
            Console.WriteLine($"Entry by: {jList[selection].PatientDoctor}");
            Console.WriteLine("\n\rEnter the new entry text: \n\r");

            string text = Console.ReadLine();

            // Down to logic to create it
            _logic.AddJournalEntry(jList[selection], text);

            // Back to main journal display
            DisplayJournal(jList, selection);
        }

        /// <summary>
        /// Update sthe JOURNAL ENTRY AREA only 
        /// </summary>
        /// <param name="entryList"></param>
        /// <param name="index"></param>
        private void ShowJournalEntry(List<JournalEntry> entryList, int index)
        {
            // Clear the area

            string clearString = new string(' ', Console.WindowWidth);
            Console.SetCursorPosition(0, entryY);
            for (int i = entryY; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(clearString);
            }

            // Formatting
            Console.SetCursorPosition(2, entryY);
            Console.Write($"Entry nr: {index + 1}");

            Console.SetCursorPosition(2, entryY + 2);
            Console.Write($"Date: {entryList[index].Date.ToString("dd-mm-yyyy")} at {entryList[index].Date.ToString("HH:mm:ss")}");

            Console.SetCursorPosition(2, entryY + 4);
            Console.Write(entryList[index].Text);

            // Menu

            Console.SetCursorPosition(40, Console.BufferHeight - 2);
            Console.Write("K - New Entry");


            Console.SetCursorPosition(60, Console.WindowHeight - 2);

            if(index > 0) // Color based on availability
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

            Console.Write("P - Previous Entry");

            if(index >= entryList.Count - 1) // same
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.SetCursorPosition(85, Console.WindowHeight - 2);
            Console.Write("N - Next Entry");
            Console.CursorVisible = false;

            Console.ForegroundColor = ConsoleColor.White;

        }

        /// <summary>
        /// UI to create a new journal
        /// </summary>
        private void CreateNewJournalUI()
        {
            Console.Clear();

            //
            // NAME
            //

            Console.Write("Patient name: ");
            string patientName = Console.ReadLine();

            //
            // CPR
            //

            string patientCpr = "";

            while (true)
            {
                Console.Write("\n\rPatient CPR: ");
                patientCpr = Console.ReadLine();

                if(patientCpr.Length != 10)
                {
                    Console.WriteLine("Invalid CPR!");
                }
                else
                {
                    break;
                }
            }

            //
            // ADDRESS
            //

            Console.Write("\n\rAddress: ");
            string patientAddress = Console.ReadLine();

            //
            // EMAIL
            //

            Console.Write("\n\rEmail: ");
            string patientEmail = Console.ReadLine();

            //
            // TELEPHONE NUMBER
            //

            string patientTelephoneNumber = "";

            while (true)
            {
                Console.Write("\n\rTelephone number: ");
                patientTelephoneNumber = Console.ReadLine();

                if (IsInputOnlyDigits(patientTelephoneNumber))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid telephone number!");
                }
            }

            //
            // DOCTOR
            //

            Console.Write("\n\rDoctor name: ");
            string patientDoctor = Console.ReadLine();

            // Finalize in logic
            _logic.CreateNewJournal(patientName, patientCpr, patientAddress, patientEmail, patientTelephoneNumber, patientDoctor);

            MainMenu();

        }

        

        /// <summary>
        /// Returns a key stroke that is limited to a-z, 0-9
        /// </summary>
        /// <returns></returns>
        public static ConsoleKeyInfo GetValidKeyInput(bool hideInput = true)
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(hideInput);

                if (char.IsLetter(key.KeyChar) || char.IsDigit(key.KeyChar))
                {
                    return key;
                }
                else
                {
                    continue;
                }
            }
        }



        /// <summary>
        /// Requests the user to enter an integer with the corresponding request string, and
        /// makes sure the input is sanitized
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public int GetUserInputInteger(bool hideCursor = false, bool printError = false, string phrase = "")
        {
            string userInput = "";

            while (true)
            {
                if (phrase != "")
                {
                    Console.WriteLine(phrase);
                }

                // If we're hiding the cursor, use another method of collecting the input
                if (hideCursor)
                {
                    while (true)
                    {
                        var key = Console.ReadKey(true);

                        if (key.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                        userInput += key.KeyChar;
                    }
                }
                else
                {
                    userInput = Console.ReadLine();
                }

                // Empty input (only pressed enter for example)
                if (userInput.Length <= 0)
                {
                    if (printError) Console.WriteLine("Invalid input");
                    continue;
                }

                // Check that it only contains numbers
                if (!IsInputOnlyDigits(userInput))
                {
                    if (printError) Console.WriteLine("Invalid input: must only contain numbers");
                    continue;
                }
                else
                {
                    break;
                }
            }

            return int.Parse(userInput);
        }


        /// <summary>
        /// Returns true if the string only contains digits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsInputOnlyDigits(string input)
        {
            foreach (char c in input)
            {
                // check that it's a number (unicode)
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Makes it so you cannot resize or maximize it
        /// </summary>
        public void LockConsole()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
        }

    }
}
