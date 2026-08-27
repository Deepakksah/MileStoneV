using System;
using System.Drawing;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.Client;

public abstract class ViewLayout
{
	private VideoOSIconBitmapSource _iconSource;

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public virtual Image Icon { get; set; }

	public virtual VideoOSIconBitmapSource IconSource
	{
		get
		{
			return _iconSource ?? Icon.ToIconBitmapSource();
		}
		set
		{
			_iconSource = value;
		}
	}

	public abstract Rectangle[] Rectangles { get; set; }

	public abstract Guid Id { get; set; }

	public abstract string DisplayName { get; set; }

	public ViewLayout(string name, Rectangle[] layoutArray)
	{
		if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
		{
			new TrackPageViewData(typeof(ViewLayout)).Send();
		}
	}

	public ViewLayout()
	{
		if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
		{
			new TrackPageViewData(typeof(ViewLayout)).Send();
		}
	}
}
