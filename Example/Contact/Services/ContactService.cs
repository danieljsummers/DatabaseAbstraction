namespace DatabaseAbstraction.Contact.Services {

    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using DatabaseAbstraction.Contact.Models;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;

    /// <summary>
    /// This service manipulates the common contact information objects
    /// </summary>
    public sealed class ContactService {

        /// <summary>
        /// The DatabaseAbstraction instance to use
        /// </summary>
        private IDatabaseService Data { get; set; }

        /// <summary>
        /// Constructor for this service.
        /// </summary>
        /// <param name="data">
        /// An instance of a Database Service that implements
        /// <see cref="com.codeplex.dbabstraction.Interfaces.IDbService"/>
        /// </param>
        public ContactService(IDatabaseService data) {
            Data = data;
        }

        /// <summary>
        /// Insert an entire set of contact information
        /// </summary>
        /// <param name="contact">
        /// The <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set to insert.
        /// </param>
        public void InsertContact(ContactInformation contact) {

            // Insert the contact record.
            Data.Insert("contact.insert", contact);
            contact.ID = Data.Sequence("contact_contact_id");

            // Insert the addresses.
            foreach (Address address in contact.Addresses) {
                InsertAddress(address, contact.ID);
            }

            // Insert the phone numbers.
            foreach (PhoneNumber phone in contact.PhoneNumbers) {
                InsertPhone(phone, contact.ID);
            }

            // Insert the e-mail address.
            foreach (EmailAddress email in contact.EmailAddresses) {
                InsertEmail(email, contact.ID);
            }
        }

        /// <summary>
        /// Insert a new address record
        /// </summary>
        /// <param name="address">
        /// The address to be inserted
        /// </param>
        /// <param name="contactID">
        /// The contact ID to which this address applies
        /// </param>
        private void InsertAddress(Address address, int contactID) {
            Data.Insert("contact.insert.address", address);
            address.ID = Data.Sequence("address_addr_id");
        }

        /// <summary>
        /// Insert a new phone number record
        /// </summary>
        /// <param name="phone">
        /// The phone number to be inserted
        /// </param>
        /// <param name="contactID">
        /// The contact ID to which this phone number applies
        /// </param>
        private void InsertPhone(PhoneNumber phone, int contactID) {
            Data.Insert("contact.insert.phone", phone);
            phone.ID = Data.Sequence("phone_phn_id");
        }

        /// <summary>
        /// Insert a new e-mail address record
        /// </summary>
        /// <param name="email">
        /// The e-mail address to be inserted
        /// </param>
        /// <param name="contactID">
        /// The contact ID to which this e-mail address applies
        /// </param>
        private void InsertEmail(EmailAddress email, int contactID) {
            Data.Insert("contact.insert.email", email);
            email.ID = Data.Sequence("email_em_id");
        }

        /// <summary>
        /// Get a contact information set
        /// </summary>
        /// <param name="contactID">
        /// The ID of the contact information
        /// </param>
        /// <returns>
        /// The filled <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set
        /// (or null if not found)
        /// </returns>
        public ContactInformation GetContact(int contactID) {

            Dictionary<string, object> parameters = DbUtils.SingleParameter("contact_id", contactID);

            ContactInformation contact;

            using (IDataReader data = Data.SelectOne("contact.get", parameters)) {
                if (!data.Read()) return null;
                contact = new ContactInformation(data);
            }

            GetAddress(contact, parameters);
            GetPhones(contact, parameters);
            GetEmails(contact, parameters);

            return contact;
        }

        /// <summary>
        /// Get the address for a contact information set
        /// </summary>
        /// <param name="contact">
        /// The <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set being retrieved
        /// </param>
        /// <param name="parameters">
        /// The contact ID in parameter form
        /// </param>
        private void GetAddress(ContactInformation contact, Dictionary<string, object> parameters) {

            using (IDataReader data = Data.Select("contact.get.address", parameters)) {
                while (data.Read()) {
                    contact.Addresses.Add(new Address(data));
                }
            }
        }

        /// <summary>
        /// Get the phone numbers for a contact information set
        /// </summary>
        /// <param name="contact">
        /// The <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set being retrieved
        /// </param>
        /// <param name="parameters">
        /// The contact ID in parameter form
        /// </param>
        private void GetPhones(ContactInformation contact, Dictionary<string, object> parameters) {

            using (IDataReader data = Data.Select("contact.get.phone", parameters)) {
                while (data.Read()) {
                    contact.PhoneNumbers.Add(new PhoneNumber(data));
                }
            }
        }

        /// <summary>
        /// Get the e-mail addresses for a contact information set
        /// </summary>
        /// <param name="contact">
        /// The <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set being retrieved
        /// </param>
        /// <param name="parameters">
        /// The contact ID in parameter form
        /// </param>
        private void GetEmails(ContactInformation contact, Dictionary<string, object> parameters) {

            using (IDataReader data = Data.Select("contact.get.email", parameters)) {
                while (data.Read()) {
                    contact.EmailAddresses.Add(new EmailAddress(data));
                }
            }
        }

        /// <summary>
        /// Update a contact information set
        /// </summary>
        /// <param name="contact">
        /// The <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set to update
        /// </param>
        public void UpdateContact(ContactInformation contact) {
            UpdateAddresses(contact);
            UpdatePhones(contact);
            UpdateEmails(contact);
        }

        /// <summary>
        /// Update the addresses for the contact information set
        /// </summary>
        /// <param name="contact">
        /// The <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set being updated
        /// </param>
        private void UpdateAddresses(ContactInformation contact) {

            StringBuilder addressIDs = new StringBuilder("0");

            foreach (Address address in contact.Addresses) {
                if (0 == address.ID) {
                    InsertAddress(address, contact.ID);
                }
                else {
                    Data.Update("contact.update.address", address);
                }
                addressIDs.Append(",");
                addressIDs.Append(address.ID);
            }

            // Delete addresses that were removed.
            Data.Delete("contact.update.address.delete_old",
                DbUtils.SingleParameter("[]address_id", addressIDs.ToString()));
        }

        /// <summary>
        /// Update the phone numbers for the contact information set
        /// </summary>
        /// <param name="contact">
        /// The <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set being updated
        /// </param>
        private void UpdatePhones(ContactInformation contact) {

            StringBuilder phoneIDs = new StringBuilder("0");

            foreach (PhoneNumber phone in contact.PhoneNumbers) {
                if (0 == phone.ID) {
                    InsertPhone(phone, contact.ID);
                }
                else {
                    Data.Update("contact.update.phone", phone);
                }
                phoneIDs.Append(",");
                phoneIDs.Append(phone.ID);
            }

            // Delete phone numbers that were removed.
            Data.Delete("contact.update.phone.delete_old",
                DbUtils.SingleParameter("[]phone_id", phoneIDs.ToString()));
        }

        /// <summary>
        /// Update the e-mail addresses for the contact information set
        /// </summary>
        /// <param name="contact">
        /// The <see cref="com.codeplex.dbabstraction.Contact.Models.ContactInformation"/> set being updated
        /// </param>
        private void UpdateEmails(ContactInformation contact) {

            StringBuilder emailIDs = new StringBuilder("0");

            foreach (EmailAddress email in contact.EmailAddresses) {
                if (0 == email.ID) {
                    InsertEmail(email, contact.ID);
                }
                else {
                    Data.Update("contact.update.email", email);
                }
                emailIDs.Append(",");
                emailIDs.Append(email.ID);
            }

            // Delete e-mail addresses that were removed.
            Data.Delete("contact.update.email.delete_old",
                DbUtils.SingleParameter("[]email_id", emailIDs.ToString()));
        }

        /// <summary>
        /// Delele an entire contact information set
        /// </summary>
        /// <param name="contactID">
        /// The ID of the contact to be deleted
        /// </param>
        public void DeleteContact(int contactID) {

            Dictionary<string, object> parameters = DbUtils.SingleParameter("contact_id", contactID);

            Data.Delete("contact.delete.address", parameters);
            Data.Delete("contact.delete.phone", parameters);
            Data.Delete("contact.delete.email", parameters);
            Data.Delete("contact.delete", parameters);
        }

        /// <summary>
        /// Retrieve a list of states
        /// </summary>
        /// <returns>
        /// A list of key/value pairs of states
        /// </returns>
        public List<KeyValuePair<int, string>> GetStateList() {

            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

            // FIXME: hard-coded USA
            using (IDataReader data = Data.Select("contact.list.states", DbUtils.SingleParameter("country_id", 1))) {
                while (data.Read()) {
                    list.Add(new KeyValuePair<int, string>(data.GetInt32(data.GetOrdinal("id")),
                            data.GetString(data.GetOrdinal("description"))));
                }
            }

            return list;
        }

        /// <summary>
        /// Retrieve a list of contact types for phones
        /// </summary>
        /// <returns>
        /// A list of key/value pairs of phone contact types
        /// </returns>
        public List<KeyValuePair<int, string>> GetPhoneContactTypeList() {

            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

            using (IDataReader data = Data.Select("contact.list.contact_types.phone")) {
                while (data.Read()) {
                    list.Add(new KeyValuePair<int, string>(data.GetInt32(data.GetOrdinal("id")),
                            data.GetString(data.GetOrdinal("description"))));
                }
            }

            return list;
        }

        /// <summary>
        /// Retrieve a list of contact types for e-mail addresses
        /// </summary>
        /// <returns>
        /// A list of key/value pairs of e-mail addresses
        /// </returns>
        public List<KeyValuePair<int, string>> GetEmailContactTypeList() {

            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

            using (IDataReader data = Data.Select("contact.list.contact_types.email")) {
                while (data.Read()) {
                    list.Add(new KeyValuePair<int, string>(data.GetInt32(data.GetOrdinal("id")),
                            data.GetString(data.GetOrdinal("description"))));
                }
            }

            return list;
        }
    }
}