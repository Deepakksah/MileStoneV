using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public sealed class ACServer : ACOperationableInstance
{
	public ACServer(string id, string name, string iconKey, IEnumerable<ACProperty> properties, string instanceTypeId)
		: this(id, name, iconKey, properties, instanceTypeId, null)
	{
	}

	public ACServer(string id, string name, string iconKey, IEnumerable<ACProperty> properties, string instanceTypeId, IEnumerable<string> unsupportedCommandTypeIds)
		: base(id, name, iconKey, properties, instanceTypeId, unsupportedCommandTypeIds)
	{
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACServer))
		{
			return false;
		}
		return base.Equals(obj);
	}
}
