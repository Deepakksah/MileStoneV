using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTextBlockH2 : VideoOSTextBlockBase
{
	public static class VideoOSTextBlockH2Internals
	{
		public const double FontSize = 16.0;

		public static readonly FontWeight FontWeight = FontWeights.SemiBold;

		public static readonly FontFamily FontFamily = new FontFamily("Segoe UI");

		public static readonly FontStyle FontStyle = FontStyles.Normal;

		public static readonly FontStretch FontStretch = FontStretches.Normal;

		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter3<DependencyObject, Appearances, bool>((DependencyObject self, Appearances appearance, bool isEnabled) => self.FindResource(appearance switch
		{
			Appearances.HighEmphasis => isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockH2HighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockH2HighEmphasisForegroundDisabledBrush", 
			Appearances.LowEmphasis => isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockH2LowEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockH2LowEmphasisForegroundDisabledBrush", 
			Appearances.Primary => isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockH2PrimaryForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockH2PrimaryForegroundDisabledBrush", 
			_ => throw new ArgumentOutOfRangeException("Appearance", appearance, null), 
		}), "TextBrushConverter");

		public static IMultiValueConverter OpacityConverter { get; } = new LambdaConverter3<DependencyObject, Appearances, bool>(delegate(DependencyObject self, Appearances appearance, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTextBlockOpacityDisabled" : (appearance switch
			{
				Appearances.HighEmphasis => "VideoOSResourceKeysInternalVideoOSTextBlockOpacityHighEmphasis", 
				Appearances.LowEmphasis => "VideoOSResourceKeysInternalVideoOSTextBlockOpacityLowEmphasis", 
				Appearances.Primary => "VideoOSResourceKeysInternalVideoOSTextBlockOpacityEnabled", 
				_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
			}));
			return self.FindResource(resourceKey);
		}, "OpacityConverter");
	}

	public enum Appearances
	{
		HighEmphasis,
		LowEmphasis,
		Primary
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

	static VideoOSTextBlockH2()
	{
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSTextBlockH2), new PropertyMetadata(Appearances.HighEmphasis));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockH2), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockH2)));
		ThemeManager.InitializeThemingSystem();
	}
}
