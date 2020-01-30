using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// This class describes the work and actions with records of instanses of <see cref="FileCabinetRecord"/> that are stored in <see cref="List{FileCabinetRecord}"/>
    /// and <see cref="Dictionary{TKey, Tvalue}"/>, where Tvalue is <see cref="FileCabinetRecord"/> instance.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.dictionaryByFirstNameKey.FindByParam(firstName);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.dictionaryByLastNameKey.FindByParam(lastName);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDate(DateTime dayOfBirth)
        {
            return this.dictionaryByDateOfBirthKey.FindByParam(dayOfBirth);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Type GetTypeValidator()
        {
            return this.validator.GetType();
        }
    }
}
