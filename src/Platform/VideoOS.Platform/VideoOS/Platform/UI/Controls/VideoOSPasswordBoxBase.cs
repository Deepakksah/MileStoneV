using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSPasswordBoxBase : Control, INotifyPropertyChanged
{
	public static class VideoOSPasswordBoxBaseInternals
	{
		public static IMultiValueConverter OutlineBrushConverter { get; } = new LambdaConverter5<DependencyObject, bool, bool, bool, States>((DependencyObject self, bool isMouseOver, bool isFocused, bool isEnabled, States state) => self.FindResource(state switch
		{
			States.Normal => isEnabled ? ((!isFocused) ? ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSPasswordBoxOutlineNormalIdleUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSPasswordBoxOutlineNormalHoveredBrush") : "VideoOSResourceKeysInternalVideoOSPasswordBoxOutlineNormalIdleFocusedBrush") : "VideoOSResourceKeysInternalVideoOSPasswordBoxOutlineNormalDisabledBrush", 
			States.Error => isEnabled ? ((!isFocused) ? ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSPasswordBoxOutlineErrorIdleUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSPasswordBoxOutlineErrorHoveredBrush") : "VideoOSResourceKeysInternalVideoOSPasswordBoxOutlineErrorIdleFocusedBrush") : "VideoOSResourceKeysInternalVideoOSPasswordBoxOutlineErrorDisabledBrush", 
			_ => throw new ArgumentOutOfRangeException("state", state, null), 
		}), "OutlineBrushConverter");

		public static IMultiValueConverter BackgroundBrushConverter { get; } = new LambdaConverter4<DependencyObject, bool, bool, bool>(delegate(DependencyObject self, bool isMouseOver, bool isFocused, bool isEnabled)
		{
			string resourceKey = ((!isEnabled) ? "VideoOSResourceKeysInternalVideoOSPasswordBoxBackgroundDisabledBrush" : (isFocused ? "VideoOSResourceKeysInternalVideoOSPasswordBoxBackgroundIdleFocusedBrush" : ((!isMouseOver) ? "VideoOSResourceKeysInternalVideoOSPasswordBoxBackgroundIdleUnfocusedBrush" : "VideoOSResourceKeysInternalVideoOSPasswordBoxBackgroundHoveredBrush")));
			return self.FindResource(resourceKey);
		}, "BackgroundBrushConverter");
	}

	public enum States
	{
		Normal,
		Error
	}

	public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(States), typeof(VideoOSPasswordBoxBase), new PropertyMetadata(States.Normal));

	public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(VideoOSPasswordBoxBase), new PropertyMetadata(null));

	private string __initialPassword;

	public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(VideoOSPasswordBoxBase), new FrameworkPropertyMetadata(0));

	public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent("PasswordChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VideoOSDropDownBase));

	private PasswordBox _passwordBox;

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

	public string Password
	{
		get
		{
			return _passwordBox?.Password;
		}
		set
		{
			if (_passwordBox != null)
			{
				_passwordBox.Password = value;
			}
			else
			{
				__initialPassword = value;
			}
		}
	}

	public SecureString SecurePassword => _passwordBox?.SecurePassword;

	public int MaxLength
	{
		get
		{
			return (int)GetValue(MaxLengthProperty);
		}
		set
		{
			SetValue(MaxLengthProperty, value);
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public event RoutedEventHandler PasswordChanged
	{
		add
		{
			AddHandler(PasswordChangedEvent, value);
		}
		remove
		{
			RemoveHandler(PasswordChangedEvent, value);
		}
	}

	public void Paste()
	{
		_passwordBox?.Paste();
	}

	public void SelectAll()
	{
		_passwordBox?.SelectAll();
	}

	public void Clear()
	{
		_passwordBox?.Clear();
	}

	public VideoOSPasswordBoxBase()
	{
		base.Loaded += VideoOSPasswordBoxBase_Loaded;
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_passwordBox = this.GetTemplateChild<PasswordBox>("PART_PasswordBox");
		_passwordBox.Password = __initialPassword;
		_passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
		base.GotFocus += VideoOSPasswordBoxBase_GotFocus;
	}

	private void VideoOSPasswordBoxBase_Loaded(object sender, RoutedEventArgs e)
	{
		VideoOSScrollViewer source = this.FindDescendant<VideoOSScrollViewer>();
		ScrollContentPresenter scrollContentPresenter = _passwordBox.FindDescendant<ScrollContentPresenter>();
		if (scrollContentPresenter != null)
		{
			Binding binding = new Binding("Tag")
			{
				Source = source
			};
			scrollContentPresenter.SetBinding(UIElement.OpacityProperty, binding);
			base.Loaded -= VideoOSPasswordBoxBase_Loaded;
		}
	}

	private void VideoOSPasswordBoxBase_GotFocus(object sender, RoutedEventArgs e)
	{
		_passwordBox.Focus();
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
	{
		OnPropertyChanged("Password");
		OnPropertyChanged("SecurePassword");
		RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSPasswordBoxBaseAutomationPeer(this);
	}
}
