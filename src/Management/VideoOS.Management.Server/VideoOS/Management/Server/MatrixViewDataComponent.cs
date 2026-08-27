using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MatrixViewDataComponent : GeminiResourceComponent, IMatrixViewDataComponent, IDisposable
{
	private static class MatrixViewDataColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("IDMatrix");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Address = new DatabaseColumnBinder("Address");

		private static DatabaseColumnBinder Port = new DatabaseColumnBinder("Port");

		private static DatabaseColumnBinder Password = new DatabaseColumnBinder("Password");

		private static DatabaseColumnBinder LastUpdated = new DatabaseColumnBinder("LastUpdated");

		public static MatrixViewData Bind(DatabaseDataReader reader)
		{
			return new MatrixViewData
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				Address = Address.GetString(reader),
				Port = Port.GetInt32(reader),
				Password = Password.GetString(reader, allowNulls: true),
				LastModified = LastUpdated.GetDateTime(reader)
			};
		}
	}

	public IEnumerable<MatrixViewData> QueryMatrixViews()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_matrix_views");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MatrixViewData> list = new List<MatrixViewData>();
			while (databaseDataReader.Read())
			{
				list.Add(MatrixViewDataColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public MatrixViewData QueryMatrix(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_matrix_by_id");
			BindUniqueIdentifier("@IDMatrix", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MatrixViewData)null;
			}
			MatrixViewData result = MatrixViewDataColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void UpdateMatrix(MatrixViewData matrix)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_matrix");
			BindGuidType("@IDMatrix", matrix.Id);
			BindNameType("@name", matrix.Name);
			BindDescription("@description", matrix.Description);
			BindString("@address", matrix.Address, 256, allowNull: false, SqlDbType.Text);
			BindInt("@port", matrix.Port);
			BindPasswordType("@password", matrix.Password);
			ExecuteNonQuery();
		});
	}

	public void CreateMatrix(MatrixViewData matrix)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_matrix");
			BindGuidType("@IDMatrix", matrix.Id);
			BindNameType("@name", matrix.Name);
			BindDescription("@description", matrix.Description);
			BindString("@address", matrix.Address, 256, allowNull: false, SqlDbType.Text);
			BindInt("@port", matrix.Port);
			BindPasswordType("@password", matrix.Password);
			ExecuteNonQuery();
		});
	}

	public void DeleteMatrix(string id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_matrix");
			BindGuidType("@IDMatrix", id);
			ExecuteNonQuery();
		});
	}
}
