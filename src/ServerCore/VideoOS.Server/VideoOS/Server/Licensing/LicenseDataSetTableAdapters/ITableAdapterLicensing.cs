using Microsoft.Data.SqlClient;

namespace VideoOS.Server.Licensing.LicenseDataSetTableAdapters;

public interface ITableAdapterLicensing
{
	SqlConnection SqlConnection { get; set; }

	SqlTransaction SqlTransaction { get; set; }
}
