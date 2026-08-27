using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public abstract class ACInstance : ACElement
{
	[DataMember]
	public string InstanceTypeId { get; private set; }

	public ACInstance(string id, string name, string iconKey, IEnumerable<ACProperty> properties, string instanceTypeId)
		: base(id, name, iconKey, properties)
	{
		InstanceTypeId = instanceTypeId;
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACInstance aCInstance))
		{
			return false;
		}
		if (InstanceTypeId != aCInstance.InstanceTypeId)
		{
			return false;
		}
		return base.Equals(obj);
	}
}
