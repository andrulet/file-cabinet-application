﻿using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly DictionaryService<string> dictionaryByFirstNameKey = new DictionaryService<string>(new Dictionary<string, List<FileCabinetRecord>>());
        private readonly DictionaryService<string> dictionaryByLastNameKey = new DictionaryService<string>(new Dictionary<string, List<FileCabinetRecord>>());
        private readonly DictionaryService<DateTime> dictionaryByDateOfBirthKey = new DictionaryService<DateTime>(new Dictionary<DateTime, List<FileCabinetRecord>>());

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
            this.dictionaryByFirstNameKey.AddRecord(record, record.FirstName);
            this.dictionaryByLastNameKey.AddRecord(record, record.LastName);
            this.dictionaryByDateOfBirthKey.AddRecord(record, record.DateOfBirth);
            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char gender, decimal salary, short points)
        {
            CheckFieldsOnException(firstName, lastName, dateOfBirth, gender, salary, points);
            var record = this.list.ElementAt(id - 1);
            var newRecord = (FileCabinetRecord)record.Clone();
            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.Gender = gender;
            record.Salary = salary;
            record.Points = points;
            this.dictionaryByFirstNameKey.EditRecord(record, record.FirstName, newRecord.FirstName);
            this.dictionaryByLastNameKey.EditRecord(record, record.LastName, newRecord.LastName);
            this.dictionaryByDateOfBirthKey.EditRecord(record, record.DateOfBirth, newRecord.DateOfBirth);
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.dictionaryByFirstNameKey.FindByParam(firstName);
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            return this.dictionaryByLastNameKey.FindByParam(lastName);
        }

        public FileCabinetRecord[] FindByDate(DateTime dayOfBirth)
        {
            return this.dictionaryByDateOfBirthKey.FindByParam(dayOfBirth);
        }

        public bool CheckId(int id)
        {
            var record = this.list.Find(rec => rec.Id == id);
            if (record == null)
            {
                throw new ArgumentNullException($"Record with {nameof(id)} = {id} not found.");
            }
            else
            {
                return true;
            }
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
