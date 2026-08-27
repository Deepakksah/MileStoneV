using System;
using System.Drawing;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.Client;

public abstract class LoginPlugin
{
	public abstract Guid Id { get; }

	public abstract string Name { get; }

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public virtual Image Icon
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual VideoOSIconSourceBase IconSource
	{
		get
		{
			return Icon.ToIconBitmapSource();
		}
		set
		{
		}
	}

	public virtual bool IncludeInLoginFlow => false;

	protected LoginPlugin()
	{
		new TrackPageViewData(typeof(LoginPlugin)).Send();
	}

	public virtual void LoginFlowExecute()
	{
	}

	public virtual LoginUserControl GenerateCustomLoginUserControl()
	{
		return null;
	}

	public abstract void Init(string surveilanceUsername);

	public abstract void Close();
}
