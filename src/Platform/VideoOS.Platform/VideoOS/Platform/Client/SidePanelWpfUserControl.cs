using System.Windows.Controls;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public class SidePanelWpfUserControl : UserControl
{
	public SidePanelWpfUserControl()
	{
		new TrackPageViewData(typeof(SidePanelWpfUserControl)).Send();
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
