namespace DatabaseAbstraction.Contact.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// This contains queries necessary for the contact information models.
    /// It uses the "contact." query name space.
    /// </summary>
    public sealed class ContactQueryLibrary : IQueryLibrary
    {
        private static string PREFIX = "contact.";

        /// <summary>
        /// Get the queries for this library.
        /// </summary>
        /// <returns>
        /// Queries needed for the common contact information module.
        /// </returns>
        public void GetQueries(Dictionary<string, DatabaseQuery> queries)
        {
            // Insert.
            addInsert(queries);
            addInsertAddress(queries);
            addInsertPhone(queries);
            addInsertEmail(queries);

            // Select.
            addGet(queries);
            addGetAddress(queries);
            addGetPhone(queries);
            addGetEmail(queries);

            // Update.
            addUpdateAddress(queries);
            addUpdateAddressDeleteOld(queries);
            addUpdatePhone(queries);
            addUpdatePhoneDeleteOld(queries);
            addUpdateEmail(queries);
            addUpdateEmailDeleteOld(queries);

            // Delete.
            addDelete(queries);
            addDeleteAddress(queries);
            addDeletePhone(queries);
            addDeleteEmail(queries);

            // Lists.
            addListStates(queries);
            addListContactTypesPhone(queries);
            addListContactTypesEmail(queries);
        }

        /// <summary>
        /// contact.insert
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addInsert(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "insert";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"INSERT INTO contact VALUES (nextval('contact_contact_id_seq'::regclass))";
        }

        /// <summary>
        /// contact.insert.address
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addInsertAddress(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "insert.address";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"INSERT INTO address
                    (contact_id, state_id, address, city, zip_code, physical_flag, mailing_flag)
                VALUES
                    (@contact_id, @state_id, @address, @city, @zip_code, @physical_flag, @mailing_flag)";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
            queries[name].Parameters.Add("state_id", DbType.Int32);
            queries[name].Parameters.Add("address", DbType.String);
            queries[name].Parameters.Add("city", DbType.String);
            queries[name].Parameters.Add("zip_code", DbType.String);
            queries[name].Parameters.Add("physical_flag", DbType.Boolean);
            queries[name].Parameters.Add("mailing_flag", DbType.Boolean);
        }

        /// <summary>
        /// contact.insert.phone
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addInsertPhone(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "insert.phone";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"INSERT INTO phone 
                    (contact_id, contact_type_id, area_code, exchange, number, extension, comments)
                VALUES
                    (@contact_id, @contact_type_id, @area_code, @exchange, @number, NULLIF(@extension, ''),
                    NULLIF(@comments, ''))";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
            queries[name].Parameters.Add("contact_type_id", DbType.Int32);
            queries[name].Parameters.Add("area_code", DbType.String);
            queries[name].Parameters.Add("exchange", DbType.String);
            queries[name].Parameters.Add("number", DbType.String);
            queries[name].Parameters.Add("extension", DbType.String);
            queries[name].Parameters.Add("comments", DbType.String);
        }

        /// <summary>
        /// contact.insert.email
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addInsertEmail(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "insert.email";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"INSERT INTO email
                    (contact_id, contact_type_id, address, comments)
                VALUES
                    (@contact_id, @contact_type_id, @address, NULLIF(@comments, ''))";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
            queries[name].Parameters.Add("contact_type_id", DbType.Int32);
            queries[name].Parameters.Add("address", DbType.String);
            queries[name].Parameters.Add("comments", DbType.String);
        }

        /// <summary>
        /// contact.get
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addGet(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"SELECT contact_id AS id
                FROM contact
                WHERE contact_id = @contact_id";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
        }

        /// <summary>
        /// contact.get.address
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addGetAddress(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.address";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"SELECT
                    address_id, contact_id, state_id, address, city, zip_code, physical_flag, mailing_flag,
                    code        AS state_code,
                    description AS state
                FROM address
                    INNER JOIN r_state ON state_id = r_state_id
                WHERE contact_id = @contact_id";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
        }

        /// <summary>
        /// contact.get.phone
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addGetPhone(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.phone";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"SELECT
                    phone_id, contact_id, contact_type_id, area_code, exchange, number, extension, comments,
                    description AS contact_type
                FROM phone
                    INNER JOIN r_contact_type ON contact_type_id = r_contact_type_id
                WHERE contact_id = @contact_id";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
        }

        /// <summary>
        /// contact.get.email
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addGetEmail(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.email";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"SELECT
                    email_id, contact_id, contact_type_id, address, comments,
                    description AS contact_type
                FROM email
                    INNER JOIN r_contact_type ON contact_type_id = r_contact_type_id
                WHERE contact_id = @contact_id";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
        }

        /// <summary>
        /// contact.update.address
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addUpdateAddress(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.address";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"UPDATE address
                SET state_id      = @state_id,
                    address       = @address,
                    city          = @city,
                    zip_code      = @zip_code,
                    physical_flag = @physical_flag,
                    mailing_flag  = @mailing_flag
                WHERE address_id = @address_id";

            queries[name].Parameters.Add("state_id", DbType.Int32);
            queries[name].Parameters.Add("address", DbType.String);
            queries[name].Parameters.Add("city", DbType.String);
            queries[name].Parameters.Add("zip_code", DbType.String);
            queries[name].Parameters.Add("physical_flag", DbType.Boolean);
            queries[name].Parameters.Add("mailing_flag", DbType.Boolean);
            queries[name].Parameters.Add("address_id", DbType.Int32);
        }

        /// <summary>
        /// contact.update.address.delete_old
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addUpdateAddressDeleteOld(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.address.delete_old";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"DELETE FROM address
                WHERE   contact_id = @contact_id
                    AND address_id NOT IN ([]address_id)";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
            queries[name].Parameters.Add("[]address_id", DbType.String);
        }

        /// <summary>
        /// contact.update.phone
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addUpdatePhone(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.phone";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"UPDATE phone
                SET contact_type_id = @contact_type_id,
                    area_code       = @area_code,
                    exchange        = @exchange,
                    number          = @number,
                    extension       = NULLIF(@extension, ''),
                    comments        = NULLIF(@comments, '')
                WHERE phone_id = @phone_id";

            queries[name].Parameters.Add("contact_type_id", DbType.Int32);
            queries[name].Parameters.Add("area_code", DbType.String);
            queries[name].Parameters.Add("exchange", DbType.String);
            queries[name].Parameters.Add("number", DbType.String);
            queries[name].Parameters.Add("extension", DbType.String);
            queries[name].Parameters.Add("comments", DbType.String);
            queries[name].Parameters.Add("phone_id", DbType.Int32);
        }

        /// <summary>
        /// contact.update.phone.delete_old
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addUpdatePhoneDeleteOld(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.phone.delete_old";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"DELETE FROM phone
                WHERE   contact_id = @contact_id
                    AND phone_id NOT IN ([]phone_id)";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
            queries[name].Parameters.Add("[]phone_id", DbType.String);
        }

        /// <summary>
        /// contact.update.email
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addUpdateEmail(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.email";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"UPDATE email
                SET contact_type_id = @contact_type_id,
                    address         = @address,
                    comments        = NULLIF(@comments, '')
                WHERE email_id = @email_id";

            queries[name].Parameters.Add("contact_type_id", DbType.Int32);
            queries[name].Parameters.Add("address", DbType.String);
            queries[name].Parameters.Add("comments", DbType.String);
        }

        /// <summary>
        /// contact.update.email.delete_old
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addUpdateEmailDeleteOld(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.email.delete_old";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"DELETE FROM email
                WHERE   contact_id = @contact_id
                    AND email_id NOT IN ([]email_id)";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
            queries[name].Parameters.Add("[]email_id", DbType.String);
        }

        /// <summary>
        /// contact.delete
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addDelete(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "delete";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"DELETE FROM contact
                WHERE contact_id = @contact_id";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
        }

        /// <summary>
        /// contact.delete.address
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addDeleteAddress(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "delete.address";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"DELETE FROM address
                WHERE contact_id = @contact_id";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
        }

        /// <summary>
        /// contact.delete.phone
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addDeletePhone(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "delete.phone";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"DELETE FROM phone
                WHERE contact_id = @contact_id";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
        }

        /// <summary>
        /// contact.delete.email
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addDeleteEmail(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "delete.email";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"DELETE FROM email
                WHERE contact_id = @contact_id";

            queries[name].Parameters.Add("contact_id", DbType.Int32);
        }

        /// <summary>
        /// contact.list.states
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addListStates(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "list.states";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"SELECT
                    r_state_id                        AS id,
                    description || '(' || code || ')' AS description
                FROM r_state
                WHERE country_id = @country_id
                ORDER BY description";

            queries[name].Parameters.Add("country_id", DbType.Int32);
        }

        /// <summary>
        /// contact.list.contact_types.phone
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addListContactTypesPhone(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "list.contact_types.phone";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"SELECT r_contact_type_id AS id, description
                FROM r_contact_type
                WHERE phone_flag = TRUE
                ORDER BY sort_order";
        }

        /// <summary>
        /// contact.list.contact_types.email
        /// </summary>
        /// <param name="queries">
        /// The query library being built.
        /// </param>
        private void addListContactTypesEmail(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "list.contact_types.email";

            queries.Add(name, new DatabaseQuery());

            queries[name].SQL =
                @"SELECT r_contact_type_id AS id, description
                FROM r_contact_type
                WHERE email_flag = TRUE
                ORDER BY sort_order";
        }
    }
}