using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public sealed class ACCommandType : ACType
{
	[DataMember]
	public int Order { get; private set; }

	public ACCommandType(string id, string name, string iconKey, IEnumerable<ACProperty> properties, IEnumerable<string> categories, int order)
		: base(id, name, iconKey, properties, categories)
	{
		Order = order;
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACCommandType aCCommandType))
		{
			return false;
		}
		if (Order != aCCommandType.Order)
		{
			return false;
		}
		return base.Equals(obj);
	}
}
