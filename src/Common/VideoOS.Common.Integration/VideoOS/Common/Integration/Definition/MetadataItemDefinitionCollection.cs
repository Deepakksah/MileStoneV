using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.Definition;

public class MetadataItemDefinitionCollection : KeyedCollection<MetadataItemHwid, MetadataItemDefinition>
{
	protected override MetadataItemHwid GetKeyForItem(MetadataItemDefinition item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.Hwid;
	}

	public MetadataItemDefinition FindByReferenceId(Guid referenceId)
	{
		using (IEnumerator<MetadataItemDefinition> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MetadataItemDefinition current = enumerator.Current;
				if (current.ReferenceId == referenceId)
				{
					return current;
				}
			}
		}
		return null;
	}

	public bool TryGetValue(MetadataItemHwid hwid, out MetadataItemDefinition value)
	{
		value = null;
		try
		{
			value = base[hwid];
			return true;
		}
		catch (KeyNotFoundException)
		{
			return false;
		}
	}
}
