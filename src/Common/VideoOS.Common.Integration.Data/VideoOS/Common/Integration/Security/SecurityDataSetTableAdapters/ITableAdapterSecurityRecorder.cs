namespace VideoOS.Common.Integration.Security.SecurityDataSetTableAdapters;

public interface ITableAdapterSecurityRecorder : ITableAdapterSecurity
{
	void FillByRecorder(SecurityDataSet ds, string idRecorder);
}
