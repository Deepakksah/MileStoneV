using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.Popups;

public partial class PopoverContent : Grid, IComponentConnector
{
	private class AnchorInfo
	{
		public FrameworkElement Anchor { get; }

		public FrameworkElement AnchorShadow { get; }

		public CustomPopupPlacement PopupPlacement { get; set; }

		public AnchorInfo(FrameworkElement anchor, FrameworkElement anchorShadow)
		{
			Anchor = anchor;
			AnchorShadow = anchorShadow;
		}
	}

	public static class PopoverContentInternals
	{
		public const string AnchorStyleKey = "PopoverContentAnchorStyleKey";

		public const string AnchorShadowStyleKey = "PopoverContentAnchorShadowStyleKey";

		public const string ContentTemplateSelectorKey = "PopoverContentContentTemplateSelectorKey";

		public const string TextTemplateKey = "PopoverContentTextTemplateKey";

		public const string CustomTemplateKey = "PopoverContentCustomTemplateKey";

		public const double TextPopupMaxWidth = 300.0;

		public static IMultiValueConverter BackgroundConverter { get; } = new LambdaConverter2<DependencyObject, VideoOSPopover.Appearances>((DependencyObject self, VideoOSPopover.Appearances appearance) => self.FindResource(appearance switch
		{
			VideoOSPopover.Appearances.PrimaryDark => "VideoOSResourceKeysInternalVideoOSPopoverPrimaryDarkBackgroundBrush", 
			VideoOSPopover.Appearances.PrimaryLight => "VideoOSResourceKeysInternalVideoOSPopoverPrimaryLightBackgroundBrush", 
			VideoOSPopover.Appearances.Accent => "VideoOSResourceKeysInternalVideoOSPopoverAccentBackgroundBrush", 
			VideoOSPopover.Appearances.Neutral => "VideoOSResourceKeysInternalVideoOSPopoverNeutralBackgroundBrush", 
			_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
		}), "BackgroundConverter");

		public static IMultiValueConverter MaxWidthConverter { get; } = new LambdaConverter1<object>((object content) => (content is string || content is TextBlock) ? ((object)300.0) : DependencyProperty.UnsetValue, "MaxWidthConverter");

		public static IMultiValueConverter TextAlignmentConverter { get; } = new LambdaConverter1<VideoOSPopover.TextAlignments>((VideoOSPopover.TextAlignments textAlignment) => textAlignment switch
		{
			VideoOSPopover.TextAlignments.Left => System.Windows.TextAlignment.Left, 
			VideoOSPopover.TextAlignments.Center => System.Windows.TextAlignment.Center, 
			VideoOSPopover.TextAlignments.Right => System.Windows.TextAlignment.Right, 
			_ => throw new ArgumentOutOfRangeException("textAlignment", textAlignment, null), 
		}, "TextAlignmentConverter");
	}

	public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register("Appearance", typeof(VideoOSPopover.Appearances), typeof(PopoverContent), new PropertyMetadata(VideoOSPopover.Appearances.PrimaryDark, AppearanceChangedCallback));

