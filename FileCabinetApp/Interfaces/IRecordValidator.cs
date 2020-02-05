using System;

namespace FileCabinetApp.Interfaces
{
    /// <summary>
    /// This interface contains methods for validation.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates all parameteres for creating or editing the record.
        /// </summary>
        /// <param name="parameters">Parameters for creating the instance of <see cref="FileCabinetRecord"/> class.</param>
        void ValidateParameters(ParametersForRecord parameters);
    }
}