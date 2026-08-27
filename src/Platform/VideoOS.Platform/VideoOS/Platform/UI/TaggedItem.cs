using System;

namespace VideoOS.Platform.UI;

public class TaggedItem
{
	private object _item;

	private Guid _id;

	public Guid Tag => _id;

	public TaggedItem(Guid id, object item)
	{
		_item = item;
		_id = id;
	}

	public override string ToString()
	{
		if (_item == null)
		{
			return "Unknown";
		}
		return _item.ToString();
	}
}
