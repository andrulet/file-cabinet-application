﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal salary, short points)
        {
            CheckFieldsOnException(firstName, lastName, dateOfBirth, gender, salary, points);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Salary = salary,
                Points = points,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        private static void CheckStringOnException(string stringCheck)
        {
            if (stringCheck.Length < 2 || stringCheck.Length > 60)
            {
                throw new ArgumentException(stringCheck + " - word length less than 2 simbols or more than 60.");
            }

            if (string.IsNullOrEmpty(stringCheck))
            {
                throw new ArgumentNullException(stringCheck + " - word is null or empty.");
            }
        }

        private static void CheckFieldsOnException(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal salary, short points)
        {
            CheckStringOnException(firstName);
            CheckStringOnException(lastName);
            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("Invalid input date of birth.");
            }

            if ((gender != 'M') && (gender != 'F'))
            {
                throw new ArgumentException("Invalid input gender");
            }

            if (points > short.MaxValue || points < 0)
            {
                throw new ArgumentException("Invalid points. The number of points less than 0 or greater than " + short.MaxValue);
            }

            if (salary > decimal.MaxValue || salary < 0)
            {
                throw new ArgumentException("Invalid salary. The salary less than 0 or greater than " + decimal.MaxValue);
            }
        }
    }
}
