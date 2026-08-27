using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public abstract class ACOperationableType : ACType
{
	[DataMember]
	public IEnumerable<string> StateTypeIds { get; private set; }

	[DataMember]
	public IEnumerable<string> CommandTypeIds { get; private set; }

	[DataMember]
	public IEnumerable<string> EventTypeIds { get; private set; }

	public ACOperationableType(string id, string name, string iconKey, IEnumerable<ACProperty> properties, IEnumerable<string> categories, IEnumerable<string> stateTypeIds, IEnumerable<string> commandTypeIds, IEnumerable<string> eventTypeIds)
		: base(id, name, iconKey, properties, categories)
	{
		StateTypeIds = new ReadOnlyCollection<string>((stateTypeIds != null) ? stateTypeIds.ToList() : new List<string>());
		CommandTypeIds = new ReadOnlyCollection<string>((commandTypeIds != null) ? commandTypeIds.ToList() : new List<string>());
		EventTypeIds = new ReadOnlyCollection<string>((eventTypeIds != null) ? eventTypeIds.ToList() : new List<string>());
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
		if (StateTypeIds == null)
		{
			StateTypeIds = new ReadOnlyCollection<string>(new List<string>());
		}
		if (CommandTypeIds == null)
		{
			CommandTypeIds = new ReadOnlyCollection<string>(new List<string>());
		}
		if (EventTypeIds == null)
		{
			EventTypeIds = new ReadOnlyCollection<string>(new List<string>());
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACOperationableType aCOperationableType))
		{
			return false;
		}
		if (!ACElement.CompareCollections(StateTypeIds, aCOperationableType.StateTypeIds))
		{
			return false;
		}
		if (!ACElement.CompareCollections(CommandTypeIds, aCOperationableType.CommandTypeIds))
		{
			return false;
		}
		if (!ACElement.CompareCollections(EventTypeIds, aCOperationableType.EventTypeIds))
		{
			return false;
		}
		return base.Equals(obj);
	}
}
