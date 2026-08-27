using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSScrollViewer : ScrollViewer
{
	public static class VideoOSScrollViewerInternals
	{
		public const string ScrollViewerTemplateKey = "VideoOSScrollViewerScrollViewerTemplateKey";

		public static IMultiValueConverter ScrollUnderEdgeTopVisibilityConverter { get; } = new LambdaConverter2<bool, double>((bool showScrollUnderEdges, double verticalOffset) => (!showScrollUnderEdges || !(verticalOffset > 0.0)) ? Visibility.Collapsed : Visibility.Visible, "ScrollUnderEdgeTopVisibilityConverter");

		public static IMultiValueConverter ScrollUnderEdgeBottomVisibilityConverter { get; } = new LambdaConverter4<bool, double, double, double>(delegate(bool showScrollUnderEdges, double verticalOffset, double extentHeight, double viewportHeight)
		{
			if (!showScrollUnderEdges)
			{
				return Visibility.Collapsed;
			}
			double num = extentHeight - viewportHeight;
			return (!(verticalOffset < num)) ? Visibility.Collapsed : Visibility.Visible;
		}, "ScrollUnderEdgeBottomVisibilityConverter");
	}

	private static class SystemParametersEx
	{
		private static class NativeMethods
		{
			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[SuppressUnmanagedCodeSecurity]
			[SecurityCritical]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool SystemParametersInfo(int nAction, int nParam, ref int value, int ignore);
		}

		public static int WheelScrollChars
		{
			get
			{
				int value = 0;
				NativeMethods.SystemParametersInfo(108, 0, ref value, 0);
				return value;
			}
		}
	}

	public static readonly DependencyProperty ShowScrollUnderEdgesProperty;

	public static readonly DependencyProperty ScrollBarContextMenuOpeningCommandProperty;

	public static readonly DependencyProperty IsScrollBarContextMenuEnabledProperty;

	public static readonly DependencyProperty IsContentBorderVisibleProperty;

	public static readonly DependencyProperty ShowVerticalScrollUnderEdgesProperty;

	public static readonly DependencyProperty IsHorizontalMouseWheelScrollingEnabledProperty;

	public bool IsScrollBarContextMenuEnabled
	{
		get
		{
			return (bool)GetValue(IsScrollBarContextMenuEnabledProperty);
		}
		set
		{
			SetValue(IsScrollBarContextMenuEnabledProperty, value);
		}
	}

	public bool IsContentBorderVisible
	{
		get
		{
			return (bool)GetValue(IsContentBorderVisibleProperty);
		}
		set
		{
			SetValue(IsContentBorderVisibleProperty, value);
		}
	}

	public bool ShowVerticalScrollUnderEdges
	{
		get
		{
			return (bool)GetValue(ShowVerticalScrollUnderEdgesProperty);
		}
		set
		{
			SetValue(ShowVerticalScrollUnderEdgesProperty, value);
		}
	}

	public bool IsHorizontalMouseWheelScrollingEnabled
	{
		get
		{
			return (bool)GetValue(IsHorizontalMouseWheelScrollingEnabledProperty);
		}
		set
		{
			SetValue(IsHorizontalMouseWheelScrollingEnabledProperty, value);
		}
	}

	static VideoOSScrollViewer()
	{
		ShowScrollUnderEdgesProperty = DependencyProperty.RegisterAttached("ShowScrollUnderEdges", typeof(bool), typeof(VideoOSScrollViewer), new PropertyMetadata(false));
		ScrollBarContextMenuOpeningCommandProperty = DependencyProperty.RegisterAttached("ScrollBarContextMenuOpeningCommand", typeof(ICommand), typeof(VideoOSScrollViewer), new PropertyMetadata((object)null));
		IsScrollBarContextMenuEnabledProperty = DependencyProperty.Register("IsScrollBarContextMenuEnabled", typeof(bool), typeof(VideoOSScrollViewer), new PropertyMetadata(true));
		IsContentBorderVisibleProperty = DependencyProperty.Register("IsContentBorderVisible", typeof(bool), typeof(VideoOSScrollViewer), new PropertyMetadata(false, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			VideoOSStyleScrollBar.SetIsContentBorderVisible((FrameworkElement)o, (bool)args.NewValue);
		}));
		ShowVerticalScrollUnderEdgesProperty = DependencyProperty.Register("ShowVerticalScrollUnderEdges", typeof(bool), typeof(VideoOSScrollViewer), new PropertyMetadata(false, ShowVerticalScrollUnderEdgesChanged));
		IsHorizontalMouseWheelScrollingEnabledProperty = DependencyProperty.Register("IsHorizontalMouseWheelScrollingEnabled", typeof(bool), typeof(VideoOSScrollViewer), new PropertyMetadata(true));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSScrollViewer), new FrameworkPropertyMetadata(typeof(VideoOSScrollViewer)));
		ThemeManager.InitializeThemingSystem();
		ScrollViewer.VerticalScrollBarVisibilityProperty.OverrideMetadata(typeof(VideoOSScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));
	}

	public static void SetShowScrollUnderEdges(DependencyObject element, bool value)
	{
		element.SetValue(ShowScrollUnderEdgesProperty, value);
	}

	public static bool GetShowScrollUnderEdges(DependencyObject element)
	{
		return (bool)element.GetValue(ShowScrollUnderEdgesProperty);
	}

	public static void SetScrollBarContextMenuOpeningCommand(DependencyObject element, ICommand value)
	{
		element.SetValue(ScrollBarContextMenuOpeningCommandProperty, value);
	}

	public static ICommand GetScrollBarContextMenuOpeningCommand(DependencyObject element)
	{
		return (ICommand)element.GetValue(ScrollBarContextMenuOpeningCommandProperty);
	}

	private static void ShowVerticalScrollUnderEdgesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SetShowScrollUnderEdges(d, (bool)e.NewValue);
	}

	public VideoOSScrollViewer()
	{
		base.Resources[typeof(ScrollBar)] = (Style)ThemeManager.Instance.FindStaticResource("VideoOSStyleScrollBarScrollBarStyleKey");
		SetScrollBarContextMenuOpeningCommand(this, new DelegateCommand(ScrollBarContextMenuOpening));
	}

	private void ScrollBarContextMenuOpening(object args)
	{
		if (!IsScrollBarContextMenuEnabled)
		{
			((RoutedEventArgs)((EventsToCommandMappingArgs)args).OriginalEventArgs).Handled = true;
		}
	}

	protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
	{
		if (!IsHorizontalMouseWheelScrollingEnabled || (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
		{
			base.OnPreviewMouseWheel(e);
			return;
		}
		int wheelScrollChars = SystemParametersEx.WheelScrollChars;
		for (int i = 0; i < wheelScrollChars; i++)
		{
			if (e.Delta < 0)
			{
				LineRight();
			}
			else
			{
				LineLeft();
			}
		}
		e.Handled = true;
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSScrollViewerAutomationPeer(this);
	}
}
