using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public partial class CellText : ContentControl, IComponentConnector
{
	public static class CellTextInternals
	{
		public const string TallSelectedTemplateKey = "CellTextTallSelectedTemplateKey";

		public const string TallDeselectedTemplateKey = "CellTextTallDeselectedTemplateKey";

		public const string NormalSelectedTemplateKey = "CellTextNormalSelectedTemplateKey";

		public const string NormalDeselectedTemplateKey = "CellTextNormalDeselectedTemplateKey";

		public const string CompactSelectedTemplateKey = "CellTextCompactSelectedTemplateKey";

		public const string CompactDeselectedTemplateKey = "CellTextCompactDeselectedTemplateKey";
	}

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CellText), new PropertyMetadata(string.Empty));

	public string Text
	{
		get
		{
			return (string)GetValue(TextProperty);
		}
		set
		{
			SetValue(TextProperty, value);
		}
	}

	public CellText()
	{
		InitializeComponent();
	}
}
