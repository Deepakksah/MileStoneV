using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSProgressBar : ProgressBar
{
	public static class VideoOSProgressBarInternals
	{
		public static IMultiValueConverter IndeterminateBorderWidthConverter { get; } = new LambdaConverter1<double>((double progressBarWidth) => CalculateIndeterminateBorderWidth(progressBarWidth), "IndeterminateBorderWidthConverter");

		public static IMultiValueConverter IndeterminateBorderMarginConverter { get; } = new LambdaConverter2<object, double>((object animationValue, double progressbarWidth) => (animationValue is double num) ? ((object)new Thickness(num * (progressbarWidth - CalculateIndeterminateBorderWidth(progressbarWidth)), 0.0, 0.0, 0.0)) : Binding.DoNothing, "IndeterminateBorderMarginConverter");

		private static double CalculateIndeterminateBorderWidth(double progressBarWidth)
		{
			return progressBarWidth / 4.0;
		}
	}

	static VideoOSProgressBar()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSProgressBar), new FrameworkPropertyMetadata(typeof(VideoOSProgressBar)));
		ThemeManager.InitializeThemingSystem();
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSProgressBar), new FrameworkPropertyMetadata(false));
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSProgressBarAutomationPeer(this);
	}
}
