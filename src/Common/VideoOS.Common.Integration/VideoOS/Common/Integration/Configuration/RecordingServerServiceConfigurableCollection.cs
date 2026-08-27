using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Configuration;

public class RecordingServerServiceConfigurableCollection : IEnumerable<RecordingServerServiceConfigurable>, IEnumerable
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<Guid, RecordingServerServiceConfigurable> dictionary;

	public RecordingServerServiceConfigurable this[Guid id]
	{
		get
		{
			if (dictionary.TryGetValue(id, out var value))
			{
				return value;
			}
			Refresh();
			if (dictionary.TryGetValue(id, out value))
			{
				return value;
			}
			return null;
		}
	}

	public RecordingServerServiceConfigurableCollection(ConfigurationManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		configurationManager = manager;
		dictionary = new RollbackDictionary<Guid, RecordingServerServiceConfigurable>(configurationManager, this);
		ConfigurationDataSet.RecordingServerServiceConfigurableDataTable recordingServerServiceConfigurable = configurationManager.Dataset.RecordingServerServiceConfigurable;
		configurationManager.EnsureTableDataLoaded(recordingServerServiceConfigurable);
		foreach (ConfigurationDataSet.RecordingServerServiceConfigurableRow item in recordingServerServiceConfigurable)
		{
			RecordingServerServiceConfigurable recordingServerServiceConfigurable2 = new RecordingServerServiceConfigurable(configurationManager, item);
			dictionary.Add(recordingServerServiceConfigurable2.ID, recordingServerServiceConfigurable2);
		}
	}

	public void Remove(Guid id)
	{
		RecordingServerServiceConfigurable recordingServerServiceConfigurable = this[id];
		if (recordingServerServiceConfigurable != null)
		{
			dictionary.Remove(id);
			recordingServerServiceConfigurable.DataRow.Delete();
		}
	}

	private void Refresh()
	{
		ConfigurationDataSet.RecordingServerServiceConfigurableDataTable recordingServerServiceConfigurable = configurationManager.Dataset.RecordingServerServiceConfigurable;
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetConfigurationDataSetTables(new string[1] { recordingServerServiceConfigurable.TableName });
		configurationManager.DisableEventHandling = true;
		recordingServerServiceConfigurable.BeginLoadData();
		foreach (ConfigurationDataSet.RecordingServerServiceConfigurableRow item in configurationDataSet.RecordingServerServiceConfigurable)
		{
			Guid guid = new Guid(item.PKID);
			if (dictionary.ContainsKey(guid))
			{
				((ConfigurationDataSet.RecordingServerServiceConfigurableRow)this[guid].DataRow).ItemArray = item.ItemArray;
				continue;
			}
			recordingServerServiceConfigurable.ImportRow(item);
			ConfigurationDataSet.RecordingServerServiceConfigurableRow row = recordingServerServiceConfigurable.FindByPKID(item.PKID);
			RecordingServerServiceConfigurable recordingServerServiceConfigurable2 = new RecordingServerServiceConfigurable(configurationManager, row);
			dictionary.Add(recordingServerServiceConfigurable2.ID, recordingServerServiceConfigurable2);
		}
		recordingServerServiceConfigurable.AcceptChanges();
		recordingServerServiceConfigurable.EndLoadData();
		configurationManager.DisableEventHandling = false;
	}

	public IEnumerator<RecordingServerServiceConfigurable> GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}
}
