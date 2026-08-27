namespace VideoOS.Common.Integration.Security.SecurityDataSetTableAdapters;

public interface ITableAdapterSecurityRole : ITableAdapterSecurity
{
	void FillByRole(SecurityDataSet ds, string idRole);
}
