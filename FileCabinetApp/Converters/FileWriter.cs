using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.Converters
{
    /// <summary>
    /// This class works with Filestraam and operations with records.
    /// </summary>
    internal class FileWriter
    {
        private const int StatusOffSet = 0;
        private const int IdOffSet = 2;
        private const int LengthArrayOfStatus = IdOffSet - StatusOffSet;
        private const int FirstNameOffSet = 6;
        private const int LengthArrayOfId = FirstNameOffSet - IdOffSet;
        private const int LastNameOffSet = 126;
        private const int LengthArrayOfFirstName = LastNameOffSet - FirstNameOffSet;
        private const int YearOffSet = 246;
        private const int LengthArrayOfLastName = YearOffSet - LastNameOffSet;
        private const int MonthOffSet = 250;
        private const int LengthArrayOfYear = MonthOffSet - YearOffSet;
        private const int DayOffSet = 254;
        private const int LengthArrayOfMonth = DayOffSet - MonthOffSet;
        private const int GenderOffSet = 258;
        private const int LengthArrayOfDay = GenderOffSet - DayOffSet;
        private const int SalaryOffSet = 260;
        private const int LengthArrayOfGender = SalaryOffSet - GenderOffSet;
        private const int PointOffSet = 276;
        private const int LengthArrayOfSalary = PointOffSet - SalaryOffSet;
        private const int RecordSize = 278;
        private const int LengthArrayOfPoint = RecordSize - PointOffSet;

        private static short status = 1;

        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWriter"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileWriter(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Writes record in file.
        /// </summary>
        /// <param name="record">The record for writting in file.</param>
        public void WriteNewRecord(FileCabinetRecord record)
        {
            byte[] recordByte = new byte[RecordSize];
            Array.Copy(ShortToBytes(status), 0, recordByte, StatusOffSet, LengthArrayOfStatus);
            Array.Copy(IntToBytes(record.Id), 0, recordByte, IdOffSet, LengthArrayOfId);
            Array.Copy(StringToBytes(record.FirstName), 0, recordByte, FirstNameOffSet, LengthArrayOfFirstName);
            Array.Copy(StringToBytes(record.LastName), 0, recordByte, LastNameOffSet, LengthArrayOfLastName);
            Array.Copy(IntToBytes(record.DateOfBirth.Year), 0, recordByte, YearOffSet, LengthArrayOfYear);
            Array.Copy(IntToBytes(record.DateOfBirth.Month), 0, recordByte, MonthOffSet, LengthArrayOfMonth);
            Array.Copy(IntToBytes(record.DateOfBirth.Day), 0, recordByte, DayOffSet, LengthArrayOfDay);
            Array.Copy(CharToBytes(record.Gender), 0, recordByte, GenderOffSet, LengthArrayOfGender);
            Array.Copy(DecimalToBytes(record.Salary), 0, recordByte, SalaryOffSet, LengthArrayOfSalary);
            Array.Copy(ShortToBytes(record.Points), 0, recordByte, PointOffSet, LengthArrayOfPoint);
            this.fileStream.Seek(0, SeekOrigin.End);
            this.fileStream.Write(recordByte, 0, recordByte.Length);
            this.fileStream.Flush();
        }

        private static byte[] ShortToBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        private static byte[] IntToBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }

        private static byte[] StringToBytes(string value)
        {
            byte[] arrayOfChars = new byte[120];
            var chars = Encoding.Default.GetBytes(value);
            chars.CopyTo(arrayOfChars, 0);
            return arrayOfChars;
        }

        private static byte[] CharToBytes(char value)
        {
            return BitConverter.GetBytes(value);
        }

        private static byte[] DecimalToBytes(decimal value)
        {
            var bits = decimal.GetBits(value);
            List<byte> bytes = new List<byte>();
            foreach (var i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            return bytes.ToArray();
        }
    }
}
