namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public interface ITableAdapterHardware : ITableAdapter
{
	void FillByHardware(ConfigurationDataSet ds, string idHardware);
}
