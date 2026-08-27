using System;
using System.Windows.Forms;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use SidePanelWpfUserControl instead.", false)]
public class SidePanelUserControl : UserControl
{
	public SidePanelUserControl()
	{
		new TrackPageViewData(typeof(SidePanelUserControl), deprecated: true).Send();
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
