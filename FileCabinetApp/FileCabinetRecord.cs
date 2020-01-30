using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetRecord : ICloneable
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public char Gender { get; set; }

        public decimal Salary { get; set; }

        public short Points { get; set; }

        public void Copy(FileCabinetRecord other)
        {
            if (other == null)
            {
                throw new ArgumentNullException($"Copy {nameof(other)} is null");
            }

            this.Id = other.Id;
            this.FirstName = other.FirstName;
            this.LastName = other.LastName;
            this.DateOfBirth = other.DateOfBirth;
            this.Gender = other.Gender;
            this.Salary = other.Salary;
            this.Points = other.Points;
        }

        public object Clone()
        {
            return new FileCabinetRecord
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Salary = this.Salary,
                Points = this.Points,
            };
        }
    }
}
