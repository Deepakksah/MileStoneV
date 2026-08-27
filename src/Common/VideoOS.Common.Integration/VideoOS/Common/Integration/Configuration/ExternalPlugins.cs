using System;
using System.Collections;

namespace VideoOS.Common.Integration.Configuration;

public class ExternalPlugins : DictionaryBase
{
	private ConfigurationDataSet.ExternalPluginsDataTable _table;

	private ConfigurationManager _configurationManager;

	public ExternalPlugin this[Guid PluginID] => (ExternalPlugin)base.Dictionary[PluginID];

	public ICollection Keys => base.Dictionary.Keys;

	public ICollection Values => base.Dictionary.Values;

	public ExternalPlugins(ConfigurationManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		_configurationManager = manager;
		_table = _configurationManager.Dataset.ExternalPlugins;
		_configurationManager.EnsureTableDataLoaded(_table);
		foreach (ConfigurationDataSet.ExternalPluginsRow row in _table.Rows)
		{
			Add(row);
		}
	}

	public ExternalPlugin Add(ConfigurationDataSet.ExternalPluginsRow row)
	{
		ExternalPlugin externalPlugin = new ExternalPlugin(_configurationManager, row);
		base.Dictionary.Add(externalPlugin.PluginID, externalPlugin);
		return externalPlugin;
	}

	public ExternalPlugin Add(Guid PluginID, string pluginType, string data)
	{
		ExternalPlugin externalPlugin = new ExternalPlugin(_configurationManager, _table.AddExternalPluginsRow(DateTime.UtcNow, PluginID.ToString(), pluginType, data));
		base.Dictionary.Add(externalPlugin.PluginID, externalPlugin);
		return externalPlugin;
	}

	public bool Contains(Guid PluginID)
	{
		return base.Dictionary.Contains(PluginID);
	}

	public void Remove(Guid PluginID)
	{
		if (this[PluginID] != null)
		{
			ConfigurationDataSet.ExternalPluginsRow dataRow = this[PluginID].DataRow;
			base.Dictionary.Remove(PluginID);
			dataRow.Delete();
		}
	}

	public new void Clear()
	{
		ArrayList arrayList = new ArrayList();
		foreach (ExternalPlugin value in Values)
		{
			arrayList.Add(value.PluginID);
		}
		foreach (Guid item in arrayList)
		{
			Remove(item);
		}
	}
}
