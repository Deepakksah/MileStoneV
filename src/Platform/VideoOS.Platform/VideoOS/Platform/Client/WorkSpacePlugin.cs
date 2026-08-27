using System;
using System.Drawing;
using System.Xml;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.UI;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Client;

public abstract class WorkSpacePlugin : PropertyClass
{
	private ViewAndLayoutItem _viewAndLayoutItem;

	private WorkSpaceNotification _workSpaceNotification;

	public abstract Guid Id { get; }

	public abstract string Name { get; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public virtual Image Icon => VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx];

	public virtual VideoOSIconSourceBase IconSource
	{
		get
		{
			if (VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx] != Icon)
			{
				return Icon.ToIconBitmapSource();
			}
			return new VideoOSIconBuiltInSource
			{
				Icon = VideoOSIconBuiltInSource.Icons.Plugin
			};
		}
	}

	public virtual bool Live => true;

	public virtual bool ShowTimeline => true;

	public virtual bool IsSetupStateSupported => false;

	public ViewAndLayoutItem ViewAndLayoutItem => _viewAndLayoutItem;

	public WorkSpaceNotification Notification
	{
		get
		{
			return _workSpaceNotification;
		}
		set
		{
			_workSpaceNotification = value;
			if (this.NotificationChangedEvent != null)
			{
				this.NotificationChangedEvent(this, new EventArgs());
			}
		}
	}

	public event EventHandler NotificationChangedEvent;

	public WorkSpacePlugin()
	{
		_viewAndLayoutItem = InternalCommandService.Instance.CreateWorkSpaceViewAndLayoutItem(Id);
		_viewAndLayoutItem.Layout = new Rectangle[1]
		{
			new Rectangle(0, 0, 999, 999)
		};
		new TrackPageViewData(typeof(WorkSpacePlugin)).Send();
	}

	public abstract void Init();

	public abstract void Close();

	public virtual void ViewItemConfigurationModified(int index)
	{
	}

	public bool LoadProperties(bool userPrivate)
	{
		XmlNode optionsConfiguration = Configuration.Instance.GetOptionsConfiguration(Id, userPrivate);
		if (optionsConfiguration != null)
		{
			Deserialize(optionsConfiguration);
			return true;
		}
		return false;
	}

	public bool SaveProperties(bool userPrivate)
	{
		Configuration.Instance.SaveOptionsConfiguration(Id, userPrivate, SerializeToXmlNode());
		return true;
	}
}
