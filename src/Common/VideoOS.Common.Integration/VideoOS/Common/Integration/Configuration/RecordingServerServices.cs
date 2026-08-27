using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class RecordingServerServices : CollectionBase
{
	public new RecordingServerService this[Guid id]
	{
		get
		{
			if (!(base[id] is RecordingServerService))
			{
				Refresh();
			}
			return base[id] as RecordingServerService;
		}
	}

	public RecordingServerServices(ConfigurationManager manager)
		: base(manager, manager.Dataset.RecordingServerService)
	{
	}

	private void Refresh()
	{
		using ConfigurationDataSet configurationDataSet = _configurationManager.dataLoader.GetConfigurationDataSetTables(new string[1] { _table.TableName });
		_configurationManager.DisableEventHandling = true;
		_table.BeginLoadData();
		foreach (ConfigurationDataSet.RecordingServerServiceRow item in configurationDataSet.RecordingServerService)
		{
			Guid id = new Guid(item.pkid);
			if (Contains(id))
			{
				((ConfigurationDataSet.RecordingServerServiceRow)this[id].DataRow).ItemArray = item.ItemArray;
				continue;
			}
			_table.ImportRow(item);
			ConfigurationDataSet.RecordingServerServiceRow row = ((ConfigurationDataSet.RecordingServerServiceDataTable)_table).FindBypkid(item.pkid);
			AddInternal(row);
		}
		_table.AcceptChanges();
		_table.EndLoadData();
		_configurationManager.DisableEventHandling = false;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new RecordingServerService(_configurationManager, row as ConfigurationDataSet.RecordingServerServiceRow);
	}
}
