using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Export
{
    /// <summary>
    /// Contains method of writing record in csv format.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private const string Delimiter = ",";
        private const string Headline = "Id, first name, last name, date of birth, salary, points";

        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));

            this.writer.WriteLine(Headline);
        }

        /// <summary>
        /// Writes the record in csv format.
        /// </summary>
        /// <param name="record">The record.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.writer.WriteLine(MakeStringFromRecord(record));
        }

        private static string MakeStringFromRecord(FileCabinetRecord record)
        {
            var sb = new StringBuilder();
            sb.Append(record.Id).Append(Delimiter);
            sb.Append(record.FirstName).Append(Delimiter);
            sb.Append(record.LastName).Append(Delimiter);
            sb.Append(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)).Append(Delimiter);
            sb.Append(record.Salary.ToString(CultureInfo.InvariantCulture)).Append(Delimiter);
            sb.Append(record.Points).Append(Delimiter);
            return sb.ToString();
        }
    }
}
