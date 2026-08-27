using System;
using System.Windows.Forms;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use SettingsPanelPlugin instead.", false)]
public class OptionsDialogUserControl : UserControl
{
	public OptionsDialogUserControl()
	{
		new TrackPageViewData(typeof(OptionsDialogUserControl), deprecated: true).Send();
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
