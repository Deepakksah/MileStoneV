using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSButtonAccentBase : VideoOSButtonBase
{
	public static class VideoOSButtonAccentBaseInternals
	{
		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, bool>(delegate(DependencyObject self, bool forcePressedState, bool isMouseOver, bool isPressed, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSButtonAccentBackgroundDisabledBrush" : ((forcePressedState || isPressed) ? "VideoOSResourceKeysInternalVideoOSButtonAccentBackgroundPressedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSButtonAccentBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSButtonAccentBackgroundHoveredBrush")));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");
	}

	public VideoOSButtonAccentBase()
	{
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		BulletDecorator templateChild = this.GetTemplateChild<BulletDecorator>("PART_BulletDecorator");
		ThemeManager.Instance.ApplyThemeOnElement(templateChild, ThemeType.Light);
	}
}
