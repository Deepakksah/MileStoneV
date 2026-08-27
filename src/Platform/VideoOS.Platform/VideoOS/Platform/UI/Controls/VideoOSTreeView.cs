using System;
using System.Collections;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xaml;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Controls.TreeView.Internal;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTreeView : Control
{
	public static class VideoOSTreeViewInternals
	{
		public const double TallNodeHeight = 36.0;

		public const double TallNodeIndentation = 22.0;

		public const double TallNodeContentMinWidth = 125.0;

		public const double NormalNodeHeight = 30.0;

		public const double NormalNodeIndentation = 16.0;

		public const double NormalNodeContentMinWidth = 100.0;

		public const string TallItemContentDataTemplateKey = "VideoOSTreeViewTallItemContentDataTemplateKey";

		public const string NormalItemContentDataTemplateKey = "VideoOSTreeViewNormalItemContentDataTemplateKey";

		private static readonly VideoOSIconBuiltInSource CollapsedNodeIconSource = new VideoOSIconBuiltInSource
		{
			Icon = VideoOSIconBuiltInSource.Icons.Chevron_Right,
			RtlIcon = VideoOSIconBuiltInSource.Icons.Chevron_Left
		};

		private static readonly VideoOSIconBuiltInSource ExpandedNodeIconSource = new VideoOSIconBuiltInSource
		{
			Icon = VideoOSIconBuiltInSource.Icons.Chevron_Down
		};

		public static IMultiValueConverter NodeIconAutoColorBrushConverter { get; } = new LambdaConverter6<DependencyObject, bool, bool, bool, bool, bool>(delegate(DependencyObject self, bool isEnabled, bool isMouseOver, bool isMouseOverBackground, bool isSelected, bool isFocused)
		{
			string resourceKey = ((!isEnabled) ? (isSelected ? "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorSelectedDisabledBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorDeselectedDisabledBrush") : (isSelected ? ((!(isMouseOver || isMouseOverBackground)) ? (isFocused ? "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorSelectedIdleFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorSelectedIdleUnfocusedBrush") : "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorSelectedHoveredFocusedBrush") : ((!isFocused) ? ((isMouseOver || isMouseOverBackground) ? "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorDeselectedHoveredUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorDeselectedIdleUnfocusedBrush") : ((isMouseOver || isMouseOverBackground) ? "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorDeselectedHoveredFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemIconAutoColorDeselectedIdleFocusedBrush"))));
			return self.FindResource(resourceKey);
		}, "NodeIconAutoColorBrushConverter");

		public static IMultiValueConverter NodeContentTemplateConverter { get; } = new LambdaConverter4<DataTemplate, NodeAppearances, DataTemplate, DataTemplate>(delegate(DataTemplate contentTemplate, NodeAppearances appearance, DataTemplate tallTemplate, DataTemplate normalTemplate)
		{
			if (contentTemplate != null)
			{
				return contentTemplate;
			}
			return (appearance != NodeAppearances.Normal) ? tallTemplate : normalTemplate;
		}, "NodeContentTemplateConverter");

		public static IMultiValueConverter ExpanderButtonIconSourceConverter { get; } = new LambdaConverter1<bool>((bool isExpanded) => (!isExpanded) ? CollapsedNodeIconSource : ExpandedNodeIconSource, "ExpanderButtonIconSourceConverter");

		public static IMultiValueConverter TreeViewNodeItemsPresenterMarginConverter { get; } = new LambdaConverter1<NodeAppearances>((NodeAppearances appearance) => new Thickness((appearance == NodeAppearances.Normal) ? 16.0 : 22.0, 0.0, 0.0, 0.0), "TreeViewNodeItemsPresenterMarginConverter");

		public static IMultiValueConverter TreeNodeBackgroundMarginConverter { get; } = new LambdaConverter2<CustomTreeView, FrameworkElement>((CustomTreeView treeView, FrameworkElement nodeCanvas) => new Thickness(0.0 - treeView.GetHorizontalOffsetFromContentContainer(nodeCanvas), 0.0, 0.0, 0.0), "TreeNodeBackgroundMarginConverter");

		public static IMultiValueConverter TreeNodeGridMinWidthConverter { get; } = new LambdaConverter3<double, ActionButtonAlignments, double>((double marginLeft, ActionButtonAlignments horizontalAlignment, double allowedWidth) => (horizontalAlignment == ActionButtonAlignments.Right) ? ((object)(allowedWidth + marginLeft)) : ((object)double.NaN), "TreeNodeGridMinWidthConverter");

		public static IMultiValueConverter TreeNodeGridMaxWidthConverter { get; } = new LambdaConverter4<CustomTreeView, FrameworkElement, double, double>(delegate(CustomTreeView treeView, FrameworkElement nodeCanvas, double allowedWidth, double canvasLeftValue)
		{
			double horizontalOffsetFromContentContainer = treeView.GetHorizontalOffsetFromContentContainer(nodeCanvas);
			return double.IsNaN(canvasLeftValue) ? ((object)(allowedWidth - horizontalOffsetFromContentContainer)) : ((object)(allowedWidth - horizontalOffsetFromContentContainer - canvasLeftValue));
		}, "TreeNodeGridMaxWidthConverter");

		public static IMultiValueConverter TreeNodeGridCanvasLeftConverter { get; } = new LambdaConverter4<CustomTreeView, FrameworkElement, NodeAppearances, double>(delegate(CustomTreeView treeView, FrameworkElement nodeCanvas, NodeAppearances appearance, double treeViewContentContainerActualWidth)
		{
			double horizontalOffsetFromContentContainer = treeView.GetHorizontalOffsetFromContentContainer(nodeCanvas);
			double num = ((appearance == NodeAppearances.Normal) ? 100.0 : 125.0);
			double val = horizontalOffsetFromContentContainer + num - treeViewContentContainerActualWidth;
			return 0.0 - Math.Min(horizontalOffsetFromContentContainer, Math.Max(0.0, val));
		}, "TreeNodeGridCanvasLeftConverter");

		public static IMultiValueConverter TreeNodeBackgroundBrushConverter { get; } = new LambdaConverter7<DependencyObject, bool, bool, bool, bool, bool, bool>(delegate(DependencyObject self, bool isEnabled, bool isMouseOver, bool isMouseOverBackground, bool isSelected, bool isFocused, bool isActionListOpen)
		{
			string resourceKey = ((!isEnabled) ? (isSelected ? "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundSelectedDisabledBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundDeselectedDisabledBrush") : (isSelected ? ((!(isMouseOver || isMouseOverBackground || isActionListOpen)) ? (isFocused ? "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundSelectedIdleFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundSelectedIdleUnfocusedBrush") : "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundSelectedHoveredFocusedBrush") : (isActionListOpen ? (isFocused ? "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundDeselectedHoveredFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundDeselectedHoveredUnfocusedBrush") : ((!isFocused) ? ((isMouseOver || isMouseOverBackground) ? "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundDeselectedHoveredUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundDeselectedIdleUnfocusedBrush") : ((isMouseOver || isMouseOverBackground) ? "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundDeselectedHoveredFocusedBrush" : "VideoOSResourceKeysInternalVideoOSTreeViewItemBackgroundDeselectedIdleFocusedBrush")))));
			return self.FindResource(resourceKey);
		}, "TreeNodeBackgroundBrushConverter");

		public static IMultiValueConverter CommandActionListVisibilityConverter { get; } = new LambdaConverter5<ICollection, bool, bool, bool, bool>((ICollection actions, bool isOpen, bool isSelected, bool isMouseOver, bool isMouseOverBackground) => (actions != null && actions.Count > 0 && (isOpen || isMouseOver || isMouseOverBackground || isSelected)) ? ((object)Visibility.Visible) : ((object)Visibility.Collapsed), "CommandActionListVisibilityConverter");
	}

	public enum NodeAppearances
	{
		Tall,
		Normal
	}

	public enum ActionButtonAlignments
	{
		Left,
		Right
	}

	public static readonly RoutedEvent SelectedItemChangedEvent;

	public static readonly DependencyProperty ItemsSourceProperty;

	private static readonly DependencyPropertyKey SelectedItemPropertyKey;

	public static readonly DependencyProperty SelectedItemProperty;

	public static readonly DependencyProperty NodeAppearanceProperty;

	public static readonly DependencyProperty VerticalScrollBarVisibilityProperty;

	public static readonly DependencyProperty IsBorderVisibleProperty;

	public static readonly DependencyProperty ContentTemplateProperty;

	public static readonly DependencyProperty ActionButtonAlignmentProperty;

	public static readonly DependencyProperty ItemIsExpandedPropertyNameProperty;

	public static readonly DependencyProperty ItemIsEnabledPropertyNameProperty;

	public static readonly DependencyProperty ItemIsSelectedPropertyNameProperty;

	public static readonly DependencyProperty ItemChildrenPropertyNameProperty;

	public static readonly DependencyProperty ItemIconSourcePropertyNameProperty;

	public static readonly DependencyProperty ItemDataPropertyNameProperty;

	public static readonly DependencyProperty ItemContentTemplatePropertyNameProperty;

	public static readonly DependencyProperty ItemActionsPropertyNameProperty;

	public static readonly DependencyProperty ToggleTreeNodeExpansionCommandProperty;

	public static readonly DependencyProperty ConstrainedContentWidthProperty;

	private CustomTreeView _treeView;

	public ICollection ItemsSource
	{
		get
		{
			return (ICollection)GetValue(ItemsSourceProperty);
		}
		set
		{
			SetValue(ItemsSourceProperty, value);
		}
	}

	public object SelectedItem
	{
		get
		{
			return GetValue(SelectedItemProperty);
		}
		private set
		{
			SetValue(SelectedItemPropertyKey, value);
		}
	}

	public NodeAppearances NodeAppearance
	{
		get
		{
			return (NodeAppearances)GetValue(NodeAppearanceProperty);
		}
		set
		{
			SetValue(NodeAppearanceProperty, value);
		}
	}

	public ScrollBarVisibility VerticalScrollBarVisibility
	{
		get
		{
			return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
		}
		set
		{
			SetValue(VerticalScrollBarVisibilityProperty, value);
		}
	}

	public bool IsBorderVisible
	{
		get
		{
			return (bool)GetValue(IsBorderVisibleProperty);
		}
		set
		{
			SetValue(IsBorderVisibleProperty, value);
		}
	}

	public DataTemplate ContentTemplate
	{
		get
		{
			return (DataTemplate)GetValue(ContentTemplateProperty);
		}
		set
		{
			SetValue(ContentTemplateProperty, value);
		}
	}

	public ActionButtonAlignments ActionButtonAlignment
	{
		get
		{
			return (ActionButtonAlignments)GetValue(ActionButtonAlignmentProperty);
		}
		set
		{
			SetValue(ActionButtonAlignmentProperty, value);
		}
	}

	public string ItemIsExpandedPropertyName
	{
		get
		{
			return (string)GetValue(ItemIsExpandedPropertyNameProperty);
		}
		set
		{
			SetValue(ItemIsExpandedPropertyNameProperty, value);
		}
	}

	public string ItemIsEnabledPropertyName
	{
		get
		{
			return (string)GetValue(ItemIsEnabledPropertyNameProperty);
		}
		set
		{
			SetValue(ItemIsEnabledPropertyNameProperty, value);
		}
	}

	public string ItemIsSelectedPropertyName
	{
		get
		{
			return (string)GetValue(ItemIsSelectedPropertyNameProperty);
		}
		set
		{
			SetValue(ItemIsSelectedPropertyNameProperty, value);
		}
	}

	public string ItemChildrenPropertyName
	{
		get
		{
			return (string)GetValue(ItemChildrenPropertyNameProperty);
		}
		set
		{
			SetValue(ItemChildrenPropertyNameProperty, value);
		}
	}

	public string ItemIconSourcePropertyName
	{
		get
		{
			return (string)GetValue(ItemIconSourcePropertyNameProperty);
		}
		set
		{
			SetValue(ItemIconSourcePropertyNameProperty, value);
		}
	}

	public string ItemDataPropertyName
	{
		get
		{
			return (string)GetValue(ItemDataPropertyNameProperty);
		}
		set
		{
			SetValue(ItemDataPropertyNameProperty, value);
		}
	}

	public string ItemContentTemplatePropertyName
	{
		get
		{
			return (string)GetValue(ItemContentTemplatePropertyNameProperty);
		}
		set
		{
			SetValue(ItemContentTemplatePropertyNameProperty, value);
		}
	}

	public string ItemActionsPropertyName
	{
		get
		{
			return (string)GetValue(ItemActionsPropertyNameProperty);
		}
		set
		{
			SetValue(ItemActionsPropertyNameProperty, value);
		}
	}

	public ICommand ToggleTreeNodeExpansionCommand
	{
		get
		{
			return (ICommand)GetValue(ToggleTreeNodeExpansionCommandProperty);
		}
		set
		{
			SetValue(ToggleTreeNodeExpansionCommandProperty, value);
		}
	}

	public double ConstrainedContentWidth
	{
		get
		{
			return (double)GetValue(ConstrainedContentWidthProperty);
		}
		set
		{
			SetValue(ConstrainedContentWidthProperty, value);
		}
	}

	public event RoutedEventHandler SelectedItemChanged
	{
		add
		{
			AddHandler(SelectedItemChangedEvent, value);
		}
		remove
		{
			RemoveHandler(SelectedItemChangedEvent, value);
		}
	}

	public event EventHandler<DependencyPropertyChangedEventArgs> ItemPropertyNameChanged;

	static VideoOSTreeView()
	{
		SelectedItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSTreeView));
		ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ICollection), typeof(VideoOSTreeView), new PropertyMetadata(null));
		SelectedItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItem", typeof(object), typeof(VideoOSTreeView), new FrameworkPropertyMetadata(null));
		SelectedItemProperty = SelectedItemPropertyKey.DependencyProperty;
		NodeAppearanceProperty = DependencyProperty.Register("NodeAppearance", typeof(NodeAppearances), typeof(VideoOSTreeView), new PropertyMetadata(NodeAppearances.Tall));
		VerticalScrollBarVisibilityProperty = DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(VideoOSTreeView), new PropertyMetadata(ScrollBarVisibility.Auto));
		IsBorderVisibleProperty = DependencyProperty.Register("IsBorderVisible", typeof(bool), typeof(VideoOSTreeView), new PropertyMetadata(true));
		ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ActionButtonAlignmentProperty = DependencyProperty.Register("ActionButtonAlignment", typeof(ActionButtonAlignments), typeof(VideoOSTreeView), new PropertyMetadata(ActionButtonAlignments.Right));
		ItemIsExpandedPropertyNameProperty = DependencyProperty.Register("ItemIsExpandedPropertyName", typeof(string), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ItemIsEnabledPropertyNameProperty = DependencyProperty.Register("ItemIsEnabledPropertyName", typeof(string), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ItemIsSelectedPropertyNameProperty = DependencyProperty.Register("ItemIsSelectedPropertyName", typeof(string), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ItemChildrenPropertyNameProperty = DependencyProperty.Register("ItemChildrenPropertyName", typeof(string), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ItemIconSourcePropertyNameProperty = DependencyProperty.Register("ItemIconSourcePropertyName", typeof(string), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ItemDataPropertyNameProperty = DependencyProperty.Register("ItemDataPropertyName", typeof(string), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ItemContentTemplatePropertyNameProperty = DependencyProperty.Register("ItemContentTemplatePropertyName", typeof(string), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ItemActionsPropertyNameProperty = DependencyProperty.Register("ItemActionsPropertyName", typeof(string), typeof(VideoOSTreeView), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			((VideoOSTreeView)o).ItemPropertyNameChanged?.Invoke(o, args);
		}));
		ToggleTreeNodeExpansionCommandProperty = DependencyProperty.Register("ToggleTreeNodeExpansionCommand", typeof(ICommand), typeof(VideoOSTreeView), new PropertyMetadata(null));
		ConstrainedContentWidthProperty = DependencyProperty.Register("ConstrainedContentWidth", typeof(double), typeof(VideoOSTreeView), new PropertyMetadata(0.0));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTreeView), new FrameworkPropertyMetadata(typeof(VideoOSTreeView)));
		ThemeManager.InitializeThemingSystem();
		UIElement.FocusableProperty.OverrideMetadata(typeof(VideoOSTreeView), new FrameworkPropertyMetadata(false));
		FrameworkElement.HorizontalAlignmentProperty.OverrideMetadata(typeof(VideoOSTreeView), new FrameworkPropertyMetadata(HorizontalAlignment.Left));
	}

	public VideoOSTreeView()
	{
		ToggleTreeNodeExpansionCommand = new DelegateCommand(ToggleTreeNodeExpansion);
		base.PreviewKeyDown += OnPreviewKeyDown;
		CustomTreeViewItem.AddCustomRequestBringIntoViewHandler(this, CustomRequestBringIntoView);
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		if (_treeView != null)
		{
			throw new XamlException("Template has already been applied.");
		}
		_treeView = this.GetTemplateChild<CustomTreeView>("PART_TreeView");
		_treeView.VideoOSTreeView = this;
		_treeView.Resources[typeof(ScrollViewer)] = ThemeManager.Instance.FindStaticResource(VideoOSStaticResourceKeys.ScrollViewerStyleKey);
		_treeView.SelectedItemChanged += _treeView_SelectedItemChanged;
		Binding binding = new Binding("ScrollViewerViewportWidth")
		{
			Source = _treeView
		};
		SetBinding(ConstrainedContentWidthProperty, binding);
	}

	public void Refresh()
	{
		_treeView?.Items.Refresh();
	}

	private void OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Multiply)
		{
			e.Handled = true;
		}
	}

	private void _treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		SelectedItem = _treeView.SelectedItem;
		RaiseEvent(new RoutedEventArgs(SelectedItemChangedEvent));
	}

	private void CustomRequestBringIntoView(object sender, RoutedEventArgs e)
	{
		ScrollViewer scrollViewer = this.FindDescendant<ScrollViewer>();
		CustomTreeViewItem customTreeViewItem = (CustomTreeViewItem)e.OriginalSource;
		if (customTreeViewItem != null)
		{
			double num = ((NodeAppearance == NodeAppearances.Normal) ? 30.0 : 36.0);
			Point point = customTreeViewItem.TransformToVisual(scrollViewer).Transform(new Point(0.0, 0.0));
			if (point.Y < 0.0)
			{
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + point.Y);
			}
			else if (point.Y >= scrollViewer.ViewportHeight - num)
			{
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + num);
			}
		}
		e.Handled = true;
	}

	private void ToggleTreeNodeExpansion(object parameter)
	{
		TreeViewItem obj = (TreeViewItem)parameter;
		obj.IsExpanded = !obj.IsExpanded;
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSTreeViewAutomationPeer(this);
	}
}
