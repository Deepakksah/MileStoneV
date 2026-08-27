using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public static class VideoOSStyleToolTip
{
	public static class VideoOSStyleToolTipInternals
	{
		public const string ToolTipStyleKey = "VideoOSStyleToolTipToolTipStyleKey";

		public const string ToolTipWithBorderStyleKey = "VideoOSStyleToolTipToolTipWithBorderStyleKey";

		public const string ToolTipContentTemplateSelectorKey = "VideoOSStyleToolTipToolTipContentTemplateSelectorKey";

		public const string TextTemplateKey = "VideoOSStyleToolTipTextTemplateKey";

		public const string CustomTemplateKey = "VideoOSStyleToolTipCustomTemplateKey";

		public static CornerRadius ToolTipCornerRadius = new CornerRadius(2.0);

		public const double ToolTipMaxWidth = 300.0;

		public static IMultiValueConverter ToolTipVisibleConverter { get; } = new LambdaConverter1<object>((object content) => (content == null || (content is string value && string.IsNullOrWhiteSpace(value))) ? ((object)Visibility.Hidden) : ((object)Visibility.Visible), "ToolTipVisibleConverter");
	}

	static VideoOSStyleToolTip()
	{
		ThemeManager.InitializeThemingSystem();
	}
}
