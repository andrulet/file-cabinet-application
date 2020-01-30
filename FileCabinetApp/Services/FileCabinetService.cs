using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// This class describes the work and actions with records of instanses of <see cref="FileCabinetRecord"/> that are stored in <see cref="List{FileCabinetRecord}"/>
    /// and <see cref="Dictionary{TKey, Tvalue}"/>, where Tvalue is <see cref="FileCabinetRecord"/> instance.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly DictionaryService<string> dictionaryByFirstNameKey = new DictionaryService<string>(new Dictionary<string, List<FileCabinetRecord>>());
        private readonly DictionaryService<string> dictionaryByLastNameKey = new DictionaryService<string>(new Dictionary<string, List<FileCabinetRecord>>());
        private readonly DictionaryService<DateTime> dictionaryByDateOfBirthKey = new DictionaryService<DateTime>(new Dictionary<DateTime, List<FileCabinetRecord>>());

        /// <summary>
        ///  Creates a new <see cref="FileCabinetRecord"/> class record and saves it in the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="parameters">Parameters for creating the instance of <see cref="FileCabinetRecord"/> class.</param>
        /// <returns>The Id of record.</returns>
        public int CreateRecord(ParametersForRecord parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            CheckFieldsOnException(parameters);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = parameters.DateOfBirth,
                Gender = parameters.Gender,
                Salary = parameters.Salary,
                Points = parameters.Points,
            };

            this.list.Add(record);
            this.dictionaryByFirstNameKey.AddRecord(record, record.FirstName);
            this.dictionaryByLastNameKey.AddRecord(record, record.LastName);
            this.dictionaryByDateOfBirthKey.AddRecord(record, record.DateOfBirth);
            return record.Id;
        }

        /// <summary>
        /// Change information about <see cref="FileCabinetRecord"/> class record and saves it in the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="parameters">Parameters for creating the instance of <see cref="FileCabinetRecord"/> class.</param>
        public void EditRecord(ParametersForRecord parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            CheckFieldsOnException(parameters);
            var record = this.list.ElementAt(parameters.Id - 1);
            var newRecord = (FileCabinetRecord)record.Clone();
            record.FirstName = parameters.FirstName;
            record.LastName = parameters.LastName;
            record.DateOfBirth = parameters.DateOfBirth;
            record.Gender = parameters.Gender;
            record.Salary = parameters.Salary;
            record.Points = parameters.Points;
            this.dictionaryByFirstNameKey.EditRecord(record, record.FirstName, newRecord.FirstName);
            this.dictionaryByLastNameKey.EditRecord(record, record.LastName, newRecord.LastName);
            this.dictionaryByDateOfBirthKey.EditRecord(record, record.DateOfBirth, newRecord.DateOfBirth);
        }

        /// <summary>
        /// Gets array of records.
        /// </summary>
        /// <returns>Array of <see cref="FileCabinetRecord"/> class.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// Gets number of records in the list of records.
        /// </summary>
        /// <returns>Count of record in the list.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Finds the records in the dictionary by the first name of records.
        /// </summary>
        /// <param name="firstName">The key for searching in the dictionary.</param>
        /// <returns>Array of the records.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.dictionaryByFirstNameKey.FindByParam(firstName);
        }

        /// <summary>
        /// Finds the records in the dictionary by the last name of records.
        /// </summary>
        /// <param name="lastName">The key for searching in the dictionary.</param>
        /// <returns>Array of the records.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            return this.dictionaryByLastNameKey.FindByParam(lastName);
        }

        /// <summary>
        /// Finds the records in the dictionary by the date of the birth of records.
        /// </summary>
        /// <param name="dayOfBirth">The key for searching in the dictionary.</param>
        /// <returns>Array of the records.</returns>
        public FileCabinetRecord[] FindByDate(DateTime dayOfBirth)
        {
            return this.dictionaryByDateOfBirthKey.FindByParam(dayOfBirth);
        }

        /// <summary>
        /// Checks id on list.
        /// </summary>
        /// <param name="id">Id to check.</param>
        /// <returns>True - if list contains an record with <paramref name="id"/>, false if not.</returns>
        public bool CheckId(int id)
        {
            var record = this.list.Find(rec => rec.Id == id);
            if (record == null)
            {
                throw new ArgumentNullException($"Record with {nameof(id)} = {id} not found.");
            }
            else
            {
                return true;
            }
        }

        private static void CheckStringOnException(string stringCheck)
        {
            if (stringCheck.Length < 2 || stringCheck.Length > 60)
            {
                throw new ArgumentException(stringCheck + " - word length less than 2 simbols or more than 60.");
            }

            if (string.IsNullOrEmpty(stringCheck))
            {
                throw new ArgumentNullException(stringCheck + " - word is null or empty.");
            }
        }

        private static void CheckFieldsOnException(ParametersForRecord parameters)
        {
            var dateOfBirth = parameters.DateOfBirth;
            var gender = parameters.Gender;
            var points = parameters.Points;
            var salary = parameters.Salary;
            CheckStringOnException(parameters.FirstName);
            CheckStringOnException(parameters.LastName);
            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"Invalid input {nameof(dateOfBirth)}({dateOfBirth})");
            }

            if ((gender != 'M') && (gender != 'F'))
            {
                throw new ArgumentException($"Invalid input {nameof(gender)}({gender})");
            }

            if (points > short.MaxValue || points < 0)
            {
                throw new ArgumentException($"Invalid {nameof(points)}({points}). The points mast be greater than 0 or less than " + short.MaxValue);
            }

            if (salary > decimal.MaxValue || salary < 0)
            {
                throw new ArgumentException($"Invalid {nameof(salary)}({salary}). The salary mast be greater than 0 or less than " + decimal.MaxValue);
            }
        }
    }
}
