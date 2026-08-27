using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSDropDownLarge : VideoOSDropDownBorderedBase
{
	public static class VideoOSDropDownLargeInternals
	{
		public static IMultiValueConverter SelectedItemTextAppearanceConverter { get; } = new LambdaConverter2<object, string>((object selectedItem, string customDisplayValue) => (customDisplayValue != null || selectedItem != null) ? ((object)VideoOSTextBlockBodyMedium.Appearances.HighEmphasis) : ((object)VideoOSTextBlockBodyMedium.Appearances.LowEmphasis), "SelectedItemTextAppearanceConverter");
	}

	static VideoOSDropDownLarge()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSDropDownLarge), new FrameworkPropertyMetadata(typeof(VideoOSDropDownLarge)));
		ThemeManager.InitializeThemingSystem();
	}
}
