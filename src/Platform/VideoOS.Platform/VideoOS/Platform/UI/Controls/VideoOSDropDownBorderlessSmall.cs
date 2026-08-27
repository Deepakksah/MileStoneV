using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSDropDownBorderlessSmall : VideoOSDropDownBorderlessBase
{
	public static class VideoOSDropDownBorderlessSmallInternals
	{
		public static IMultiValueConverter SelectedItemTextAppearanceConverter { get; } = new LambdaConverter3<bool, bool, bool>(delegate(bool isOpen, bool isEnabled, bool isMouseOver)
		{
			VideoOSTextBlockInteractiveSmall.Appearances appearances = VideoOSTextBlockInteractiveSmall.Appearances.HighEmphasis;
			if (isOpen || (isEnabled && isMouseOver))
			{
				appearances = VideoOSTextBlockInteractiveSmall.Appearances.Primary;
			}
			return appearances;
		}, "SelectedItemTextAppearanceConverter");

		public static IMultiValueConverter SelectedItemTextInteractiveStateConverter { get; } = new LambdaConverter3<bool, bool, bool>(delegate(bool isOpen, bool isMouseOver, bool isMouseLeftButtonDown)
		{
			VideoOSTextBlockInteractiveSmall.InteractiveStates interactiveStates = ((isOpen || isMouseLeftButtonDown) ? VideoOSTextBlockInteractiveSmall.InteractiveStates.Pressed : (isMouseOver ? VideoOSTextBlockInteractiveSmall.InteractiveStates.Hovered : VideoOSTextBlockInteractiveSmall.InteractiveStates.Idle));
			return interactiveStates;
		}, "SelectedItemTextInteractiveStateConverter");
	}

	static VideoOSDropDownBorderlessSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSDropDownBorderlessSmall), new FrameworkPropertyMetadata(typeof(VideoOSDropDownBorderlessSmall)));
		ThemeManager.InitializeThemingSystem();
	}

	public override VideoOSContextMenuBase CreateContextMenu()
	{
		return new VideoOSContextMenuSmall();
	}
}
