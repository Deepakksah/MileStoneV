using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSInlineMessageMedium : VideoOSInlineMessageBase
{
	public static class VideoOSInlineMessageMediumInternals
	{
		public static IMultiValueConverter InlineMessageTextAppearanceConverter { get; } = new LambdaConverter2<Appearances, bool>((Appearances appearance, bool attenuateMessageText) => (appearance == Appearances.Error && !attenuateMessageText) ? ((object)VideoOSTextBlockBodyMedium.Appearances.Destructive) : ((object)VideoOSTextBlockBodyMedium.Appearances.HighEmphasis), "InlineMessageTextAppearanceConverter");
	}

	static VideoOSInlineMessageMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSInlineMessageMedium), new FrameworkPropertyMetadata(typeof(VideoOSInlineMessageMedium)));
		ThemeManager.InitializeThemingSystem();
	}
}
