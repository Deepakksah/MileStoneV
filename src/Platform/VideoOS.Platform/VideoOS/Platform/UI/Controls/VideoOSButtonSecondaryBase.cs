using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSButtonSecondaryBase : VideoOSButtonBase
{
	public static class VideoOSButtonSecondaryBaseInternals
	{
		public static IMultiValueConverter OutlineBrushConverter { get; } = new LambdaConverter6<DependencyObject, bool, bool, bool, bool, bool>(delegate(DependencyObject self, bool forcePressedState, bool isDestructive, bool isMouseOver, bool isPressed, bool isEnabled)
		{
			string resourceKey = (isDestructive ? ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSButtonSecondaryDestructiveOutlineDisabledBrush" : ((forcePressedState || isPressed) ? "VideoOSResourceKeysInternalVideoOSButtonSecondaryDestructiveOutlinePressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSButtonSecondaryDestructiveOutlineIdleBrush" : "VideoOSResourceKeysInternalVideoOSButtonSecondaryDestructiveOutlineHoveredBrush"))) : ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSButtonSecondaryNondestructiveOutlineDisabledBrush" : ((forcePressedState || isPressed) ? "VideoOSResourceKeysInternalVideoOSButtonSecondaryNondestructiveOutlinePressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSButtonSecondaryNondestructiveOutlineIdleBrush" : "VideoOSResourceKeysInternalVideoOSButtonSecondaryNondestructiveOutlineHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "OutlineBrushConverter");
	}

	public static readonly DependencyProperty IsDestructiveProperty = DependencyProperty.Register("IsDestructive", typeof(bool), typeof(VideoOSButtonSecondaryBase), new PropertyMetadata(false));

	public bool IsDestructive
	{
		get
		{
			return (bool)GetValue(IsDestructiveProperty);
		}
		set
		{
			SetValue(IsDestructiveProperty, value);
		}
	}

	public VideoOSButtonSecondaryBase()
	{
	}
}
