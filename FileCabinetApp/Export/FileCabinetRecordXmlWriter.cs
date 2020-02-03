using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Export
{
    /// <summary>
    /// Contains method of writing record in xml format.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private const string RecordTag = "record";
        private const string IdTag = "id";
        private const string NameTag = "name";
        private const string FisrtAttr = "first";
        private const string LastAttr = "last";
        private const string DateOfBirthTag = "dateOfBirth";
        private const string Gender = "gender";
        private const string Salary = "salary";
        private const string Points = "points";

        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">The xml writer.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Writes the record in xml format.
        /// </summary>
        /// <param name="record">The record.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.writer.WriteStartElement(RecordTag);
            this.writer.WriteAttributeString(IdTag, record.Id.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteStartElement(NameTag);
            this.writer.WriteAttributeString(FisrtAttr, record.FirstName);
            this.writer.WriteAttributeString(LastAttr, record.LastName);
            this.writer.WriteEndElement();
            this.writer.WriteStartElement(DateOfBirthTag);
            this.writer.WriteValue(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();
            this.writer.WriteStartElement(Gender);
            this.writer.WriteValue(record.Gender);
            this.writer.WriteEndElement();
            this.writer.WriteStartElement(Salary);
            this.writer.WriteValue(record.Salary);
            this.writer.WriteEndElement();
            this.writer.WriteStartElement(Points);
            this.writer.WriteValue(record.Points);
            this.writer.WriteEndElement();
            this.writer.WriteEndElement();
        }
    }
}
