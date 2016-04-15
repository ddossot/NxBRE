namespace NxBRE.Util
{
	using System;
	using System.Configuration;
	using System.Data;

	/// <summary>NxBRE Data utilities.</summary>
	/// <remarks>This class provides very basic methods, it is a demonstrator of what is feasable.
	/// The user is invited to create his/her own data access utilities.
	/// </remarks>
	/// <author>David Dossot</author>
	public abstract class DataAccess {
		private DataAccess() {}
		
		/// <summary>
		/// Get a value from a DataRow column.
		/// </summary>
		/// <param name="dr">The DataRow to read from.</param>
		/// <param name="columnName">The name of the column to read.</param>
		/// <returns>The read object.</returns>
		/// <see cref="System.Data.DataRow"/>
		public static object GetDataRowColumnValue(DataRow dr, string columnName) {
			return dr[columnName];
		}	
		
		/// <summary>
		/// Set a value in DataRow column.
		/// </summary>
		/// <param name="dr">The DataRow to modify.</param>
		/// <param name="columnName">The name of the column to set.</param>
		/// <param name="columnValue">The new value for the column.</param>
		/// <see cref="System.Data.DataRow"/>
		public static void SetDataRowColumnValue(DataRow dr, string columnName, object columnValue) {
			dr[columnName] = columnValue;
		}	
		
		/// <summary>
		/// Get a value from a Array column.
		/// </summary>
		/// <param name="array">The Array to read from.</param>
		/// <param name="columnIndex">The name of the column to read.</param>
		/// <returns>The read object.</returns>
		public static object GetArrayColumnValue(object[] array, int columnIndex) {
			return array[columnIndex];
		}	
		
		/// <summary>
		/// Set a value in Array column.
		/// </summary>
		/// <param name="array">The Array to modify.</param>
		/// <param name="columnIndex">The name of the column to set.</param>
		/// <param name="columnValue">The new value for the column.</param>
		public static void SetArrayColumnValue(object[] array, int columnIndex, object columnValue) {
			array[columnIndex] = columnValue;
		}	
	}
}
