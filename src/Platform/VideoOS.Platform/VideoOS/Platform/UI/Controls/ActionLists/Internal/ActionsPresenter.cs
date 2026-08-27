using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Resources;

namespace VideoOS.Platform.UI.Controls.ActionLists.Internal;

public partial class ActionsPresenter : Canvas, IComponentConnector
{
	public enum ControlSizes
	{
		ExtraSmall,
		Small,
		Medium
	}

	private class VideoOSActionInfo
	{
		public int Index { get; }

		public VideoOSActionBase Action { get; }

		public FrameworkElement FrameworkElement { get; }

		public VideoOSActionInfo(int index, VideoOSActionBase action, FrameworkElement frameworkElement)
		{
			Index = index;
			Action = action;
			FrameworkElement = frameworkElement;
		}
	}

	public bool __isOpen;

	public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ICollection<VideoOSActionBase>), typeof(ActionsPresenter), new PropertyMetadata(null, CollectionHelperBase.CollectionChangedCallback));

	public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(double), typeof(ActionsPresenter), new PropertyMetadata(2.0, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((ActionsPresenter)o).Refresh();
	}));

	public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(ActionsPresenter), new PropertyMetadata(HorizontalAlignment.Left, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((ActionsPresenter)o).Refresh();
	}));

	private VideoOSMenuButtonBase _overflowMenuControl;

	private readonly Dictionary<VideoOSActionBase, VideoOSActionInfo> _actionsNotAlwaysInOverflow = new Dictionary<VideoOSActionBase, VideoOSActionInfo>();

	private readonly List<VideoOSActionBase> _actionsNotAlwaysInOverflowOrderedByOverflowOrder = new List<VideoOSActionBase>();

	private readonly HashSet<VideoOSActionBase> _actionsAlwaysInOverflow = new HashSet<VideoOSActionBase>();

	public static IMultiValueConverter ActionTooltipConverter { get; } = new LambdaConverter2<string, string>((string text, string tooltip) => (!object.Equals(text, tooltip)) ? tooltip : null, "ActionTooltipConverter");

	public ControlSizes ControlSize { get; set; }

	public bool IsOpen
	{
		get
		{
			return __isOpen;
		}
		set
		{
			if (__isOpen != value)
			{
				__isOpen = value;
				this.IsOpenChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public ICollection<VideoOSActionBase> Actions
	{
		get
		{
			return (ICollection<VideoOSActionBase>)GetValue(ActionsProperty);
		}
		set
		{
			SetValue(ActionsProperty, value);
		}
	}

	public double Spacing
	{
		get
		{
			return (double)GetValue(SpacingProperty);
		}
		set
		{
			SetValue(SpacingProperty, value);
		}
	}

	public HorizontalAlignment HorizontalContentAlignment
	{
		get
		{
			return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
		}
		set
		{
			SetValue(HorizontalContentAlignmentProperty, value);
		}
	}

	public event EventHandler IsOpenChanged;

	public ActionsPresenter()
	{
		CollectionHelper<VideoOSActionBase>.Initialize(this, ActionsProperty, ActionsCollectionChanged);
		InitializeComponent();
	}

	public FrameworkElement GetFrameworkElementForAction(VideoOSActionBase action)
	{
		if (_overflowMenuControl != null && _overflowMenuControl.Actions != null && _overflowMenuControl.Actions.Contains(action))
		{
			return _overflowMenuControl;
		}
		foreach (FrameworkElement child in base.Children)
		{
			if (child.DataContext == action)
			{
				return child;
			}
		}
		return null;
	}

	public FrameworkElement GetActionFrameworkElementUnderMouseCursor()
	{
		foreach (FrameworkElement child in base.Children)
		{
			if (child.IsVisible && child.IsMouseOver)
			{
				if (child is IActionControlHolder { ActionableElement: var actionableElement })
				{
					return actionableElement;
				}
				if (child is VideoOSMenuButtonBase self)
				{
					return self.FindDescendant<Button>();
				}
			}
		}
		return null;
	}

	private void ActionsCollectionChanged(CollectionChangedInfo<VideoOSActionBase> collectionChangedInfo)
	{
		UpdateChildren();
	}

	private void Refresh()
	{
		InvalidateMeasure();
		if (IsFullyLoaded() && base.VisualParent is FrameworkElement frameworkElement)
		{
			UpdateChildPositions(frameworkElement.ActualWidth);
		}
	}

	protected override Size MeasureOverride(Size constraint)
	{
		if (!IsFullyLoaded())
		{
			return base.MeasureOverride(constraint);
		}
		double num = 0.0;
		Size availableSize = new Size(double.PositiveInfinity, constraint.Height);
		_overflowMenuControl.Measure(availableSize);
		Dictionary<VideoOSActionBase, VideoOSActionInfo> dictionary = TrimActionsNotInOverflow();
		if (dictionary.Any())
		{
			foreach (KeyValuePair<VideoOSActionBase, VideoOSActionInfo> item in dictionary)
			{
				if (item.Key.IsVisible)
				{
					item.Value.FrameworkElement.Measure(availableSize);
					if (num > 0.0)
					{
						num += Spacing;
					}
					num += item.Value.FrameworkElement.DesiredSize.Width;
				}
			}
		}
		if (_actionsAlwaysInOverflow.Any((VideoOSActionBase a) => a.IsVisible))
		{
			if (_actionsNotAlwaysInOverflow.Any((KeyValuePair<VideoOSActionBase, VideoOSActionInfo> a) => a.Key.IsVisible))
			{
				num += Spacing;
			}
			num += _overflowMenuControl.DesiredSize.Width;
		}
		return new Size(num, (num > 0.0) ? GetActionListHeight() : 0.0);
	}

	private void UpdateChildren()
	{
		if (!base.IsLoaded)
		{
			return;
		}
		_actionsAlwaysInOverflow.Clear();
		Dictionary<VideoOSActionBase, int> actions = new Dictionary<VideoOSActionBase, int>();
		if (Actions != null)
		{
			foreach (VideoOSActionBase action in Actions)
			{
				actions.Add(action, actions.Count);
				if (action.OverflowOrder <= 0)
				{
					_actionsAlwaysInOverflow.Add(action);
				}
			}
		}
		foreach (VideoOSActionBase item in _actionsNotAlwaysInOverflow.Keys.ToList())
		{
			item.PropertyChanged -= Action_PropertyChanged;
			if (!actions.ContainsKey(item) || _actionsAlwaysInOverflow.Contains(item))
			{
				FrameworkElement frameworkElement = _actionsNotAlwaysInOverflow[item].FrameworkElement;
				frameworkElement.SizeChanged -= ActionControl_SizeChanged;
				frameworkElement.Loaded -= ActionFrameworkElement_Loaded;
				_actionsNotAlwaysInOverflow.Remove(item);
				if (frameworkElement is ActionGroupControlHolder actionGroupControlHolder)
				{
					actionGroupControlHolder.IsOpenChanged -= ActionGroupControlHolder_IsOpenChanged;
				}
				base.Children.Remove(frameworkElement);
			}
		}
		foreach (VideoOSActionBase key in actions.Keys)
		{
			key.PropertyChanged -= Action_PropertyChanged;
			key.PropertyChanged += Action_PropertyChanged;
			if (_actionsNotAlwaysInOverflow.ContainsKey(key) || _actionsAlwaysInOverflow.Contains(key))
			{
				continue;
			}
			FrameworkElement frameworkElement2;
			if (key is VideoOSActionActivate dataContext)
			{
				frameworkElement2 = new ActionActivateControlHolder
				{
					DataContext = dataContext,
					ControlSize = ControlSize
				};
			}
			else if (key is VideoOSActionSelect dataContext2)
			{
				frameworkElement2 = new ActionSelectControlHolder
				{
					DataContext = dataContext2,
					ControlSize = ControlSize
				};
			}
			else if (key is VideoOSActionHold dataContext3)
			{
				frameworkElement2 = new ActionHoldControlHolder
				{
					DataContext = dataContext3,
					ControlSize = ControlSize
				};
			}
			else if (key is VideoOSActionGroup dataContext4)
			{
				ActionGroupControlHolder actionGroupControlHolder2 = new ActionGroupControlHolder();
				actionGroupControlHolder2.DataContext = dataContext4;
				actionGroupControlHolder2.ControlSize = ControlSize;
				actionGroupControlHolder2.IsOpenChanged += ActionGroupControlHolder_IsOpenChanged;
				frameworkElement2 = actionGroupControlHolder2;
			}
			else
			{
				if (!(key is VideoOSActionSeparator))
				{
					throw new ArgumentOutOfRangeException("action", key.GetType().Name, null);
				}
				frameworkElement2 = new ActionSeparatorControlHolder
				{
					ControlSize = ControlSize
				};
			}
			frameworkElement2.Visibility = Visibility.Hidden;
			frameworkElement2.SizeChanged += ActionControl_SizeChanged;
			frameworkElement2.Loaded += ActionFrameworkElement_Loaded;
			_actionsNotAlwaysInOverflow.Add(key, new VideoOSActionInfo(actions[key], key, frameworkElement2));
			AutomationProperties.SetAutomationId(frameworkElement2, key.AutomationId);
			base.Children.Add(frameworkElement2);
		}
		_actionsNotAlwaysInOverflowOrderedByOverflowOrder.Clear();
		_actionsNotAlwaysInOverflowOrderedByOverflowOrder.AddRange(from x in _actionsNotAlwaysInOverflow.Keys
			orderby x.OverflowOrder, actions[x] descending
			select x);
		InvalidateMeasure();
	}

	private void Action_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "IsVisible")
		{
			Refresh();
		}
	}

	private void ActionsPresenter_OnLoaded(object sender, RoutedEventArgs e)
	{
		if (_overflowMenuControl == null)
		{
			switch (ControlSize)
			{
			case ControlSizes.Medium:
				_overflowMenuControl = new VideoOSMenuButtonTransparentMedium();
				break;
			case ControlSizes.Small:
				_overflowMenuControl = new VideoOSMenuButtonTransparentSmall();
				break;
			case ControlSizes.ExtraSmall:
				_overflowMenuControl = new VideoOSMenuButtonTransparentExtraSmall();
				break;
			default:
				throw new ArgumentOutOfRangeException("ControlSize", ControlSize, null);
			}
			_overflowMenuControl.Visibility = Visibility.Hidden;
			_overflowMenuControl.IconSource = new VideoOSIconBuiltInSource
			{
				Icon = VideoOSIconBuiltInSource.Icons.Ellipsis
			};
			_overflowMenuControl.IsMenuIndicatorIconVisible = false;
			_overflowMenuControl.Opened += _overflowMenuControl_Opened;
			_overflowMenuControl.Closed += _overflowMenuControl_Closed;
			_overflowMenuControl.SizeChanged += _overflowMenuControl_SizeChanged;
			_overflowMenuControl.ToolTip = Strings.VideoOSActionList_OverflowButtonTooltip;
			Binding binding = new Binding("AreActionsFocusable")
			{
				RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(VideoOSActionListBase), 1)
			};
			_overflowMenuControl.SetBinding(UIElement.FocusableProperty, binding);
			AutomationProperties.SetAutomationId(_overflowMenuControl, "OverflowMenuControl");
			base.Children.Add(_overflowMenuControl);
		}
		UpdateChildren();
	}

	private void ActionGroupControlHolder_IsOpenChanged(object sender, EventArgs e)
	{
		UpdateIsOpenProperty();
	}

	private void _overflowMenuControl_Opened(object sender, RoutedEventArgs e)
	{
		UpdateIsOpenProperty();
	}

	private void _overflowMenuControl_Closed(object sender, RoutedEventArgs e)
	{
		UpdateIsOpenProperty();
	}

	private void UpdateIsOpenProperty()
	{
		IsOpen = _overflowMenuControl.IsOpen || _actionsNotAlwaysInOverflow.Values.Where((VideoOSActionInfo x) => x.FrameworkElement is ActionGroupControlHolder).Any((VideoOSActionInfo x) => ((ActionGroupControlHolder)x.FrameworkElement).IsOpen);
	}

	private void ActionControl_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		Refresh();
	}

	private void ActionFrameworkElement_Loaded(object sender, RoutedEventArgs e)
	{
		Refresh();
	}

	private void _overflowMenuControl_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		Refresh();
	}

	private bool IsFullyLoaded()
	{
		if (base.IsLoaded && _overflowMenuControl != null)
		{
			return _actionsNotAlwaysInOverflow.Values.All((VideoOSActionInfo info) => info.FrameworkElement.IsLoaded);
		}
		return false;
	}

	private double GetActionListHeight()
	{
		switch (ControlSize)
		{
		case ControlSizes.ExtraSmall:
		case ControlSizes.Small:
			return 24.0;
		case ControlSizes.Medium:
			return 32.0;
		default:
			throw new ArgumentOutOfRangeException("ControlSize", ControlSize, null);
		}
	}

	private double GetNeededWidthForVisibleActions(Dictionary<VideoOSActionBase, VideoOSActionInfo> actionsNotInOverflow, bool overflowButtonVisible)
	{
		double num = actionsNotInOverflow.Where((KeyValuePair<VideoOSActionBase, VideoOSActionInfo> x) => x.Key.IsVisible).Sum((KeyValuePair<VideoOSActionBase, VideoOSActionInfo> x) => x.Value.FrameworkElement.ActualWidth) + (double)Math.Max(0, actionsNotInOverflow.Count((KeyValuePair<VideoOSActionBase, VideoOSActionInfo> a) => a.Key.IsVisible) - 1) * Spacing;
		if (overflowButtonVisible)
		{
			if (actionsNotInOverflow.Any((KeyValuePair<VideoOSActionBase, VideoOSActionInfo> a) => a.Key.IsVisible))
			{
				num += Spacing;
			}
			num += _overflowMenuControl.ActualWidth;
		}
		return num;
	}

	public void UpdateChildPositions(double availableWidth)
	{
		if (!IsFullyLoaded() || availableWidth == 0.0)
		{
			return;
		}
		HashSet<VideoOSActionBase> hashSet = new HashSet<VideoOSActionBase>();
		Dictionary<VideoOSActionBase, VideoOSActionInfo> dictionary = TrimActionsNotInOverflow();
		List<VideoOSActionBase> list = _actionsNotAlwaysInOverflowOrderedByOverflowOrder.ToList();
		while (dictionary.Any())
		{
			if (dictionary.Where((KeyValuePair<VideoOSActionBase, VideoOSActionInfo> x) => x.Key.IsVisible).Sum((KeyValuePair<VideoOSActionBase, VideoOSActionInfo> x) => x.Value.FrameworkElement.ActualWidth) + Spacing * (double)(dictionary.Count((KeyValuePair<VideoOSActionBase, VideoOSActionInfo> a) => a.Key.IsVisible) - 1) + ((_actionsAlwaysInOverflow.Any((VideoOSActionBase a) => a.IsVisible) || hashSet.Any((VideoOSActionBase a) => a.IsVisible)) ? (_overflowMenuControl.ActualWidth + Spacing) : 0.0) - availableWidth > 1E-05)
			{
				VideoOSActionBase videoOSActionBase = list.First();
				hashSet.Add(videoOSActionBase);
				dictionary.Remove(videoOSActionBase);
				list.Remove(videoOSActionBase);
				if (IsLastActionSeparatorAction(dictionary, out var separator))
				{
					hashSet.Add(separator);
					dictionary.Remove(separator);
					list.Remove(separator);
				}
			}
			else
			{
				if (!IsLastActionSeparatorAction(dictionary, out var separator2))
				{
					break;
				}
				hashSet.Add(separator2);
				dictionary.Remove(separator2);
				list.Remove(separator2);
			}
		}
		bool flag = _actionsAlwaysInOverflow.Any((VideoOSActionBase a) => a.IsVisible) || hashSet.Any((VideoOSActionBase a) => a.IsVisible);
		double neededWidthForVisibleActions = GetNeededWidthForVisibleActions(dictionary, flag);
		double num = 0.0;
		if (HorizontalContentAlignment == HorizontalAlignment.Right)
		{
			num = availableWidth - neededWidthForVisibleActions;
		}
		else if (HorizontalContentAlignment == HorizontalAlignment.Center)
		{
			num = (availableWidth - neededWidthForVisibleActions) / 2.0;
		}
		double actionListHeight = GetActionListHeight();
		if (Actions != null)
		{
			List<VideoOSActionBase> list2 = new List<VideoOSActionBase>(Actions);
			foreach (VideoOSActionBase item in list2)
			{
				if (!_actionsNotAlwaysInOverflow.ContainsKey(item))
				{
					continue;
				}
				FrameworkElement frameworkElement = _actionsNotAlwaysInOverflow[item].FrameworkElement;
				if (item.IsVisible && dictionary.ContainsKey(item))
				{
					frameworkElement.Visibility = Visibility.Visible;
					Canvas.SetLeft(frameworkElement, num);
					Canvas.SetTop(frameworkElement, Math.Max(0.0, (actionListHeight - frameworkElement.ActualHeight) / 2.0));
					num += frameworkElement.ActualWidth;
					if (list2.Last() != item || flag)
					{
						num += Spacing;
					}
				}
				else
				{
					frameworkElement.Visibility = Visibility.Hidden;
				}
			}
		}
		List<VideoOSActionBase> overflowMenuActions = GetOverflowMenuActions(hashSet);
		if (flag && overflowMenuActions.Any((VideoOSActionBase a) => a.IsVisible))
		{
			_overflowMenuControl.Actions = overflowMenuActions;
			_overflowMenuControl.Visibility = Visibility.Visible;
			Canvas.SetLeft(_overflowMenuControl, num);
			Canvas.SetTop(_overflowMenuControl, Math.Max(0.0, (actionListHeight - _overflowMenuControl.ActualHeight) / 2.0));
		}
		else
		{
			_overflowMenuControl.Visibility = Visibility.Hidden;
			_overflowMenuControl.Actions = null;
		}
	}

	private Dictionary<VideoOSActionBase, VideoOSActionInfo> TrimActionsNotInOverflow()
	{
		Dictionary<VideoOSActionBase, VideoOSActionInfo> dictionary = new Dictionary<VideoOSActionBase, VideoOSActionInfo>(_actionsNotAlwaysInOverflow);
		if (Actions == null)
		{
			return dictionary;
		}
		List<VideoOSActionBase> list = new List<VideoOSActionBase>(Actions);
		foreach (VideoOSActionBase item in list)
		{
			if (!item.IsVisible || item is VideoOSActionSeparator)
			{
				dictionary.Remove(item);
				continue;
			}
			break;
		}
		for (int num = list.Count - 1; num >= 0; num--)
		{
			VideoOSActionBase videoOSActionBase = list[num];
			if (videoOSActionBase.IsVisible && !(videoOSActionBase is VideoOSActionSeparator))
			{
				break;
			}
			dictionary.Remove(videoOSActionBase);
		}
		return dictionary;
	}

	private static bool IsLastActionSeparatorAction(Dictionary<VideoOSActionBase, VideoOSActionInfo> actionsNotInOverflow, out VideoOSActionSeparator separator)
	{
		separator = null;
		if (actionsNotInOverflow.Any() && (from info in actionsNotInOverflow.Values
			where info.Action.IsVisible
			orderby info.Index
			select info).LastOrDefault()?.Action is VideoOSActionSeparator videoOSActionSeparator)
		{
			separator = videoOSActionSeparator;
			return true;
		}
		return false;
	}

	private List<VideoOSActionBase> GetOverflowMenuActions(HashSet<VideoOSActionBase> actionsTemporaryInOverflow)
	{
		if (Actions == null)
		{
			return null;
		}
		List<VideoOSActionBase> list = Actions.Where((VideoOSActionBase x) => (_actionsAlwaysInOverflow.Contains(x) || actionsTemporaryInOverflow.Contains(x)) && x.IsVisible).ToList();
		HashSet<object> hashSet = new HashSet<object>();
		foreach (VideoOSActionBase item in list)
		{
			if (item is VideoOSActionSeparator)
			{
				hashSet.Add(item);
				continue;
			}
			break;
		}
		for (int num = list.Count - 1; num >= 0; num--)
		{
			VideoOSActionBase videoOSActionBase = list[num];
			if (!(videoOSActionBase is VideoOSActionSeparator))
			{
				break;
			}
			hashSet.Add(videoOSActionBase);
		}
		list.RemoveAll(hashSet.Contains);
		return list;
	}
}
