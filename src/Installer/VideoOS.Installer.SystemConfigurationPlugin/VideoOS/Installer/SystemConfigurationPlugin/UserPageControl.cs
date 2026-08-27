using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public partial class UserPageControl : UserControl, IHavePassword, IComponentConnector
{
	public PasswordBox PasswordBox => UserPasswordBox;

	public PasswordBox PasswordRepeatBox => UserPasswordRepeatBox;

	public UserPageControl()
	{
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
