using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSDropDownBorderlessMedium : VideoOSDropDownBorderlessBase
{
	public static class VideoOSDropDownBorderlessMediumInternals
	{
		public static IMultiValueConverter SelectedItemTextAppearanceConverter { get; } = new LambdaConverter3<bool, bool, bool>(delegate(bool isOpen, bool isEnabled, bool isMouseOver)
		{
			VideoOSTextBlockInteractiveMedium.Appearances appearances = VideoOSTextBlockInteractiveMedium.Appearances.HighEmphasis;
			if (isOpen || (isEnabled && isMouseOver))
			{
				appearances = VideoOSTextBlockInteractiveMedium.Appearances.Primary;
			}
			return appearances;
		}, "SelectedItemTextAppearanceConverter");

		public static IMultiValueConverter SelectedItemTextInteractiveStateConverter { get; } = new LambdaConverter3<bool, bool, bool>(delegate(bool isOpen, bool isMouseOver, bool isMouseLeftButtonDown)
		{
			VideoOSTextBlockInteractiveMedium.InteractiveStates interactiveStates = ((isOpen || isMouseLeftButtonDown) ? VideoOSTextBlockInteractiveMedium.InteractiveStates.Pressed : (isMouseOver ? VideoOSTextBlockInteractiveMedium.InteractiveStates.Hovered : VideoOSTextBlockInteractiveMedium.InteractiveStates.Idle));
			return interactiveStates;
		}, "SelectedItemTextInteractiveStateConverter");
	}

	static VideoOSDropDownBorderlessMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSDropDownBorderlessMedium), new FrameworkPropertyMetadata(typeof(VideoOSDropDownBorderlessMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
