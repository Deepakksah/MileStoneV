using System;
using System.Collections.Generic;

namespace VideoOS.Platform;

public class GenericEventConfigItem : ConfigItem
{
	protected const string GENERIC_GENERATOR_ID = "D1AF534B-1048-4D15-BC4D-A88AF82E8D5B";

	protected const string SHADOW_ID = "ShadowId";

	private Guid _userdefinedEventId = Guid.Empty;

	public Guid ShadowId
	{
		get
		{
			return _userdefinedEventId;
		}
		protected set
		{
			_userdefinedEventId = value;
		}
	}

	protected GenericEventConfigItem(FQID fqid, string name)
		: base(fqid, name)
	{
	}

	protected GenericEventConfigItem(Item item)
		: base(item.FQID, item.Name)
	{
		foreach (KeyValuePair<string, string> property in item.Properties)
		{
			Properties.Add(property.Key, property.Value);
		}
	}

	public virtual void RemoveShadowEvent()
	{
		ShadowId = Guid.Empty;
	}

	public virtual bool Rename(string newName)
	{
		return true;
	}

	public virtual bool Create()
	{
		if (!Properties.ContainsKey("ShadowId") && ShadowId != Guid.Empty)
		{
			Properties.Add("ShadowId", _userdefinedEventId.ToString());
			return true;
		}
		return false;
	}
}
