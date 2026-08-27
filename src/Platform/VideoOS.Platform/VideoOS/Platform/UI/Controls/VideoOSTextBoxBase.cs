using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSTextBoxBase : TextBox
{
	public static class VideoOSTextBoxBaseInternals
	{
		public static IMultiValueConverter OutlineBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, States>((DependencyObject self, bool isMouseOver, bool isFocused, bool isEnabled, States state) => self.FindResource(state switch
		{
			States.Normal => isEnabled ? ((!isFocused) ? ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSTextBoxOutlineNormalIdleUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTextBoxOutlineNormalHoveredBrush") : "VideoOSResourceKeysInternalVideoOSTextBoxOutlineNormalIdleFocusedBrush") : "VideoOSResourceKeysInternalVideoOSTextBoxOutlineNormalDisabledBrush", 
			States.Error => isEnabled ? ((!isFocused) ? ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSTextBoxOutlineErrorIdleUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTextBoxOutlineErrorHoveredBrush") : "VideoOSResourceKeysInternalVideoOSTextBoxOutlineErrorIdleFocusedBrush") : "VideoOSResourceKeysInternalVideoOSTextBoxOutlineErrorDisabledBrush", 
			_ => throw new ArgumentOutOfRangeException("state", state, null), 
		}), "OutlineBrushConverter");

		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isFocused, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSTextBoxBackgroundDisabledBrush" : (isFocused ? "VideoOSResourceKeysInternalVideoOSTextBoxBackgroundIdleFocusedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSTextBoxBackgroundIdleUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSTextBoxBackgroundHoveredBrush")));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");

		public static IMultiValueConverter ClearButtonVisibilityConverter { get; } = new LambdaConverter2<string, Modes>((string text, Modes mode) => (mode != Modes.Search || string.IsNullOrEmpty(text)) ? Visibility.Collapsed : Visibility.Visible, "ClearButtonVisibilityConverter");

		public static IMultiValueConverter ContentHostMarginConverter { get; } = new LambdaConverter2<TextWrapping, bool>((TextWrapping textWrapping, bool acceptsReturn) => (textWrapping == TextWrapping.NoWrap && !acceptsReturn) ? ((object)new Thickness(0.0)) : ((object)new Thickness(0.0, 3.0, 0.0, 2.0)), "ContentHostMarginConverter");

		public static IMultiValueConverter PlaceholderTextMarginConverter { get; } = new LambdaConverter2<VerticalAlignment, Thickness>(delegate(VerticalAlignment verticalContentAlignment, Thickness contentHostMargin)
		{
			switch (verticalContentAlignment)
			{
			case VerticalAlignment.Top:
			case VerticalAlignment.Stretch:
				return new Thickness(2.0, contentHostMargin.Top, 0.0, contentHostMargin.Bottom);
			case VerticalAlignment.Bottom:
				return new Thickness(2.0, contentHostMargin.Top, 0.0, contentHostMargin.Bottom + 1.0);
			case VerticalAlignment.Center:
				return new Thickness(2.0, contentHostMargin.Top - 1.0, 0.0, contentHostMargin.Bottom);
			default:
				throw new ArgumentOutOfRangeException("verticalContentAlignment", verticalContentAlignment, null);
			}
		}, "PlaceholderTextMarginConverter");

		public static IMultiValueConverter PlaceholderTextTextWrappingConverter { get; } = new LambdaConverter2<TextWrapping, bool>((TextWrapping textWrapping, bool acceptsReturn) => acceptsReturn ? TextWrapping.Wrap : textWrapping, "PlaceholderTextTextWrappingConverter");
	}

	public enum Modes
	{
		Normal,
		Search
	}

	public enum States
	{
		Normal,
		Error
	}

	public static readonly DependencyProperty EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty;

	public static readonly DependencyProperty ClearTextCommandProperty;

	public static readonly DependencyProperty ModeProperty;

	public static readonly DependencyProperty StateProperty;

	public static readonly DependencyProperty PlaceholderTextProperty;

	private VideoOSScrollViewer _scrollViewer;

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

	public DelegateCommand ClearTextCommand
	{
		get
		{
			return (DelegateCommand)GetValue(ClearTextCommandProperty);
		}
		private set
		{
			SetValue(ClearTextCommandProperty, value);
		}
	}

	public Modes Mode
	{
		get
		{
			return (Modes)GetValue(ModeProperty);
		}
		set
		{
			SetValue(ModeProperty, value);
		}
	}

	public States State
	{
		get
		{
			return (States)GetValue(StateProperty);
		}
		set
		{
			SetValue(StateProperty, value);
		}
	}

	public string PlaceholderText
	{
		get
		{
			return (string)GetValue(PlaceholderTextProperty);
		}
		set
		{
			SetValue(PlaceholderTextProperty, value);
		}
	}

	static VideoOSTextBoxBase()
	{
		EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty = DependencyProperty.Register("EnsureFocusIsNotLostWhenActivatedUsingSpaceKey", typeof(bool), typeof(VideoOSTextBoxBase), new PropertyMetadata(false));
		ClearTextCommandProperty = DependencyProperty.Register("ClearTextCommand", typeof(DelegateCommand), typeof(VideoOSTextBoxBase), new PropertyMetadata((object)null));
		ModeProperty = DependencyProperty.Register("Mode", typeof(Modes), typeof(VideoOSTextBoxBase), new PropertyMetadata(Modes.Normal));
		StateProperty = DependencyProperty.Register("State", typeof(States), typeof(VideoOSTextBoxBase), new PropertyMetadata(States.Normal));
		PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(VideoOSTextBoxBase), new PropertyMetadata(null));
		Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(VideoOSTextBoxBase), new FrameworkPropertyMetadata(VerticalAlignment.Center));
	}

	public VideoOSTextBoxBase()
	{
		ClearTextCommand = new DelegateCommand(ClearTextCommandFunction);
		base.Loaded += VideoOSTextBoxBase_Loaded;
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_scrollViewer = this.GetTemplateChild<VideoOSScrollViewer>("PART_ContentHost");
		_scrollViewer.Loaded += VideoOSScrollViewer_Loaded;
	}

	private void VideoOSTextBoxBase_Loaded(object sender, RoutedEventArgs e)
	{
		if (base.MinLines != 1)
		{
			string text = base.Text;
			base.Text += " ";
			UpdateLayout();
			base.Text = text;
		}
		base.Loaded -= VideoOSTextBoxBase_Loaded;
	}

	private void VideoOSScrollViewer_Loaded(object sender, RoutedEventArgs e)
	{
		ScrollContentPresenter scrollContentPresenter = _scrollViewer.FindDescendant<ScrollContentPresenter>();
		if (scrollContentPresenter != null)
		{
			Binding binding = new Binding("Tag")
			{
				Source = _scrollViewer
			};
			scrollContentPresenter.SetBinding(UIElement.OpacityProperty, binding);
			_scrollViewer.Loaded -= VideoOSScrollViewer_Loaded;
		}
	}

	private void ClearTextCommandFunction(object args)
	{
		base.Text = string.Empty;
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSTextBoxBaseAutomationPeer(this);
	}
}
