using Microsoft.Data.SqlClient;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public interface ITableAdapter
{
	SqlConnection Connection { get; set; }

	SqlTransaction SqlTransaction { get; set; }

	void Fill(ConfigurationDataSet ds);
}
