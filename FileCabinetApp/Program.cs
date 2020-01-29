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
        };

        private static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the count of records", "The 'stat' prints the count of records." },
            new string[] { "create", "creates a new record about person", "The 'create' creates a new record about person." },
            new string[] { "list", "shows a list of records", "The 'list' shows a list of records." },
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
            bool flag = true;
            do
            {
                Console.Write("First name: ");
                string firstName = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(firstName))
                {
                    Console.WriteLine($"You entered incorrect {nameof(firstName)}({firstName}) person. Enter again");
                    continue;
                }

                Console.Write("Last name: ");
                string lastName = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(lastName))
                {
                    Console.WriteLine($"You entered incorrect {nameof(lastName)}({lastName}) person. Enter again");
                    continue;
                }

                Console.Write("Date of birth: ");
                if (!DateTime.TryParse(Console.ReadLine().Trim(), Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out DateTime dayBirth))
                {
                    Console.WriteLine($"You entered incorrect {nameof(dayBirth)}({dayBirth}) person. Enter again");
                    continue;
                }

                Console.Write("Your gender: ");
                char gender = char.Parse(Console.ReadLine().Trim().ToUpperInvariant());
                if (gender != 'M' & gender != 'F')
                {
                    Console.WriteLine($"You entered incorrect {nameof(gender)}({gender}) person. Enter again");
                    continue;
                }

                Console.Write("Your salary: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
                {
                    Console.WriteLine($"You entered incorrect {nameof(salary)}({salary}) person. Enter again");
                    continue;
                }

                Console.Write("Your points: ");
                if (!short.TryParse(Console.ReadLine(), out short points))
                {
                    Console.WriteLine($"You entered incorrect {nameof(points)}({points}) person. Enter again");
                    continue;
                }

                int number = Program.FileCabinetService.CreateRecord(firstName, lastName, dayBirth, gender, salary, points);
                Console.WriteLine($"Record #{number} is created");
                flag = false;
            }
            while (flag);
        }

        private static void List(string parameters)
        {
            var list = Program.FileCabinetService.GetRecords();
            foreach (var x in list)
            {
                Console.WriteLine($"#{x.Id}, {x.FirstName}, {x.LastName}, {x.DateOfBirth.ToString("yyyy-MMM-dd", Thread.CurrentThread.CurrentCulture)}, {x.Gender}, {x.Salary}, {x.Points}");
            }
        }
    }
}