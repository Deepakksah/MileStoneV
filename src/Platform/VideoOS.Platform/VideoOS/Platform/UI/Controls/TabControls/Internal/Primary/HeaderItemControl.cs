using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public partial class HeaderItemControl : UserControl, INotifyPropertyChanged, IComponentConnector
{
	private int __headerContentPadding = MaxHeaderContentPadding;

	public static IMultiValueConverter IsTabStopConverter { get; } = new LambdaConverter2<object, object>((object item, object selectedItem) => item == selectedItem, "IsTabStopConverter");

	public static IMultiValueConverter BackgroundConverter { get; } = new LambdaConverter7<DependencyObject, object, object, bool, bool, bool, bool>(delegate(DependencyObject self, object item, object selectedItem, bool isEnabled, bool isMouseOver, bool isActionListOpen, bool isMouseLeftButtonDown)
	{
		object resourceKey = ((item == selectedItem) ? ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundSelectedDisabledBrush" : (isMouseLeftButtonDown ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundSelectedPressedBrush" : ((!(isMouseOver || isActionListOpen)) ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundSelectedIdleBrush" : "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundSelectedHoveredBrush"))) : ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundDeselectedDisabledBrush" : (isMouseLeftButtonDown ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundDeselectedPressedBrush" : ((!(isMouseOver || isActionListOpen)) ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundDeselectedIdleBrush" : "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabBackgroundDeselectedHoveredBrush"))));
		return self.FindResource(resourceKey);
	}, "BackgroundConverter");

	public static IMultiValueConverter SelectedIndicatorFillConverter { get; } = new LambdaConverter4<DependencyObject, object, object, bool>(delegate(DependencyObject self, object item, object selectedItem, bool isEnabled)
	{
		object resourceKey = ((item != selectedItem) ? ((object)VideoOSResourceKeys.TransparentBrush) : (isEnabled ? "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabSelectedIndicatorIdleBrush" : "VideoOSResourceKeysInternalVideoOSTabControlHeaderTabSelectedIndicatorDisabledBrush"));
		return self.FindResource(resourceKey);
	}, "SelectedIndicatorFillConverter");

	public static IMultiValueConverter ActionListVisibilityConverter { get; } = new LambdaConverter5<ICollection<VideoOSActionBase>, object, object, bool, bool>((ICollection<VideoOSActionBase> actions, object item, object selectedItem, bool isOpen, bool isMouseOver) => (actions != null && actions.Count > 0) ? ((item == selectedItem || isOpen || isMouseOver) ? ((object)Visibility.Visible) : ((object)Visibility.Hidden)) : ((object)Visibility.Collapsed), "ActionListVisibilityConverter");

	public static IMultiValueConverter TextMaxWidthConverter { get; } = new LambdaConverter6<VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes, double, Visibility, double, Visibility, double>(delegate(VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes itemSizeMode, double iconTextAndAlertGrid, Visibility iconVisibility, double iconWidth, Visibility alertVisibility, double alertWidth)
	{
		switch (itemSizeMode)
		{
		case VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes.Nonuniform:
			return 208.0;
		case VideoOSTabControlPrimaryHorizontalBase.ItemSizeModes.Uniform:
		{
			double val = iconTextAndAlertGrid - ((iconVisibility == Visibility.Visible) ? (iconWidth + 8.0) : 0.0) - ((alertVisibility == Visibility.Visible) ? (alertWidth + 8.0) : 0.0);
			return Math.Max(0.0, val);
		}
		default:
			throw new ArgumentOutOfRangeException("itemSizeMode", itemSizeMode, null);
		}
	}, "TextMaxWidthConverter");

	public static IMultiValueConverter ContentGridMarginLeft { get; } = new LambdaConverter1<int>((int headerContentPadding) => new GridLength(ResolveContentMarginLeft(headerContentPadding)), "ContentGridMarginLeft");

	public static IMultiValueConverter ContentGridMarginRight { get; } = new LambdaConverter2<int, ICollection>((int headerContentPadding, ICollection actions) => new GridLength(ResolveContentMarginRight(headerContentPadding, actions)), "ContentGridMarginRight");

	public static IMultiValueConverter IconTextSpacingConverter { get; } = new LambdaConverter3<Visibility, Visibility, Visibility>((Visibility iconVisibility, Visibility textVisibility, Visibility alertVisibility) => (iconVisibility == Visibility.Visible && (textVisibility == Visibility.Visible || alertVisibility == Visibility.Visible)) ? ((object)new GridLength(8.0)) : ((object)new GridLength(0.0)), "IconTextSpacingConverter");

	public static IMultiValueConverter TextAlertSpacingConverter { get; } = new LambdaConverter2<Visibility, Visibility>((Visibility textVisibility, Visibility alertVisibility) => (textVisibility == Visibility.Visible && alertVisibility == Visibility.Visible) ? ((object)new GridLength(8.0)) : ((object)new GridLength(0.0)), "TextAlertSpacingConverter");

	public static int MaxHeaderContentPadding => 24;

	public static int MinHeaderContentPadding => 8;

	public Size CurrentSize => new Size(ResolveWidthWithHeaderContentPadding(HeaderContentPadding), base.ActualHeight);

	public int HeaderContentPadding
	{
		get
		{
			return __headerContentPadding;
		}
		set
		{
			if (__headerContentPadding != value)
			{
				__headerContentPadding = value;
				OnPropertyChanged("HeaderContentPadding");
			}
		}
	}

	public event EventHandler<EventArgs> TabHeaderSizeChanged;

	public event PropertyChangedEventHandler PropertyChanged;

	private static int ResolveContentMarginLeft(int headerContentPadding)
	{
		return headerContentPadding;
	}

	private static int ResolveContentMarginRight(int headerContentPadding, ICollection actions)
	{
		if (actions == null || actions.Count <= 0)
		{
			return headerContentPadding;
		}
		return 32;
	}

	public double ResolveWidthWithHeaderContentPadding(int headerContentPadding)
	{
		return _iconTextAndAlertGrid.ActualWidth + (double)ResolveContentMarginLeft(headerContentPadding) + (double)ResolveContentMarginRight(headerContentPadding, (ICollection)_actionList.Actions);
	}

	public HeaderItemControl()
	{
		InitializeComponent();
	}

	private void ContentGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		this.TabHeaderSizeChanged?.Invoke(this, EventArgs.Empty);
	}

	private void HeaderItemControl_OnDragInitiated(object sender, RoutedEventArgs e)
	{
		this.FindAncestor<VideoOSTabControlBase>().RaiseEvent(new VideoOSTabControlBase.ItemDragInitiatedEventArgs(VideoOSTabControlBase.ItemDragInitiatedEvent, base.DataContext, this));
	}

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
