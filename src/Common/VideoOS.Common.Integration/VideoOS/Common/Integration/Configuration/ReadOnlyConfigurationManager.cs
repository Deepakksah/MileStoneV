namespace VideoOS.Common.Integration.Configuration;

public class ReadOnlyConfigurationManager : ConfigurationManager
{
	public ReadOnlyConfigurationManager(IConfigurationDataSetLoader configurationDataSetLoader)
		: base(new ConfigurationDataSet(), configurationDataSetLoader)
	{
	}
}
