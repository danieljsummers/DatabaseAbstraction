using System;
using com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces;
using MySql.Data.MySqlClient;

namespace com.codeplex.dbabstraction.DatabaseAbstraction {
	/// <summary>
	/// A MySQL implementation of a database service.
	/// </summary>
	public class MySqlDatabaseService : DatabaseService, IDatabaseService {

		/// <summary>
		/// Constructor for the MySQL database service.
		/// </summary>
		/// <param name="classes">
		/// Classes that contain query libraries to use when initializing the service.
		/// </param>
		public MySqlDatabaseService(string connectionString, params IQueryLibrary[] classes)
			: base(classes) {
			
			// Connect to the database.
			Connection = new MySqlConnection(connectionString);
		}
		
		
		/// <summary>
		/// Get the next value in a database sequence 
		/// </summary>
		/// <param name="sequenceName">
		/// The name of the sequence
		/// </param>
		/// <returns>
		/// An exception; MySQL does not support sequences
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// MySQL does not support sequences
		/// </exception>
		public int Sequence(string sequenceName) {
			throw new InvalidOperationException("MySQL does not support sequences");
		}
	}
}