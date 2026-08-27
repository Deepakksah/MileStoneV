namespace VideoOS.Common.Integration.Configuration;

public class HostingServerItems
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<int, HostingServer> dictionary;

	public HostingServerItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<int, HostingServer>(configurationManager, this);
		foreach (ConfigurationDataSet.HostingServerRow row in this.configurationManager.Dataset.HostingServer.Rows)
		{
			AddItem(row);
		}
	}

	public HostingServer GetHostingServerById(int id)
	{
		if (dictionary.TryGetValue(id, out var value))
		{
			return value;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(HostingServer), id);
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHostingServersByIds(new int[1] { id });
		if (configurationDataSet.HostingServer.Rows.Count == 0)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(HostingServer), id);
		}
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.HostingServer);
		ConfigurationDataSet.HostingServerRow hostingServerRow = (ConfigurationDataSet.HostingServerRow)configurationDataSet.HostingServer.Rows[0];
		configurationManager.Dataset.HostingServer.ImportRow(hostingServerRow);
		ConfigurationDataSet.HostingServerRow row = configurationManager.Dataset.HostingServer.FindBypkid(hostingServerRow.pkid);
		value = AddItem(row);
		configurationManager.EndLoadData(configurationManager.Dataset.HostingServer);
		configurationManager.DisableEventHandling = false;
		return value;
	}

	private HostingServer AddItem(ConfigurationDataSet.HostingServerRow row)
	{
		HostingServer hostingServer = new HostingServer(configurationManager, row);
		dictionary.Add(hostingServer.Id, hostingServer);
		return hostingServer;
	}
}
