using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaLower.Controls;

public partial class SetupControl : UserControl, IComponentConnector
{
	public event EventHandler SetupButtonClicked;

	public SetupControl()
	{
		InitializeComponent();
	}

	private void _setupButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.SetupButtonClicked != null)
		{
			this.SetupButtonClicked(this, new EventArgs());
		}
	}
}
