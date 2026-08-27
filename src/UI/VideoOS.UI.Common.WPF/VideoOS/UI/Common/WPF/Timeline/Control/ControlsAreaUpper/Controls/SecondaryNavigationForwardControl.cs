using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls;

public partial class SecondaryNavigationForwardControl : UserControl, IComponentConnector
{
	public SecondaryNavigationForwardControlModel Model { get; private set; }

	public event EventHandler<EventArgs> NavigateNextSequenceButtonClicked;

	public event EventHandler<EventArgs> NavigateLastButtonClicked;

	public SecondaryNavigationForwardControl()
	{
		InitializeComponent();
		Model = new SecondaryNavigationForwardControlModel();
		base.DataContext = Model;
	}

	private void _navigateNextSequenceButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.NavigateNextSequenceButtonClicked != null)
		{
			this.NavigateNextSequenceButtonClicked(this, new EventArgs());
		}
	}

	private void _navigateLastButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.NavigateLastButtonClicked != null)
		{
			this.NavigateLastButtonClicked(this, new EventArgs());
		}
	}
}
