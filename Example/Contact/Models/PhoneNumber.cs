namespace DatabaseAbstraction.Contact.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;

    /// <summary>
    /// This represents a single phone number.
    /// </summary>
    public sealed class PhoneNumber : IDatabaseModel
    {
        /// <summary>
        /// The ID for this phone number.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The ID of the contact to which this phone number belongs.
        /// </summary>
        public int ContactID { get; set; }

        /// <summary>
        /// The type of contact for this phone number.
        /// </summary>
        public ContactType? ContactType { get; set; }

        /// <summary>
        /// The area code for this phone number.
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// The exchange for this phone number.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// The number for this phone number.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The extension for this phone number.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Comments about this phone number.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Retrieve the full phone number in display format.
        /// </summary>
        public string DisplayFormat
        {
            get
            {
                return String.Format("({0}) {1}-{2}", AreaCode, Exchange, Number)
                    + ((!String.IsNullOrEmpty(Extension)) ? " x" + Extension : "");
            }
        }

        /// <summary>
        /// Constructor for an empty object.
        /// </summary>
        public PhoneNumber() { }

        /// <summary>
        /// Constructor for a populated object.
        /// </summary>
        /// <param name="reader">
        /// An open data reader, pointing to the row to use.
        /// </param>
        public PhoneNumber(IDataReader reader)
        {
            ID = reader.GetInt32(reader.GetOrdinal("phone_id"));
            ContactID = reader.GetInt32(reader.GetOrdinal("contact_id"));
            ContactType = (ContactType?)reader.GetInt32(reader.GetOrdinal("contact_type_id"));
            AreaCode = reader.GetString(reader.GetOrdinal("area_code"));
            Exchange = reader.GetString(reader.GetOrdinal("exchange"));
            Number = reader.GetString(reader.GetOrdinal("number"));
            Extension = NullUtils.GetStringOrNull(reader, "extension");
            Comments = NullUtils.GetStringOrNull(reader, "comments");
        }

        /// <summary>
        /// Get the properties of this object as parameters to be bound to a SQL statement 
        /// </summary>
        /// <returns>
        /// The key/value pairs representing the properties of this object.
        /// </returns>
        public Dictionary<string, object> DataParameters()
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("phone_id", ID);
            parameters.Add("contact_id", ContactID);
            parameters.Add("contact_type_id", ContactType.Value);
            parameters.Add("area_code", AreaCode);
            parameters.Add("exchange", Exchange);
            parameters.Add("number", Number);
            parameters.Add("extension", Extension);
            parameters.Add("comments", Comments);

            return parameters;
        }
    }
}