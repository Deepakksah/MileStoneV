using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public sealed class ACUnitType : ACOperationableType
{
	public ACUnitType(string id, string name, string iconKey, IEnumerable<ACProperty> properties, IEnumerable<string> categories, IEnumerable<string> stateTypeIds, IEnumerable<string> commandTypeIds, IEnumerable<string> eventTypeIds)
		: base(id, name, iconKey, properties, categories, stateTypeIds, commandTypeIds, eventTypeIds)
	{
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACUnitType))
		{
			return false;
		}
		return base.Equals(obj);
	}
}
