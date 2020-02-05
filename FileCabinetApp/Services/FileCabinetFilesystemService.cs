using System;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Provides operations with records using file system.
    /// </summary>
    public class FileCabinetFileSystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="validator">Reference on IRecordValidator.</param>
        public FileCabinetFileSystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
            this.validator = validator;
        }

        /// <inheritdoc/>
        public int CreateRecord(ParametersForRecord parameters)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            throw new NotImplementedException();
        }
    }
}
