using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Internal.Util;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public abstract class ACElement
{
	[DataMember]
	public string Id { get; private set; }

	[DataMember]
	public string Name { get; private set; }

	[DataMember]
	public string IconKey { get; private set; }

	[DataMember]
	public IEnumerable<ACProperty> Properties { get; private set; }

	public ACElement(string id, string name, string iconKey, IEnumerable<ACProperty> properties)
	{
		Id = id;
		Name = name;
		IconKey = iconKey ?? string.Empty;
		Properties = new ReadOnlyCollection<ACProperty>((properties != null) ? properties.ToList() : new List<ACProperty>());
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
		if (Properties == null)
		{
			Properties = new ReadOnlyCollection<ACProperty>(new List<ACProperty>());
		}
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (!(obj is ACElement aCElement))
		{
			return false;
		}
		if (Id != aCElement.Id)
		{
			return false;
		}
		if (Name != aCElement.Name)
		{
			return false;
		}
		if (IconKey != aCElement.IconKey)
		{
			return false;
		}
		if (!CompareCollections(Properties, aCElement.Properties))
		{
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}

	public static bool CompareCollections<T>(IEnumerable<T> first, IEnumerable<T> second)
	{
		return CollectionComparisonUtil.MultisetCompare(first, second);
	}
}
