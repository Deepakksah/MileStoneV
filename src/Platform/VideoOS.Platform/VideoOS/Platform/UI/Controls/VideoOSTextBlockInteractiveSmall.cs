using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTextBlockInteractiveSmall : VideoOSTextBlockBase
{
	public static class VideoOSTextBlockInteractiveSmallInternals
	{
		public const double FontSize = 12.0;

		public static readonly FontWeight FontWeight = FontWeights.SemiBold;

		public static readonly FontFamily FontFamily = new FontFamily("Segoe UI");

		public static readonly FontStyle FontStyle = FontStyles.Normal;

		public static readonly FontStretch FontStretch = FontStretches.Normal;

		public static IMultiValueConverter TextBrushConverter { get; } = new LambdaConverter4<DependencyObject, Appearances, InteractiveStates, bool>(delegate(DependencyObject self, Appearances appearance, InteractiveStates interactiveState, bool isEnabled)
		{
			string resourceKey;
			switch (appearance)
			{
			case Appearances.HighEmphasis:
				resourceKey = (isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallHighEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallHighEmphasisForegroundDisabledBrush");
				break;
			case Appearances.LowEmphasis:
				resourceKey = (isEnabled ? "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallLowEmphasisForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallLowEmphasisForegroundDisabledBrush");
				break;
			case Appearances.Primary:
			case Appearances.PrimaryUnderlined:
				resourceKey = (isEnabled ? (interactiveState switch
				{
					InteractiveStates.Idle => "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundIdleBrush", 
					InteractiveStates.Hovered => "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundHoveredBrush", 
					InteractiveStates.Pressed => "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundPressedBrush", 
					_ => throw new ArgumentOutOfRangeException("interactiveState", interactiveState, null), 
				}) : "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundDisabledBrush");
				break;
			case Appearances.Destructive:
				resourceKey = (isEnabled ? (interactiveState switch
				{
					InteractiveStates.Idle => "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallDestructiveForegroundIdleBrush", 
					InteractiveStates.Hovered => "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallDestructiveForegroundHoveredBrush", 
					InteractiveStates.Pressed => "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallDestructiveForegroundPressedBrush", 
					_ => throw new ArgumentOutOfRangeException("interactiveState", interactiveState, null), 
				}) : "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallDestructiveForegroundDisabledBrush");
				break;
			default:
				throw new ArgumentOutOfRangeException("appearance", appearance, null);
			}
			return self.FindResource(resourceKey);
		}, "TextBrushConverter");

		public static IValueConverter TextDecorationsConverter { get; } = new LambdaConverter1<Appearances>(delegate(Appearances appearance)
		{
			TextDecorationCollection textDecorationCollection = new TextDecorationCollection();
			if (appearance == Appearances.PrimaryUnderlined)
			{
				textDecorationCollection.Add(System.Windows.TextDecorations.Underline);
			}
			return textDecorationCollection;
		}, "TextDecorationsConverter");

		public static IMultiValueConverter OpacityConverter { get; } = new LambdaConverter3<DependencyObject, Appearances, bool>(delegate(DependencyObject self, Appearances appearance, bool isEnabled)
		{
			string resourceKey;
			if (!isEnabled)
			{
				resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockOpacityDisabled";
			}
			else
			{
				switch (appearance)
				{
				case Appearances.HighEmphasis:
					resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockOpacityHighEmphasis";
					break;
				case Appearances.LowEmphasis:
					resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockOpacityLowEmphasis";
					break;
				case Appearances.Primary:
				case Appearances.PrimaryUnderlined:
				case Appearances.Destructive:
					resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockOpacityEnabled";
					break;
				default:
					throw new ArgumentOutOfRangeException("appearance", appearance, null);
				}
			}
			return self.FindResource(resourceKey);
		}, "OpacityConverter");
	}

	public enum Appearances
	{
		HighEmphasis,
		LowEmphasis,
		Primary,
		PrimaryUnderlined,
		Destructive
	}

	public enum InteractiveStates
	{
		Idle,
		Hovered,
		Pressed
	}

	public static readonly DependencyProperty AppearanceProperty;

	public static readonly DependencyProperty InteractiveStateProperty;

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

	public InteractiveStates InteractiveState
	{
		get
		{
			return (InteractiveStates)GetValue(InteractiveStateProperty);
		}
		set
		{
			SetValue(InteractiveStateProperty, value);
		}
	}

	static VideoOSTextBlockInteractiveSmall()
	{
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSTextBlockInteractiveSmall), new PropertyMetadata(Appearances.Primary));
		InteractiveStateProperty = DependencyProperty.Register("InteractiveState", typeof(InteractiveStates), typeof(VideoOSTextBlockInteractiveSmall), new PropertyMetadata(InteractiveStates.Idle));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockInteractiveSmall), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockInteractiveSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
