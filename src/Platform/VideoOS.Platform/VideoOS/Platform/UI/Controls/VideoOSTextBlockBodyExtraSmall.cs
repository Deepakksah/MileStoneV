using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTextBlockBodyExtraSmall : VideoOSTextBlockBase
{
	public static class VideoOSTextBlockBodyExtraSmallInternals
	{
		public const double FontSize = 10.0;

		public static readonly FontWeight FontWeight = FontWeights.Regular;

		public static readonly FontFamily FontFamily = new FontFamily("Segoe UI");

		public static readonly FontStyle FontStyle = FontStyles.Normal;

		public static readonly FontStretch FontStretch = FontStretches.Normal;

		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter3<DependencyObject, Appearances, bool>((DependencyObject self, Appearances appearance, bool isEnabled) => self.FindResource(appearance switch
		{
			Appearances.HighEmphasis => isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockBodyExtraSmallHighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodyExtraSmallHighEmphasisForegroundDisabledBrush", 
			Appearances.LowEmphasis => isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockBodyExtraSmallLowEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodyExtraSmallLowEmphasisForegroundDisabledBrush", 
			Appearances.Destructive => isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockBodyExtraSmallDestructiveForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodyExtraSmallDestructiveForegroundDisabledBrush", 
			_ => throw new ArgumentOutOfRangeException("Appearance", appearance, null), 
		}), "TextBrushConverter");

		public static IMultiValueConverter OpacityConverter { get; } = new LambdaConverter3<DependencyObject, Appearances, bool>(delegate(DependencyObject self, Appearances appearance, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTextBlockOpacityDisabled" : (appearance switch
			{
				Appearances.HighEmphasis => "VideoOSResourceKeysInternalVideoOSTextBlockOpacityHighEmphasis", 
				Appearances.LowEmphasis => "VideoOSResourceKeysInternalVideoOSTextBlockOpacityLowEmphasis", 
				Appearances.Destructive => "VideoOSResourceKeysInternalVideoOSTextBlockOpacityEnabled", 
				_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
			}));
			return self.FindResource(resourceKey);
		}, "OpacityConverter");
	}

	public enum Appearances
	{
		HighEmphasis,
		LowEmphasis,
		Destructive
	}

	public static readonly DependencyProperty AppearanceProperty;

	public Appearances Appearance
	{
		get
		{
			return (Appearances)GetValue(AppearanceProperty);
		}
		set
		{
			SetValue(AppearanceProperty, value);
		}
	}

	static VideoOSTextBlockBodyExtraSmall()
	{
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSTextBlockBodyExtraSmall), new PropertyMetadata(Appearances.HighEmphasis));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockBodyExtraSmall), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockBodyExtraSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
