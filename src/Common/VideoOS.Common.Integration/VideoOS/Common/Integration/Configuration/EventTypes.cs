using System;
using System.Data;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Configuration;

public class EventTypes : CollectionBase
{
	public new EventType this[Guid id] => base[id] as EventType;

	public EventTypes(ConfigurationManager manager)
		: base(manager, manager.Dataset.EventTypes)
	{
	}

	public EventType GetByReferenceId(Guid referenceId)
	{
		foreach (object value in base.Values)
		{
			EventType obj = value as EventType;
			if (obj != null && obj.ID == referenceId)
			{
				return value as EventType;
			}
		}
		return null;
	}

	public EventType GetByName(string name)
	{
		foreach (object value in base.Values)
		{
			if ((value as EventType)?.Name == name)
			{
				return value as EventType;
			}
		}
		return null;
	}

	public EventType GetByDisplayName(string displayName)
	{
		foreach (object value in base.Values)
		{
			if ((value as EventType)?.Definition.DisplayName == displayName)
			{
				return value as EventType;
			}
		}
		return null;
	}

	public EventType Add(Guid idEventType, string name, string definition, string generatorType, string generatorSubtype, string idGenerator, bool builtIn)
	{
		return Add(_configurationManager.Dataset.EventTypes.AddEventTypesRow(DateTime.UtcNow, idEventType.ToString(), definition, generatorType, idGenerator, builtIn, name, generatorSubtype)) as EventType;
	}

	public EventType AddNewExternalEvent(Guid id, string name, string subtype)
	{
		EventDefinition eventDefinition = new EventDefinition(id, name, name, string.Empty, string.Empty, string.Empty, EventGeneratorType.External.ToString(), subtype, string.Empty, string.Empty, EventGeneratorType.External.ToString(), EventSource.FilterTypes.None, occursGlobally: true, builtIn: false, id);
		return Add(eventDefinition.ID, eventDefinition.Name, eventDefinition.SaveAsXml(), eventDefinition.GeneratorType, eventDefinition.GeneratorSubtype, eventDefinition.GeneratorID, builtIn: false);
	}

	public EventType AddNewUserDefinedEvent(Guid id, string name)
	{
		return AddNewExternalEvent(id, name, "UserDefined");
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new EventType(_configurationManager, row as ConfigurationDataSet.EventTypesRow);
	}

	public void Refresh()
	{
		using ConfigurationDataSet configurationDataSet = _configurationManager.dataLoader.GetConfigurationDataSetTables(new string[1] { _table.TableName });
		_configurationManager.DisableEventHandling = true;
		_table.BeginLoadData();
		foreach (ConfigurationDataSet.EventTypesRow row2 in configurationDataSet.EventTypes.Rows)
		{
			Guid id = new Guid(row2.IDEventType);
			if (Contains(id))
			{
				((ConfigurationDataSet.EventTypesRow)this[id].DataRow).ItemArray = row2.ItemArray;
				continue;
			}
			_table.ImportRow(row2);
			ConfigurationDataSet.EventTypesRow row = ((ConfigurationDataSet.EventTypesDataTable)_table).FindByIDEventType(row2.IDEventType);
			AddInternal(row);
		}
		_table.AcceptChanges();
		_table.EndLoadData();
		_configurationManager.DisableEventHandling = false;
	}
}
