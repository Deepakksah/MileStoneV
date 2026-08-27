using System.Data;
using Microsoft.Data.SqlClient;

namespace VideoOS.Common.Integration.Security.SecurityDataSetTableAdapters;

public static class UpdateHelper
{
	public static void UpdateDataAdapter(SqlDataAdapter sqlDataAdapter, DataSet value, SqlTransaction transaction)
	{
		sqlDataAdapter.UpdateCommand.Transaction = transaction;
		sqlDataAdapter.DeleteCommand.Transaction = transaction;
		sqlDataAdapter.InsertCommand.Transaction = transaction;
		sqlDataAdapter.Update(value);
		sqlDataAdapter.UpdateCommand.Transaction = null;
		sqlDataAdapter.DeleteCommand.Transaction = null;
		sqlDataAdapter.InsertCommand.Transaction = null;
	}
}
