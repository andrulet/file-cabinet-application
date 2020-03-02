using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileCabinetApp.Converters;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Provides operations with records using file system.
    /// </summary>
    public class FileCabinetFileSystemService : IFileCabinetService
    {
        private readonly IRecordValidator validator;
        private readonly FileWorker fileWorker;
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly DictionaryService<string> dictionaryByFirstNameKey = new DictionaryService<string>(new Dictionary<string, List<FileCabinetRecord>>());
        private readonly DictionaryService<string> dictionaryByLastNameKey = new DictionaryService<string>(new Dictionary<string, List<FileCabinetRecord>>());
        private readonly DictionaryService<DateTime> dictionaryByDateOfBirthKey = new DictionaryService<DateTime>(new Dictionary<DateTime, List<FileCabinetRecord>>());

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="validator">Reference on IRecordValidator.</param>
        public FileCabinetFileSystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.fileWorker = new FileWorker(fileStream);
            var countOfRecords = this.fileWorker.GetCountOfRecordsInFile();
            var arrayofRecords = new FileCabinetRecord[countOfRecords];
            this.fileWorker.GetRecords().CopyTo(arrayofRecords, 0);
            this.list = arrayofRecords.ToList();
            this.AddRecordsToDictionary();
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
            this.fileWorker.WriteNewRecord(record);
            this.dictionaryByFirstNameKey.AddRecord(record, record.FirstName);
            this.dictionaryByLastNameKey.AddRecord(record, record.LastName);
            this.dictionaryByDateOfBirthKey.AddRecord(record, record.DateOfBirth);
            return this.list.Count;
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
            this.fileWorker.EditRecordInFile(record);
            this.dictionaryByFirstNameKey.EditRecord(record, record.FirstName, newRecord.FirstName);
            this.dictionaryByLastNameKey.EditRecord(record, record.LastName, newRecord.LastName);
            this.dictionaryByDateOfBirthKey.EditRecord(record, record.DateOfBirth, newRecord.DateOfBirth);
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
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            if (this.fileWorker.GetCountOfRecordsInFile() == 0)
            {
                throw new ArgumentNullException($"The count of records in the storage equels 0");
            }

            return this.fileWorker.GetRecords();
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.list.Count;
        }

        private void AddRecordsToDictionary()
        {
            foreach (var record in this.list)
            {
                this.dictionaryByFirstNameKey.AddRecord(record, record.FirstName);
                this.dictionaryByLastNameKey.AddRecord(record, record.LastName);
                this.dictionaryByDateOfBirthKey.AddRecord(record, record.DateOfBirth);
            }
        }
    }
}
