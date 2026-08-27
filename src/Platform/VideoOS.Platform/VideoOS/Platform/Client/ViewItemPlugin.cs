using System;
using System.Drawing;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.Client;

public abstract class ViewItemPlugin
{
	private VideoOSIconSourceBase _iconSource;

	public abstract Guid Id { get; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public virtual Image Icon { get; }

	public virtual VideoOSIconSourceBase IconSource
	{
		get
		{
			return _iconSource ?? Icon.ToIconBitmapSource();
		}
		protected set
		{
			_iconSource = value;
		}
	}

	public abstract string Name { get; }

	public virtual bool HideSetupItem => false;

	public virtual bool PreventInclusion => false;

	public virtual bool IsPrintEnabled => true;

	protected ViewItemPlugin()
	{
		new TrackPageViewData(typeof(ViewItemPlugin)).Send();
	}

	public abstract ViewItemManager GenerateViewItemManager();

	public abstract void Init();

	public abstract void Close();
}
