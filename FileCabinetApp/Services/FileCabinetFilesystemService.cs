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
            return this.list.Count;
        }

        /// <inheritdoc/>
        public void EditRecord(ParametersForRecord parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDate(DateTime dayOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
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
    }
}
