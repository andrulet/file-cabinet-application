using System;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <inheritdoc/>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <inheritdoc/>
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
