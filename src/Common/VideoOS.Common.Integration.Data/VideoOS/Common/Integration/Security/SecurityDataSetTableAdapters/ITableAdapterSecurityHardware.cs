namespace VideoOS.Common.Integration.Security.SecurityDataSetTableAdapters;

public interface ITableAdapterSecurityHardware : ITableAdapterSecurity
{
	void FillByHardware(SecurityDataSet ds, string idHardware);
}
