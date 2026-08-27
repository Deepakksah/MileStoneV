using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSIcon : Control
{
	public static class VideoOSIconInternals
	{
		public const string PathContentKey = "VideoOSIconPathContentKey";

		public const string BitmapTemplateKey = "VideoOSIconBitmapTemplateKey";

		public const string ResourceTemplateKey = "VideoOSIconResourceTemplateKey";

		public const string UriTemplateKey = "VideoOSIconUriTemplateKey";

		public const string PathTemplateKey = "VideoOSIconPathTemplateKey";

		public const string MultiPathTemplateKey = "VideoOSIconMultiPathTemplateKey";

		public const string IconContentDataTemplateSelectorKey = "VideoOSIconIconContentDataTemplateSelectorKey";

		public static IMultiValueConverter FlowDirectionConverter { get; } = new LambdaConverter2<bool, FlowDirection>((bool followFlowDirection, FlowDirection iconFlowDirection) => followFlowDirection ? ((object)iconFlowDirection) : ((object)FlowDirection.LeftToRight), "FlowDirectionConverter");

		public static IMultiValueConverter ContentConverter { get; } = new LambdaConverter3<DependencyObject, VideoOSIconSourceBase, FlowDirection>((DependencyObject self, VideoOSIconSourceBase iconSource, FlowDirection flowDirection) => (iconSource is VideoOSIconBuiltInSource videoOSIconBuiltInSource) ? videoOSIconBuiltInSource.ResolveIconSource(flowDirection, self) : iconSource, "ContentConverter");

		public static IMultiValueConverter OpacityConverter { get; } = new LambdaConverter3<DependencyObject, bool, Appearances>((DependencyObject self, bool isEnabled, Appearances appearance) => (!isEnabled) ? ((double)self.FindResource(VideoOSResourceKeys.IconOpacityDisabled)) : ((appearance == Appearances.HighEmphasis) ? ((double)self.FindResource(VideoOSResourceKeys.IconOpacityEnabledHighEmphasis)) : ((double)self.FindResource(VideoOSResourceKeys.IconOpacityEnabledLowEmphasis))), "OpacityConverter");

		public static IMultiValueConverter AutoColorBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, Brush, Brush>((DependencyObject self, bool applyAutoColor, Brush autoBrush, Brush nonAutoBrush) => applyAutoColor ? ((autoBrush != null) ? autoBrush : ((Brush)self.FindResource("VideoOSResourceKeysInternalVideoOSIconAutoColorIdleBrush"))) : nonAutoBrush, "AutoColorBrushConverter");

		public static IMultiValueConverter CharacterToStringConverter { get; } = new LambdaConverter1<char>((char character) => character.ToString(CultureInfo.InvariantCulture), "CharacterToStringConverter");
	}

	public enum Appearances
	{
		HighEmphasis,
		LowEmphasis
	}

	public static readonly DependencyProperty IconSourceProperty;

	public static readonly DependencyProperty AppearanceProperty;

	public static readonly DependencyProperty AutoColorBrushProperty;

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return (VideoOSIconSourceBase)GetValue(IconSourceProperty);
		}
		set
		{
			SetValue(IconSourceProperty, value);
		}
	}

	public Appearances Appearance
	{
		get
		{
			return (Appearances)GetValue(AppearanceProperty);
		}
		set
		{
			SetValue(AppearanceProperty, value);
		}
	}

	public Brush AutoColorBrush
	{
		get
		{
			return (Brush)GetValue(AutoColorBrushProperty);
		}
		set
		{
			SetValue(AutoColorBrushProperty, value);
		}
	}

	static VideoOSIcon()
	{
		IconSourceProperty = DependencyProperty.Register("IconSource", typeof(VideoOSIconSourceBase), typeof(VideoOSIcon), new PropertyMetadata(null));
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSIcon), new PropertyMetadata(Appearances.HighEmphasis));
		AutoColorBrushProperty = DependencyProperty.Register("AutoColorBrush", typeof(Brush), typeof(VideoOSIcon), new PropertyMetadata(null));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSIcon), new FrameworkPropertyMetadata(typeof(VideoOSIcon)));
		ThemeManager.InitializeThemingSystem();
		UIElement.FocusableProperty.OverrideMetadata(typeof(VideoOSIcon), new FrameworkPropertyMetadata(false));
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSIconAutomationPeer(this);
	}
}
