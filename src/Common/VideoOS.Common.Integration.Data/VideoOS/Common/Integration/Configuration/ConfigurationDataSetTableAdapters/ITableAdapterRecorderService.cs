namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public interface ITableAdapterRecorderService : ITableAdapter
{
	void FillByRecorderService(ConfigurationDataSet ds, string idRecorderService);
}
