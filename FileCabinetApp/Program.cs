using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Includes logic of integration with user.
    /// </summary>
    public static class Program
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

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
        private static FileCabinetService fileCabinetService;

        /// <summary>
        /// Start point of the application.
        /// </summary>
        /// <param name="args">Array of a console string.</param>
        public static void Main(string[] args)
        {
            try
            {
                if (args == null)
                {
                    throw new ArgumentNullException(nameof(args));
                }

                ValidArgs(args);
                Console.OutputEncoding = Encoding.UTF8;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Console.WriteLine($"{Resources.Resources.DevelopedBy}{Resources.Resources.DeveloperName}");
                Console.WriteLine(Resources.Resources.HintMessage);
                Console.WriteLine();

                do
                {
                    Console.Write(Resources.Resources.GreaterThan);
                    var inputs = Console.ReadLine().Split(' ', 2);
                    const int commandIndex = 0;
                    var command = inputs[commandIndex];

                    if (string.IsNullOrEmpty(command))
                    {
                        Console.WriteLine(Resources.Resources.HintMessage);
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
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
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
                Console.WriteLine(Resources.Resources.AvailableCommands);

                foreach (var helpMessage in HelpMessages)
                {
                    Console.WriteLine($"\t{helpMessage[Program.CommandHelpIndex]}\t- {helpMessage[Program.DescriptionHelpIndex]}");
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine(Resources.Resources.Exiting);
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            try
            {
                ParametersForRecord converParameters = Convertor();
                int number = Program.fileCabinetService.CreateRecord(converParameters);
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
                if (!int.TryParse(parameters, out int id) || id < 1 || id > int.MaxValue)
                {
                    throw new ArgumentException($"Incorrect {nameof(id)}({id}). Id must be more than 0 and less {int.MaxValue}.");
                }

                if (fileCabinetService.CheckId(id))
                {
                    ParametersForRecord converParameters = Convertor();
                    fileCabinetService.EditRecord(new ParametersForRecord(
                        converParameters.FirstName,
                        converParameters.LastName,
                        converParameters.DateOfBirth,
                        converParameters.Gender,
                        converParameters.Salary,
                        converParameters.Points,
                        id));
                    Console.WriteLine($"Record #{id} is created");
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
                    list = fileCabinetService.FindByFirstName(command[1].Trim('"'));
                    ShortShowRecords(list);
                }
                else if (string.Equals(command[0], "LASTNAME", StringComparison.InvariantCultureIgnoreCase))
                {
                    list = fileCabinetService.FindByLastName(command[1].Trim('"'));
                    ShortShowRecords(list);
                }
                else if (string.Equals(command[0], "DATE", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!DateTime.TryParse(command[1].Trim('"'), out DateTime date))
                    {
                        throw new ArgumentException($"Incorrect entered key - {date}.");
                    }

                    list = fileCabinetService.FindByDate(date);
                    ShortShowRecords(list);
                }
                else
                {
                    throw new ArgumentException($"Incorrect command - {parameters}.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
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
            var list = Program.fileCabinetService.GetRecords();
            foreach (var x in list)
            {
                Console.WriteLine($"#{x.Id}, {x.FirstName}, {x.LastName}, {x.DateOfBirth.ToString("yyyy-MMM-dd", Thread.CurrentThread.CurrentCulture)}, {x.Gender}, {x.Salary}, {x.Points}");
            }
        }

        private static ParametersForRecord Convertor()
        {
            Console.Write(Resources.Resources.FirstName);
            string firstName = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException($"You entered incorrect {nameof(firstName)}({firstName}) person, {nameof(firstName)} can't be a null or empty. Enter again");
            }

            Console.Write(Resources.Resources.LastName);
            string lastName = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException($"You entered incorrect {nameof(lastName)}({lastName}) person, {nameof(firstName)} can't be a null or empty. Enter again");
            }

            Console.Write(Resources.Resources.DateOfBirth);
            if (!DateTime.TryParse(Console.ReadLine().Trim(), Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out DateTime dayBirth))
            {
                throw new ArgumentException($"You entered incorrect {nameof(dayBirth)}({dayBirth}) person. Enter again");
            }

            Console.Write(Resources.Resources.Gender);
            if (!char.TryParse(Console.ReadLine().Trim().ToUpperInvariant(), out char gender))
            {
                throw new ArgumentException($"You entered incorrect {nameof(gender)}({gender}) person. Enter again");
            }

            Console.Write(Resources.Resources.Salary);
            if (!decimal.TryParse(Console.ReadLine().Trim(), out decimal salary))
            {
                throw new ArgumentException($"You entered incorrect {nameof(salary)}({salary}) person. Enter again");
            }

            Console.Write(Resources.Resources.Points);
            if (!short.TryParse(Console.ReadLine().Trim(), out short points))
            {
                throw new ArgumentException($"You entered incorrect {nameof(points)}({points}) person. Enter again");
            }

            return new ParametersForRecord(firstName, lastName, dayBirth, gender, salary, points);
        }

        private static void ShortShowRecords(FileCabinetRecord[] list)
        {
            foreach (var x in list)
            {
                Console.WriteLine($"#{x.Id}, {x.FirstName}, {x.LastName}, {x.DateOfBirth.ToString("yyyy-MMM-dd", Thread.CurrentThread.CurrentCulture)}");
            }
        }

        private static void ValidArgs(string[] args)
        {
            if (args.Length == 0)
            {
                fileCabinetService = new FileCabinetService(new DefaultValidator());
                Console.WriteLine(Resources.Resources.defaultValidation);
                return;
            }

            string[] commands = new string[args.Length + 1];
            if (args[0].StartsWith("--", StringComparison.InvariantCultureIgnoreCase) && args.Length == 1)
            {
                commands = args[0].Substring(2).ToUpperInvariant().Split('=', 2);
            }
            else if (args[0].StartsWith("-", StringComparison.InvariantCultureIgnoreCase) && args.Length == 2)
            {
                commands = new string[args.Length];
                commands[0] = args[0].Substring(1).ToUpperInvariant();
                commands[1] = args[1].ToUpperInvariant();
            }

            if (commands[^1] != null)
            {
                if (commands[0].Equals("VALIDATION-RULES", StringComparison.InvariantCultureIgnoreCase) ||
                commands[0].Equals("V", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (commands[1].Equals("CUSTOM", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fileCabinetService = new FileCabinetService(new CustomValidator());
                        Console.WriteLine(Resources.Resources.castomValidation);
                        return;
                    }
                    else if (commands[1].Equals("DEFAULT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fileCabinetService = new FileCabinetService(new DefaultValidator());
                        Console.WriteLine(Resources.Resources.defaultValidation);
                        return;
                    }
                    else
                    {
                        fileCabinetService = new FileCabinetService(new DefaultValidator());
                        Console.WriteLine(Resources.Resources.unknownValidation);
                        return;
                    }
                }
            }

            fileCabinetService = new FileCabinetService(new DefaultValidator());
            Console.WriteLine(Resources.Resources.unknownValidation);
        }
    }
}