using System;
using System.Windows.Markup;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated because it is a wrapper class for a Form control. Windows Forms controls are no longer supported. Use SidePanelWpfUserControl instead.", false)]
public partial class SidePanelUserControlWpfWrapper : SidePanelWpfUserControl, IComponentConnector
{
	private SidePanelUserControl _winFormsControl;

	public SidePanelUserControl WinFormsControl
	{
		set
		{
			_winFormsControl = value;
			_childHost.Height = _winFormsControl.Height;
			_childHost.Child = value;
		}
	}

	public SidePanelUserControlWpfWrapper()
	{
		InitializeComponent();
	}

	public override void Init()
	{
		if (_winFormsControl != null)
		{
			_winFormsControl.Init();
		}
		base.Init();
	}

	public override void Close()
	{
		_childHost.Child = null;
		_childHost.Dispose();
		if (_winFormsControl != null)
		{
			_winFormsControl.Close();
			_winFormsControl.Dispose();
			_winFormsControl = null;
		}
		base.Close();
	}
}
