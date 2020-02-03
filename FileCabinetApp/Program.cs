using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Resource;
using FileCabinetApp.Services;
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
            new Tuple<string, Action<string>>("export", Export),
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
            new string[] { "export", "export records to csv file", "The 'export' export records to csv file." },
        };

        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService;
        private static Func<string, Tuple<bool, string>> firstNameValidator;
        private static Func<string, Tuple<bool, string>> lastNameValidator;
        private static Func<DateTime, Tuple<bool, string>> dateOfBirthValidator;
        private static Func<char, Tuple<bool, string>> genderValidator;
        private static Func<decimal, Tuple<bool, string>> salaryValidator;
        private static Func<short, Tuple<bool, string>> pointsValidator;

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
                Console.WriteLine($"{Resources.DevelopedBy}{Resources.DeveloperName}");
                Console.WriteLine(Resources.HintMessage);
                Console.WriteLine();

                do
                {
                    Console.Write(Resources.GreaterThan);
                    var inputs = Console.ReadLine().Split(' ', 2);
                    const int commandIndex = 0;
                    var command = inputs[commandIndex];

                    if (string.IsNullOrEmpty(command))
                    {
                        Console.WriteLine(Resources.HintMessage);
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
                Console.WriteLine(Resources.AvailableCommands);

                foreach (var helpMessage in HelpMessages)
                {
                    Console.WriteLine($"\t{helpMessage[Program.CommandHelpIndex]}\t- {helpMessage[Program.DescriptionHelpIndex]}");
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine(Resources.Exiting);
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            GetValidators();
            Console.Write(Resources.FirstName);
            var firstName = ReadInput(Converter.StringConvertor, firstNameValidator);
            Console.Write(Resources.LastName);
            var lastName = ReadInput(Converter.StringConvertor, lastNameValidator);
            Console.Write(Resources.DateOfBirth);
            var dateOfBirth = ReadInput(Converter.DateConvertor, dateOfBirthValidator);
            Console.Write(Resources.Gender);
            var gender = ReadInput(Converter.CharConvertor, genderValidator);
            Console.Write(Resources.Salary);
            var salary = ReadInput(Converter.DecimalConvertor, salaryValidator);
            Console.Write(Resources.Points);
            var points = ReadInput(Converter.ShortConvertor, pointsValidator);
            var validParametres = new ParametersForRecord(firstName, lastName, dateOfBirth, gender, salary, points);
            int number = fileCabinetService.CreateRecord(validParametres);
            Console.WriteLine($"Record #{number} is created");
        }

        private static void Edit(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                Console.WriteLine($"You entered incorrect {parameter}.");
            }

            if (int.TryParse(parameter, out int id))
            {
                if (fileCabinetService.GetRecords().Where(rec => rec.Id == id) != null)
                {
                    GetValidators();
                    Console.Write(Resources.FirstName);
                    var firstName = ReadInput(Converter.StringConvertor, firstNameValidator);
                    Console.Write(Resources.LastName);
                    var lastName = ReadInput(Converter.StringConvertor, lastNameValidator);
                    Console.Write(Resources.DateOfBirth);
                    var dateOfBirth = ReadInput(Converter.DateConvertor, dateOfBirthValidator);
                    Console.Write(Resources.Gender);
                    var gender = ReadInput(Converter.CharConvertor, genderValidator);
                    Console.Write(Resources.Salary);
                    var salary = ReadInput(Converter.DecimalConvertor, salaryValidator);
                    Console.Write(Resources.Points);
                    var points = ReadInput(Converter.ShortConvertor, pointsValidator);
                    var validParametres = new ParametersForRecord(firstName, lastName, dateOfBirth, gender, salary, points, id);
                    fileCabinetService.EditRecord(validParametres);
                    Console.WriteLine($"Record #{id} is updated.");
                }
                else
                {
                    Console.WriteLine($"The record with Id = {id} not found.");
                }
            }
            else
            {
                Console.WriteLine($"{nameof(parameter)}({parameter}) must be an integer.");
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

                ReadOnlyCollection<FileCabinetRecord> list;
                var command = parameters.Trim().ToUpperInvariant().Split(' ');
                if (string.Equals(command[0], "FIRSTNAME", StringComparison.InvariantCultureIgnoreCase))
                {
                    list = fileCabinetService.FindByFirstName(command[1].Trim('"'));
                    CheckCollectionOnEmpty(list);
                    ShortShowRecords(list);
                }
                else if (string.Equals(command[0], "LASTNAME", StringComparison.InvariantCultureIgnoreCase))
                {
                    list = fileCabinetService.FindByLastName(command[1].Trim('"'));
                    CheckCollectionOnEmpty(list);
                    ShortShowRecords(list);
                }
                else if (string.Equals(command[0], "DATE", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!DateTime.TryParse(command[1].Trim('"'), out DateTime date))
                    {
                        throw new ArgumentException($"Incorrect entered key - {date}.");
                    }

                    list = fileCabinetService.FindByDate(date);
                    CheckCollectionOnEmpty(list);
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

        private static void Export(string parametres)
        {
            if (string.IsNullOrEmpty(parametres))
            {
                Console.WriteLine($"The {nameof(parametres)}({parametres}) can't be null or empty.");
                return;
            }

            var commands = parametres.Trim().ToLower(Thread.CurrentThread.CurrentCulture).Split(' ', 2);
            if (commands.Length != 2)
            {
                Console.WriteLine($"You entered incorrect count of the {nameof(commands)}({commands.Length})");
                return;
            }

            string exportType = commands[0];
            string path = commands[1];
            string format;
            Action<string> write;
            if (string.Equals(exportType, "csv", StringComparison.InvariantCultureIgnoreCase))
            {
                write = WriteCsv;
                format = "^([^/]+)(.csv)$";
            }
            else
            {
                Console.WriteLine($"You entered incorrect {nameof(exportType)}({exportType}). For example csv OR xml.");
                return;
            }

            if (!Regex.IsMatch(path, format))
            {
                Console.WriteLine($@"You entered incorrect {nameof(path)}({path}). For example - d:\records.csv OR d:\records.xml");
                return;
            }

            if (File.Exists(path) && !IsRewriteFile(path))
            {
                return;
            }

            write(path);
        }

        private static void List(string parameters)
        {
            var list = Program.fileCabinetService.GetRecords();
            foreach (var x in list)
            {
                Console.WriteLine($"#{x.Id}, {x.FirstName}, {x.LastName}, {x.DateOfBirth.ToString("yyyy-MMM-dd", Thread.CurrentThread.CurrentCulture)}, {x.Gender}, {x.Salary}, {x.Points}");
            }
        }

        private static void ShortShowRecords(ReadOnlyCollection<FileCabinetRecord> list)
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
                Console.WriteLine(Resources.defaultValidation);
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
                        Console.WriteLine(Resources.castomValidation);
                        return;
                    }
                    else if (commands[1].Equals("DEFAULT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fileCabinetService = new FileCabinetService(new DefaultValidator());
                        Console.WriteLine(Resources.defaultValidation);
                        return;
                    }
                    else
                    {
                        fileCabinetService = new FileCabinetService(new DefaultValidator());
                        Console.WriteLine(Resources.unknownValidation);
                        return;
                    }
                }
            }

            fileCabinetService = new FileCabinetService(new DefaultValidator());
            Console.WriteLine(Resources.unknownValidation);
        }

        private static void CheckCollectionOnEmpty(ReadOnlyCollection<FileCabinetRecord> collection)
        {
            if (collection.Count == 0)
            {
                throw new ArgumentNullException($"There is a key in the dictionary, but the {nameof(collection)} is empty.");
            }
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static void GetValidators()
        {
            if (fileCabinetService.GetTypeValidator() == typeof(DefaultValidator))
            {
                firstNameValidator = DefaultValidator.FirstNameValidator;
                lastNameValidator = DefaultValidator.LastNameValidator;
                dateOfBirthValidator = DefaultValidator.DateTimeValidator;
                genderValidator = DefaultValidator.CharValidator;
                salaryValidator = DefaultValidator.DecimalValidator;
                pointsValidator = DefaultValidator.ShortValidator;
            }

            if (fileCabinetService.GetTypeValidator() == typeof(CustomValidator))
            {
                firstNameValidator = CustomValidator.FirstNameValidator;
                lastNameValidator = CustomValidator.LastNameValidator;
                dateOfBirthValidator = CustomValidator.DateTimeValidator;
                genderValidator = CustomValidator.CharValidator;
                salaryValidator = CustomValidator.DecimalValidator;
                pointsValidator = CustomValidator.ShortValidator;
            }
        }

        private static bool IsRewriteFile(string path)
        {
            Console.WriteLine($"File is exist - rewrite {path}? [Y/n]");
            char answer;
            bool isCorrect;
            do
            {
                isCorrect = char.TryParse(Console.ReadLine().Trim(), out answer) &&
                         (answer == 'Y' || answer == 'n');
                if (!isCorrect)
                {
                    Console.Write($"You entered incorrect {nameof(answer)}({answer}). Please, correct your input.");
                }
            }
            while (!isCorrect);

            return answer == 'Y';
        }

        private static void WriteCsv(string path)
        {
            try
            {
                var writer = new StreamWriter(File.Create(path));
                fileCabinetService.MakeSnapshot().SaveToCsv(writer);
                writer.Close();
                Console.WriteLine($"All records are exported to file {path}.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Export failed: can't open file {path}.");
            }
        }
    }
}