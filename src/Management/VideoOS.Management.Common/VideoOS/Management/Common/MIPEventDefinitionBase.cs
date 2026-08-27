using System;
using System.Xml;
using System.Xml.XPath;

namespace VideoOS.Management.Common;

public abstract class MIPEventDefinitionBase
{
	private readonly EventDefinition _eventDefinition;

	public string DisplayName
	{
		get
		{
			return _eventDefinition.DisplayName;
		}
		set
		{
			_eventDefinition.DisplayName = value;
		}
	}

	public Guid GeneratorId
	{
		get
		{
			if (!_eventDefinition.GeneratorID.IsGuid())
			{
				return Guid.Empty;
			}
			return new Guid(_eventDefinition.GeneratorID);
		}
		set
		{
			_eventDefinition.GeneratorID = value.ToString();
		}
	}

	public string GeneratorName
	{
		get
		{
			return _eventDefinition.GeneratorName;
		}
		set
		{
			_eventDefinition.GeneratorName = value;
		}
	}

	public string GeneratorGroupId
	{
		get
		{
			return _eventDefinition.GeneratorGroupId;
		}
		set
		{
			_eventDefinition.GeneratorGroupId = value;
		}
	}

	public string GeneratorGroupName
	{
		get
		{
			return _eventDefinition.GeneratorGroupName;
		}
		set
		{
			_eventDefinition.GeneratorGroupName = value;
		}
	}

	public Guid? StateGroupId
	{
		get
		{
			return _eventDefinition.StateGroupId;
		}
		set
		{
			_eventDefinition.StateGroupId = value;
		}
	}

	public string State
	{
		get
		{
			return _eventDefinition.State;
		}
		set
		{
			_eventDefinition.State = value;
		}
	}

	protected EventSource[] EventSources
	{
		get
		{
			return _eventDefinition.Sources;
		}
		set
		{
			_eventDefinition.Sources = value;
		}
	}

	protected MIPEventDefinitionBase(GeneratorType generatorType, EventDefinition definition)
	{
		_eventDefinition = definition ?? throw new ArgumentNullException("definition");
		if (!Enum.TryParse<GeneratorType>(_eventDefinition.GeneratorType, out var result) || result != generatorType)
		{
			throw new ArgumentException(FormattableString.Invariant($"Invalid event definition. GeneratorType must be {generatorType}."), "definition");
		}
	}

	protected MIPEventDefinitionBase(GeneratorType generatorType, Guid generatorId, string generatorName, string generatorGroupId, string generatorGroupName, Guid? stateGroupId, string state)
	{
		_eventDefinition = new EventDefinition(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, generatorType.ToString(), string.Empty, generatorId.ToString(), generatorName, generatorGroupId, generatorGroupName, stateGroupId, state, string.Empty, EventSource.FilterTypes.None, occursGlobally: true, builtIn: false, Guid.Empty);
	}

	public string SaveAsXml()
	{
		return _eventDefinition.SaveAsXml();
	}

	protected static EventDefinition ParseDefinitionXml(string definition)
	{
		try
		{
			return new EventDefinition(definition);
		}
		catch (XmlException innerException)
		{
			throw new ArgumentException("Invalid event definition", "definition", innerException);
		}
		catch (XPathException innerException2)
		{
			throw new ArgumentException("Invalid event definition", "definition", innerException2);
		}
	}
}
