using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSCheckBoxBase : CheckBox
{
	public static class VideoOSCheckBoxBaseInternals
	{
		public static IMultiValueConverter OutlineBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedOutlineDisabledBrush" : (isPressed ? "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedOutlinePressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedOutlineIdleBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedOutlineHoveredBrush")));
			return self.FindResource(resourceKey);
		}, "OutlineBrushConverter");

		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, bool isChecked)
		{
			string resourceKey = ((!isEnabled) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSCheckBoxSelectedBackgroundDisabledBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedBackgroundDisabledBrush") : (isPressed ? (isChecked ? "VideoOSResourceKeysInternalVideoOSCheckBoxSelectedBackgroundPressedBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedBackgroundPressedBrush") : ((!isMouseOver) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSCheckBoxSelectedBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedBackgroundIdleBrush") : (isChecked ? "VideoOSResourceKeysInternalVideoOSCheckBoxSelectedBackgroundHoveredBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedBackgroundHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");

		public static IMultiValueConverter TickBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, bool isChecked)
		{
			string resourceKey = ((!isEnabled) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSCheckBoxSelectedTickDisabledBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedTickDisabledBrush") : (isPressed ? (isChecked ? "VideoOSResourceKeysInternalVideoOSCheckBoxSelectedTickPressedBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedTickPressedBrush") : ((!isMouseOver) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSCheckBoxSelectedTickIdleBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedTickIdleBrush") : (isChecked ? "VideoOSResourceKeysInternalVideoOSCheckBoxSelectedTickHoveredBrush" : "VideoOSResourceKeysInternalVideoOSCheckBoxDeselectedTickHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "TickBrushConverter");
	}

	public static readonly DependencyProperty MinLinesProperty;

	public static readonly DependencyProperty MaxLinesProperty;

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

	static VideoOSCheckBoxBase()
	{
		MinLinesProperty = DependencyProperty.Register("MinLines", typeof(int), typeof(VideoOSCheckBoxBase), new PropertyMetadata(1));
		MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(VideoOSCheckBoxBase), new PropertyMetadata(int.MaxValue));
		FrameworkElement.CursorProperty.OverrideMetadata(typeof(VideoOSCheckBoxBase), new FrameworkPropertyMetadata(Cursors.Hand));
	}

	public VideoOSCheckBoxBase()
	{
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSCheckBoxBaseAutomationPeer(this);
	}
}
