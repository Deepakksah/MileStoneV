using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSInlineMessageSmall : VideoOSInlineMessageBase
{
	public static class VideoOSInlineMessageSmallInternals
	{
		public static IMultiValueConverter InlineMessageTextAppearanceConverter { get; } = new LambdaConverter2<Appearances, bool>((Appearances appearance, bool attenuateMessageText) => (appearance == Appearances.Error && !attenuateMessageText) ? ((object)VideoOSTextBlockBodySmall.Appearances.Destructive) : ((object)VideoOSTextBlockBodySmall.Appearances.HighEmphasis), "InlineMessageTextAppearanceConverter");
	}

	static VideoOSInlineMessageSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSInlineMessageSmall), new FrameworkPropertyMetadata(typeof(VideoOSInlineMessageSmall)));
		ThemeManager.InitializeThemingSystem();
	}
}
