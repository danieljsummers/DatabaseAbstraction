namespace DatabaseAbstraction.Contact.Models
{
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Utils;

    /// <summary>
    /// This represents a set of contact information.
    /// </summary>
    public sealed class ContactInformation : IParameterProvider, IQueryFragmentProvider
    {
        #region Properties

        /// <summary>
        /// The ID for this contact.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The addresses for this contact.
        /// </summary>
        public IList<Address> Addresses { get; private set; }

        /// <summary>
        /// The phone numbers for this contact.
        /// </summary>
        public IList<PhoneNumber> PhoneNumbers { get; private set; }

        /// <summary>
        /// The e-mail addresses for this contact.
        /// </summary>
        public IList<EmailAddress> EmailAddresses { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for an empty object
        /// </summary>
        public ContactInformation()
        {
            Addresses = new List<Address>();
            PhoneNumbers = new List<PhoneNumber>();
            EmailAddresses = new List<EmailAddress>();
        }

        /// <summary>
        /// Constructor for a populated object
        /// </summary>
        /// <param name="poReader">
        /// An open data reader, pointing to the row to use.
        /// </param>
        /// <remarks>
        /// NOTE: Does not populate any of the lists.
        /// </remarks>
        public ContactInformation(IDataReader poReader) : this()
        {
            ID = poReader.GetInt32(poReader.GetOrdinal("id"));
        }

        #endregion

        #region IParameterProvider Implementation

        /// <summary>
        /// Get the properties of this object to be bound to a SQL statement. 
        /// </summary>
        /// <returns>
        /// The key/value pairs representing the properties of this object.
        /// </returns>
        public IDictionary<string, object> Parameters()
        {
            return DbUtils.SingleParameter("id", ID);
        }

        #endregion

        #region IQueryFragmentProvider Implementation

        /// <summary>
        /// Create query fragments for this model
        /// </summary>
        /// <param name="fragments">
        /// The fragment dictionary being built
        /// </param>
        public void Fragments(IDictionary<string, QueryFragment> fragments)
        {
            // From
            fragments.Add("contact.from.join_contact_type", FromJoinContactTypeFragment());

            // Where
            fragments.Add("contact.where.contact_id", WhereContactIDFragment());
        }

        /// <summary>
        /// contact.from.join_contact_type
        /// </summary>
        /// <returns>
        /// The query fragment
        /// </returns>
        private QueryFragment FromJoinContactTypeFragment()
        {
            return new QueryFragment
            {
                SQL = "INNER JOIN r_contact_type ON contact_type_id = r_contact_type_id"
            };
        }
        /// <summary>
        /// contact.where.contact_id
        /// </summary>
        /// <returns>
        /// The query fragment
        /// </returns>
        private QueryFragment WhereContactIDFragment()
        {
            QueryFragment fragment = new QueryFragment
            {
                SQL = "WHERE contact_id = @contact_id"
            };
            fragment.Parameters.Add("contact_id", DbType.Int32);

            return fragment;
        }

        #endregion
    }
}