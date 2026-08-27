using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSRadioButtonBase : RadioButton
{
	public static class VideoOSRadioButtonBaseInternals
	{
		public static IMultiValueConverter OutlineBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, bool isChecked)
		{
			string resourceKey = ((!isEnabled) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSRadioButtonSelectedOutlineDisabledBrush" : "VideoOSResourceKeysInternalVideoOSRadioButtonDeselectedOutlineDisabledBrush") : (isPressed ? (isChecked ? "VideoOSResourceKeysInternalVideoOSRadioButtonSelectedOutlinePressedBrush" : "VideoOSResourceKeysInternalVideoOSRadioButtonDeselectedOutlinePressedBrush") : ((!isMouseOver) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSRadioButtonSelectedOutlineIdleBrush" : "VideoOSResourceKeysInternalVideoOSRadioButtonDeselectedOutlineIdleBrush") : (isChecked ? "VideoOSResourceKeysInternalVideoOSRadioButtonSelectedOutlineHoveredBrush" : "VideoOSResourceKeysInternalVideoOSRadioButtonDeselectedOutlineHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "OutlineBrushConverter");

		public static IMultiValueConverter DotBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isPressed, bool isEnabled, bool isChecked)
		{
			string resourceKey = ((!isEnabled) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSRadioButtonSelectedDotDisabledBrush" : "VideoOSResourceKeysInternalVideoOSRadioButtonDeselectedDotDisabledBrush") : (isPressed ? (isChecked ? "VideoOSResourceKeysInternalVideoOSRadioButtonSelectedDotPressedBrush" : "VideoOSResourceKeysInternalVideoOSRadioButtonDeselectedDotPressedBrush") : ((!isMouseOver) ? (isChecked ? "VideoOSResourceKeysInternalVideoOSRadioButtonSelectedDotIdleBrush" : "VideoOSResourceKeysInternalVideoOSRadioButtonDeselectedDotIdleBrush") : (isChecked ? "VideoOSResourceKeysInternalVideoOSRadioButtonSelectedDotHoveredBrush" : "VideoOSResourceKeysInternalVideoOSRadioButtonDeselectedDotHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "DotBrushConverter");
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

	static VideoOSRadioButtonBase()
	{
		MinLinesProperty = DependencyProperty.Register("MinLines", typeof(int), typeof(VideoOSRadioButtonBase), new PropertyMetadata(1));
		MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(VideoOSRadioButtonBase), new PropertyMetadata(int.MaxValue));
		FrameworkElement.CursorProperty.OverrideMetadata(typeof(VideoOSRadioButtonBase), new FrameworkPropertyMetadata(Cursors.Hand));
	}

	public VideoOSRadioButtonBase()
	{
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSRadioButtonBaseAutomationPeer(this);
	}
}
