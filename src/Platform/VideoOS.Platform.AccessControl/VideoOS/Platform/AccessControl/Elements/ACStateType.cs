using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public sealed class ACStateType : ACType
{
	public ACStateType(string id, string name, string iconKey, IEnumerable<ACProperty> properties, IEnumerable<string> categories)
		: base(id, name, iconKey, properties, categories)
	{
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACStateType))
		{
			return false;
		}
		return base.Equals(obj);
	}
}
