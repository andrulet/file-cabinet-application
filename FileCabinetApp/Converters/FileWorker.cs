using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileCabinetApp.Converters
{
    /// <summary>
    /// This class works with Filestraam and operations with records.
    /// </summary>
    internal class FileWorker
    {
        private const int StatusOffSet = 0;
        private const int IdOffSet = 2;
        private const int LengthArrayOfStatus = IdOffSet - StatusOffSet;
        private const int FirstNameOffSet = 6;
        private const int LengthArrayOfId = FirstNameOffSet - IdOffSet;
        private const int LastNameOffSet = 126;
        private const int LengthArrayOfName = LastNameOffSet - FirstNameOffSet;
        private const int YearOffSet = 246;
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
        private const short Status = 1;
        private static int countOfRecords;
        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWorker"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        public FileWorker(FileStream fileStream)
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
            Array.Copy(ShortToBytes(Status), 0, recordByte, StatusOffSet, LengthArrayOfStatus);
            Array.Copy(IntToBytes(record.Id), 0, recordByte, IdOffSet, LengthArrayOfId);
            Array.Copy(StringToBytes(record.FirstName), 0, recordByte, FirstNameOffSet, LengthArrayOfName);
            Array.Copy(StringToBytes(record.LastName), 0, recordByte, LastNameOffSet, LengthArrayOfName);
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

        /// <summary>
        /// Gets records from the file.
        /// </summary>
        /// <returns>Collection of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.GetCountOfRecordsInFile();
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            FileCabinetRecord fileCabinet;
            byte[] record = new byte[RecordSize * countOfRecords];
            this.fileStream.Seek(0, SeekOrigin.Begin);
            this.fileStream.Read(record, 0, RecordSize * countOfRecords);
            for (int i = 0; i < countOfRecords; i++)
            {
                fileCabinet = new FileCabinetRecord
                {
                    Id = BitConverter.ToInt32(record, (i * RecordSize) + IdOffSet),
                    FirstName = ReadName(record, (i * RecordSize) + FirstNameOffSet),
                    LastName = ReadName(record, (i * RecordSize) + LastNameOffSet),
                    DateOfBirth = ReadDateTimeFromFile(record, i * RecordSize),
                    Gender = BitConverter.ToChar(record, (i * RecordSize) + GenderOffSet),
                    Salary = ReadDecimalFromFile(record, i * RecordSize),
                    Points = BitConverter.ToInt16(record, (i * RecordSize) + PointOffSet),
                };
                records.Add(fileCabinet);
            }

            this.fileStream.Flush();
            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Gets the count of records.
        /// </summary>
        /// <returns>The count of records.</returns>
        public int GetCountOfRecordsInFile()
        {
            countOfRecords = (int)this.fileStream.Length / RecordSize;
            return countOfRecords;
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

        private static DateTime ReadDateTimeFromFile(byte[] array, int countOfBytes)
        {
            int year = BitConverter.ToInt32(array, countOfBytes + YearOffSet);
            int month = BitConverter.ToInt32(array, countOfBytes + MonthOffSet);
            int day = BitConverter.ToInt32(array, countOfBytes + DayOffSet);
            return new DateTime(year, month, day);
        }

        private static decimal ReadDecimalFromFile(byte[] array, int countOfBytes)
        {
            byte[] decimalInBytes = new byte[LengthArrayOfSalary];
            Array.Copy(array, countOfBytes + SalaryOffSet, decimalInBytes, 0, LengthArrayOfSalary);
            var bits = new int[]
            {
                BitConverter.ToInt32(decimalInBytes, 0),
                BitConverter.ToInt32(decimalInBytes, 4),
                BitConverter.ToInt32(decimalInBytes, 8),
                BitConverter.ToInt32(decimalInBytes, 12),
            };

            return new decimal(bits);
        }

        private static string ReadName(byte[] array, int startIndex)
        {
            var name = Encoding.UTF8.GetString(array, startIndex, LengthArrayOfName);
            return name.Remove(name.IndexOf(Convert.ToChar(0), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