	public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(VideoOSPopover.TextAlignments), typeof(PopoverContent), new PropertyMetadata(VideoOSPopover.TextAlignments.Left));

	public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(PopoverContent), new PropertyMetadata(null));

	private readonly Popup _hostPopup;

	private AnchorInfo _topLeftAnchorInfo;

	private AnchorInfo _topCenterAnchorInfo;

	private AnchorInfo _topRightAnchorInfo;

	private AnchorInfo _leftTopAnchorInfo;

	private AnchorInfo _leftCenterAnchorInfo;

	private AnchorInfo _leftBottomAnchorInfo;

	private AnchorInfo _bottomRightAnchorInfo;

	private AnchorInfo _bottomCenterAnchorInfo;

	private AnchorInfo _bottomLeftAnchorInfo;

	private AnchorInfo _rightBottomAnchorInfo;

	private AnchorInfo _rightCenterAnchorInfo;

	private AnchorInfo _rightTopAnchorInfo;

	private readonly List<AnchorInfo> _anchors = new List<AnchorInfo>();

	public VideoOSPopover.Appearances Appearance
	{
		get
		{
			return (VideoOSPopover.Appearances)GetValue(AppearanceProperty);
		}
		set
		{
			SetValue(AppearanceProperty, value);
		}
	}

	public VideoOSPopover.TextAlignments TextAlignment
	{
		get
		{
			return (VideoOSPopover.TextAlignments)GetValue(TextAlignmentProperty);
		}
		set
		{
			SetValue(TextAlignmentProperty, value);
		}
	}

	public object Content
	{
		get
		{
			return GetValue(ContentProperty);
		}
		set
		{
			SetValue(ContentProperty, value);
		}
	}

	private static void AppearanceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		PopoverContent popoverContent = (PopoverContent)d;
		switch (popoverContent.Appearance)
		{
		case VideoOSPopover.Appearances.PrimaryDark:
			VideoOSTheming.SetTheme(popoverContent._contentControl, ThemeType.Dark);
			break;
		case VideoOSPopover.Appearances.PrimaryLight:
		case VideoOSPopover.Appearances.Accent:
		case VideoOSPopover.Appearances.Neutral:
			VideoOSTheming.SetTheme(popoverContent._contentControl, ThemeType.Light);
			break;
		default:
			throw new ArgumentOutOfRangeException("Appearance", popoverContent.Appearance, null);
		}
	}

	public PopoverContent(Popup hostPopup)
	{
		InitializeComponent();
		_topLeftAnchorInfo = new AnchorInfo(_topLeftAnchor, _topLeftAnchorShadow);
		_topCenterAnchorInfo = new AnchorInfo(_topCenterAnchor, _topCenterAnchorShadow);
		_topRightAnchorInfo = new AnchorInfo(_topRightAnchor, _topRightAnchorShadow);
		_leftTopAnchorInfo = new AnchorInfo(_leftTopAnchor, _leftTopAnchorShadow);
		_leftCenterAnchorInfo = new AnchorInfo(_leftCenterAnchor, _leftCenterAnchorShadow);
		_leftBottomAnchorInfo = new AnchorInfo(_leftBottomAnchor, _leftBottomAnchorShadow);
		_bottomRightAnchorInfo = new AnchorInfo(_bottomRightAnchor, _bottomRightAnchorShadow);
		_bottomCenterAnchorInfo = new AnchorInfo(_bottomCenterAnchor, _bottomCenterAnchorShadow);
		_bottomLeftAnchorInfo = new AnchorInfo(_bottomLeftAnchor, _bottomLeftAnchorShadow);
		_rightBottomAnchorInfo = new AnchorInfo(_rightBottomAnchor, _rightBottomAnchorShadow);
		_rightCenterAnchorInfo = new AnchorInfo(_rightCenterAnchor, _rightCenterAnchorShadow);
		_rightTopAnchorInfo = new AnchorInfo(_rightTopAnchor, _rightTopAnchorShadow);
		_anchors.Add(_topLeftAnchorInfo);
		_anchors.Add(_topCenterAnchorInfo);
		_anchors.Add(_topRightAnchorInfo);
		_anchors.Add(_leftTopAnchorInfo);
		_anchors.Add(_leftCenterAnchorInfo);
		_anchors.Add(_leftBottomAnchorInfo);
		_anchors.Add(_bottomRightAnchorInfo);
		_anchors.Add(_bottomCenterAnchorInfo);
		_anchors.Add(_bottomLeftAnchorInfo);
		_anchors.Add(_rightBottomAnchorInfo);
		_anchors.Add(_rightCenterAnchorInfo);
		_anchors.Add(_rightTopAnchorInfo);
		_hostPopup = hostPopup;
		_hostPopup.Opened += _hostPopup_Opened;
		_hostPopup.LayoutUpdated += _hostPopup_LayoutUpdated;
		_hostPopup.CustomPopupPlacementCallback = CustomPopupPlacementCallback;
		base.Loaded += PopoverContent_Loaded;
		base.Unloaded += PopoverContent_Unloaded;
	}

	private void PopoverContent_Loaded(object sender, RoutedEventArgs e)
	{
		VideoOSTheming.SetTheme(_contentControl, ThemeType.Dark);
		_contentControl.FlowDirection = ClientControl.Instance.FlowDirection;
		ClientControl.Instance.FlowDirectionChanged += ClientControl_OnFlowDirectionChanged;
	}

	private void PopoverContent_Unloaded(object sender, RoutedEventArgs e)
	{
		ClientControl.Instance.FlowDirectionChanged -= ClientControl_OnFlowDirectionChanged;
	}

	private void ClientControl_OnFlowDirectionChanged(object sender, EventArgs e)
	{
		_contentControl.FlowDirection = ClientControl.Instance.FlowDirection;
	}

	private void _hostPopup_LayoutUpdated(object sender, EventArgs e)
	{
		if (_hostPopup.IsOpen)
		{
			UpdateAnchors();
		}
	}

	private void _hostPopup_Opened(object sender, EventArgs e)
	{
		UpdateAnchors();
	}

	private CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
	{
		FrameworkElement frameworkElement = _hostPopup.PlacementTarget as FrameworkElement;
		double num = 0.0;
		double num2 = 31.0 * this.GetScreenScaleX();
		double num3 = 31.0 * this.GetScreenScaleX();
		if (frameworkElement != null && frameworkElement.FlowDirection == FlowDirection.RightToLeft)
		{
			num = 0.0 - targetSize.Width;
		}
		CustomPopupPlacement customPopupPlacement = new CustomPopupPlacement(new Point(targetSize.Width / 2.0 - num2 + num, 0.0 - popupSize.Height), PopupPrimaryAxis.Vertical);
		_bottomLeftAnchorInfo.PopupPlacement = customPopupPlacement;
		CustomPopupPlacement customPopupPlacement2 = new CustomPopupPlacement(new Point((0.0 - popupSize.Width) / 2.0 + targetSize.Width / 2.0 + num, 0.0 - popupSize.Height), PopupPrimaryAxis.Vertical);
		_bottomCenterAnchorInfo.PopupPlacement = customPopupPlacement2;
		CustomPopupPlacement customPopupPlacement3 = new CustomPopupPlacement(new Point(0.0 - popupSize.Width + targetSize.Width / 2.0 + num2 + num, 0.0 - popupSize.Height), PopupPrimaryAxis.Vertical);
		_bottomRightAnchorInfo.PopupPlacement = customPopupPlacement3;
		CustomPopupPlacement customPopupPlacement4 = new CustomPopupPlacement(new Point(targetSize.Width + num, 0.0 - popupSize.Height + targetSize.Height / 2.0 + num3), PopupPrimaryAxis.Horizontal);
		_leftBottomAnchorInfo.PopupPlacement = customPopupPlacement4;
		CustomPopupPlacement customPopupPlacement5 = new CustomPopupPlacement(new Point(targetSize.Width + num, (0.0 - popupSize.Height) / 2.0 + targetSize.Height / 2.0), PopupPrimaryAxis.Horizontal);
		_leftCenterAnchorInfo.PopupPlacement = customPopupPlacement5;
		CustomPopupPlacement customPopupPlacement6 = new CustomPopupPlacement(new Point(targetSize.Width + num, targetSize.Height / 2.0 - num3), PopupPrimaryAxis.Horizontal);
		_leftTopAnchorInfo.PopupPlacement = customPopupPlacement6;
		CustomPopupPlacement customPopupPlacement7 = new CustomPopupPlacement(new Point(0.0 - popupSize.Width + targetSize.Width / 2.0 + num2 + num, targetSize.Height), PopupPrimaryAxis.Vertical);
		_topRightAnchorInfo.PopupPlacement = customPopupPlacement7;
		CustomPopupPlacement customPopupPlacement8 = new CustomPopupPlacement(new Point((0.0 - popupSize.Width) / 2.0 + targetSize.Width / 2.0 + num, targetSize.Height), PopupPrimaryAxis.Vertical);
		_topCenterAnchorInfo.PopupPlacement = customPopupPlacement8;
		CustomPopupPlacement customPopupPlacement9 = new CustomPopupPlacement(new Point(targetSize.Width / 2.0 - num2 + num, targetSize.Height), PopupPrimaryAxis.Vertical);
		_topLeftAnchorInfo.PopupPlacement = customPopupPlacement9;
		CustomPopupPlacement customPopupPlacement10 = new CustomPopupPlacement(new Point(0.0 - popupSize.Width + num, targetSize.Height / 2.0 - num3), PopupPrimaryAxis.Horizontal);
		_rightTopAnchorInfo.PopupPlacement = customPopupPlacement10;
		CustomPopupPlacement customPopupPlacement11 = new CustomPopupPlacement(new Point(0.0 - popupSize.Width + num, (0.0 - popupSize.Height) / 2.0 + targetSize.Height / 2.0), PopupPrimaryAxis.Horizontal);
		_rightCenterAnchorInfo.PopupPlacement = customPopupPlacement11;
		CustomPopupPlacement customPopupPlacement12 = new CustomPopupPlacement(new Point(0.0 - popupSize.Width + num, 0.0 - popupSize.Height + targetSize.Height / 2.0 + num3), PopupPrimaryAxis.Horizontal);
		_rightBottomAnchorInfo.PopupPlacement = customPopupPlacement12;
		List<CustomPopupPlacement> list = new List<CustomPopupPlacement>();
		if (frameworkElement != null && frameworkElement.FlowDirection == FlowDirection.RightToLeft)
		{
			list.Add(customPopupPlacement2);
			list.Add(customPopupPlacement3);
			list.Add(customPopupPlacement);
			list.Add(customPopupPlacement8);
			list.Add(customPopupPlacement7);
			list.Add(customPopupPlacement9);
			list.Add(customPopupPlacement11);
			list.Add(customPopupPlacement10);
			list.Add(customPopupPlacement12);
			list.Add(customPopupPlacement5);
			list.Add(customPopupPlacement6);
			list.Add(customPopupPlacement4);
		}
		else
		{
			list.Add(customPopupPlacement2);
			list.Add(customPopupPlacement);
			list.Add(customPopupPlacement3);
			list.Add(customPopupPlacement8);
			list.Add(customPopupPlacement9);
			list.Add(customPopupPlacement7);
			list.Add(customPopupPlacement5);
			list.Add(customPopupPlacement6);
			list.Add(customPopupPlacement4);
			list.Add(customPopupPlacement11);
			list.Add(customPopupPlacement10);
			list.Add(customPopupPlacement12);
		}
		return list.ToArray();
	}

	private void UpdateAnchors()
	{
		foreach (AnchorInfo anchor in _anchors)
		{
			anchor.Anchor.Visibility = Visibility.Hidden;
			anchor.AnchorShadow.Visibility = Visibility.Hidden;
		}
		if (!(_hostPopup.PlacementTarget is FrameworkElement { IsLoaded: not false, IsVisible: not false } frameworkElement))
		{
			return;
		}
		Point point = TranslatePoint(new Point(0.0, 0.0), frameworkElement);
		point = new Point(point.X * this.GetScreenScaleX(), point.Y * this.GetScreenScaleY());
		if (frameworkElement.FlowDirection == FlowDirection.RightToLeft)
		{
			point = new Point(0.0 - point.X, point.Y);
		}
		AnchorInfo anchorInfo = null;
		double num = double.MaxValue;
		foreach (AnchorInfo anchor2 in _anchors)
		{
			CustomPopupPlacement popupPlacement = anchor2.PopupPlacement;
			double num2 = Math.Sqrt(Math.Pow(popupPlacement.Point.X - point.X, 2.0) + Math.Pow(popupPlacement.Point.Y - point.Y, 2.0));
			if (num >= num2)
			{
				num = num2;
				anchorInfo = anchor2;
			}
		}
		if (anchorInfo != null && num <= 10.0)
		{
			anchorInfo.Anchor.Visibility = Visibility.Visible;
			anchorInfo.AnchorShadow.Visibility = Visibility.Visible;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
