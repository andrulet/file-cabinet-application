using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    public class DictionaryService<T>
    {
        private readonly Dictionary<T, List<FileCabinetRecord>> dictionary;

        private T key;

        public DictionaryService(Dictionary<T, List<FileCabinetRecord>> dictionary)
        {
            this.dictionary = dictionary;
        }

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

        public FileCabinetRecord[] FindByParam(T byParam)
        {
            this.CheckKeyDictionaryOnValid(byParam);
            var records = this.dictionary[byParam].ToArray();
            if (records.Length == 0)
            {
                throw new ArgumentNullException($"There is a key in the dictionary, but the {nameof(records)} is empty.");
            }

            return records;
        }

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