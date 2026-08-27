using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSPanel : ContentControl
{
	public static class VideoOSPanelInternals
	{
		public static IMultiValueConverter BackgroundConverter { get; } = new LambdaConverter2<DependencyObject, BackgroundAppearances>((DependencyObject self, BackgroundAppearances appearance) => self.FindResource(appearance switch
		{
			BackgroundAppearances.Transparent => VideoOSResourceKeys.TransparentBrush, 
			BackgroundAppearances.Background => "VideoOSResourceKeysInternalBackgroundBrushForWindowAndDialog", 
			BackgroundAppearances.Highlight => "VideoOSResourceKeysInternalHighlightBrushForWindowAndDialog", 
			BackgroundAppearances.EmptyState => "VideoOSResourceKeysInternalEmptyStateBrushForWindowAndDialog", 
			BackgroundAppearances.Subtle => "VideoOSResourceKeysInternalSubtleBrushForWindowAndDialog", 
			_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
		}), "BackgroundConverter");
	}

	public enum BackgroundAppearances
	{
		Transparent,
		Background,
		Highlight,
		EmptyState,
		Subtle
	}

	public static readonly DependencyProperty BackgroundAppearanceProperty;

	public static readonly DependencyProperty IsBorderVisibleProperty;

	public BackgroundAppearances BackgroundAppearance
	{
		get
		{
			return (BackgroundAppearances)GetValue(BackgroundAppearanceProperty);
		}
		set
		{
			SetValue(BackgroundAppearanceProperty, value);
		}
	}

	public bool IsBorderVisible
	{
		get
		{
			return (bool)GetValue(IsBorderVisibleProperty);
		}
		set
		{
			SetValue(IsBorderVisibleProperty, value);
		}
	}

	static VideoOSPanel()
	{
		BackgroundAppearanceProperty = DependencyProperty.Register("BackgroundAppearance", typeof(BackgroundAppearances), typeof(VideoOSPanel), new PropertyMetadata(BackgroundAppearances.Transparent));
		IsBorderVisibleProperty = DependencyProperty.Register("IsBorderVisible", typeof(bool), typeof(VideoOSPanel), new PropertyMetadata(false));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSPanel), new FrameworkPropertyMetadata(typeof(VideoOSPanel)));
		ThemeManager.InitializeThemingSystem();
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSPanel), new FrameworkPropertyMetadata(false));
		UIElement.FocusableProperty.OverrideMetadata(typeof(VideoOSPanel), new FrameworkPropertyMetadata(false));
		Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(VideoOSPanel), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
		Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(VideoOSPanel), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSPanelAutomationPeer(this);
	}
}
