using System.Windows.Controls;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public class PropertiesWpfUserControl : UserControl
{
	public PropertiesWpfUserControl()
	{
		new TrackPageViewData(typeof(PropertiesWpfUserControl)).Send();
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
