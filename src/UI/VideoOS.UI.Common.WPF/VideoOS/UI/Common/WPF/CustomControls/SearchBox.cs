using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace VideoOS.UI.Common.WPF.CustomControls;

[TemplatePart(Name = "PART_ClearButton", Type = typeof(ButtonBase))]
[TemplateVisualState(GroupName = "WatermarkStates", Name = "WatermarkVisible")]
[TemplateVisualState(GroupName = "WatermarkStates", Name = "WatermarkHidden")]
[TemplateVisualState(GroupName = "WatermarkStates", Name = "ClearButtonVisible")]
public class SearchBox : TextBox
{
	public class SearchTextChangedEventArgs : EventArgs
	{
		public string OldValue { get; }

		public string NewValue { get; }

		public SearchTextChangedEventArgs(string oldValue, string newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}

	private const string VISUAL_STATE_WATERMARK_STATES = "WatermarkStates";

	private const string VISUAL_STATE_WATERMARK_VISIBLE = "WatermarkVisible";

	private const string VISUAL_STATE_CLEAR_BUTTON_VISIBLE = "ClearButtonVisible";

	private const string VISUAL_STATE_WATERMARK_HIDDEN = "WatermarkHidden";

	private const string PART_CLEAR_BUTTON = "PART_ClearButton";

	public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(SearchBox));

	public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(SearchBox), new FrameworkPropertyMetadata(string.Empty, searchTextPropertyChanged));

	public static readonly DependencyProperty MillisecondsDelayProperty = DependencyProperty.Register("MillisecondsDelay", typeof(int), typeof(SearchBox), new PropertyMetadata(300, MillisecondsDelayUpdated));

	private ButtonBase _clearButton;

	private readonly DispatcherTimer _textTransferTimer;

	public int MillisecondsDelay
	{
		get
		{
			return (int)GetValue(MillisecondsDelayProperty);
		}
		set
		{
			SetValue(MillisecondsDelayProperty, value);
		}
	}

	public string WatermarkText
	{
		get
		{
			return (string)GetValue(WatermarkTextProperty);
		}
		set
		{
			SetValue(WatermarkTextProperty, value);
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

	public event EventHandler<SearchTextChangedEventArgs> SearchTextChanged;

	private static void MillisecondsDelayUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		SearchBox searchBox = (SearchBox)d;
		searchBox._textTransferTimer.Interval = TimeSpan.FromMilliseconds(searchBox.MillisecondsDelay);
	}

	public SearchBox()
	{
		_textTransferTimer = new DispatcherTimer();
		_textTransferTimer.Interval = TimeSpan.FromMilliseconds(MillisecondsDelay);
		_textTransferTimer.Tick += _textTransferTimer_Tick;
		base.IsKeyboardFocusWithinChanged += SearchBox_IsKeyboardFocusWithinChanged;
		base.Loaded += SearchBox_Loaded;
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_clearButton = GetTemplateChild("PART_ClearButton") as ButtonBase;
		if (_clearButton != null)
		{
			_clearButton.Click += _clearTextBoxElement_Click;
		}
	}

	protected override void OnTextChanged(TextChangedEventArgs e)
	{
		_textTransferTimer.Stop();
		base.OnTextChanged(e);
		_textTransferTimer.Start();
		UpdateVisualState();
	}

	private void SearchBox_Loaded(object sender, RoutedEventArgs e)
	{
		base.ContextMenu = null;
		UpdateVisualState();
	}

	private void UpdateVisualState()
	{
		if (string.IsNullOrEmpty(base.Text) && !base.IsKeyboardFocusWithin)
		{
			VisualStateManager.GoToState(this, "WatermarkVisible", useTransitions: true);
		}
		else if (string.IsNullOrEmpty(base.Text))
		{
			VisualStateManager.GoToState(this, "WatermarkHidden", useTransitions: true);
		}
		else
		{
			VisualStateManager.GoToState(this, "ClearButtonVisible", useTransitions: true);
		}
	}

	private void SearchBox_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		UpdateVisualState();
	}

	private static void searchTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		SearchBox searchBox = sender as SearchBox;
		if (searchBox.Text != searchBox.SearchText)
		{
			searchBox.SetCurrentValue(TextBox.TextProperty, e.NewValue);
			searchBox.SelectionStart = searchBox.Text.Length;
		}
		searchBox.SearchTextChanged?.Invoke(searchBox, new SearchTextChangedEventArgs(e.OldValue as string, e.NewValue as string));
	}

	private void _clearTextBoxElement_Click(object sender, RoutedEventArgs e)
	{
		SetCurrentValue(SearchTextProperty, string.Empty);
	}

	private void _textTransferTimer_Tick(object sender, EventArgs e)
	{
		SetCurrentValue(SearchTextProperty, base.Text);
		_textTransferTimer.Stop();
	}
}
