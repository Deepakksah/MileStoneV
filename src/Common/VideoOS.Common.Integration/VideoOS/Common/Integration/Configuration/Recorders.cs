using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class Recorders : CollectionBase
{
	public new Recorder this[Guid id] => base[id] as Recorder;

	public Recorders(ConfigurationManager manager)
		: base(manager, manager.Dataset.Recorders)
	{
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new Recorder(_configurationManager, row as ConfigurationDataSet.RecordersRow);
	}

	public override void Remove(Guid id)
	{
		Recorder recorder = this[id];
		recorder.Hardware.Clear();
		_configurationManager.DeviceDataDistributionEntries.RemoveByRecorder(recorder);
		_configurationManager.RecordingServerServices.Remove(recorder.MasterServiceId);
		_configurationManager.RecordingServerServiceConfigurable.Remove(recorder.MasterServiceId);
		base.Remove(id);
	}

	public void Refresh()
	{
		using ConfigurationDataSet configurationDataSet = _configurationManager.dataLoader.GetConfigurationDataSetTables(new string[1] { _table.TableName });
		_configurationManager.DisableEventHandling = true;
		foreach (ConfigurationDataSet.RecordersRow row in configurationDataSet.Recorders.Rows)
		{
			if (Guid.TryParse(row.IDRecorder, out var result) && Contains(result))
			{
				ConfigurationDataSet.RecordersRow obj = (ConfigurationDataSet.RecordersRow)this[result].DataRow;
				obj.Table.BeginLoadData();
				obj.ItemArray = row.ItemArray;
				obj.AcceptChanges();
				obj.Table.EndLoadData();
			}
		}
		_configurationManager.DisableEventHandling = false;
	}
}
