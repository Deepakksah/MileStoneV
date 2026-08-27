using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTextBlockEmptyState : VideoOSTextBlockBase
{
	public static class VideoOSTextBlockEmptyStateInternals
	{
		public const double FontSize = 16.0;

		public static readonly FontWeight FontWeight = FontWeights.Light;

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
				if (appearance != Appearances.LowEmphasis)
				{
					throw new ArgumentOutOfRangeException("appearance", appearance, null);
				}
				resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockOpacityLowEmphasis";
			}
			return self.FindResource(resourceKey);
		}, "OpacityConverter");
	}

	public enum Appearances
	{
		LowEmphasis
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

	static VideoOSTextBlockEmptyState()
	{
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSTextBlockEmptyState), new PropertyMetadata(Appearances.LowEmphasis));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockEmptyState), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockEmptyState)));
		ThemeManager.InitializeThemingSystem();
	}
}
