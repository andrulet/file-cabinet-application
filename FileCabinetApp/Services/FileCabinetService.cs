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
        /// <param name="firstName">The first name property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="lastName">The last name property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="dateOfBirth">The date of birth property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="gender">The gender property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="salary">The salary property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="points">The points property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <returns>The Id of record.</returns>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal salary, short points)
        {
            CheckFieldsOnException(firstName, lastName, dateOfBirth, gender, salary, points);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Salary = salary,
                Points = points,
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
        /// <param name="id">The id property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="firstName">The first name property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="lastName">The last name property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="dateOfBirth">The date of birth property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="gender">The gender property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="salary">The salary property for creating of <see cref="FileCabinetRecord"/> class.</param>
        /// <param name="points">The points property for creating of <see cref="FileCabinetRecord"/> class.</param>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char gender, decimal salary, short points)
        {
            CheckFieldsOnException(firstName, lastName, dateOfBirth, gender, salary, points);
            var record = this.list.ElementAt(id - 1);
            var newRecord = (FileCabinetRecord)record.Clone();
            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.Gender = gender;
            record.Salary = salary;
            record.Points = points;
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

        private static void CheckFieldsOnException(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal salary, short points)
        {
            CheckStringOnException(firstName);
            CheckStringOnException(lastName);
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
