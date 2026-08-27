using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSButtonPrimaryBase : VideoOSButtonBase
{
	public static class VideoOSButtonPrimaryBaseInternals
	{
		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter6<DependencyObject, bool, bool, bool, bool, bool>(delegate(DependencyObject self, bool forcePressedState, bool isDestructive, bool isMouseOver, bool isPressed, bool isEnabled)
		{
			string resourceKey = (isDestructive ? ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSButtonPrimaryDestructiveBackgroundDisabledBrush" : ((forcePressedState || isPressed) ? "VideoOSResourceKeysInternalVideoOSButtonPrimaryDestructiveBackgroundPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSButtonPrimaryDestructiveBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSButtonPrimaryDestructiveBackgroundHoveredBrush"))) : ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSButtonPrimaryNondestructiveBackgroundDisabledBrush" : ((forcePressedState || isPressed) ? "VideoOSResourceKeysInternalVideoOSButtonPrimaryNondestructiveBackgroundPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSButtonPrimaryNondestructiveBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSButtonPrimaryNondestructiveBackgroundHoveredBrush"))));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");
	}

	public static readonly DependencyProperty IsDestructiveProperty = DependencyProperty.Register("IsDestructive", typeof(bool), typeof(VideoOSButtonPrimaryBase), new PropertyMetadata(false));

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

	public VideoOSButtonPrimaryBase()
	{
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		BulletDecorator templateChild = this.GetTemplateChild<BulletDecorator>("PART_BulletDecorator");
		ThemeManager.Instance.ApplyThemeOnElement(templateChild, ThemeType.Dark);
	}
}
