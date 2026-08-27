using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSInlineMessageBase : ContentControl, INotifyPropertyChanged
{
	public static class VideoOSInlineMessageBaseInternals
	{
		public const string VideoOSTextBlockInteractiveMediumLinkLabelStyleKey = "VideoOSInlineMessageBaseVideoOSTextBlockInteractiveMediumLinkLabelStyleKey";

		public const string VideoOSTextBlockInteractiveSmallLinkLabelStyleKey = "VideoOSInlineMessageBaseVideoOSTextBlockInteractiveSmallLinkLabelStyleKey";

		public static IValueConverter InlineMessageIconSourceConverter { get; } = new LambdaConverter1<Appearances>((Appearances appearance) => appearance switch
		{
			Appearances.Error => new VideoOSIconBuiltInSource
			{
				Icon = VideoOSIconBuiltInSource.Icons.Error_Combined
			}, 
			Appearances.Warning => new VideoOSIconBuiltInSource
			{
				Icon = VideoOSIconBuiltInSource.Icons.Warning_Combined
			}, 
			Appearances.Confirmation => new VideoOSIconBuiltInSource
			{
				Icon = VideoOSIconBuiltInSource.Icons.Confirmation_Combined
			}, 
			Appearances.Information => new VideoOSIconBuiltInSource
			{
				Icon = VideoOSIconBuiltInSource.Icons.Information_Combined
			}, 
			_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
		}, "InlineMessageIconSourceConverter");

		public static IMultiValueConverter InlineMessageSmallEllipsisForegroundConverter { get; } = new LambdaConverter5<DependencyObject, VideoOSTextBlockBodySmall, string, string, bool>((DependencyObject self, VideoOSTextBlockBodySmall textBlock, string text, string linkLabel, bool isEnabled) => ResolveEllipsisForegroundBrush(self, textBlock, text, linkLabel, isEnabled, (textBlock.Appearance == VideoOSTextBlockBodySmall.Appearances.Destructive) ? "VideoOSResourceKeysInternalVideoOSTextBlockBodySmallDestructiveForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodySmallHighEmphasisForegroundIdleBrush", (textBlock.Appearance == VideoOSTextBlockBodySmall.Appearances.Destructive) ? "VideoOSResourceKeysInternalVideoOSTextBlockBodySmallDestructiveForegroundDisabledBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodySmallHighEmphasisForegroundDisabledBrush", "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundIdleBrush", "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundDisabledBrush"), "InlineMessageSmallEllipsisForegroundConverter");

		public static IMultiValueConverter InlineMessageSmallLinkTextForegroundConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isEnabled, bool isMouseOver, bool isMouseLeftButtonDown)
		{
			string resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundIdleBrush";
			if (!isEnabled)
			{
				resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundDisabledBrush";
			}
			else if (isMouseOver)
			{
				resourceKey = (isMouseLeftButtonDown ? "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundPressedBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveSmallPrimaryForegroundHoveredBrush");
			}
			return self.FindResource(resourceKey);
		}, "InlineMessageSmallLinkTextForegroundConverter");

		public static IMultiValueConverter InlineMessageMediumEllipsisForegroundConverter { get; } = new LambdaConverter5<DependencyObject, VideoOSTextBlockBodyMedium, string, string, bool>((DependencyObject self, VideoOSTextBlockBodyMedium textBlock, string text, string linkLabel, bool isEnabled) => ResolveEllipsisForegroundBrush(self, textBlock, text, linkLabel, isEnabled, (textBlock.Appearance == VideoOSTextBlockBodyMedium.Appearances.Destructive) ? "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumDestructiveForegroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumHighEmphasisForegroundIdleBrush", (textBlock.Appearance == VideoOSTextBlockBodyMedium.Appearances.Destructive) ? "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumDestructiveForegroundDisabledBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockBodyMediumHighEmphasisForegroundDisabledBrush", "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveMediumPrimaryForegroundIdleBrush", "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveMediumPrimaryForegroundDisabledBrush"), "InlineMessageMediumEllipsisForegroundConverter");

		public static IMultiValueConverter InlineMessageMediumLinkTextForegroundConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isEnabled, bool isMouseOver, bool isMouseLeftButtonDown)
		{
			string resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveMediumPrimaryForegroundIdleBrush";
			if (!isEnabled)
			{
				resourceKey = "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveMediumPrimaryForegroundDisabledBrush";
			}
			else if (isMouseOver)
			{
				resourceKey = (isMouseLeftButtonDown ? "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveMediumPrimaryForegroundPressedBrush" : "VideoOSResourceKeysInternalVideoOSTextBlockInteractiveMediumPrimaryForegroundHoveredBrush");
			}
			return self.FindResource(resourceKey);
		}, "InlineMessageMediumLinkTextForegroundConverter");
	}

	public enum Appearances
	{
		Error,
		Warning,
		Confirmation,
		Information
	}

	public static readonly DependencyProperty AppearanceProperty;

	public static readonly DependencyProperty MinLinesProperty;

	public static readonly DependencyProperty MaxLinesProperty;

	public static readonly DependencyProperty LinkTextProperty;

	public static readonly RoutedEvent LinkClickedEvent;

	private double __contentOffset;

	public static readonly DependencyProperty AttenuateMessageTextProperty;

	public static readonly DependencyProperty LinkTextClickCommandProperty;

	private VideoOSTextBlockBase _messageTextBlock;

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

	public string LinkText
	{
		get
		{
			return (string)GetValue(LinkTextProperty);
		}
		set
		{
			SetValue(LinkTextProperty, value);
		}
	}

	public double ContentOffset
	{
		get
		{
			return __contentOffset;
		}
		private set
		{
			if (Math.Abs(value - __contentOffset) >= double.Epsilon)
			{
				__contentOffset = value;
				OnPropertyChanged("ContentOffset");
			}
		}
	}

	public bool AttenuateMessageText
	{
		get
		{
			return (bool)GetValue(AttenuateMessageTextProperty);
		}
		set
		{
			SetValue(AttenuateMessageTextProperty, value);
		}
	}

	public DelegateCommand LinkTextClickCommand
	{
		get
		{
			return (DelegateCommand)GetValue(LinkTextClickCommandProperty);
		}
		private set
		{
			SetValue(LinkTextClickCommandProperty, value);
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public event RoutedEventHandler LinkClicked
	{
		add
		{
			AddHandler(LinkClickedEvent, value);
		}
		remove
		{
			RemoveHandler(LinkClickedEvent, value);
		}
	}

	static VideoOSInlineMessageBase()
	{
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSInlineMessageBase), new PropertyMetadata(Appearances.Error));
		MinLinesProperty = DependencyProperty.Register("MinLines", typeof(int), typeof(VideoOSInlineMessageBase), new PropertyMetadata(1));
		MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(VideoOSInlineMessageBase), new PropertyMetadata(int.MaxValue));
		LinkTextProperty = DependencyProperty.Register("LinkText", typeof(string), typeof(VideoOSInlineMessageBase), new PropertyMetadata((object)null));
		LinkClickedEvent = EventManager.RegisterRoutedEvent("LinkClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSInlineMessageBase));
		AttenuateMessageTextProperty = DependencyProperty.Register("AttenuateMessageText", typeof(bool), typeof(VideoOSInlineMessageBase), new PropertyMetadata(false));
		LinkTextClickCommandProperty = DependencyProperty.Register("LinkTextClickCommand", typeof(DelegateCommand), typeof(VideoOSInlineMessageBase), new PropertyMetadata((object)null));
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSInlineMessageBase), new FrameworkPropertyMetadata(false));
	}

	public VideoOSInlineMessageBase()
	{
		LinkTextClickCommand = new DelegateCommand(delegate
		{
			RaiseEvent(new RoutedEventArgs(LinkClickedEvent));
		});
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_messageTextBlock = this.GetTemplateChild<VideoOSTextBlockBase>("PART_MessageTextBlock");
		_messageTextBlock.Loaded += delegate
		{
			ContentOffset = _messageTextBlock.TranslatePoint(new Point(0.0, 0.0), this).X;
		};
	}

	public double GetUnconstrainedWidth(string message = null)
	{
		string text = (base.Content as string) ?? string.Empty;
		if (!string.IsNullOrWhiteSpace(LinkText))
		{
			text += LinkText;
		}
		return (_messageTextBlock?.MeasureString(message ?? text).Width ?? 0.0) + 24.0;
	}

	public bool WouldTextBeConstrained(int allowedLineCount)
	{
		return _messageTextBlock?.WouldTextBeConstrained(allowedLineCount) ?? false;
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private static Brush ResolveEllipsisForegroundBrush(DependencyObject self, TextBlock textBlock, string text, string linkLabel, bool isEnabled, string textEllipsisResourceName, string textEllipsisDisabledResourceName, string linkLabelEllipsisResourceName, string linkLabelEllipsisDisabledResourceName)
	{
		if (string.IsNullOrWhiteSpace(linkLabel))
		{
			return (Brush)self.FindResource(isEnabled ? textEllipsisResourceName : textEllipsisDisabledResourceName);
		}
		Size size = textBlock.MeasureString(text + "...");
		bool flag = textBlock.ActualWidth < size.Width;
		if (textBlock.TextWrapping != TextWrapping.NoWrap && size.Height < textBlock.ActualHeight)
		{
			flag = VideoOSTextBlockBase.IsWrappingTextConstrained(textBlock, textBlock.ActualHeight, text);
		}
		if (flag)
		{
			return (Brush)self.FindResource(isEnabled ? textEllipsisResourceName : textEllipsisDisabledResourceName);
		}
		return (Brush)self.FindResource(isEnabled ? linkLabelEllipsisResourceName : linkLabelEllipsisDisabledResourceName);
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSInlineMessageBaseAutomationPeer(this);
	}
}
