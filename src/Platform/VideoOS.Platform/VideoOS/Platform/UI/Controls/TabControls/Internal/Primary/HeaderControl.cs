using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public partial class HeaderControl : UserControl, IComponentConnector
{
	public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ICollection), typeof(HeaderControl), new PropertyMetadata(null, CollectionHelperBase.CollectionChangedCallback));

	public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(HeaderControl), new PropertyMetadata(null, SelectedItemChangedCallback));

	public static readonly DependencyProperty TabMarginProperty = DependencyProperty.Register("TabMargin", typeof(Thickness), typeof(HeaderControl), new PropertyMetadata(null));

	public static readonly DependencyProperty ItemSizeModeProperty = DependencyProperty.Register("ItemSizeMode", typeof(VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes), typeof(HeaderControl), new PropertyMetadata(VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes.Nonuniform, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((HeaderControl)o).InvalidateMeasure();
	}));

	public static readonly DependencyProperty ItemSeparatorPlacementProperty = DependencyProperty.Register("ItemSeparatorPlacement", typeof(VideoOSTabControlBase.ItemSeparatorPlacements), typeof(HeaderControl), new FrameworkPropertyMetadata(VideoOSTabControlBase.ItemSeparatorPlacements.Between, FrameworkPropertyMetadataOptions.AffectsMeasure));

	private readonly List<HeaderItemControl> _headerItemControls = new List<HeaderItemControl>();

	private bool _invalidateMeasurePending;

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
		set
		{
			SetValue(SelectedItemProperty, value);
		}
	}

	public Thickness TabMargin
	{
		get
		{
			return (Thickness)GetValue(TabMarginProperty);
		}
		set
		{
			SetValue(TabMarginProperty, value);
		}
	}

	public VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes ItemSizeMode
	{
		get
		{
			return (VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes)GetValue(ItemSizeModeProperty);
		}
		set
		{
			SetValue(ItemSizeModeProperty, value);
		}
	}

	public VideoOSTabControlBase.ItemSeparatorPlacements ItemSeparatorPlacement
	{
		get
		{
			return (VideoOSTabControlBase.ItemSeparatorPlacements)GetValue(ItemSeparatorPlacementProperty);
		}
		set
		{
			SetValue(ItemSeparatorPlacementProperty, value);
		}
	}

	private static void SelectedItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		HeaderControl headerControl = (HeaderControl)d;
		if (!headerControl.FindAncestor<VideoOSTabControlBase>().IsKeyboardFocusWithin)
		{
			return;
		}
		HeaderItemControl selectedHeaderItem = headerControl._canvas.Children.OfType<HeaderItemControl>().FirstOrDefault((HeaderItemControl hic) => hic.DataContext == headerControl.SelectedItem);
		if (selectedHeaderItem != null && selectedHeaderItem.IsVisible)
		{
			headerControl.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate
			{
				selectedHeaderItem.Focus();
			});
		}
		else if (headerControl.SelectedItem != null)
		{
			headerControl.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate
			{
				headerControl._headerItemOverflowMenu.Focus();
			});
		}
	}

	public HeaderControl()
	{
		CollectionHelper.Initialize(this, ItemsSourceProperty, ItemsCollectionChanged);
		InitializeComponent();
		_headerItemOverflowMenu.DataContext = null;
		UpdateChildren();
	}

	private void ItemsCollectionChanged(CollectionChangedInfo obj)
	{
		UpdateChildren();
	}

	protected override Size MeasureOverride(Size constraint)
	{
		base.MeasureOverride(constraint);
		List<HeaderItemControl> list = new List<HeaderItemControl>();
		List<HeaderItemControl> list2 = new List<HeaderItemControl>(_headerItemControls);
		double tabWidth = double.NaN;
		if (ItemSizeMode == VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes.Uniform)
		{
			double num = 144.0;
			double val = 224.0;
			while (list2.Any())
			{
				double num2 = constraint.Width;
				if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.Between))
				{
					num2 -= (double)(list2.Count - 1);
				}
				if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.BeforeFirst))
				{
					num2 -= 1.0;
				}
				if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.AfterLast))
				{
					num2 -= 1.0;
				}
				if (list.Any())
				{
					num2 -= _headerItemOverflowMenu.ActualWidth;
					if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.Between))
					{
						num2 -= 1.0;
					}
				}
				tabWidth = num2 / (double)list2.Count;
				if (tabWidth >= num)
				{
					break;
				}
				list.Add(list2.Last());
				list2.Remove(list2.Last());
			}
			if (list.Any())
			{
				tabWidth = num;
			}
			else
			{
				tabWidth = Math.Min(tabWidth, val);
			}
			_headerItemControls.ForEach(delegate(HeaderItemControl x)
			{
				x.HeaderContentPadding = HeaderItemControl.MaxHeaderContentPadding;
				x.Width = tabWidth;
			});
		}
		else
		{
			_headerItemControls.ForEach(delegate(HeaderItemControl x)
			{
				x.Width = double.NaN;
			});
			int headerContentPadding = HeaderItemControl.MaxHeaderContentPadding;
			while (true)
			{
				double num3 = constraint.Width;
				if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.Between))
				{
					num3 -= (double)(list2.Count - 1);
				}
				if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.BeforeFirst))
				{
					num3 -= 1.0;
				}
				if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.AfterLast))
				{
					num3 -= 1.0;
				}
				if (_headerItemControls.Sum((HeaderItemControl x) => x.ResolveWidthWithHeaderContentPadding(headerContentPadding)) + (double)_headerItemControls.Count - 1.0 <= num3)
				{
					_headerItemControls.ForEach(delegate(HeaderItemControl x)
					{
						x.HeaderContentPadding = headerContentPadding;
					});
					break;
				}
				if (headerContentPadding == HeaderItemControl.MinHeaderContentPadding)
				{
					double num4 = 0.0;
					foreach (HeaderItemControl headerItemControl in _headerItemControls)
					{
						if (!list.Any() && num4 + headerItemControl.ResolveWidthWithHeaderContentPadding(HeaderItemControl.MinHeaderContentPadding) <= num3 - _headerItemOverflowMenu.ActualWidth)
						{
							num4 += headerItemControl.CurrentSize.Width;
							if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.Between))
							{
								num4 += 1.0;
							}
						}
						else
						{
							list.Add(headerItemControl);
							list2.Remove(headerItemControl);
						}
					}
					_headerItemControls.ForEach(delegate(HeaderItemControl x)
					{
						x.HeaderContentPadding = HeaderItemControl.MinHeaderContentPadding;
					});
					break;
				}
				headerContentPadding--;
			}
		}
		List<ContentControl> list3 = new List<ContentControl>();
		double height = 0.0;
		double num5 = 0.0;
		if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.BeforeFirst))
		{
			_separatorBeforeFirstTab.Visibility = Visibility.Visible;
			list3.Add(_separatorBeforeFirstTab);
			Canvas.SetLeft(_separatorBeforeFirstTab, num5);
			num5 += 1.0;
		}
		else
		{
			_separatorBeforeFirstTab.Visibility = Visibility.Hidden;
		}
		foreach (HeaderItemControl item in list2)
		{
			height = Math.Max(height, item.ActualHeight);
			item.Visibility = Visibility.Visible;
			Canvas.SetLeft(item, num5);
			num5 += (double.IsNaN(tabWidth) ? item.CurrentSize.Width : tabWidth);
			if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.Between) && (list.Any() || item != list2.Last()))
			{
				ContentControl contentControl = (ContentControl)item.Tag;
				contentControl.Visibility = Visibility.Visible;
				list3.Add(contentControl);
				Canvas.SetLeft(contentControl, num5);
				num5 += 1.0;
			}
			else
			{
				((ContentControl)item.Tag).Visibility = Visibility.Hidden;
			}
		}
		if (list.Any())
		{
			foreach (HeaderItemControl item2 in list)
			{
				item2.Visibility = Visibility.Hidden;
				((ContentControl)item2.Tag).Visibility = Visibility.Hidden;
			}
			height = Math.Max(height, _headerItemOverflowMenu.ActualHeight);
			_headerItemOverflowMenu.Items = list.Select((HeaderItemControl x) => x.DataContext).Reverse().ToList();
			_headerItemOverflowMenu.Visibility = Visibility.Visible;
			Canvas.SetLeft(_headerItemOverflowMenu, num5);
			num5 += _headerItemOverflowMenu.ActualWidth;
		}
		else
		{
			_headerItemOverflowMenu.Items = null;
			_headerItemOverflowMenu.Visibility = Visibility.Hidden;
		}
		if (ItemSeparatorPlacement.HasFlag(VideoOSTabControlBase.ItemSeparatorPlacements.AfterLast))
		{
			_separatorAfterLastTab.Visibility = Visibility.Visible;
			list3.Add(_separatorAfterLastTab);
			Canvas.SetLeft(_separatorAfterLastTab, num5);
			num5 += 1.0;
		}
		else
		{
			_separatorAfterLastTab.Visibility = Visibility.Hidden;
		}
		list3.ForEach(delegate(ContentControl x)
		{
			Canvas.SetTop(x, (int)(height - x.ActualHeight) / 2);
		});
		_canvas.InvalidateArrange();
		return new Size(num5, height);
	}

	private void UpdateChildren()
	{
		HashSet<object> hashSet = new HashSet<object>();
		if (ItemsSource != null)
		{
			foreach (object item in ItemsSource)
			{
				hashSet.Add(item);
			}
		}
		Dictionary<object, HeaderItemControl> dictionary = _headerItemControls.ToDictionary((HeaderItemControl x) => x.DataContext, (HeaderItemControl x) => x);
		foreach (KeyValuePair<object, HeaderItemControl> item2 in dictionary)
		{
			if (!hashSet.Contains(item2.Key))
			{
				item2.Value.TabHeaderSizeChanged -= HeaderItemControl_TabHeaderSizeChanged;
				item2.Value.MouseLeftButtonDown -= HeaderItemControl_MouseLeftButtonDownOrUp;
				item2.Value.MouseLeftButtonUp -= HeaderItemControl_MouseLeftButtonDownOrUp;
				_headerItemControls.Remove(item2.Value);
				_canvas.Children.Remove((ContentControl)item2.Value.Tag);
				_canvas.Children.Remove(item2.Value);
			}
		}
		foreach (object item3 in hashSet)
		{
			if (!dictionary.ContainsKey(item3))
			{
				ContentControl contentControl = new ContentControl();
				contentControl.Focusable = false;
				contentControl.ContentTemplate = (DataTemplate)FindResource("TabSeparatorTemplate");
				HeaderItemControl headerItemControl = new HeaderItemControl();
				headerItemControl.DataContext = item3;
				headerItemControl.TabHeaderSizeChanged += HeaderItemControl_TabHeaderSizeChanged;
				headerItemControl.MouseLeftButtonDown += HeaderItemControl_MouseLeftButtonDownOrUp;
				headerItemControl.MouseLeftButtonUp += HeaderItemControl_MouseLeftButtonDownOrUp;
				headerItemControl.Tag = contentControl;
				_headerItemControls.Add(headerItemControl);
				_canvas.Children.Add(headerItemControl);
				_canvas.Children.Add(contentControl);
			}
		}
		ScheduleInvalidateMeasure();
	}

	private void HeaderItemControl_MouseLeftButtonDownOrUp(object sender, MouseButtonEventArgs e)
	{
		VideoOSTabControlBase videoOSTabControlBase = this.FindAncestor<VideoOSTabControlBase>();
		if ((videoOSTabControlBase.ItemMouseSelectionMode == VideoOSTabControlBase.ItemMouseSelectionModes.LeftButtonDown && e.ButtonState == MouseButtonState.Pressed) || (videoOSTabControlBase.ItemMouseSelectionMode == VideoOSTabControlBase.ItemMouseSelectionModes.LeftButtonUp && e.ButtonState == MouseButtonState.Released))
		{
			SelectedItem = ((FrameworkElement)sender).DataContext;
			Focus();
		}
	}

	private void HeaderItemControl_TabHeaderSizeChanged(object sender, EventArgs e)
	{
		ScheduleInvalidateMeasure();
	}

	private void HeaderControl_OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key != Key.Left && e.Key != Key.Right)
		{
			return;
		}
		List<object> list = ItemsSource.OfType<object>().Where(IsItemEnabled).ToList();
		if (list.Any())
		{
			if (e.Key == Key.Right)
			{
				if (SelectedItem == null || SelectedItem == list.Last())
				{
					SelectedItem = list.First();
				}
				else
				{
					SelectedItem = list[list.IndexOf(SelectedItem) + 1];
				}
			}
			else if (e.Key == Key.Left)
			{
				if (SelectedItem == null || SelectedItem == list.First())
				{
					SelectedItem = list.Last();
				}
				else
				{
					SelectedItem = list[list.IndexOf(SelectedItem) - 1];
				}
			}
		}
		e.Handled = true;
	}

	private bool IsItemEnabled(object item)
	{
		if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
		{
			return videoOSTabControlPrimaryItem.IsEnabled;
		}
		VideoOSTabControlBase videoOSTabControlBase = this.FindAncestor<VideoOSTabControlBase>();
		return item.ResolvePropertyValue(videoOSTabControlBase.ItemIsEnabledPropertyName, throwOnError: false, defaultValue: true);
	}

	private void ScheduleInvalidateMeasure()
	{
		if (!_invalidateMeasurePending)
		{
			_invalidateMeasurePending = true;
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate
			{
				InvalidateMeasure();
				_invalidateMeasurePending = false;
			});
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
