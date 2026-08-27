using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.FormIntegration;

public partial class AirspaceWrapper : ContentControl, IComponentConnector, IStyleConnector
{
	public bool SetTopMost { get; set; }

	private WindowsFormsHost FormsHost { get; set; }

	private Grid MainGrid { get; set; }

	public AirspaceWrapper()
	{
		InitializeComponent();
	}

	private void Grid_Initialized(object sender, EventArgs e)
	{
		MainGrid = sender as Grid;
	}

	private void WindowsFormsHost_Loaded(object sender, RoutedEventArgs e)
	{
		FormsHost = sender as WindowsFormsHost;
		if (FormsHost != null)
		{
			NativeMethods.SetWindowPos(FormsHost.Handle, SetTopMost ? NativeMethods.HWND_TOP : NativeMethods.HWND_BOTTOM, 0, 0, 0, 0, 3);
		}
	}
}
