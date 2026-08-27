using System;
using System.Collections;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class RecorderPlugins : DictionaryBase
{
	private ConfigurationManager _manager;

	private ConfigurationDataSet.RecorderPluginsDataTable _table;

	public RecorderPlugin this[Guid recorderPluginId] => (RecorderPlugin)base.Dictionary[recorderPluginId];

	public ICollection Keys => base.Dictionary.Keys;

	public ICollection Values => base.Dictionary.Values;

	public RecorderPlugins(ConfigurationManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		_manager = manager;
		_table = _manager.Dataset.RecorderPlugins;
		_manager.EnsureTableDataLoaded(_table);
		foreach (ConfigurationDataSet.RecorderPluginsRow row in _table.Rows)
		{
			Add(row);
		}
	}

	public RecorderPlugin Add(ConfigurationDataSet.RecorderPluginsRow row)
	{
		RecorderPlugin recorderPlugin = new RecorderPlugin(_manager, row);
		base.Dictionary.Add(recorderPlugin.ID, recorderPlugin);
		return recorderPlugin;
	}

	public RecorderPlugin Add(Guid recorderPluginId, string name, string description)
	{
		RecorderPlugin recorderPlugin = new RecorderPlugin(_manager, _table.AddRecorderPluginsRow(DateTime.UtcNow, recorderPluginId.ToString(), name, description));
		base.Dictionary.Add(recorderPlugin.ID, recorderPlugin);
		return recorderPlugin;
	}

	public bool Contains(Guid recorderPluginId)
	{
		return base.Dictionary.Contains(recorderPluginId);
	}

	public void Remove(Guid recorderPluginId)
	{
		if (this[recorderPluginId] != null)
		{
			DataRow dataRow = this[recorderPluginId].DataRow;
			base.Dictionary.Remove(recorderPluginId);
			dataRow.Delete();
		}
	}

	public new void Clear()
	{
		ArrayList arrayList = new ArrayList();
		foreach (RecorderPlugin value in Values)
		{
			arrayList.Add(value.ID);
		}
		foreach (Guid item in arrayList)
		{
			Remove(item);
		}
	}
}
