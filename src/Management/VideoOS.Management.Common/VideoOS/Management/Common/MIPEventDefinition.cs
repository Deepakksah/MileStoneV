using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Management.Common;

public class MIPEventDefinition : MIPEventDefinitionBase
{
	public Collection<EventSource> Sources { get; private set; }

	public MIPEventDefinition(string definition)
		: this(MIPEventDefinitionBase.ParseDefinitionXml(definition))
	{
	}

	public MIPEventDefinition(EventDefinition definition)
		: base(GeneratorType.MIP, definition)
	{
		if (base.EventSources.Length == 0)
		{
			throw new ArgumentException("Invalid event definition. No sources have been specified.", "definition");
		}
		if (base.EventSources.Any((EventSource s) => !s.Type.IsGuid()))
		{
			throw new ArgumentException("Invalid event definition. Source type must be a GUID.", "definition");
		}
		if (base.EventSources.Any((EventSource s) => s.Filters.Any((EventSourceFilter f) => f.FilterType != EventSource.FilterTypes.MIP)))
		{
			throw new ArgumentException("Invalid event definition. If specified, source filter type must be MIP.", "definition");
		}
		if (base.EventSources.Any((EventSource s) => s.Filters.Any((EventSourceFilter f) => !f.Value.IsGuid())))
		{
			throw new ArgumentException("Invalid event definition. Source filter value must be a GUID.", "definition");
		}
		SetSourceCollection(base.EventSources);
	}

	public MIPEventDefinition(Guid generatorId, string generatorName, params EventSource[] sources)
		: this(generatorId, generatorName, string.Empty, string.Empty, null, string.Empty, sources)
	{
	}

	public MIPEventDefinition(Guid generatorId, string generatorName, string generatorGroupId, string generatorGroupName, Guid? stateGroupId, string state, params EventSource[] sources)
		: base(GeneratorType.MIP, generatorId, generatorName, generatorGroupId, generatorGroupName, stateGroupId, state)
	{
		SetSourceCollection(sources);
	}

	private void SetSourceCollection(IEnumerable<EventSource> sources)
	{
		ObservableCollection<EventSource> observableCollection = (ObservableCollection<EventSource>)(Sources = new ObservableCollection<EventSource>(sources));
		observableCollection.CollectionChanged += delegate
		{
			base.EventSources = Sources.ToArray();
		};
		base.EventSources = Sources.ToArray();
	}
}
