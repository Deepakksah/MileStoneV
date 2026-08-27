using System;
using System.Data;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Configuration;

public class EventType : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.EventTypesRow _datarow;

	public DataRow DataRow => _datarow;

	public Guid ID
	{
		get
		{
			return new Guid(_datarow.IDEventType);
		}
		set
		{
			_datarow.IDEventType = value.ToString();
		}
	}

	public EventDefinition Definition
	{
		get
		{
			return new EventDefinition(_datarow.Definition);
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_datarow.Definition = value.SaveAsXml();
		}
	}

	public string DefinitionXml
	{
		get
		{
			return _datarow.Definition;
		}
		set
		{
			_datarow.Definition = value;
		}
	}

	public string GeneratorType
	{
		get
		{
			return _datarow.GeneratorType;
		}
		set
		{
			_datarow.GeneratorType = value;
		}
	}

	public string GeneratorId
	{
		get
		{
			return _datarow.IDGenerator;
		}
		set
		{
			_datarow.IDGenerator = value;
		}
	}

	public string GeneratorSubtype
	{
		get
		{
			return _datarow.GeneratorSubType;
		}
		set
		{
			_datarow.GeneratorSubType = value;
		}
	}

	public string Name
	{
		get
		{
			return _datarow.Name;
		}
		set
		{
			_datarow.Name = value;
		}
	}

	public string Description => string.Empty;

	public bool BuiltIn
	{
		get
		{
			return _datarow.Builtin;
		}
		set
		{
			_datarow.Builtin = value;
		}
	}

	public string DisplaySortKey => Definition.DisplayName;

	public EventType(ConfigurationManager configurationManager, ConfigurationDataSet.EventTypesRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
