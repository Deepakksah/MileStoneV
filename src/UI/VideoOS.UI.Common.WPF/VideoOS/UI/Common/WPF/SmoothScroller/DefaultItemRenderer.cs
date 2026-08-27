using System;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.SmoothScroller;

public partial class DefaultItemRenderer : UserControl, IComponentConnector
{
	private static Random random = new Random();

	public int Index
	{
		set
		{
			_textBlock.Text = value.ToString();
		}
	}

	public DefaultItemRenderer()
	{
		InitializeComponent();
		_grid.Background = new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));
		_grid.Background.Freeze();
	}
}
