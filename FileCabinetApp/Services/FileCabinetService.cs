using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileCabinetApp.Validators;

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
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">Reference on IRRecordValidator.</param>
        public FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }

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

            this.validator.ValidateParameters(parameters);
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

            this.validator.ValidateParameters(parameters);
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
        /// <returns>List of <see cref="FileCabinetRecord"/> class.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
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
        /// <returns>List of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.dictionaryByFirstNameKey.FindByParam(firstName);
        }

        /// <summary>
        /// Finds the records in the dictionary by the last name of records.
        /// </summary>
        /// <param name="lastName">The key for searching in the dictionary.</param>
        /// <returns>List of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.dictionaryByLastNameKey.FindByParam(lastName);
        }

        /// <summary>
        /// Finds the records in the dictionary by the date of the birth of records.
        /// </summary>
        /// <param name="dayOfBirth">The key for searching in the dictionary.</param>
        /// <returns>List of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDate(DateTime dayOfBirth)
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
    }
}
