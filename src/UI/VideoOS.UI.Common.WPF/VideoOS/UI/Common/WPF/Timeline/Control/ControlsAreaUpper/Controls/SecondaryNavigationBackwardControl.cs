using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls;

public partial class SecondaryNavigationBackwardControl : UserControl, IComponentConnector
{
	public SecondaryNavigationBackwardControlModel Model { get; private set; }

	public event EventHandler<EventArgs> NavigateFirstButtonClicked;

	public event EventHandler<EventArgs> NavigatePreviousSequenceButtonClicked;

	public SecondaryNavigationBackwardControl()
	{
		InitializeComponent();
		Model = new SecondaryNavigationBackwardControlModel();
		base.DataContext = Model;
	}

	private void _navigateFirstButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.NavigateFirstButtonClicked != null)
		{
			this.NavigateFirstButtonClicked(this, new EventArgs());
		}
	}

	private void _navigatePreviousSequenceButton_Click(object sender, RoutedEventArgs e)
	{
		if (this.NavigatePreviousSequenceButtonClicked != null)
		{
			this.NavigatePreviousSequenceButtonClicked(this, new EventArgs());
		}
	}
}
