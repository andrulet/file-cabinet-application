using System;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class contains custom data validation methods.
    /// </summary>
    internal class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validates <see cref="string"/> the first name parameter of record.
        /// </summary>
        /// <param name="firstName"><see cref="string"/> for validation.</param>
        /// <returns><see cref="Tuple{T1, T2}"/>, where T1 is <see cref="bool"/>, true if the conditions are met,
        /// false if doesn't. T2 is <see cref="string"/> message.</returns>
        public static Tuple<bool, string> FirstNameValidator(string firstName)
        {
            var less = 2;
            var more = 30;
            if (string.IsNullOrEmpty(firstName))
            {
                return new Tuple<bool, string>(false, firstName + " - the word is null or empty.");
            }

            var isValid = firstName.Length > less && firstName.Length < more && !firstName.Contains(' ', StringComparison.InvariantCultureIgnoreCase);
            return new Tuple<bool, string>(isValid, firstName + $" - length less than {less} simbols or more than {more}. Or consist of two or more words.");
        }

        /// <summary>
        /// Validates <see cref="string"/> the last name parameter of record.
        /// </summary>
        /// <param name="lastName"><see cref="string"/> for validation.</param>
        /// <returns><see cref="Tuple{T1, T2}"/>, where T1 is <see cref="bool"/>, true if the conditions are met,
        /// false if doesn't. T2 is <see cref="string"/> message.</returns>
        public static Tuple<bool, string> LastNameValidator(string lastName)
        {
            var less = 4;
            var more = 40;
            if (string.IsNullOrEmpty(lastName))
            {
                return new Tuple<bool, string>(false, lastName + " - the word is null or empty.");
            }

            var isValid = lastName.Length > less && lastName.Length < more && !lastName.Contains(' ', StringComparison.InvariantCultureIgnoreCase);
            return new Tuple<bool, string>(isValid, lastName + $" - length less than {less} simbols or more than {more}. Or consist of two or more words.");
        }

        /// <summary>
        /// Validates <see cref="DateTime"/> the date of birth parameter of record.
        /// </summary>
        /// <param name="dateOfBirth"><see cref="DateTime"/> for validation.</param>
        /// <returns><see cref="Tuple{T1, T2}"/>, where T1 is <see cref="bool"/>, true if the conditions are met,
        /// false if doesn't. T2 is <see cref="string"/> message.</returns>
        public static Tuple<bool, string> DateTimeValidator(DateTime dateOfBirth)
        {
            var fromDate = new DateTime(1950, 1, 1);
            var isValid = dateOfBirth > fromDate && dateOfBirth < DateTime.Now;
            return new Tuple<bool, string>(isValid, $"The {dateOfBirth} must be between {fromDate} and {DateTime.Now}.");
        }

        /// <summary>
        /// Validates <see cref="char"/> the gender parameter of record.
        /// </summary>
        /// <param name="gender"><see cref="char"/> for validation.</param>
        /// <returns><see cref="Tuple{T1, T2}"/>, where T1 is <see cref="bool"/>, true if the conditions are met,
        /// false if doesn't. T2 is <see cref="string"/> message.</returns>
        public static Tuple<bool, string> CharValidator(char gender)
        {
            var isValid = gender == 'M' | gender == 'F';
            return new Tuple<bool, string>(isValid, $"The {gender} must be 'M'/'m' or 'F'/'f'.");
        }

        /// <summary>
        /// Validates <see cref="decimal"/> the salary parameter of record.
        /// </summary>
        /// <param name="salary"><see cref="decimal"/> for validation.</param>
        /// <returns><see cref="Tuple{T1, T2}"/>, where T1 is <see cref="bool"/>, true if the conditions are met,
        /// false if doesn't. T2 is <see cref="string"/> message.</returns>
        public static Tuple<bool, string> DecimalValidator(decimal salary)
        {
            var less = 100;
            var more = decimal.MaxValue;
            var isValid = salary > less && salary < more;
            return new Tuple<bool, string>(isValid, $"The salary must be greter than {less} or less than {more}.");
        }

        /// <summary>
        /// Validates <see cref="short"/> the points parameter of record.
        /// </summary>
        /// <param name="points"><see cref="short"/> for validation.</param>
        /// <returns><see cref="Tuple{T1, T2}"/>, where T1 is <see cref="bool"/>, true if the conditions are met,
        /// false if doesn't. T2 is <see cref="string"/> message.</returns>
        public static Tuple<bool, string> ShortValidator(short points)
        {
            var less = 10;
            var more = short.MaxValue;
            var isValid = points > less && points < more;
            return new Tuple<bool, string>(isValid, $"The salary must be greter than {less} or less than {more}.");
        }

        /// <inheritdoc/>
        public void ValidateParameters(ParametersForRecord parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var validation = FirstNameValidator(parameters.FirstName);
            if (!FirstNameValidator(parameters.FirstName).Item1)
            {
                throw new ArgumentException(validation.Item2);
            }

            validation = LastNameValidator(parameters.LastName);
            if (!LastNameValidator(parameters.LastName).Item1)
            {
                throw new ArgumentException(validation.Item2);
            }

            validation = DateTimeValidator(parameters.DateOfBirth);
            if (!validation.Item1)
            {
                throw new ArgumentException(validation.Item2);
            }

            validation = CharValidator(parameters.Gender);
            if (!validation.Item1)
            {
                throw new ArgumentException(validation.Item2);
            }

            validation = DecimalValidator(parameters.Salary);
            if (!validation.Item1)
            {
                throw new ArgumentException(validation.Item2);
            }

            validation = ShortValidator(parameters.Points);
            if (!validation.Item1)
            {
                throw new ArgumentException(validation.Item2);
            }
        }
    }
}
