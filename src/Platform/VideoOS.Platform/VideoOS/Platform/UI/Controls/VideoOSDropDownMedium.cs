using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSDropDownMedium : VideoOSDropDownBorderedBase
{
	public static class VideoOSDropDownMediumInternals
	{
		public static IMultiValueConverter SelectedItemTextAppearanceConverter { get; } = new LambdaConverter2<object, string>((object selectedItem, string customDisplayValue) => (customDisplayValue != null || selectedItem != null) ? ((object)VideoOSTextBlockBodyMedium.Appearances.HighEmphasis) : ((object)VideoOSTextBlockBodyMedium.Appearances.LowEmphasis), "SelectedItemTextAppearanceConverter");
	}

	static VideoOSDropDownMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSDropDownMedium), new FrameworkPropertyMetadata(typeof(VideoOSDropDownMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
