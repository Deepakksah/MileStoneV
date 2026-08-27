using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSToggleButtonIconBase : VideoOSToggleButtonBase
{
	public static class VideoOSIconSourceBaseInternals
	{
		public static IMultiValueConverter OutlineBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, bool isChecked)
		{
			string resourceKey = ((!isEnabled) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonIconOutlineOnDisabledBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonIconOutlineOffDisabledBrush") : (isPressed ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonIconOutlineOnPressedBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonIconOutlineOffPressedBrush") : ((!isMouseOver) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonIconOutlineOnIdleBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonIconOutlineOffIdleBrush") : (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonIconOutlineOnHoveredBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonIconOutlineOffHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "OutlineBrushConverter");

		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, bool isChecked)
		{
			string resourceKey = ((!isEnabled) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonIconBackgroundOnDisabledBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonIconBackgroundOffDisabledBrush") : (isPressed ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonIconBackgroundOnPressedBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonIconBackgroundOffPressedBrush") : ((!isMouseOver) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonIconBackgroundOnIdleBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonIconBackgroundOffIdleBrush") : (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonIconBackgroundOnHoveredBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonIconBackgroundOffHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");
	}

	public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register("IconSource", typeof(VideoOSIconSourceBase), typeof(VideoOSToggleButtonIconBase), new PropertyMetadata(null));

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return (VideoOSIconSourceBase)GetValue(IconSourceProperty);
		}
		set
		{
			SetValue(IconSourceProperty, value);
		}
	}

	public VideoOSToggleButtonIconBase()
	{
	}
}
