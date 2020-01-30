using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class contains of the methods for defualt validation.
    /// </summary>
    internal class DefaultValidator : IRecordValidator
    {
        /// <inheritdoc/>
        public void CheckStringOnException(string stringCheck)
        {
            var lessLength = 2;
            var moreLength = 60;
            if (string.IsNullOrEmpty(stringCheck))
            {
                throw new ArgumentNullException(stringCheck + " - word is null or empty.");
            }

            if (stringCheck.Length < lessLength || stringCheck.Length > moreLength)
            {
                throw new ArgumentException($"{stringCheck} - word length less than {lessLength} simbols or more than {moreLength}.");
            }
        }

        /// <inheritdoc/>
        public void ValidateParameters(ParametersForRecord parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}- word is null or empty.");
            }

            var dateOfBirth = parameters.DateOfBirth;
            var gender = parameters.Gender;
            var points = parameters.Points;
            var salary = parameters.Salary;
            this.CheckStringOnException(parameters.FirstName);
            this.CheckStringOnException(parameters.LastName);
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
