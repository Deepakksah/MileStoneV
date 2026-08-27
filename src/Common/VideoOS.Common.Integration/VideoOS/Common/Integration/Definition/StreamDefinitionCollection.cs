using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.Definition;

public class StreamDefinitionCollection : KeyedCollection<StreamHwid, StreamDefinition>
{
	protected override StreamHwid GetKeyForItem(StreamDefinition item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.Hwid;
	}

	public StreamDefinition FindByReferenceId(Guid referenceId)
	{
		using (IEnumerator<StreamDefinition> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				StreamDefinition current = enumerator.Current;
				if (current.ReferenceId == referenceId)
				{
					return current;
				}
			}
		}
		return null;
	}

	public bool TryGetValue(StreamHwid hwid, out StreamDefinition value)
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
