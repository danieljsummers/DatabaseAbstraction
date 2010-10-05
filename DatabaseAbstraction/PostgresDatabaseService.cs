using System;
using System.Collections.Generic;
using System.Data;
using com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces;
using Npgsql;

namespace com.codeplex.dbabstraction.DatabaseAbstraction {
	/// <summary>
	/// A PostgreSQL implementation of a database service.
	/// </summary>
	public class PostgresDatabaseService : DatabaseService, IDatabaseService {

		/// <summary>
		/// Constructor for the PostgreSQL database service.
		/// </summary>
		/// <param name="classes">
		/// Classes that contain query libraries to use when initializing the service.
		/// </param>
		public PostgresDatabaseService(string connectionString, params IQueryLibrary[] classes)
			: base(classes) {
			
			// Connect to the database.
			Connection = new NpgsqlConnection(connectionString);
		}
		
		
		/// <summary>
		/// Get the next value in a database sequence 
		/// </summary>
		/// <param name="sequenceName">
		/// The name of the sequence
		/// </param>
		/// <returns>
		/// The value of the sequence
		/// </returns>
		/// FIXME: this is wrong
		public int Sequence(string sequenceName) {
			
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("[]sequence_name", sequenceName);
			
			using (IDataReader reader = SelectOne("database.sequence.postgres", parameters)) {
				if (reader.NextResult()) {
					reader.Read();
					return Convert.ToInt32(reader["sequence_value"]);
				}
				else {
					return 0;
				}
			}
		}
	}
}