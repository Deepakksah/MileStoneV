using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSButtonTransparentBase : VideoOSButtonBase
{
	public static class VideoOSButtonTransparentBaseInternals
	{
		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool forcePressedState, bool isMouseOver, bool isPressed, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSButtonTransparentBackgroundDisabledBrush" : ((forcePressedState || isPressed) ? "VideoOSResourceKeysInternalVideoOSButtonTransparentBackgroundPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSButtonTransparentBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSButtonTransparentBackgroundHoveredBrush")));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");
	}

	public VideoOSButtonTransparentBase()
	{
	}
}
