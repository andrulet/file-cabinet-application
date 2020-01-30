using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Thic class consists of methods for convert entered parameters.
    /// </summary>
    internal static class Converter
    {
        private const string Name = "first name or last name";

        /// <summary>
        /// Converts input <paramref name="data"/> in to <see cref="string"/>.
        /// </summary>
        /// <param name="data">Input the first name or last name of person.</param>
        /// <returns>The <see cref="Tuple{T1, T2, T3}"/>.</returns>
        public static Tuple<bool, string, string> StringConvertor(string data)
        {
            return new Tuple<bool, string, string>(true, Name, data?.Trim());
        }

        /// <summary>
        /// Converts input <paramref name="data"/> in to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="data">Input the date of birth of person.</param>
        /// <returns>The <see cref="Tuple{T1, T2, T3}"/>.</returns>
        public static Tuple<bool, string, DateTime> DateConvertor(string data)
        {
            bool flag = DateTime.TryParse(data?.Trim(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out DateTime dayBirth);
            return new Tuple<bool, string, DateTime>(flag, dayBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US")), dayBirth);
        }

        /// <summary>
        /// Converts input <paramref name="data"/> in to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="data">Input the gender of person.</param>
        /// <returns>The <see cref="Tuple{T1, T2, T3}"/>.</returns>
        public static Tuple<bool, string, char> CharConvertor(string data)
        {
            bool flag = char.TryParse(data?.Trim().ToUpperInvariant(), out char gender);
            return new Tuple<bool, string, char>(flag, nameof(gender), gender);
        }

        /// <summary>
        /// Converts input <paramref name="data"/> in to <see cref="decimal"/>.
        /// </summary>
        /// <param name="data">Input the salary of person.</param>
        /// <returns>The <see cref="Tuple{T1, T2, T3}"/>.</returns>
        public static Tuple<bool, string, decimal> DecimalConvertor(string data)
        {
            bool flag = decimal.TryParse(data?.Trim(), out decimal salary);
            return new Tuple<bool, string, decimal>(flag, nameof(salary), salary);
        }

        /// <summary>
        /// Converts input <paramref name="data"/> in to <see cref="short"/>.
        /// </summary>
        /// <param name="data">Input the points of person.</param>
        /// <returns>The <see cref="Tuple{T1, T2, T3}"/>.</returns>
        public static Tuple<bool, string, short> ShortConvertor(string data)
        {
            bool flag = short.TryParse(data?.Trim(), out short points);
            return new Tuple<bool, string, short>(flag, nameof(points), points);
        }
    }
}
