using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileCabinetApp.Interfaces
{
    /// <summary>
    /// Describes the work and actions with records of instanses of <see cref="FileCabinetRecord"/> that are stored in <see cref="List{FileCabinetRecord}"/>
    /// and <see cref="Dictionary{TKey, Tvalue}"/>, where Tvalue is <see cref="FileCabinetRecord"/> instance.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        ///  Creates a new <see cref="FileCabinetRecord"/> class record and saves it in the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="parameters">Parameters for creating the instance of <see cref="FileCabinetRecord"/> class.</param>
        /// <returns>The Id of record.</returns>
        int CreateRecord(ParametersForRecord parameters);

        /// <summary>
        /// Change information about <see cref="FileCabinetRecord"/> class record and saves it in the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="parameters">Parameters for creating the instance of <see cref="FileCabinetRecord"/> class.</param>
        void EditRecord(ParametersForRecord parameters);

        /// <summary>
        /// Gets array of records.
        /// </summary>
        /// <returns>List of <see cref="FileCabinetRecord"/> class.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets number of records in the list of records.
        /// </summary>
        /// <returns>Count of record in the list.</returns>
        int GetStat();

        /// <summary>
        /// Finds the records in the dictionary by the first name of records.
        /// </summary>
        /// <param name="firstName">The key for searching in the dictionary.</param>
        /// <returns>List of the records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds the records in the dictionary by the last name of records.
        /// </summary>
        /// <param name="lastName">The key for searching in the dictionary.</param>
        /// <returns>List of the records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds the records in the dictionary by the date of the birth of records.
        /// </summary>
        /// <param name="dayOfBirth">The key for searching in the dictionary.</param>
        /// <returns>List of the records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByDate(DateTime dayOfBirth);

        /// <summary>
        /// Checks id on list.
        /// </summary>
        /// <param name="id">Id to check.</param>
        /// <returns>True - if list contains an record with <paramref name="id"/>, false if not.</returns>
        bool CheckId(int id);
    }
}
