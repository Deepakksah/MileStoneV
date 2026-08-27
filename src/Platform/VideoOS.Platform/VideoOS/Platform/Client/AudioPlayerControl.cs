using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use AudioPlayer instead.", false)]
public abstract class AudioPlayerControl : UserControl
{
	public delegate void ConnectionStateChangedHandler(object sender, ConnectionState connectionState);

	private object _regObj1;

	private IContainer components;

	public abstract FQID MicrophoneFQID { get; set; }

	public virtual FQID SpeakerFQID
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual FQID PlaybackControllerFQID
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public abstract double Volume { get; set; }

	public abstract bool Mute { get; set; }

	public abstract int ConnectionState { get; }

	public abstract event ConnectResponseEventHandler ConnectResponseEvent;

	public virtual event ConnectionStateChangedHandler ConnectionStateChangedEvent;

	public AudioPlayerControl()
	{
		InitializeComponent();
	}

	public virtual void Initialize()
	{
		if (_regObj1 == null)
		{
			_regObj1 = EnvironmentManager.Instance.RegisterReceiver(SystemConfigChangedEvent, new MessageIdFilter(MessageId.System.LocalConfigurationChangedIndication));
		}
	}

	public abstract void Connect();

	public abstract void Disconnect();

	public virtual void Close()
	{
		EnvironmentManager.Instance.UnRegisterReceiver(_regObj1);
		_regObj1 = null;
	}

	public abstract void StartLive();

	public abstract void StartBrowse();

	public abstract void StartSetup();

	private object SystemConfigChangedEvent(VideoOS.Platform.Messaging.Message message, FQID dest, FQID sender)
	{
		if (message.Data is List<FQID> list)
		{
			foreach (FQID item in list)
			{
				if (item.ObjectId == MicrophoneFQID.ObjectId && item.ServerId.UserContext == MicrophoneFQID.ServerId.UserContext)
				{
					MicrophoneFQID = ((item.ServerId.UserContext == null) ? Configuration.Instance.GetItem(item) : item.ServerId.UserContext.Configuration.GetItem(item))?.FQID;
					break;
				}
			}
		}
		return null;
	}

	[Obsolete("This method is no longer supported")]
	public virtual void SetAudioXml(UserContext userContext, string xml)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	}
}
