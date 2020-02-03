using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// Describes working with <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="T">Type of Key in <see cref="DictionaryService{T}"/>.</typeparam>
    public class DictionaryService<T>
    {
        private readonly Dictionary<T, List<FileCabinetRecord>> dictionary;

        private T key;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryService{T}"/> class.
        /// </summary>
        /// <param name="dictionary">Instance of <see cref="Dictionary{TKey, TValue}"/>.</param>
        public DictionaryService(Dictionary<T, List<FileCabinetRecord>> dictionary)
        {
            this.dictionary = dictionary;
        }

        /// <summary>
        /// Add new record <see cref="FileCabinetRecord"/> in the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="fileCabinet">Add <see cref="FileCabinetRecord"/>.</param>
        /// <param name="key">The key of <see cref="FileCabinetRecord"/>.</param>
        public void AddRecord(FileCabinetRecord fileCabinet, T key)
        {
            this.key = GetKeyForDictionary(key);
            if (this.CheckKeyInDictionary(this.key))
            {
                this.dictionary[this.key].Add(fileCabinet);
            }
            else
            {
                this.dictionary.Add(this.key, new List<FileCabinetRecord>());
                this.dictionary[this.key].Add(fileCabinet);
            }
        }

        /// <summary>
        /// Find <see cref="FileCabinetRecord"/>s in the <see cref="Dictionary{TKey, TValue}"/> by key <paramref name="byParam"/>.
        /// </summary>
        /// <param name="byParam">Key of the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <returns>List of the <see cref="FileCabinetRecord"/>s.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByParam(T byParam)
        {
            this.CheckKeyDictionaryOnValid(byParam);
            var records = this.dictionary[byParam];
            if (records.Count == 0)
            {
                throw new ArgumentNullException($"There is a key in the dictionary, but the {nameof(records)} is empty.");
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        /// <summary>
        /// Edits record in the <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="newFileRecord"><see cref="FileCabinetRecord"/> what user want to changes.</param>
        /// <param name="newFileRecordKey">Key of <paramref name="newFileRecord"/>.</param>
        /// <param name="keyEdit">The key in the <see cref="DictionaryService{T}"/>, where the <see cref="FileCabinetRecord"/> will be change.</param>
        public void EditRecord(FileCabinetRecord newFileRecord, T newFileRecordKey, T keyEdit)
        {
            keyEdit = GetKeyForDictionary(keyEdit);
            newFileRecordKey = GetKeyForDictionary(newFileRecordKey);
            var record = this.dictionary[keyEdit].Find(rec => rec.Id == newFileRecord.Id);
            if (!this.dictionary.ContainsKey(newFileRecordKey))
            {
                this.RemoveRecord(record, keyEdit);
                record = (FileCabinetRecord)newFileRecord?.Clone();
                this.AddRecord(record, newFileRecordKey);
            }
            else
            {
                record.Copy(newFileRecord);
            }
        }

        private static dynamic GetKeyForDictionary(T key)
        {
            if (typeof(T) == typeof(string))
            {
                return key.ToString().ToUpperInvariant();
            }

            if (typeof(T) == typeof(DateTime))
            {
                return key;
            }

            throw new ArgumentException($"Dictionary isn't cantain {nameof(key).ToString(CultureInfo.CurrentCulture)}.");
        }

        private void RemoveRecord(FileCabinetRecord fileCabinet, T key)
        {
            this.key = GetKeyForDictionary(key);
            this.dictionary[this.key].Remove(fileCabinet);
        }

        private bool CheckKeyInDictionary(T key)
        {
            return this.dictionary.ContainsKey(GetKeyForDictionary(key)) ? true : false;
        }

        private void CheckKeyDictionaryOnValid(T key)
        {
            if (!this.CheckKeyInDictionary(key))
            {
                throw new KeyNotFoundException($"The {nameof(key).ToString(CultureInfo.CurrentCulture)} isn't found");
            }
        }
    }
}