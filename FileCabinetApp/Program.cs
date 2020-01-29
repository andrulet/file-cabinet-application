using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Andrei Zakharchuk";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly FileCabinetService FileCabinetService = new FileCabinetService();

        private static readonly Tuple<string, Action<string>>[] Commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the count of records", "The 'stat' prints the count of records." },
            new string[] { "create", "creates a new record about person", "The 'create' creates a new record about person." },
            new string[] { "list", "shows a list of records", "The 'list' shows a list of records." },
            new string[] { "edit", "changes found by id list entry", "The 'edit' changes found by id list entry." },
            new string[] { "find", "finds sheet entries by the specified field (firstname, lastname, dateofbirth)", "The 'find' finds sheet entries by the specified field (firstname, lastname, dateofbirth)" },
        };

        private static bool isRunning = true;

        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(Commands, 0, Commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    Commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(HelpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in HelpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.FileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            try
            {
                Tuple<string, string, DateTime, char, decimal, short> paramConvert = Convertor();
                int number = Program.FileCabinetService.CreateRecord(paramConvert.Item1, paramConvert.Item2, paramConvert.Item3, paramConvert.Item4, paramConvert.Item5, paramConvert.Item6);
                Console.WriteLine($"Record #{number} is created");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                Create(string.Empty);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Create(string.Empty);
            }
        }

        private static void Edit(string parameters)
        {
            try
            {
                if (!int.TryParse(parameters, out int id) || id < 0 || id > int.MaxValue)
                {
                    throw new ArgumentException($"Incorrect {nameof(id)}({id}). Id must be more than 0 and less {int.MaxValue}.");
                }

                if (FileCabinetService.CheckId(id))
                {
                    Tuple<string, string, DateTime, char, decimal, short> paramConvert = Convertor();
                    FileCabinetService.EditRecord(id, paramConvert.Item1, paramConvert.Item2, paramConvert.Item3, paramConvert.Item4, paramConvert.Item5, paramConvert.Item6);
                    Console.WriteLine($"Record #{id} is created");
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                Create(string.Empty);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Create(string.Empty);
            }
        }

        private static void Find(string parameters)
        {
            try
            {
                if (string.IsNullOrEmpty(parameters))
                {
                    throw new ArgumentNullException($"The {nameof(parameters)} can't be null or empty.");
                }

                FileCabinetRecord[] list;
                var command = parameters.Trim().ToUpperInvariant().Split(' ');
                if (string.Equals(command[0], "FIRSTNAME", StringComparison.InvariantCultureIgnoreCase))
                {
                    list = FileCabinetService.FindByFirstName(command[1].Trim('"'));
                    ShortShowRecords(list);
                }
                else
                {
                    throw new ArgumentException($"Incorrect command - {parameters}.");
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void List(string parameters)
        {
            var list = Program.FileCabinetService.GetRecords();
            foreach (var x in list)
            {
                Console.WriteLine($"#{x.Id}, {x.FirstName}, {x.LastName}, {x.DateOfBirth.ToString("yyyy-MMM-dd", Thread.CurrentThread.CurrentCulture)}, {x.Gender}, {x.Salary}, {x.Points}");
            }
        }

        private static Tuple<string, string, DateTime, char, decimal, short> Convertor()
        {
            Console.Write("First name: ");
            string firstName = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException($"You entered incorrect {nameof(firstName)}({firstName}) person, {nameof(firstName)} can't be a null or empty. Enter again");
            }

            Console.Write("Last name: ");
            string lastName = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException($"You entered incorrect {nameof(lastName)}({lastName}) person, {nameof(firstName)} can't be a null or empty. Enter again");
            }

            Console.Write("Date of birth: ");
            if (!DateTime.TryParse(Console.ReadLine().Trim(), Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out DateTime dayBirth))
            {
                throw new ArgumentException($"You entered incorrect {nameof(dayBirth)}({dayBirth}) person. Enter again");
            }

            Console.Write("Your gender: ");
            if (!char.TryParse(Console.ReadLine().Trim().ToUpperInvariant(), out char gender))
            {
                throw new ArgumentException($"You entered incorrect {nameof(gender)}({gender}) person. Enter again");
            }

            Console.Write("Your salary: ");
            if (!decimal.TryParse(Console.ReadLine().Trim(), out decimal salary))
            {
                throw new ArgumentException($"You entered incorrect {nameof(salary)}({salary}) person. Enter again");
            }

            Console.Write("Your points: ");
            if (!short.TryParse(Console.ReadLine().Trim(), out short points))
            {
                throw new ArgumentException($"You entered incorrect {nameof(points)}({points}) person. Enter again");
            }

            return new Tuple<string, string, DateTime, char, decimal, short>(firstName, lastName, dayBirth, gender, salary, points);
        }

        private static void ShortShowRecords(FileCabinetRecord[] list)
        {
            foreach (var x in list)
            {
                Console.WriteLine($"#{x.Id}, {x.FirstName}, {x.DateOfBirth.ToString("yyyy-MMM-dd", Thread.CurrentThread.CurrentCulture)}");
            }
        }
    }
}