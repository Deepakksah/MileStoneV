namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public interface ITableAdapterRecorder : ITableAdapter
{
	void FillByRecorder(ConfigurationDataSet ds, string idRecorder);
}
