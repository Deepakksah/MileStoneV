using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSButtonTertiaryBase : VideoOSButtonBase
{
	public static class VideoOSButtonTertiaryBaseInternals
	{
		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool forcePressedState, bool isMouseOver, bool isPressed, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSButtonTertiaryBackgroundDisabledBrush" : ((forcePressedState || isPressed) ? "VideoOSResourceKeysInternalVideoOSButtonTertiaryBackgroundPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSButtonTertiaryBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSButtonTertiaryBackgroundHoveredBrush")));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");
	}

	public VideoOSButtonTertiaryBase()
	{
	}
}
