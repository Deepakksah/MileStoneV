using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSButtonBase : Button
{
	public static class VideoOSButtonBaseInternals
	{
		public const double ButtonLargeDesiredMinWidth = 80.0;

		public const double ButtonLargeIconOnlyDesiredMinWidth = 40.0;

		public const double ButtonMediumDesiredMinWidth = 64.0;

		public const double ButtonMediumIconOnlyDesiredMinWidth = 32.0;

		public const double ButtonSmallDesiredMinWidth = 48.0;

		public const double ButtonSmallIconOnlyDesiredMinWidth = 24.0;

		public const double ButtonExtraSmallDesiredMinWidth = 32.0;

		public const double ButtonExtraSmallIconOnlyDesiredMinWidth = 16.0;

		public static IMultiValueConverter MinimumWidthConverter { get; } = new LambdaConverter7<double, double, double, double, double, string, bool>(delegate(double minWidth, double maxWidth, double desiredMinWidth, double desiredMinWidthIconOnly, double availableWidth, string buttonText, bool isChevronDownIconVisible)
		{
			double val = ((string.IsNullOrWhiteSpace(buttonText) && !isChevronDownIconVisible) ? desiredMinWidthIconOnly : desiredMinWidth);
			double num = Math.Min(availableWidth, val);
			if (num < minWidth)
			{
				num = minWidth;
			}
			return (num < maxWidth) ? num : maxWidth;
		}, "MinimumWidthConverter");

		public static IMultiValueConverter VideoOSTextBlockInteractiveMediumInteractiveStateConverter { get; } = new LambdaConverter3<bool, bool, bool>(delegate(bool forcePressedState, bool isMouseOver, bool isPressed)
		{
			VideoOSTextBlockInteractiveMedium.InteractiveStates interactiveStates = ((forcePressedState || isPressed) ? VideoOSTextBlockInteractiveMedium.InteractiveStates.Pressed : (isMouseOver ? VideoOSTextBlockInteractiveMedium.InteractiveStates.Hovered : VideoOSTextBlockInteractiveMedium.InteractiveStates.Idle));
			return interactiveStates;
		}, "VideoOSTextBlockInteractiveMediumInteractiveStateConverter");

		public static IMultiValueConverter VideoOSTextBlockInteractiveSmallInteractiveStateConverter { get; } = new LambdaConverter3<bool, bool, bool>(delegate(bool forcePressedState, bool isMouseOver, bool isPressed)
		{
			VideoOSTextBlockInteractiveSmall.InteractiveStates interactiveStates = ((forcePressedState || isPressed) ? VideoOSTextBlockInteractiveSmall.InteractiveStates.Pressed : (isMouseOver ? VideoOSTextBlockInteractiveSmall.InteractiveStates.Hovered : VideoOSTextBlockInteractiveSmall.InteractiveStates.Idle));
			return interactiveStates;
		}, "VideoOSTextBlockInteractiveSmallInteractiveStateConverter");

		public static IMultiValueConverter VideoOSTextBlockInteractiveExtraSmallInteractiveStateConverter { get; } = new LambdaConverter3<bool, bool, bool>(delegate(bool forcePressedState, bool isMouseOver, bool isPressed)
		{
			VideoOSTextBlockInteractiveExtraSmall.InteractiveStates interactiveStates = ((forcePressedState || isPressed) ? VideoOSTextBlockInteractiveExtraSmall.InteractiveStates.Pressed : (isMouseOver ? VideoOSTextBlockInteractiveExtraSmall.InteractiveStates.Hovered : VideoOSTextBlockInteractiveExtraSmall.InteractiveStates.Idle));
			return interactiveStates;
		}, "VideoOSTextBlockInteractiveExtraSmallInteractiveStateConverter");

		public static IValueConverter VideoOSTextBlockInteractiveMediumAppearanceConverter { get; } = new LambdaConverter1<bool>(delegate(bool isDestructive)
		{
			VideoOSTextBlockInteractiveMedium.Appearances appearances = ((!isDestructive) ? VideoOSTextBlockInteractiveMedium.Appearances.Primary : VideoOSTextBlockInteractiveMedium.Appearances.Destructive);
			return appearances;
		}, "VideoOSTextBlockInteractiveMediumAppearanceConverter");

		public static IValueConverter VideoOSTextBlockInteractiveSmallAppearanceConverter { get; } = new LambdaConverter1<bool>(delegate(bool isDestructive)
		{
			VideoOSTextBlockInteractiveSmall.Appearances appearances = ((!isDestructive) ? VideoOSTextBlockInteractiveSmall.Appearances.Primary : VideoOSTextBlockInteractiveSmall.Appearances.Destructive);
			return appearances;
		}, "VideoOSTextBlockInteractiveSmallAppearanceConverter");

		public static IValueConverter VideoOSTextBlockInteractiveExtraSmallAppearanceConverter { get; } = new LambdaConverter1<bool>(delegate(bool isDestructive)
		{
			VideoOSTextBlockInteractiveExtraSmall.Appearances appearances = ((!isDestructive) ? VideoOSTextBlockInteractiveExtraSmall.Appearances.Primary : VideoOSTextBlockInteractiveExtraSmall.Appearances.Destructive);
			return appearances;
		}, "VideoOSTextBlockInteractiveExtraSmallAppearanceConverter");
	}

	public static readonly RoutedEvent IsPressedChangedEvent;

	public static readonly DependencyProperty IconSourceProperty;

	public static readonly DependencyProperty EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty;

	public static readonly DependencyProperty IsChevronDownIconVisibleProperty;

	public static readonly DependencyProperty ForcePressedStateProperty;

	public static readonly DependencyProperty AvailableWidthProperty;

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

	public bool EnsureFocusIsNotLostWhenActivatedUsingSpaceKey
	{
		get
		{
			return (bool)GetValue(EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty);
		}
		set
		{
			SetValue(EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty, value);
		}
	}

	public bool IsChevronDownIconVisible
	{
		get
		{
			return (bool)GetValue(IsChevronDownIconVisibleProperty);
		}
		set
		{
			SetValue(IsChevronDownIconVisibleProperty, value);
		}
	}

	public bool ForcePressedState
	{
		get
		{
			return (bool)GetValue(ForcePressedStateProperty);
		}
		set
		{
			SetValue(ForcePressedStateProperty, value);
		}
	}

	public double AvailableWidth
	{
		get
		{
			return (double)GetValue(AvailableWidthProperty);
		}
		private set
		{
			SetValue(AvailableWidthProperty, value);
		}
	}

	public event RoutedEventHandler IsPressedChanged
	{
		add
		{
			AddHandler(IsPressedChangedEvent, value);
		}
		remove
		{
			RemoveHandler(IsPressedChangedEvent, value);
		}
	}

	static VideoOSButtonBase()
	{
		IsPressedChangedEvent = EventManager.RegisterRoutedEvent("IsPressedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSButtonBase));
		IconSourceProperty = DependencyProperty.Register("IconSource", typeof(VideoOSIconSourceBase), typeof(VideoOSButtonBase), new PropertyMetadata(null));
		EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty = DependencyProperty.Register("EnsureFocusIsNotLostWhenActivatedUsingSpaceKey", typeof(bool), typeof(VideoOSButtonBase), new PropertyMetadata(false));
		IsChevronDownIconVisibleProperty = DependencyProperty.Register("IsChevronDownIconVisible", typeof(bool), typeof(VideoOSButtonBase), new PropertyMetadata(false));
		ForcePressedStateProperty = DependencyProperty.Register("ForcePressedState", typeof(bool), typeof(VideoOSButtonBase), new PropertyMetadata(false));
		AvailableWidthProperty = DependencyProperty.Register("AvailableWidth", typeof(double), typeof(VideoOSButtonBase), new PropertyMetadata(0.0));
		FrameworkElement.CursorProperty.OverrideMetadata(typeof(VideoOSButtonBase), new FrameworkPropertyMetadata(Cursors.Hand));
	}

	public VideoOSButtonBase()
	{
		base.PreviewKeyDown += VideoOSButtonBase_PreviewKeyDown;
	}

	protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
	{
		base.OnIsPressedChanged(e);
		RaiseEvent(new RoutedEventArgs(IsPressedChangedEvent));
	}

	protected override Size MeasureOverride(Size constraint)
	{
		AvailableWidth = constraint.Width;
		return base.MeasureOverride(constraint);
	}

	private void VideoOSButtonBase_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (EnsureFocusIsNotLostWhenActivatedUsingSpaceKey && e.Key == Key.Space)
		{
			RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
			base.Command?.Execute(base.CommandParameter);
			e.Handled = true;
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSButtonBaseAutomationPeer(this);
	}
}
