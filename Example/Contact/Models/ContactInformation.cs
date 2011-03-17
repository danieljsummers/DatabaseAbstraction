namespace DatabaseAbstraction.Contact.Models {

    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;

    /// <summary>
    /// This represents a set of contact information.
    /// </summary>
    public sealed class ContactInformation : IDatabaseModel {

        /// <summary>
        /// The ID for this contact.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The addresses for this contact.
        /// </summary>
        public List<Address> Addresses { get; private set; }

        /// <summary>
        /// The phone numbers for this contact.
        /// </summary>
        public List<PhoneNumber> PhoneNumbers { get; private set; }

        /// <summary>
        /// The e-mail addresses for this contact.
        /// </summary>
        public List<EmailAddress> EmailAddresses { get; private set; }

        /// <summary>
        /// Constructor for a populated object.
        /// </summary>
        /// <param name="poReader">An open data reader, pointing to the row to use.</param>
        /// <remarks>NOTE: Does not populate any of the lists.</remarks>
        public ContactInformation(IDataReader poReader) {
            ID = poReader.GetInt32(poReader.GetOrdinal("id"));
            Addresses = new List<Address>();
            PhoneNumbers = new List<PhoneNumber>();
            EmailAddresses = new List<EmailAddress>();
        }

        /// <summary>
        /// Constructor for an empty object.
        /// </summary>
        public ContactInformation() {
            Addresses = new List<Address>();
            PhoneNumbers = new List<PhoneNumber>();
            EmailAddresses = new List<EmailAddress>();
        }

        /// <summary>
        /// Get the properties of this object to be bound to a SQL statement. 
        /// </summary>
        /// <returns>
        /// The key/value pairs representing the properties of this object.
        /// </returns>
        public Dictionary<string, object> DataParameters() {

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("id", ID);

            return parameters;
        }
    }
}