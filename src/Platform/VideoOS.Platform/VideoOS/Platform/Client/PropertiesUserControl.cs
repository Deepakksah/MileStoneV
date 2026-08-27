using System;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use PropertiesWpfUserControl instead.", false)]
public class PropertiesUserControl : UserControl
{
	public PropertiesUserControl()
	{
		new TrackPageViewData(typeof(PropertiesUserControl), deprecated: true).Send();
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.Name = "PropertiesUserControl";
		base.Size = new System.Drawing.Size(267, 71);
		base.ResumeLayout(false);
	}
}
