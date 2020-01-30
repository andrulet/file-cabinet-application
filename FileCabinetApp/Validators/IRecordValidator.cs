using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This interface contains of the methods for validation.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates entered parameters.
        /// </summary>
        /// <param name="parameters">Parameters for validating.</param>
        void ValidateParameters(ParametersForRecord parameters);

        /// <summary>
        /// Checks string parametr on excrption.
        /// </summary>
        /// <param name="stringCheck">String for checking.</param>
        void CheckStringOnException(string stringCheck);
    }
}
