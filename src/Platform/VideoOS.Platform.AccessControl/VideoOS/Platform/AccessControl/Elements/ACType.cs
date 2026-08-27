using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public abstract class ACType : ACElement
{
	[DataMember]
	public IEnumerable<string> Categories { get; protected set; }

	public ACType(string id, string name, string iconKey, IEnumerable<ACProperty> properties, IEnumerable<string> categories)
		: base(id, name, iconKey, properties)
	{
		Categories = new ReadOnlyCollection<string>((categories != null) ? categories.ToList() : new List<string>());
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
		if (Categories == null)
		{
			Categories = new ReadOnlyCollection<string>(new List<string>());
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACType aCType))
		{
			return false;
		}
		if (!ACElement.CompareCollections(Categories, aCType.Categories))
		{
			return false;
		}
		return base.Equals(obj);
	}
}
