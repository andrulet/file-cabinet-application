using System;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Services
{
    /// <inheritdoc/>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <inheritdoc/>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
