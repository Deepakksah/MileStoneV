using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Markup;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated because it is a wrapper class for a Form control. Windows Forms controls are no longer supported. Use PropertiesWpfUserControl instead.", false)]
public partial class PropertiesUserControlWpfWrapper : PropertiesWpfUserControl, IComponentConnector
{
	private PropertiesUserControl _winFormsControl;

	public PropertiesUserControl WinFormsControl
	{
		set
		{
			_winFormsControl = value;
			_childHost.Height = _winFormsControl.Height;
			_winFormsControl.Dock = DockStyle.Fill;
			_childHost.Child = value;
		}
	}

	public PropertiesUserControlWpfWrapper()
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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
