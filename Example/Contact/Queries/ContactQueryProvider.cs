namespace DatabaseAbstraction.Contact.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// This contains queries necessary for the contact information models.
    /// It uses the "contact." query name space by default.
    /// </summary>
    public sealed class ContactQueryProvider : IDatabaseQueryProvider
    {
        /// <summary>
        /// The prefix to use for queries in this library
        /// </summary>
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }
        private string _prefix = "contact.";

        /// <summary>
        /// Get the queries for this library
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        public void Queries(IDictionary<string, DatabaseQuery> queries)
        {
            // Select
            queries.Add(Prefix + "get", Get());
            queries.Add(Prefix + "get.address", GetAddress());
            queries.Add(Prefix + "get.phone", GetPhone());
            queries.Add(Prefix + "get.email", GetEmail());

            // Insert
            queries.Add(Prefix + "insert", Insert());
            queries.Add(Prefix + "insert.address", InsertAddress());
            queries.Add(Prefix + "insert.phone", InsertPhone());
            queries.Add(Prefix + "insert.email", InsertEmail());

            // Update
            queries.Add(Prefix + "update.address", UpdateAddress());
            queries.Add(Prefix + "update.address.delete_old", UpdateAddressDeleteOld());
            queries.Add(Prefix + "update.phone", UpdatePhone());
            queries.Add(Prefix + "update.phone.delete_old", UpdatePhoneDeleteOld());
            queries.Add(Prefix + "update.email", UpdateEmail());
            queries.Add(Prefix + "update.email.delete_old", UpdateEmailDeleteOld());

            // Delete
            queries.Add(Prefix + "delete", Delete());
            queries.Add(Prefix + "delete.address", DeleteAddress());
            queries.Add(Prefix + "delete.phone", DeletePhone());
            queries.Add(Prefix + "delete.email", DeleteEmail());

            // Lists
            queries.Add(Prefix + "list.states", ListStates());
        }

        #region Select

        /// <summary>
        /// contact.get
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery Get()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT contact_id AS id
                    FROM contact"
            };
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");

            return query;
        }

        /// <summary>
        /// contact.get.address
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery GetAddress()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT
                    address_id, contact_id, state_id, address, city, zip_code, physical_flag, mailing_flag,
                    code        AS state_code,
                    description AS state
                FROM address
                    INNER JOIN r_state ON state_id = r_state_id"
            };
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");

            return query;
        }

        /// <summary>
        /// contact.get.phone
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery GetPhone()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT
                    phone_id, contact_id, contact_type_id, area_code, exchange, number, extension, comments,
                    description AS contact_type
                FROM phone"
            };
            query.Fragments.Add(QueryFragmentType.From, "contact.from.join_contact_type");
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");

            return query;
        }

        /// <summary>
        /// contact.get.email
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery GetEmail()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT
                    email_id, contact_id, contact_type_id, address, comments,
                    description AS contact_type
                FROM email"
            };
            query.Fragments.Add(QueryFragmentType.From, "contact.from.join_contact_type");
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");

            return query;
        }

        #endregion

        #region Insert

        /// <summary>
        /// contact.insert
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery Insert()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = "INSERT INTO contact VALUES (@contact_id)"
            };
            query.Parameters.Add("contact_id", DbType.Int32);

            return query;
        }

        /// <summary>
        /// contact.insert.address
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery InsertAddress()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"INSERT INTO address
                    (contact_id, state_id, address, city, zip_code, physical_flag, mailing_flag)
                VALUES
                    (@contact_id, @state_id, @address, @city, @zip_code, @physical_flag, @mailing_flag)"
            };
            query.Parameters.Add("contact_id", DbType.Int32);
            query.Parameters.Add("state_id", DbType.Int32);
            query.Parameters.Add("address", DbType.String);
            query.Parameters.Add("city", DbType.String);
            query.Parameters.Add("zip_code", DbType.String);
            query.Parameters.Add("physical_flag", DbType.Boolean);
            query.Parameters.Add("mailing_flag", DbType.Boolean);

            return query;
        }

        /// <summary>
        /// contact.insert.phone
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery InsertPhone()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"INSERT INTO phone 
                    (contact_id, contact_type_id, area_code, exchange, number, extension, comments)
                VALUES
                    (@contact_id, @contact_type_id, @area_code, @exchange, @number, NULLIF(@extension, ''),
                    NULLIF(@comments, ''))"
            };
            query.Parameters.Add("contact_id", DbType.Int32);
            query.Parameters.Add("contact_type_id", DbType.Int32);
            query.Parameters.Add("area_code", DbType.String);
            query.Parameters.Add("exchange", DbType.String);
            query.Parameters.Add("number", DbType.String);
            query.Parameters.Add("extension", DbType.String);
            query.Parameters.Add("comments", DbType.String);

            return query;
        }

        /// <summary>
        /// contact.insert.email
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery InsertEmail()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"INSERT INTO email
                    (contact_id, contact_type_id, address, comments)
                VALUES
                    (@contact_id, @contact_type_id, @address, NULLIF(@comments, ''))"
            };
            query.Parameters.Add("contact_id", DbType.Int32);
            query.Parameters.Add("contact_type_id", DbType.Int32);
            query.Parameters.Add("address", DbType.String);
            query.Parameters.Add("comments", DbType.String);

            return query;
        }

        #endregion

        #region Update

        /// <summary>
        /// contact.update.address
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery UpdateAddress()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"UPDATE address
                SET state_id      = @state_id,
                    address       = @address,
                    city          = @city,
                    zip_code      = @zip_code,
                    physical_flag = @physical_flag,
                    mailing_flag  = @mailing_flag
                WHERE address_id = @address_id"
            };
            query.Parameters.Add("state_id", DbType.Int32);
            query.Parameters.Add("address", DbType.String);
            query.Parameters.Add("city", DbType.String);
            query.Parameters.Add("zip_code", DbType.String);
            query.Parameters.Add("physical_flag", DbType.Boolean);
            query.Parameters.Add("mailing_flag", DbType.Boolean);
            query.Parameters.Add("address_id", DbType.Int32);

            return query;
        }

        /// <summary>
        /// contact.update.address.delete_old
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdateAddressDeleteOld()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM address"
            };

            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");
            query.AfterFragment.Add(QueryFragmentType.Where, "AND address_id NOT IN ([]address_id)");
            query.Parameters.Add("[]address_id", DbType.String);

            return query;
        }

        /// <summary>
        /// contact.update.phone
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery UpdatePhone()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"UPDATE phone
                SET contact_type_id = @contact_type_id,
                    area_code       = @area_code,
                    exchange        = @exchange,
                    number          = @number,
                    extension       = NULLIF(@extension, ''),
                    comments        = NULLIF(@comments, '')
                WHERE phone_id = @phone_id"
            };
            query.Parameters.Add("contact_type_id", DbType.Int32);
            query.Parameters.Add("area_code", DbType.String);
            query.Parameters.Add("exchange", DbType.String);
            query.Parameters.Add("number", DbType.String);
            query.Parameters.Add("extension", DbType.String);
            query.Parameters.Add("comments", DbType.String);
            query.Parameters.Add("phone_id", DbType.Int32);

            return query;
        }

        /// <summary>
        /// contact.update.phone.delete_old
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdatePhoneDeleteOld()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM phone"
            };
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");
            query.AfterFragment.Add(QueryFragmentType.Where, "AND phone_id NOT IN ([]phone_id)");
            query.Parameters.Add("[]phone_id", DbType.String);

            return query;
        }

        /// <summary>
        /// contact.update.email
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery UpdateEmail()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"UPDATE email
                SET contact_type_id = @contact_type_id,
                    address         = @address,
                    comments        = NULLIF(@comments, '')
                WHERE email_id = @email_id"
            };
            query.Parameters.Add("contact_type_id", DbType.Int32);
            query.Parameters.Add("address", DbType.String);
            query.Parameters.Add("comments", DbType.String);

            return query;
        }

        /// <summary>
        /// contact.update.email.delete_old
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdateEmailDeleteOld()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM email"
            };
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");
            query.AfterFragment.Add(QueryFragmentType.Where, "AND email_id NOT IN ([]email_id)");
            query.Parameters.Add("[]email_id", DbType.String);

            return query;
        }

        #endregion

        #region Delete

        /// <summary>
        /// contact.delete
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery Delete()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM contact"
            };
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");

            return query;
        }

        /// <summary>
        /// contact.delete.address
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery DeleteAddress()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM address"
            };
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");

            return query;
        }

        /// <summary>
        /// contact.delete.phone
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery DeletePhone()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM phone"
            };
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");

            return query;
        }

        /// <summary>
        /// contact.delete.email
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery DeleteEmail()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM email"
            };
            query.Fragments.Add(QueryFragmentType.Where, "contact.where.contact_id");

            return query;
        }

        #endregion

        #region Lists

        /// <summary>
        /// contact.list.states
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery ListStates()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"SELECT
                    r_state_id                         AS id,
                    description || ' (' || code || ')' AS description
                FROM r_state
                WHERE country_id = @country_id
                ORDER BY description"
            };
            query.Parameters.Add("country_id", DbType.Int32);

            return query;
        }

        #endregion
    }
}