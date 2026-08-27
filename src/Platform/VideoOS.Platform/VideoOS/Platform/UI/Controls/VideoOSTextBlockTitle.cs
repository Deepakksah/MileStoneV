using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTextBlockTitle : VideoOSTextBlockBase
{
	public static class VideoOSTextBlockTitleInternals
	{
		public const double FontSize = 24.0;

		public static readonly FontWeight FontWeight = FontWeights.SemiBold;

		public static readonly FontFamily FontFamily = new FontFamily("Segoe UI");

		public static readonly FontStyle FontStyle = FontStyles.Normal;

		public static readonly FontStretch FontStretch = FontStretches.Normal;

		public static IMultiValueConverter OpacityConverter { get; } = new LambdaConverter3<DependencyObject, Appearances, bool>(delegate(DependencyObject self, Appearances appearance, bool isEnabled)
		{
			string resourceKey;
			if (!isEnabled)
			{
				resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockOpacityDisabled";
			}
			else
			{
				if (appearance != Appearances.HighEmphasis)
				{
					throw new ArgumentOutOfRangeException("appearance", appearance, null);
				}
				resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockOpacityHighEmphasis";
			}
			return self.FindResource(resourceKey);
		}, "OpacityConverter");
	}

	public enum Appearances
	{
		HighEmphasis
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

	static VideoOSTextBlockTitle()
	{
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSTextBlockTitle), new PropertyMetadata(Appearances.HighEmphasis));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockTitle), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockTitle)));
		ThemeManager.InitializeThemingSystem();
	}
}
