namespace com.codeplex.dbabstraction.Contact.Models {

    using System.Collections.Generic;
    using System.Data;
    using com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces;

    /// <summary>
    /// This represents a single physical or mailing address.
    /// </summary>
    public sealed class Address : IDatabaseModel {

        /// <summary>
        /// The ID of this address
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The contact ID to which this address belongs
        /// </summary>
        public int ContactID { get; set; }

        /// <summary>
        /// The street or PO Box address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// The city for this address
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state for this address
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// The ZIP code for this address
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Whether this is a physical address
        /// </summary>
        public bool IsPhysical { get; set; }

        /// <summary>
        /// Whether this is a mailing address
        /// </summary>
        public bool IsMailing { get; set; }

        /// <summary>
        /// Constructor for an empty address entry
        /// </summary>
        public Address() {
            State = new State();
        }

        /// <summary>
        /// Constructor for a filled address entry
        /// </summary>
        /// <param name="reader">
        /// An open data reader, pointing to the row to use
        /// </param>
        public Address(IDataReader reader) {

            ID = reader.GetInt32(reader.GetOrdinal("address_id"));
            ContactID = reader.GetInt32(reader.GetOrdinal("contact_id"));
            StreetAddress = reader.GetString(reader.GetOrdinal("address"));
            City = reader.GetString(reader.GetOrdinal("city"));
            ZipCode = reader.GetString(reader.GetOrdinal("zip_code"));
            IsPhysical = reader.GetBoolean(reader.GetOrdinal("physical_flag"));
            IsMailing = reader.GetBoolean(reader.GetOrdinal("mailing_flag"));

            State = new State {
                ID = reader.GetInt32(reader.GetOrdinal("state_id")),
                Code = reader.GetString(reader.GetOrdinal("state_code")),
                Name = reader.GetString(reader.GetOrdinal("state"))
            };
        }

        /// <summary>
        /// Get the properties of this object as parameters to be bound to a SQL statement 
        /// </summary>
        /// <returns>
        /// The key/value pairs representing the properties of this object
        /// </returns>
        public Dictionary<string, object> DataParameters() {

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("address_id", ID);
            parameters.Add("contact_id", ContactID);
            parameters.Add("address", StreetAddress);
            parameters.Add("city", City);
            parameters.Add("state_id", State.ID);
            parameters.Add("zip_code", ZipCode);
            parameters.Add("physical_flag", IsPhysical);
            parameters.Add("mailing_flag", IsMailing);

            return parameters;
        }
    }
}