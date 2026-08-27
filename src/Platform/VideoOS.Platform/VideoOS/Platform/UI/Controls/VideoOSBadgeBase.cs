using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Resources;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSBadgeBase : ContentControl
{
	public static class VideoOSBadgeBaseInternals
	{
		public static IMultiValueConverter BadgeTextConverter { get; } = new LambdaConverter2<int, int>(delegate(int count, int maxDisplayCount)
		{
			if (count < 0)
			{
				return string.Empty;
			}
			return (maxDisplayCount >= 0 && count > maxDisplayCount) ? string.Format(CultureInfo.CurrentCulture, Strings.VideoOSBadge_CountExceededLimitedText, maxDisplayCount) : count.ToString(CultureInfo.CurrentCulture);
		}, "BadgeTextConverter");
	}

	public static readonly DependencyProperty CountProperty;

	public static readonly DependencyProperty MaxDisplayCountProperty;

	public int Count
	{
		get
		{
			return (int)GetValue(CountProperty);
		}
		set
		{
			SetValue(CountProperty, value);
		}
	}

	public int MaxDisplayCount
	{
		get
		{
			return (int)GetValue(MaxDisplayCountProperty);
		}
		set
		{
			SetValue(MaxDisplayCountProperty, value);
		}
	}

	static VideoOSBadgeBase()
	{
		CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(VideoOSBadgeBase), new PropertyMetadata(-1));
		MaxDisplayCountProperty = DependencyProperty.Register("MaxDisplayCount", typeof(int), typeof(VideoOSBadgeBase), new PropertyMetadata(-1));
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSBadgeBase), new FrameworkPropertyMetadata(false));
	}

	public VideoOSBadgeBase()
	{
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSBadgeBaseAutomationPeer(this);
	}
}
