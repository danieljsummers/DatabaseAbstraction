using System;
using System.Collections.Generic;

namespace com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces {
	/// <summary>
	/// This defines methods required for models to be used as parameters for an
	/// <see cref="com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces.IDatabaseModel"/> implementation.
	/// </summary>
	public interface IDatabaseModel {
		
		/// <summary>
		/// The properties of the object as a string/object key/value pair
		/// </summary>
		/// <returns>
		/// The properties as a string/object dictionary.
		/// </returns>
		Dictionary<string, object> DataParameters();
	}
}