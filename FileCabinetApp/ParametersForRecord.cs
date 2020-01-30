using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Consists of properties for creating and changing the instances of the <see cref="FileCabinetRecord"/> class.
    /// </summary>
    public class ParametersForRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersForRecord"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="salary">The salary.</param>
        /// <param name="points">The point.</param>
        /// <param name="id">The Id.</param>
        public ParametersForRecord(string firstName, string lastName, DateTime dateOfBirth, char gender, decimal salary, short points, int id = 0)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Salary = salary;
            this.Points = points;
            this.Id = id;
        }

        /// <summary>
        /// Gets the Id.
        /// </summary>
        /// <value>
        /// The Id.
        /// </value>
        public int Id { get; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; }

        /// <summary>
        /// Gets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        public DateTime DateOfBirth { get; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public char Gender { get; }

        /// <summary>
        /// Gets the salary.
        /// </summary>
        /// <value>
        /// The salary.
        /// </value>
        public decimal Salary { get; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public short Points { get; }
    }
}
