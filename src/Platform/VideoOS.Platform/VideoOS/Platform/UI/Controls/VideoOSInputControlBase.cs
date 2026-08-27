using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSInputControlBase : ContentControl
{
	public static class VideoOSInputControlBaseInternals
	{
		public static IValueConverter InlineMessageAppearanceConverter { get; } = new LambdaConverter1<MessageAppearances>((MessageAppearances messageAppearance) => messageAppearance switch
		{
			MessageAppearances.Information => VideoOSInlineMessageBase.Appearances.Information, 
			MessageAppearances.Confirmation => VideoOSInlineMessageBase.Appearances.Confirmation, 
			MessageAppearances.Warning => VideoOSInlineMessageBase.Appearances.Warning, 
			MessageAppearances.Error => VideoOSInlineMessageBase.Appearances.Error, 
			_ => throw new ArgumentOutOfRangeException("messageAppearance", messageAppearance, null), 
		}, "InlineMessageAppearanceConverter");

		public static IMultiValueConverter MessageVisibilityConverter { get; } = new LambdaConverter2<string, int>((string message, int minLines) => (string.IsNullOrWhiteSpace(message) && minLines == 0) ? Visibility.Collapsed : Visibility.Visible, "MessageVisibilityConverter");
	}

	public enum MessageAppearances
	{
		Information,
		Confirmation,
		Warning,
		Error
	}

	public static readonly DependencyProperty LabelProperty;

	public static readonly DependencyProperty MessageProperty;

	public static readonly DependencyProperty MessageAppearanceProperty;

	public static readonly DependencyProperty MessageMinLinesProperty;

	public static readonly DependencyProperty MessageMaxLinesProperty;

	public string Label
	{
		get
		{
			return (string)GetValue(LabelProperty);
		}
		set
		{
			SetValue(LabelProperty, value);
		}
	}

	public string Message
	{
		get
		{
			return (string)GetValue(MessageProperty);
		}
		set
		{
			SetValue(MessageProperty, value);
		}
	}

	public MessageAppearances MessageAppearance
	{
		get
		{
			return (MessageAppearances)GetValue(MessageAppearanceProperty);
		}
		set
		{
			SetValue(MessageAppearanceProperty, value);
		}
	}

	public int MessageMinLines
	{
		get
		{
			return (int)GetValue(MessageMinLinesProperty);
		}
		set
		{
			SetValue(MessageMinLinesProperty, value);
		}
	}

	public int MessageMaxLines
	{
		get
		{
			return (int)GetValue(MessageMaxLinesProperty);
		}
		set
		{
			SetValue(MessageMaxLinesProperty, value);
		}
	}

	static VideoOSInputControlBase()
	{
		LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(VideoOSInputControlBase), new PropertyMetadata(null));
		MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(VideoOSInputControlBase), new PropertyMetadata(null));
		MessageAppearanceProperty = DependencyProperty.Register("MessageAppearance", typeof(MessageAppearances), typeof(VideoOSInputControlBase), new PropertyMetadata(MessageAppearances.Error));
		MessageMinLinesProperty = DependencyProperty.Register("MessageMinLines", typeof(int), typeof(VideoOSInputControlBase), new PropertyMetadata(0));
		MessageMaxLinesProperty = DependencyProperty.Register("MessageMaxLines", typeof(int), typeof(VideoOSInputControlBase), new PropertyMetadata(int.MaxValue));
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSInputControlBase), new FrameworkPropertyMetadata(false));
	}

	public VideoOSInputControlBase()
	{
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSInputControlBaseAutomationPeer(this);
	}
}
