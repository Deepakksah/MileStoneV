using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSMessageBarBase : ContentControl
{
	public static class VideoOSMessageBarBaseInternals
	{
		public const int MaxLinesInUnexpandedState = 2;

		public static Size UnconstrainedSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

		public static IMultiValueConverter BackgroundConverter { get; } = new LambdaConverter3<DependencyObject, bool, Appearances>((DependencyObject self, bool isEnabled, Appearances appearance) => self.FindResource(appearance switch
		{
			Appearances.Error => isEnabled ? "VideoOSResourceKeysInternalVideoOSMessageBarErrorBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSMessageBarErrorBackgroundDisabledBrush", 
			Appearances.Warning => isEnabled ? "VideoOSResourceKeysInternalVideoOSMessageBarWarningBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSMessageBarWarningBackgroundDisabledBrush", 
			Appearances.Confirmation => isEnabled ? "VideoOSResourceKeysInternalVideoOSMessageBarConfirmationBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSMessageBarConfirmationBackgroundDisabledBrush", 
			Appearances.Information => isEnabled ? "VideoOSResourceKeysInternalVideoOSMessageBarInformationBackgroundIdleBrush" : "VideoOSResourceKeysInternalVideoOSMessageBarInformationBackgroundDisabledBrush", 
			_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
		}), "BackgroundConverter");

		public static IMultiValueConverter BarBackgroundConverter { get; } = new LambdaConverter3<DependencyObject, bool, Appearances>((DependencyObject self, bool isEnabled, Appearances appearance) => self.FindResource(appearance switch
		{
			Appearances.Error => isEnabled ? "VideoOSResourceKeysInternalVideoOSMessageBarErrorBarIdleBrush" : "VideoOSResourceKeysInternalVideoOSMessageBarErrorBarDisabledBrush", 
			Appearances.Warning => isEnabled ? "VideoOSResourceKeysInternalVideoOSMessageBarWarningBarIdleBrush" : "VideoOSResourceKeysInternalVideoOSMessageBarWarningBarDisabledBrush", 
			Appearances.Confirmation => isEnabled ? "VideoOSResourceKeysInternalVideoOSMessageBarConfirmationBarIdleBrush" : "VideoOSResourceKeysInternalVideoOSMessageBarConfirmationBarDisabledBrush", 
			Appearances.Information => isEnabled ? "VideoOSResourceKeysInternalVideoOSMessageBarInformationBarIdleBrush" : "VideoOSResourceKeysInternalVideoOSMessageBarInformationBarDisabledBrush", 
			_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
		}), "BarBackgroundConverter");

		public static IValueConverter InlineMessageAppearanceConverter { get; } = new LambdaConverter1<Appearances>((Appearances appearance) => appearance switch
		{
			Appearances.Error => VideoOSInlineMessageBase.Appearances.Error, 
			Appearances.Warning => VideoOSInlineMessageBase.Appearances.Warning, 
			Appearances.Confirmation => VideoOSInlineMessageBase.Appearances.Confirmation, 
			Appearances.Information => VideoOSInlineMessageBase.Appearances.Information, 
			_ => throw new ArgumentOutOfRangeException("appearance", appearance, null), 
		}, "InlineMessageAppearanceConverter");
	}

	public enum Appearances
	{
		Error,
		Warning,
		Confirmation,
		Information
	}

	public static readonly DependencyProperty AppearanceProperty;

	public static readonly DependencyProperty CloseButtonVisibilityProperty;

	public static readonly DependencyProperty IsExpandedProperty;

	public static readonly DependencyProperty LinkTextProperty;

	public static readonly DependencyProperty ActionsProperty;

	public static readonly RoutedEvent LinkClickedEvent;

	public static readonly RoutedEvent CloseClickedEvent;

	public static readonly DependencyProperty CloseCommandProperty;

	public static readonly DependencyProperty ToggleExpansionCommandProperty;

	private VideoOSInlineMessageBase _inlineMessage;

	private VideoOSActionListBase _actionList;

	private VideoOSButtonTransparentBase _expansionButton;

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

	public Visibility CloseButtonVisibility
	{
		get
		{
			return (Visibility)GetValue(CloseButtonVisibilityProperty);
		}
		set
		{
			SetValue(CloseButtonVisibilityProperty, value);
		}
	}

	public bool IsExpanded
	{
		get
		{
			return (bool)GetValue(IsExpandedProperty);
		}
		set
		{
			SetValue(IsExpandedProperty, value);
		}
	}

	public string LinkText
	{
		get
		{
			return (string)GetValue(LinkTextProperty);
		}
		set
		{
			SetValue(LinkTextProperty, value);
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

	public DelegateCommand CloseCommand
	{
		get
		{
			return (DelegateCommand)GetValue(CloseCommandProperty);
		}
		private set
		{
			SetValue(CloseCommandProperty, value);
		}
	}

	public DelegateCommand ToggleExpansionCommand
	{
		get
		{
			return (DelegateCommand)GetValue(ToggleExpansionCommandProperty);
		}
		private set
		{
			SetValue(ToggleExpansionCommandProperty, value);
		}
	}

	public abstract Thickness InlineActionListMargin { get; }

	public abstract Thickness OwnlineActionListMargin { get; }

	public event RoutedEventHandler LinkClicked
	{
		add
		{
			AddHandler(LinkClickedEvent, value);
		}
		remove
		{
			RemoveHandler(LinkClickedEvent, value);
		}
	}

	public event RoutedEventHandler CloseClicked
	{
		add
		{
			AddHandler(CloseClickedEvent, value);
		}
		remove
		{
			RemoveHandler(CloseClickedEvent, value);
		}
	}

	static VideoOSMessageBarBase()
	{
		AppearanceProperty = DependencyProperty.Register("Appearance", typeof(Appearances), typeof(VideoOSMessageBarBase), new PropertyMetadata(Appearances.Error));
		CloseButtonVisibilityProperty = DependencyProperty.Register("CloseButtonVisibility", typeof(Visibility), typeof(VideoOSMessageBarBase), new PropertyMetadata(Visibility.Visible));
		IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(VideoOSMessageBarBase), new PropertyMetadata(false));
		LinkTextProperty = DependencyProperty.Register("LinkText", typeof(string), typeof(VideoOSMessageBarBase), new PropertyMetadata((object)null));
		ActionsProperty = DependencyProperty.Register("Actions", typeof(ICollection<VideoOSActionBase>), typeof(VideoOSMessageBarBase), new PropertyMetadata(null));
		LinkClickedEvent = EventManager.RegisterRoutedEvent("LinkClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSMessageBarBase));
		CloseClickedEvent = EventManager.RegisterRoutedEvent("CloseClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSMessageBarBase));
		CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(DelegateCommand), typeof(VideoOSMessageBarBase), new PropertyMetadata((object)null));
		ToggleExpansionCommandProperty = DependencyProperty.Register("ToggleExpansionCommand", typeof(DelegateCommand), typeof(VideoOSMessageBarBase), new PropertyMetadata((object)null));
		Control.IsTabStopProperty.OverrideMetadata(typeof(VideoOSMessageBarBase), new FrameworkPropertyMetadata(false));
	}

	public VideoOSMessageBarBase()
	{
		CloseCommand = new DelegateCommand(delegate
		{
			RaiseEvent(new RoutedEventArgs(CloseClickedEvent));
		});
		ToggleExpansionCommand = new DelegateCommand(delegate
		{
			IsExpanded = !IsExpanded;
		});
		base.Loaded += OnLoaded;
		base.Unloaded += OnUnloaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		AddContentChangedHandlers();
		ContentChanged(null, EventArgs.Empty);
	}

	private void OnUnloaded(object sender, RoutedEventArgs e)
	{
		RemoveContentChangedHandlers();
	}

	private void AddContentChangedHandlers()
	{
		RemoveContentChangedHandlers();
		DependencyPropertyDescriptor.FromProperty(ContentControl.ContentProperty, typeof(VideoOSMessageBarBase)).AddValueChanged(this, ContentChanged);
		DependencyPropertyDescriptor.FromProperty(LinkTextProperty, typeof(VideoOSMessageBarBase)).AddValueChanged(this, ContentChanged);
	}

	private void RemoveContentChangedHandlers()
	{
		DependencyPropertyDescriptor.FromProperty(ContentControl.ContentProperty, typeof(VideoOSMessageBarBase)).RemoveValueChanged(this, ContentChanged);
		DependencyPropertyDescriptor.FromProperty(LinkTextProperty, typeof(VideoOSMessageBarBase)).RemoveValueChanged(this, ContentChanged);
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_inlineMessage = this.GetTemplateChild<VideoOSInlineMessageBase>("PART_InlineMessage");
		_actionList = this.GetTemplateChild<VideoOSActionListBase>("PART_ActionList");
		_expansionButton = this.GetTemplateChild<VideoOSButtonTransparentBase>("PART_ExpansionButton");
		DependencyPropertyDescriptor.FromProperty(LinkTextProperty, typeof(VideoOSMessageBarBase)).AddValueChanged(this, delegate
		{
			_inlineMessage.LinkText = LinkText;
		});
		_inlineMessage.LinkClicked += delegate
		{
			RaiseEvent(new RoutedEventArgs(LinkClickedEvent));
		};
	}

	protected override Size MeasureOverride(Size constraint)
	{
		double unconstrainedWidth = _inlineMessage.GetUnconstrainedWidth();
		_actionList.Measure(VideoOSMessageBarBaseInternals.UnconstrainedSize);
		double width = _actionList.DesiredSize.Width;
		double num = constraint.Width - width - 55.0;
		if (unconstrainedWidth > num)
		{
			Grid.SetRow(_actionList, 1);
			Grid.SetColumn(_actionList, 2);
			_actionList.Margin = new Thickness(OwnlineActionListMargin.Left + _inlineMessage.ContentOffset, OwnlineActionListMargin.Top, OwnlineActionListMargin.Right, OwnlineActionListMargin.Bottom);
		}
		else
		{
			Grid.SetRow(_actionList, 0);
			Grid.SetColumn(_actionList, 3);
			if (_actionList.Actions != null && _actionList.Actions.Any())
			{
				_actionList.Margin = InlineActionListMargin;
			}
			else
			{
				_actionList.Margin = new Thickness(0.0);
			}
		}
		UpdateExpansionButtonVisibility(_inlineMessage.WouldTextBeConstrained(2));
		return base.MeasureOverride(constraint);
	}

	protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
	{
		base.OnRenderSizeChanged(sizeInfo);
		UpdateExpansionButtonVisibility(_inlineMessage.WouldTextBeConstrained(2));
	}

	private void ContentChanged(object sender, EventArgs e)
	{
		InvalidateMeasure();
	}

	private void UpdateExpansionButtonVisibility(bool visible)
	{
		if (_expansionButton != null)
		{
			_expansionButton.Visibility = ((!visible) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSMessageBarBaseAutomationPeer(this);
	}
}
