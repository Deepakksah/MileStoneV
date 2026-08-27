using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Management.Common;

public class MIPDeviceEventDefinition : MIPEventDefinitionBase
{
	public Collection<DeviceType> SourceTypes { get; private set; }

	public MIPDeviceEventDefinition(string definition)
		: this(MIPEventDefinitionBase.ParseDefinitionXml(definition))
	{
	}

	public MIPDeviceEventDefinition(EventDefinition definition)
		: base(GeneratorType.MIPDevice, definition)
	{
		DeviceType[] array = base.EventSources.Select((EventSource s) => DeviceTypesExtensions.FromBuiltInDeviceType(s.Type)).ToArray();
		if (array.Length == 0)
		{
			throw new ArgumentException("Invalid event definition. No sources have been specified.", "definition");
		}
		if (array.Contains(DeviceType.Unknown))
		{
			throw new ArgumentException("Invalid event definition. Unknown device type", "definition");
		}
		SetSourceCollection(array);
	}

	public MIPDeviceEventDefinition(Guid generatorId, string generatorName, params DeviceType[] sourceTypes)
		: this(generatorId, generatorName, string.Empty, string.Empty, null, string.Empty, sourceTypes)
	{
	}

	public MIPDeviceEventDefinition(Guid generatorId, string generatorName, string generatorGroupId, string generatorGroupName, Guid? stateGroupId, string state, params DeviceType[] sourceTypes)
		: base(GeneratorType.MIPDevice, generatorId, generatorName, generatorGroupId, generatorGroupName, stateGroupId, state)
	{
		SetSourceCollection(sourceTypes);
	}

	private void SetSourceCollection(IEnumerable<DeviceType> sourceTypes)
	{
		ObservableCollection<DeviceType> observableCollection = (ObservableCollection<DeviceType>)(SourceTypes = new ObservableCollection<DeviceType>(sourceTypes));
		observableCollection.CollectionChanged += delegate
		{
			base.EventSources = SourceTypes.Select((DeviceType t) => new EventSource(t.ToBuiltInDeviceType(), EventSource.FilterTypes.None)).ToArray();
		};
		base.EventSources = SourceTypes.Select((DeviceType t) => new EventSource(t.ToBuiltInDeviceType(), EventSource.FilterTypes.None)).ToArray();
	}
}
