using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTextBlockH3 : VideoOSTextBlockBase
{
	public static class VideoOSTextBlockH3Internals
	{
		public const double FontSize = 11.0;

		public static readonly FontWeight FontWeight = FontWeights.SemiBold;

		public static readonly FontFamily FontFamily = new FontFamily("Segoe UI");

		public static readonly FontStyle FontStyle = FontStyles.Normal;

		public static readonly FontStretch FontStretch = FontStretches.Normal;

		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter3<DependencyObject, Appearances, bool>((DependencyObject self, Appearances appearance, bool isEnabled) => self.FindResource(appearance switch
		{
			Appearances.HighEmphasis => isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockH3HighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockH3HighEmphasisForegroundDisabledBrush", 
			Appearances.LowEmphasis => isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockH3LowEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockH3LowEmphasisForegroundDisabledBrush", 
			_ => throw new ArgumentOutOfRangeException("Appearance", appearance, null), 
		}), "TextBrushConverter");

		public static IMultiValueConverter OpacityConverter { get; } = new LambdaConverter3<DependencyObject, Appearances, bool>(delegate(DependencyObject self, Appearances appearance, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTextBlockOpacityDisabled" : (appearance switch
			{
				Appearances.HighEmphasis => "VideoOSResourceKeysInternalVideoOSTextBlockOpacityHighEmphasis", 
				Appearances.LowEmphasis => "VideoOSResourceKeysInternalVideoOSTextBlockOpacityLowEmphasis", 
				_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
			}));
			return self.FindResource(resourceKey);
		}, "OpacityConverter");
	}

	public enum Appearances
	{
		HighEmphasis,
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

	static VideoOSTextBlockH3()
	{
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSTextBlockH3), new PropertyMetadata(Appearances.HighEmphasis));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockH3), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockH3)));
		ThemeManager.InitializeThemingSystem();
	}

	public VideoOSTextBlockH3()
	{
		base.Loaded += VideoOSTextBlockH3_Loaded;
		base.Unloaded += VideoOSTextBlockH3_Unloaded;
	}

	private void VideoOSTextBlockH3_Loaded(object sender, RoutedEventArgs e)
	{
		AddTextValueChangedHandler();
		TextValueChangedHandler(null, EventArgs.Empty);
	}

	private void VideoOSTextBlockH3_Unloaded(object sender, RoutedEventArgs e)
	{
		RemoveTextValueChangedHandler();
	}

	private void AddTextValueChangedHandler()
	{
		RemoveTextValueChangedHandler();
		DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock)).AddValueChanged(this, TextValueChangedHandler);
	}

	private void RemoveTextValueChangedHandler()
	{
		DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock)).RemoveValueChanged(this, TextValueChangedHandler);
	}

	private void TextValueChangedHandler(object sender, EventArgs e)
	{
		if (base.Text != null)
		{
			string text = base.Text.ToUpperCustom(CultureInfo.CurrentUICulture);
			if (base.Text != text)
			{
				SetCurrentValue(TextBlock.TextProperty, text);
			}
		}
	}
}
