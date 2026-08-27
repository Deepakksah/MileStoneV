using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.Selection;

public partial class SelectionInfo : UserControl, IComponentConnector
{
	public SelectionInfoModel Model { get; private set; }

	public SelectionInfo()
	{
		InitializeComponent();
		Model = new SelectionInfoModel();
		base.DataContext = Model;
	}
}
