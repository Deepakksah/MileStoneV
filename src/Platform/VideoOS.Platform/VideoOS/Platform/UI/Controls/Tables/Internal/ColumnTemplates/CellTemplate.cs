using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public partial class CellTemplate : ContentControl, IComponentConnector
{
	public CellTemplate()
	{
		InitializeComponent();
	}

	private void CellTemplate_OnLoaded(object sender, RoutedEventArgs e)
	{
		DataGridCell dataGridCell = this.FindAncestor<DataGridCell>();
		if (dataGridCell != null)
		{
			VideoOSTableTemplateColumn videoOSTableTemplateColumn = (VideoOSTableTemplateColumn)((VideoOSTable.ColumnDataGridTemplateColumn)dataGridCell.Column).Column;
			base.ContentTemplate = videoOSTableTemplateColumn.CellTemplate;
		}
	}
}
