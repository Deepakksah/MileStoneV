using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace VideoOS.UI.Common.WPF.SearchTextBoxControl;

public partial class SearchTextBox : UserControl, IComponentConnector
{
	public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(SearchTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SearchTextPropertyChanged));

	public static readonly DependencyProperty SearchTooltipProperty = DependencyProperty.Register("SearchTooltip", typeof(string), typeof(SearchTextBox), new FrameworkPropertyMetadata(SearchTooltipPropertyChanged));

	public static readonly DependencyProperty TextLockedProperty = DependencyProperty.Register("TextLocked", typeof(bool), typeof(SearchTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TextLockedPropertyChanged));

	public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(SearchTextBox), new FrameworkPropertyMetadata(MaxLengthPropertyChanged));

	private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

	public string SearchTooltip
	{
		get
		{
			return (string)GetValue(SearchTooltipProperty);
		}
		set
		{
			SetValue(SearchTooltipProperty, value);
		}
	}

	public string SearchText
	{
		get
		{
			return (string)GetValue(SearchTextProperty);
		}
		set
		{
			SetValue(SearchTextProperty, value);
		}
	}

	public bool TextLocked
	{
		get
		{
			return (bool)GetValue(TextLockedProperty);
		}
		set
		{
			SetValue(TextLockedProperty, value);
		}
	}

	public int SearchDelayInMilliseconds
	{
		get
		{
			return _dispatcherTimer.Interval.Milliseconds;
		}
		set
		{
			_dispatcherTimer.Interval = TimeSpan.FromMilliseconds(value);
		}
	}

	public bool EnableRequery { get; set; }

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

	public event EventHandler SearchTextChanged = delegate
	{
	};

	public event EventHandler SearchTextChangedDelayed = delegate
	{
	};

	public SearchTextBox()
	{
		InitializeComponent();
		SearchDelayInMilliseconds = 300;
		_dispatcherTimer.Tick += _dispatcherTimer_Tick;
	}

	private static void SearchTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SearchTextBox obj = d as SearchTextBox;
		obj._searchTextBox.Text = (string)e.NewValue;
		obj._dispatcherTimer.Stop();
	}

	private static void SearchTooltipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		(d as SearchTextBox)._searchTooltipTextBlock.Text = (string)e.NewValue;
	}

	private static void TextLockedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SearchTextBox obj = d as SearchTextBox;
		obj._searchTextBox.FontStyle = (((bool)e.NewValue) ? FontStyles.Italic : FontStyles.Normal);
		obj._searchTextBox.IsReadOnly = (bool)e.NewValue;
	}

	private static void MaxLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		(d as SearchTextBox)._searchTextBox.MaxLength = (int)e.NewValue;
	}

	private void SearchTextBox_OnLoaded(object sender, RoutedEventArgs e)
	{
		Update();
	}

	private void _textBox_OnTextChanged(object sender, TextChangedEventArgs e)
	{
		Update();
		this.SearchTextChanged(this, e);
		_dispatcherTimer.Stop();
		_dispatcherTimer.Start();
	}

	private void _textBox_OnIsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		Update();
		if (_dispatcherTimer.IsEnabled)
		{
			SetCurrentValue(SearchTextProperty, _searchTextBox.Text);
			_dispatcherTimer.Stop();
			this.SearchTextChangedDelayed(this, EventArgs.Empty);
		}
	}

	private void _searchTextBox_OnKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return || e.Key == Key.Return)
		{
			Update();
			if (_dispatcherTimer.IsEnabled || EnableRequery)
			{
				SetCurrentValue(SearchTextProperty, _searchTextBox.Text);
				_dispatcherTimer.Stop();
				this.SearchTextChangedDelayed(this, EventArgs.Empty);
			}
			e.Handled = true;
		}
	}

	private void _clearImage_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		TextLocked = false;
		SetCurrentValue(SearchTextProperty, string.Empty);
		_dispatcherTimer.Stop();
		this.SearchTextChangedDelayed(this, EventArgs.Empty);
	}

	private void _dispatcherTimer_Tick(object sender, EventArgs e)
	{
		SetCurrentValue(SearchTextProperty, _searchTextBox.Text);
		_dispatcherTimer.Stop();
		this.SearchTextChangedDelayed(this, EventArgs.Empty);
	}

	private void Update()
	{
		if (string.IsNullOrEmpty(_searchTextBox.Text))
		{
			_searchImage.Visibility = Visibility.Visible;
			_clearImage.Visibility = Visibility.Hidden;
		}
		else
		{
			_searchImage.Visibility = Visibility.Hidden;
			_clearImage.Visibility = Visibility.Visible;
		}
		if (string.IsNullOrEmpty(_searchTextBox.Text) && !_searchTextBox.IsKeyboardFocused)
		{
			_searchTooltipTextBlock.Visibility = Visibility.Visible;
		}
		else
		{
			_searchTooltipTextBlock.Visibility = Visibility.Hidden;
		}
	}
}
