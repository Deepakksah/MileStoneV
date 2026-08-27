using Microsoft.Data.SqlClient;

namespace VideoOS.Common.Integration.Security.SecurityDataSetTableAdapters;

public interface ITableAdapterSecurity
{
	SqlTransaction SqlTransaction { get; set; }

	SqlConnection SqlConnection { get; set; }

	void Fill(SecurityDataSet ds);

	void Update(SecurityDataSet ds, SqlTransaction transaction);
}
