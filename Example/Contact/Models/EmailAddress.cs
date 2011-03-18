namespace DatabaseAbstraction.Contact.Models {

    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;

    /// <summary>
    /// This represents a single e-mail address.
    /// </summary>
    public sealed class EmailAddress : IDatabaseModel {

        /// <summary>
        /// The ID of this e-mail address.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The ID of the contact entry to which this e-mail address belongs.
        /// </summary>
        public int ContactID { get; set; }

        /// <summary>
        /// The type of contact represented by this e-mail address.
        /// </summary>
        public ContactType? ContactType { get; set; }

        /// <summary>
        /// The actual e-mail address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Comments regarding this e-mail address.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Constructor for a populated object.
        /// </summary>
        /// <param name="poReader">An open data reader, pointing to the row to use.</param>
        public EmailAddress(IDataReader reader) {
            ID = reader.GetInt32(reader.GetOrdinal("email_id"));
            ContactID = reader.GetInt32(reader.GetOrdinal("contact_id"));
            ContactType = (ContactType?) reader.GetInt32(reader.GetOrdinal("contact_type_id"));
            Address = reader.GetString(reader.GetOrdinal("address"));
            Comments = NullUtils.GetStringOrNull(reader, "comments");
        }

        /// <summary>
        /// Constructor for an empty object.
        /// </summary>
        public EmailAddress() {
        }

        /// <summary>
        /// Get the properties of this object as parameters to be bound to a SQL statement 
        /// </summary>
        /// <returns>
        /// The key/value pairs representing the properties of this object
        /// </returns>
        public Dictionary<string, object> DataParameters() {

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("email_id", ID);
            parameters.Add("contact_id", ContactID);
            parameters.Add("contact_type_id", ContactType.Value);
            parameters.Add("address", Address);
            parameters.Add("comments", Comments);

            return parameters;
        }
    }
}