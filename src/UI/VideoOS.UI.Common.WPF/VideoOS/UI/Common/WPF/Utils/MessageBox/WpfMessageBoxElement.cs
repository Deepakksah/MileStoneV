using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using VideoOS.Common.Definition;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public class WpfMessageBoxElement : FrameworkElement
{
	private WpfMessageBoxWindow _window;

	public static readonly DependencyProperty ContentTemplateSelectorProperty = ContentControl.ContentTemplateSelectorProperty.AddOwner(typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty ContentTemplateProperty = ContentControl.ContentTemplateProperty.AddOwner(typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(WpfMessageBoxElement), new FrameworkPropertyMetadata(null, ContentPropertyChanged));

	public static readonly DependencyProperty TextProperty = TextBlock.TextProperty.AddOwner(typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty IsOpenProperty = Popup.IsOpenProperty.AddOwner(typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty ResultProperty = WpfMessageBoxWindow.ResultProperty.AddOwner(typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty TitleProperty = Window.TitleProperty.AddOwner(typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty InitCommandProperty = DependencyProperty.Register("InitCommand", typeof(ICommand), typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty MessageBoxButtonsProperty = DependencyProperty.Register("MessageBoxButtons", typeof(WpfMessageBoxButton), typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty MessageBoxConfigurationProperty = DependencyProperty.Register("MessageBoxConfiguration", typeof(WpfMessageBoxConfiguration), typeof(WpfMessageBoxElement), new PropertyMetadata((object)null));

	public static readonly DependencyProperty MessageBoxIconProperty = DependencyProperty.Register("MessageBoxIcon", typeof(WpfMessageBoxImage), typeof(WpfMessageBoxElement));

	public static readonly DependencyProperty IsOpenRequestedProperty = DependencyProperty.Register("IsOpenRequested", typeof(bool), typeof(WpfMessageBoxElement), new FrameworkPropertyMetadata(true, IsOpenRequestedPropertyChanged));

	public object Content
	{
		get
		{
			return GetValue(ContentProperty);
		}
		set
		{
			SetValue(ContentProperty, value);
		}
	}

	public System.Windows.Controls.DataTemplateSelector ContentTemplateSelector
	{
		get
		{
			return (System.Windows.Controls.DataTemplateSelector)GetValue(ContentTemplateSelectorProperty);
		}
		set
		{
			SetValue(ContentTemplateSelectorProperty, value);
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

	public bool IsOpen
	{
		get
		{
			return (bool)GetValue(IsOpenProperty);
		}
		set
		{
			SetValue(IsOpenProperty, value);
		}
	}

	public bool IsOpenRequested
	{
		get
		{
			return (bool)GetValue(IsOpenRequestedProperty);
		}
		set
		{
			SetValue(IsOpenRequestedProperty, value);
		}
	}

	public WpfMessageBoxResult Result
	{
		get
		{
			return (WpfMessageBoxResult)GetValue(ResultProperty);
		}
		set
		{
			SetValue(ResultProperty, value);
		}
	}

	public string Title
	{
		get
		{
			return (string)GetValue(TitleProperty);
		}
		set
		{
			SetValue(TitleProperty, value);
		}
	}

	public string Text
	{
		get
		{
			return (string)GetValue(TextProperty);
		}
		set
		{
			SetValue(TextProperty, value);
		}
	}

	public WpfMessageBoxButton MessageBoxButtons
	{
		get
		{
			return (WpfMessageBoxButton)GetValue(MessageBoxButtonsProperty);
		}
		set
		{
			SetValue(MessageBoxButtonsProperty, value);
		}
	}

	public WpfMessageBoxConfiguration MessageBoxConfiguration
	{
		get
		{
			return (WpfMessageBoxConfiguration)GetValue(MessageBoxConfigurationProperty);
		}
		set
		{
			SetValue(MessageBoxConfigurationProperty, value);
		}
	}

	public WpfMessageBoxImage MessageBoxIcon
	{
		get
		{
			return (WpfMessageBoxImage)GetValue(MessageBoxIconProperty);
		}
		set
		{
			SetValue(MessageBoxIconProperty, value);
		}
	}

	public ICommand InitCommand
	{
		get
		{
			return (ICommand)GetValue(InitCommandProperty);
		}
		set
		{
			SetValue(InitCommandProperty, value);
		}
	}

	public ICommand CloseCommand
	{
		get
		{
			return (ICommand)GetValue(CloseCommandProperty);
		}
		set
		{
			SetValue(CloseCommandProperty, value);
		}
	}

	protected virtual void OnContentChanged(object oldContent)
	{
		OpenOrCloseMessageBox();
	}

	protected void IsOpenRequestedChanged()
	{
		OpenOrCloseMessageBox();
	}

	private static bool IsContentValid(object content)
	{
		if (content != null)
		{
			return content != DisconnectedItem.Instance;
		}
		return false;
	}

	private void OpenOrCloseMessageBox()
	{
		if (IsOpenRequested && (IsContentValid(Content) || !string.IsNullOrEmpty(Text)))
		{
			if (IsContentValid(Content))
			{
				_window = WpfMessageBox.CreateMessageBox(this, Content, Title);
			}
			else
			{
				_window = WpfMessageBox.CreateMessageBox(this, Text, string.Empty, Title, MessageBoxButtons, MessageBoxConfiguration, MessageBoxIcon);
			}
			_window.DataContext = base.DataContext;
			_window.OwnerElement = this;
			_window.ContentTemplateSelector = ContentTemplateSelector;
			_window.ContentTemplate = ContentTemplate;
			SetCurrentValue(IsOpenProperty, true);
			WpfMessageBoxResult wpfMessageBoxResult = _window.ShowDialogEx(InitCommand);
			if (CloseCommand != null && CloseCommand.CanExecute(wpfMessageBoxResult))
			{
				CloseCommand.Execute(wpfMessageBoxResult);
			}
			SetCurrentValue(ResultProperty, wpfMessageBoxResult);
			SetCurrentValue(IsOpenProperty, false);
			_window = null;
		}
		else if (_window != null)
		{
			_window.CloseDialog();
		}
	}

	private static void ContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		(d as WpfMessageBoxElement).OnContentChanged(e.OldValue);
	}

	private static void IsOpenRequestedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		(d as WpfMessageBoxElement).IsOpenRequestedChanged();
	}
}
