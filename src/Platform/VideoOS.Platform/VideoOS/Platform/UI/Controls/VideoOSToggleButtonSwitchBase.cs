using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSToggleButtonSwitchBase : VideoOSToggleButtonBase
{
	public static class VideoOSToggleButtonSwitchBaseInternals
	{
		public static IMultiValueConverter OutlineBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchOutlineOffDisabledBrush" : (isPressed ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchOutlineOffPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchOutlineOffIdleBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchOutlineOffHoveredBrush")));
			return self.FindResource(resourceKey);
		}, "OutlineBrushConverter");

		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, bool isChecked)
		{
			string resourceKey = ((!isEnabled) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchBackgroundOnDisabledBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchBackgroundOffDisabledBrush") : (isPressed ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchBackgroundOnPressedBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchBackgroundOffPressedBrush") : ((!isMouseOver) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchBackgroundOnIdleBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchBackgroundOffIdleBrush") : (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchBackgroundOnHoveredBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchBackgroundOffHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");

		public static IMultiValueConverter DotBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, bool isChecked)
		{
			string resourceKey = ((!isEnabled) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchDotOnDisabledBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchDotOffDisabledBrush") : (isPressed ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchDotOnPressedBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchDotOffPressedBrush") : ((!isMouseOver) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchDotOnIdleBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchDotOffIdleBrush") : (isChecked ? "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchDotOnHoveredBrush" : "VideoOSResourceKeysInternalVideoOSToggleButtonSwitchDotOffHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "DotBrushConverter");
	}

	public static readonly DependencyProperty MinLinesProperty = DependencyProperty.Register("MinLines", typeof(int), typeof(VideoOSToggleButtonSwitchBase), new PropertyMetadata(1));

	public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(VideoOSToggleButtonSwitchBase), new PropertyMetadata(int.MaxValue));

	public int MinLines
	{
		get
		{
			return (int)GetValue(MinLinesProperty);
		}
		set
		{
			SetValue(MinLinesProperty, value);
		}
	}

	public int MaxLines
	{
		get
		{
			return (int)GetValue(MaxLinesProperty);
		}
		set
		{
			SetValue(MaxLinesProperty, value);
		}
	}

	public VideoOSToggleButtonSwitchBase()
	{
	}
}
