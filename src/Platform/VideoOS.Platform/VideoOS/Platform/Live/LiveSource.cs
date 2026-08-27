using System;
using System.Collections.Generic;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Live;

public abstract class LiveSource
{
	private Item _item;

	private bool _liveModeStart;

	private bool _singleFrameQueue;

	private int _framesInProgress;

	private readonly object _singleFrameLock = new object();

	private object _regObj1;

	public virtual Item Item
	{
		get
		{
			return _item;
		}
		set
		{
			_item = value;
		}
	}

	public virtual bool LiveModeStart
	{
		get
		{
			return _liveModeStart;
		}
		set
		{
			_liveModeStart = value;
		}
	}

	public bool SingleFrameQueue
	{
		get
		{
			return _singleFrameQueue;
		}
		set
		{
			_singleFrameQueue = value;
		}
	}

	public abstract event EventHandler LiveContentEvent;

	public abstract event EventHandler LiveStatusEvent;

	protected LiveSource()
	{
	}

	protected LiveSource(Item item)
	{
		_item = item;
	}

	public virtual void Init()
	{
		if (EnvironmentManager.Instance == null)
		{
			throw new MIPException("Environment is not initialized!");
		}
		_regObj1 = EnvironmentManager.Instance.RegisterReceiver(SystemConfigChangedEvent, new MessageIdFilter(MessageId.System.LocalConfigurationChangedIndication));
	}

	public virtual void Close()
	{
		if (EnvironmentManager.Instance != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_regObj1);
		}
		_regObj1 = null;
	}

	public void FinishedProcessingFrame()
	{
		lock (_singleFrameLock)
		{
			_framesInProgress--;
		}
	}

	protected void ProcessingNewFrame()
	{
		lock (_singleFrameLock)
		{
			_framesInProgress++;
		}
	}

	protected bool ProcessNextFrame()
	{
		if (SingleFrameQueue)
		{
			lock (_singleFrameLock)
			{
				return _framesInProgress == 0;
			}
		}
		return true;
	}

	private object SystemConfigChangedEvent(Message message, FQID dest, FQID sender)
	{
		if (message.Data is List<FQID> list)
		{
			foreach (FQID item2 in list)
			{
				if (item2.Kind == Kind.Camera && item2.ObjectId == Item.FQID.ObjectId && item2.ServerId.UserContext == Item.FQID.ServerId.UserContext)
				{
					Item item = ((item2.ServerId.UserContext == null) ? Configuration.Instance.GetItem(item2) : item2.ServerId.UserContext.Configuration.GetItem(item2));
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
