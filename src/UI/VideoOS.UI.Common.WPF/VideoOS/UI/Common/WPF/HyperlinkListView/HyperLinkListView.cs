using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.HyperlinkListView;

public partial class HyperLinkListView : ListView, IComponentConnector, IStyleConnector
{
	public delegate void HyperlinkClickedHandler(object sender, HyperlinkClickedArgs args);

	private bool _mousePressed;

	public event HyperlinkClickedHandler HyperlinkClickedEvent;

	private void FireHyperlinkClicked(object sender, HyperlinkClickedArgs args)
	{
		if (this.HyperlinkClickedEvent != null)
		{
			this.HyperlinkClickedEvent(sender, args);
		}
	}

	public HyperLinkListView()
	{
		InitializeComponent();
	}

	private void tx_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		_mousePressed = true;
	}

	private void tx_MouseLeave(object sender, MouseEventArgs e)
	{
		_mousePressed = false;
	}

	private void tx_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (sender is TextBlock textBlock && _mousePressed)
		{
			HyperlinkClickedArgs args = new HyperlinkClickedArgs(textBlock.DataContext as HyperlinkListViewData);
			FireHyperlinkClicked(this, args);
			_mousePressed = false;
		}
	}
}
