using System;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.Definition;

public class EventDefinitionCollection : KeyedCollection<Guid, EventDefinition>
{
	protected override Guid GetKeyForItem(EventDefinition item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.ReferenceId;
	}

	public EventDefinition Find(EventHwid eventHwid)
	{
		if (eventHwid == null)
		{
			throw new ArgumentNullException("eventHwid");
		}
		string text = eventHwid.Text;
		foreach (EventDefinition item in base.Items)
		{
			if (item.Hwid.Text == text)
			{
				return item;
			}
		}
		return null;
	}
}
