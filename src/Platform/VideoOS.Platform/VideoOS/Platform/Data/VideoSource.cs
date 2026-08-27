using System;
using System.Collections.Generic;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Data;

public abstract class VideoSource
{
	private object _regObj1;

	public virtual Item Item { get; public set; }

	public VideoSource(Item item)
	{
		Item = item;
	}

	public virtual void Init()
	{
		if (_regObj1 == null)
		{
			_regObj1 = EnvironmentManager.Instance.RegisterReceiver(SystemConfigChangedEvent, new MessageIdFilter(MessageId.System.LocalConfigurationChangedIndication));
		}
	}

	public abstract void Init(int width, int height);

	public virtual void Close()
	{
		EnvironmentManager.Instance.UnRegisterReceiver(_regObj1);
		_regObj1 = null;
	}

	public abstract object Get(DateTime dateTime);

	public abstract object GetNearest(DateTime dateTime);

	public abstract object GetAtOrBefore(DateTime dateTime);

	public abstract object GetNext();

	public abstract List<object> Get(DateTime dateTime, TimeSpan maxTimeAfter, int maxCountAfter);

	private object SystemConfigChangedEvent(Message message, FQID dest, FQID sender)
	{
		if (message.Data is List<FQID> list && Item != null)
		{
			foreach (FQID item2 in list)
			{
				if (item2.ObjectId == Item.FQID.ObjectId && item2.ServerId.UserContext == Item.FQID.ServerId.UserContext && Item.FQID.ServerId.UserContext != null)
				{
					Item item = Item.FQID.ServerId.UserContext.Configuration.GetItem(item2);
					if (item != null)
					{
						Item.FQID = item.FQID;
					}
					break;
				}
			}
		}
		return null;
	}
}
