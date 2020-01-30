using System;

namespace FileCabinetApp
{
    /// <summary>
    /// This class contains data about the recording and operations with them, describing some information about person.
    /// </summary>
    public class FileCabinetRecord : ICloneable
    {
        /// <summary>
        /// Gets or sets id of an instance a <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <value>Integer Id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of an instance a <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <value>String first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of an instance a <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <value>String last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth of an instance a <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <value>DataTime date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets gender of an instance a <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <value>Char gender.</value>
        public char Gender { get; set; }

        /// <summary>
        /// Gets or sets salary of an instance a <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <value>Decimal salary.</value>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets points of an instance a <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <value>Short points.</value>
        public short Points { get; set; }

        /// <summary>
        /// This method copies the fields from the <see cref="FileCabinetRecord"/> <paramref name="other"/> to the carrent instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="other">It's instance of the <see cref="FileCabinetRecord"/> class from which the fields are copied.</param>
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

        /// <inheritdoc/>
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
