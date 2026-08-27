using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public partial class HardwareSelectionAndConfigurationControl : UserControl, IComponentConnector
{
	private HardwareData ViewModel => base.DataContext as HardwareData;

	public HardwareSelectionAndConfigurationControl()
	{
		InitializeComponent();
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		ViewModel.ExpandComponents = !ViewModel.ExpandComponents;
		if (ViewModel.ExpandComponents && selectionPanel.Children.Count == 1)
		{
			selectionPanel.Children.Add(new DevicePanelControl());
		}
		if (!ViewModel.ExpandComponents && selectionPanel.Children.Count == 2)
		{
			selectionPanel.Children.RemoveAt(1);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
