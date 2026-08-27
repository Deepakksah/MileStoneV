using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSDropDownSmall : VideoOSDropDownBorderedBase
{
	public static class VideoOSDropDownSmallInternals
	{
		public static IMultiValueConverter SelectedItemTextAppearanceConverter { get; } = new LambdaConverter2<object, string>((object selectedItem, string customDisplayValue) => (customDisplayValue != null || selectedItem != null) ? ((object)VideoOSTextBlockBodySmall.Appearances.HighEmphasis) : ((object)VideoOSTextBlockBodySmall.Appearances.LowEmphasis), "SelectedItemTextAppearanceConverter");
	}

	static VideoOSDropDownSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSDropDownSmall), new FrameworkPropertyMetadata(typeof(VideoOSDropDownSmall)));
		ThemeManager.InitializeThemingSystem();
	}

	public override VideoOSContextMenuBase CreateContextMenu()
	{
		return new VideoOSContextMenuSmall();
	}
}
