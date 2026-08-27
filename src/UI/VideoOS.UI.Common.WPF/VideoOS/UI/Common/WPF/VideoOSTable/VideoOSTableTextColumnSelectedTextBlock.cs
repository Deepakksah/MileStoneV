using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.VideoOSTable;

public partial class VideoOSTableTextColumnSelectedTextBlock : UserControl, IComponentConnector
{
	public class VideoOSTableTextColumnSelectedTextBlockInternals
	{
		public const string SelectedTemplateKey = "VideoOSTableTextColumnSelectedTextBlockSelectedTemplateKey";

		public const string DeselectedTemplateKey = "VideoOSTableTextColumnSelectedTextBlockDeselectedTemplateKey";
	}

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(VideoOSTableTextColumnSelectedTextBlock), new FrameworkPropertyMetadata(string.Empty));

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

	public VideoOSTableTextColumnSelectedTextBlock()
	{
		InitializeComponent();
	}
}
