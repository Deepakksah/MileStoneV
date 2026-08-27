using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;
using VideoOS.Platform.AccessControl.TypeCategories;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public sealed class ACEventType : ACType
{
	public bool IsEnabled
	{
		get
		{
			return !base.Categories.Contains(ACBuiltInEventTypeCategories.Ignore);
		}
		set
		{
			if (base.Categories.Contains(ACBuiltInEventTypeCategories.Ignore) != !value)
			{
				List<string> list = base.Categories.ToList();
				if (value)
				{
					list.Remove(ACBuiltInEventTypeCategories.Ignore);
				}
				else
				{
					list.Add(ACBuiltInEventTypeCategories.Ignore);
				}
				base.Categories = new ReadOnlyCollection<string>(list);
			}
		}
	}

	public ACEventType(string id, string name, string iconKey, IEnumerable<ACProperty> properties, IEnumerable<string> categories)
		: base(id, name, iconKey, properties, categories)
	{
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACEventType))
		{
			return false;
		}
		return base.Equals(obj);
	}
}
