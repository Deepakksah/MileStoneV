namespace VideoOS.Common.Integration.Configuration;

public class IPHardwareDefinitionItems
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<string, IPHardwareDefinition> dictionary;

	public IPHardwareDefinitionItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<string, IPHardwareDefinition>(configurationManager, this);
		foreach (ConfigurationDataSet.HardwareDefinitionRow row in this.configurationManager.Dataset.HardwareDefinition.Rows)
		{
			AddItem(row);
		}
	}

	public IPHardwareDefinition AddNew(string id, string definition)
	{
		ConfigurationDataSet.HardwareDefinitionRow row = configurationManager.Dataset.HardwareDefinition.AddHardwareDefinitionRow(id, definition);
		return AddItem(row);
	}

	public bool TryGetHardwareDefinitionByDigest(string id, out IPHardwareDefinition definition)
	{
		definition = null;
		if (dictionary.TryGetValue(id, out definition))
		{
			return true;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			return false;
		}
		using (ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHardwareDefinitionByDigest(id))
		{
			if (configurationDataSet.HardwareDefinition.Rows.Count == 0)
			{
				return false;
			}
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.HardwareDefinition);
			ConfigurationDataSet.HardwareDefinitionRow hardwareDefinitionRow = (ConfigurationDataSet.HardwareDefinitionRow)configurationDataSet.HardwareDefinition.Rows[0];
			configurationManager.Dataset.HardwareDefinition.ImportRow(hardwareDefinitionRow);
			ConfigurationDataSet.HardwareDefinitionRow row = configurationManager.Dataset.HardwareDefinition.FindByC18NDigest(hardwareDefinitionRow.C18NDigest);
			definition = AddItem(row);
			configurationManager.EndLoadData(configurationManager.Dataset.HardwareDefinition);
			configurationManager.DisableEventHandling = false;
		}
		return true;
	}

	public IPHardwareDefinition GetHardwareDefinitionByDigest(string id)
	{
		if (TryGetHardwareDefinitionByDigest(id, out var definition))
		{
			return definition;
		}
		throw ConfigurationItemNotFoundException.CreateWithId(typeof(IPHardwareDefinition), id);
	}

	public void Remove(string id)
	{
		IPHardwareDefinition hardwareDefinitionByDigest = GetHardwareDefinitionByDigest(id);
		if (hardwareDefinitionByDigest != null)
		{
			dictionary.Remove(id);
			hardwareDefinitionByDigest.DataRow.Delete();
		}
	}

	private IPHardwareDefinition AddItem(ConfigurationDataSet.HardwareDefinitionRow row)
	{
		IPHardwareDefinition iPHardwareDefinition = new IPHardwareDefinition(configurationManager, row);
		dictionary.Add(iPHardwareDefinition.C18NDigest, iPHardwareDefinition);
		return iPHardwareDefinition;
	}
}
