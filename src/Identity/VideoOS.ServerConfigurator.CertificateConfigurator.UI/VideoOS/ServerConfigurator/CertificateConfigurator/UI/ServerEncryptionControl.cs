using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.UI.Common.WPF.Utils.Converters;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public partial class ServerEncryptionControl : UserControl, IComponentConnector
{
	public ServerEncryptionControl()
	{
		InitializeComponent();
		new NegationConverterExtension().ToString();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
