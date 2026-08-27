using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public class VideoOSDialog : VideoOSWindow
{
	public static class VideoOSDialogInternals
	{
		public const double MarginTop = 24.0;

		public const double MarginBottom = 24.0;

		public const double MarginLeft = 24.0;

		public const double MarginRight = 24.0;

		public const double ContentAndActionsSpacing = 16.0;

		public static readonly Thickness OuterGridMargin = new Thickness(0.0, 24.0, 0.0, 24.0);

		public static readonly Thickness ActionCanvasMargin = new Thickness(24.0, 16.0, 24.0, 0.0);

		public static readonly Thickness ActionCanvasMarginNone = new Thickness(0.0);

		public static IMultiValueConverter VerticalScrollBarPaddingConverter { get; } = new LambdaConverter1<ScrollBarVisibility>(delegate(ScrollBarVisibility scrollBarVisibility)
		{
			double num = ((scrollBarVisibility == ScrollBarVisibility.Visible) ? 16.0 : 0.0);
			return new Thickness(24.0, 0.0, 24.0 - num, 0.0);
		}, "VerticalScrollBarPaddingConverter");

		public static IMultiValueConverter VerticalScrollBarVisibilityConverter { get; } = new LambdaConverter1<object>((object content) => (content is TextBlock) ? ScrollBarVisibility.Visible : ScrollBarVisibility.Disabled, "VerticalScrollBarVisibilityConverter");

		public static IMultiValueConverter ShowVerticalScrollUnderEdgesConverter { get; } = new LambdaConverter1<object>((object content) => content is TextBlock, "ShowVerticalScrollUnderEdgesConverter");

		public static IValueConverter ActionCanvasMarginConverter { get; } = new LambdaConverter1<ICollection<VideoOSDialogActionBase>>((ICollection<VideoOSDialogActionBase> dialogActions) => (dialogActions?.Any() ?? false) ? ActionCanvasMargin : ActionCanvasMarginNone, "ActionCanvasMarginConverter");

		public static IMultiValueConverter ContentMaxWidthConverter { get; } = new LambdaConverter3<double, double?, double?>((double maxWidth, double? dialogMaxWidth, double? actionCanvasDesiredWidth) => (dialogMaxWidth.HasValue && actionCanvasDesiredWidth.HasValue) ? ((object)Math.Max(dialogMaxWidth.Value, actionCanvasDesiredWidth.Value + 24.0 + 24.0)) : ((object)maxWidth), "ContentMaxWidthConverter");
	}

	public enum ActionsSize
	{
		Large,
		Medium
	}

	public static readonly DependencyProperty DialogActionsProperty;

	public static readonly DependencyProperty DialogActionsSizeProperty;

	public static readonly DependencyProperty DialogContentMaxWidthProperty;

	public ICollection<VideoOSDialogActionBase> DialogActions
	{
		get
		{
			return (ICollection<VideoOSDialogActionBase>)GetValue(DialogActionsProperty);
		}
		set
		{
			SetValue(DialogActionsProperty, value);
		}
	}

	public ActionsSize DialogActionsSize
	{
		get
		{
			return (ActionsSize)GetValue(DialogActionsSizeProperty);
		}
		set
		{
			SetValue(DialogActionsSizeProperty, value);
		}
	}

	public double? DialogContentMaxWidth
	{
		get
		{
			return (double?)GetValue(DialogContentMaxWidthProperty);
		}
		set
		{
			SetValue(DialogContentMaxWidthProperty, value);
		}
	}

	public override string ThemeBackgroundBrushKey => "VideoOSResourceKeysInternalBackgroundBrushForDialogs";

	public override string ThemeHighlightBrushKey => "VideoOSResourceKeysInternalHighlightBrushForDialogs";

	public override string ThemeEmptyStateBrushKey => "VideoOSResourceKeysInternalEmptyStateBrushForDialogs";

	public override string ThemeSubtleBrushKey => "VideoOSResourceKeysInternalSubtleBrushForDialogs";

	static VideoOSDialog()
	{
		DialogActionsProperty = DependencyProperty.Register("DialogActions", typeof(ICollection<VideoOSDialogActionBase>), typeof(VideoOSDialog), new PropertyMetadata(null));
		DialogActionsSizeProperty = DependencyProperty.Register("DialogActionsSize", typeof(ActionsSize), typeof(VideoOSDialog), new PropertyMetadata(ActionsSize.Medium));
		DialogContentMaxWidthProperty = DependencyProperty.Register("DialogContentMaxWidth", typeof(double?), typeof(VideoOSDialog), new PropertyMetadata(null));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSDialog), new FrameworkPropertyMetadata(typeof(VideoOSDialog)));
		ThemeManager.InitializeThemingSystem();
	}

	public VideoOSDialog()
	{
		DialogActions = new ObservableCollection<VideoOSDialogActionBase>();
		base.KeyDown += OnKeyDown;
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return && DialogActions != null)
		{
			List<VideoOSDialogActionButtonBase> list = (from a in DialogActions.OfType<VideoOSDialogActionButtonBase>()
				where a.ActivateOnSubmit
				select a).ToList();
			if (list.Any())
			{
				list.ForEach(delegate(VideoOSDialogActionButtonBase a)
				{
					a.ClickCommand.Execute(null);
				});
				e.Handled = true;
			}
		}
		else if (e.Key == Key.Escape)
		{
			Close();
			e.Handled = true;
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSDialogAutomationPeer(this);
	}
}
