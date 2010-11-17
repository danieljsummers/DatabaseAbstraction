using System;
using System.Collections.Generic;
using System.Data;
using com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces;
using com.codeplex.dbabstraction.DatabaseAbstraction.Models;

namespace com.codeplex.dbabstraction.DatabaseAbstraction.Queries {
	/// <summary>
	/// This contains queries to support the database.
	/// It uses the "database" query namespace.
	/// </summary>
	public sealed class DatabaseQueryLibrary : IQueryLibrary {
		
		private static string PREFIX = "database.";
		
		#region Main
		
		public void GetQueries(Dictionary<string, DatabaseQuery> queries) {
			
			// Select.
			addSequence(queries);
		}
		
		#endregion
		
		#region Select
		
		/// <summary>
		/// database.sequence
		/// </summary>
		/// <param name="queries">
		/// The query library being built
		/// </param>
		private void addSequence(Dictionary<string, DatabaseQuery> queries) {
			
			string name = PREFIX + "sequence";
			
			queries.Add(name, new DatabaseQuery());
			queries[name].SQL = "SELECT currval('[]sequence_name_seq') AS sequence_value";
			
			queries[name].Parameters.Add("[]sequence_name", DbType.String);
		}
		
		#endregion
	}
}