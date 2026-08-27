using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public abstract class ACOperationableInstance : ACInstance
{
	[DataMember]
	public IEnumerable<string> UnsupportedCommandTypeIds { get; set; }

	[DataMember]
	public bool IsEnabled { get; set; } = true;

	public ACOperationableInstance(string id, string name, string iconKey, IEnumerable<ACProperty> properties, string instanceTypeId, IEnumerable<string> unsupportedCommandTypeIds)
		: base(id, name, iconKey, properties, instanceTypeId)
	{
		UnsupportedCommandTypeIds = new ReadOnlyCollection<string>((unsupportedCommandTypeIds != null) ? unsupportedCommandTypeIds.ToList() : new List<string>());
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
		if (UnsupportedCommandTypeIds == null)
		{
			UnsupportedCommandTypeIds = new ReadOnlyCollection<string>(new List<string>());
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACOperationableInstance aCOperationableInstance))
		{
			return false;
		}
		if (!ACElement.CompareCollections(UnsupportedCommandTypeIds, aCOperationableInstance.UnsupportedCommandTypeIds))
		{
			return false;
		}
		return base.Equals(obj);
	}
}
