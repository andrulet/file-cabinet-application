﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.Export;

namespace FileCabinetApp.Services
{
    /// <summary>
    /// This class makes snapshot all records before saves them in to file.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="cabinetRecords"><see cref="ReadOnlyCollection{FileCabinetRecord}"/>.</param>
        public FileCabinetServiceSnapshot(ReadOnlyCollection<FileCabinetRecord> cabinetRecords)
        {
            if (cabinetRecords == null)
            {
                throw new ArgumentNullException(nameof(cabinetRecords));
            }

            var i = 0;
            var listRecords = cabinetRecords;
            this.records = new FileCabinetRecord[listRecords.Count];
            foreach (var record in listRecords)
            {
                this.records[i] = record;
                i++;
            }
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <value> List of the records.</value>
        public ReadOnlyCollection<FileCabinetRecord> Records { get => this.records.ToList().AsReadOnly(); }

        /// <summary>
        /// Saves records to csv file.
        /// </summary>
        /// <param name="writer">The stream writer.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var csvWriter = new FileCabinetRecordCsvWriter(writer);
            if (this.records != null)
            {
                foreach (var record in this.records)
                {
                    csvWriter.Write(record);
                }
            }
        }

        /// <summary>
        /// Saves records to xml file.
        /// </summary>
        /// <param name="writer">The stream writer.</param>
        public void SaveToXml(StreamWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
            };

            using (var xmlWriter = XmlWriter.Create(writer, xmlSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("records");
                var fileCabinetRecordXmlWriter = new FileCabinetRecordXmlWriter(xmlWriter);
                if (this.records != null)
                {
                    foreach (var record in this.records)
                    {
                        fileCabinetRecordXmlWriter.Write(record);
                    }
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }
    }
}
