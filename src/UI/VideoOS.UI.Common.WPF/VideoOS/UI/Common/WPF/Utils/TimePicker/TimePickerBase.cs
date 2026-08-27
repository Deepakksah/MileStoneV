using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Utils.TimePicker;

public abstract partial class TimePickerBase : UserControl, IComponentConnector
{
	protected TextBoxBase _textboxInFocus;

	private TransformGroup _transformGroup;

	public static DependencyProperty PickerBackgroundBrushProperty = DependencyProperty.Register("PickerBackgroundBrush", typeof(Brush), typeof(TimePickerBase), new PropertyMetadata(Brushes.White));

	public static DependencyProperty PickerBorderBrushProperty = DependencyProperty.Register("PickerBorderBrush", typeof(Brush), typeof(TimePickerBase), new PropertyMetadata(Brushes.Black));

	public static DependencyProperty PickerArrowBrushProperty = DependencyProperty.Register("PickerArrowBrush", typeof(Brush), typeof(TimePickerBase), new PropertyMetadata(Brushes.Gray));

	public static DependencyProperty PickerArrowMouseOverBrushProperty = DependencyProperty.Register("PickerArrowMouseOverBrush", typeof(Brush), typeof(TimePickerBase), new PropertyMetadata(Brushes.Black));

	public static DependencyProperty PickerTextBrushProperty = DependencyProperty.Register("PickerTextBrush", typeof(Brush), typeof(TimePickerBase), new PropertyMetadata(Brushes.Black));

	public static DependencyProperty PickerCaretBrushProperty = DependencyProperty.Register("PickerCaretBrush", typeof(Brush), typeof(TimePickerBase), new PropertyMetadata(Brushes.Black));

	public static DependencyProperty PickerTextSelectionBrushProperty = DependencyProperty.Register("PickerTextSelectionBrush", typeof(Brush), typeof(TimePickerBase), new PropertyMetadata(Brushes.LightBlue));

	public static readonly DependencyProperty SecondsEnabledProperty = DependencyProperty.Register("SecondsEnabled", typeof(bool), typeof(TimePickerBase), new PropertyMetadata(true, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((TimePickerBase)o).ResetSecondsIfNotEnabled();
	}));

	public Brush PickerBackgroundBrush
	{
		get
		{
			return (Brush)GetValue(PickerBackgroundBrushProperty);
		}
		set
		{
			SetValue(PickerBackgroundBrushProperty, value);
		}
	}

	public Brush PickerBorderBrush
	{
		get
		{
			return (Brush)GetValue(PickerBorderBrushProperty);
		}
		set
		{
			SetValue(PickerBorderBrushProperty, value);
		}
	}

	public Brush PickerArrowBrush
	{
		get
		{
			return (Brush)GetValue(PickerArrowBrushProperty);
		}
		set
		{
			SetValue(PickerArrowBrushProperty, value);
		}
	}

	public Brush PickerArrowMouseOverBrush
	{
		get
		{
			return (Brush)GetValue(PickerArrowMouseOverBrushProperty);
		}
		set
		{
			SetValue(PickerArrowMouseOverBrushProperty, value);
		}
	}

	public Brush PickerTextBrush
	{
		get
		{
			return (Brush)GetValue(PickerTextBrushProperty);
		}
		set
		{
			SetValue(PickerTextBrushProperty, value);
		}
	}

	public Brush PickerCaretBrush
	{
		get
		{
			return (Brush)GetValue(PickerCaretBrushProperty);
		}
		set
		{
			SetValue(PickerCaretBrushProperty, value);
		}
	}

	public Brush PickerTextSelectionBrush
	{
		get
		{
			return (Brush)GetValue(PickerTextSelectionBrushProperty);
		}
		set
		{
			SetValue(PickerTextSelectionBrushProperty, value);
		}
	}

	public bool SecondsEnabled
	{
		get
		{
			return (bool)GetValue(SecondsEnabledProperty);
		}
		set
		{
			SetValue(SecondsEnabledProperty, value);
		}
	}

	public event EventHandler TimeChanged;

	public event EventHandler EnterPressed;

	protected virtual void FireTimeChangedEvent()
	{
		if (this.TimeChanged != null)
		{
			this.TimeChanged(this, new EventArgs());
		}
	}

	protected virtual void FireEnterPressed()
	{
		if (this.EnterPressed != null)
		{
			this.EnterPressed(this, new EventArgs());
		}
	}

	public TimePickerBase()
	{
		InitializeComponent();
		DisableDragDropCutAndPaste();
		string timeSeparator = Thread.CurrentThread.CurrentCulture.DateTimeFormat.TimeSeparator;
		_seperator1.Text = timeSeparator;
		_seperator2.Text = timeSeparator;
		_designator.Outcome = new string[2]
		{
			Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator,
			Thread.CurrentThread.CurrentCulture.DateTimeFormat.PMDesignator
		};
		base.IsEnabledChanged += TimePicker_IsEnabledChanged;
		base.SizeChanged += TimePicker_SizeChanged;
		_increaseButton.RenderTransformOrigin = new Point(0.5, 0.5);
		_decreaseButton.RenderTransformOrigin = new Point(0.5, 0.5);
		_transformGroup = new TransformGroup();
		_transformGroup.Children.Add(new RotateTransform(180.0));
		_transformGroup.Children.Add(new ScaleTransform(1.0, 1.0));
		_decreaseButton.RenderTransform = _transformGroup;
	}

	private void TimePicker_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		double num = 50.0;
		double num2 = 125.0;
		double val = base.ActualWidth / num2;
		double val2 = base.ActualHeight / num;
		val = Math.Min(val, 1.0);
		val2 = Math.Min(val2, 1.0);
		_increaseButton.RenderTransform = new ScaleTransform(val, val2);
		if (_transformGroup.Children[1] is ScaleTransform scaleTransform)
		{
			scaleTransform.ScaleX = val;
			scaleTransform.ScaleY = val2;
		}
	}

	private void TimePicker_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (base.IsEnabled)
		{
			_border.BorderBrush = Brushes.Black;
			DoFocus();
		}
		else
		{
			_border.BorderBrush = Brushes.Gray;
		}
	}

	private void _hour_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Right)
		{
			_minutes.Focus();
		}
		else if (e.Key == Key.Up)
		{
			_hour.Increase();
		}
		else if (e.Key == Key.Down)
		{
			_hour.Decrease();
		}
		else if (e.Key == Key.Return || e.Key == Key.Return)
		{
			FireEnterPressed();
		}
	}

	private void _minutes_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Right)
		{
			_seconds.Focus();
		}
		else if (e.Key == Key.Left)
		{
			_hour.Focus();
		}
		else if (e.Key == Key.Up)
		{
			_minutes.Increase();
		}
		else if (e.Key == Key.Down)
		{
			_minutes.Decrease();
		}
		else if (e.Key == Key.Return || e.Key == Key.Return)
		{
			FireEnterPressed();
		}
	}

	private void _seconds_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Left)
		{
			_minutes.Focus();
		}
		else if (e.Key == Key.Right && _designator.Visibility == Visibility.Visible)
		{
			_designator.Focus();
		}
		else if (e.Key == Key.Up)
		{
			_seconds.Increase();
		}
		else if (e.Key == Key.Down)
		{
			_seconds.Decrease();
		}
		else if (e.Key == Key.Return || e.Key == Key.Return)
		{
			FireEnterPressed();
		}
	}

	private void _designator_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Left)
		{
			_seconds.Focus();
		}
		else if (e.Key == Key.Up)
		{
			_designator.Increase();
		}
		else if (e.Key == Key.Down)
		{
			_designator.Decrease();
		}
	}

	private void _hour_TextChanged(object sender, TextChangedEventArgs e)
	{
		FireTimeChangedEvent();
	}

	private void _minutes_TextChanged(object sender, TextChangedEventArgs e)
	{
		FireTimeChangedEvent();
	}

	private void _seconds_TextChanged(object sender, TextChangedEventArgs e)
	{
		ResetSecondsIfNotEnabled();
		FireTimeChangedEvent();
	}

	private void _designator_TextChanged(object sender, TextChangedEventArgs e)
	{
		FireTimeChangedEvent();
	}

	private void ResetSecondsIfNotEnabled()
	{
		if (!SecondsEnabled)
		{
			_seconds.Value = 0;
		}
	}

	private void Focused(object sender, RoutedEventArgs e)
	{
		_textboxInFocus = sender as TextBoxBase;
	}

	private void Increase(object sender, RoutedEventArgs e)
	{
		if (_textboxInFocus != null)
		{
			_textboxInFocus.Increase();
		}
	}

	private void Decrease(object sender, RoutedEventArgs e)
	{
		if (_textboxInFocus != null)
		{
			_textboxInFocus.Decrease();
		}
	}

	protected void InvalidateTimePicker()
	{
		_hour.Invalidate();
		_minutes.Invalidate();
		_seconds.Invalidate();
		_designator.Invalidate();
	}

	protected virtual void DoFocus()
	{
	}

	private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		DoFocus();
	}

	private void DisableDragDropCutAndPaste()
	{
		DisableDragDropCutAndPaste(_hour);
		DisableDragDropCutAndPaste(_minutes);
		DisableDragDropCutAndPaste(_seconds);
		DisableDragDropCutAndPaste(_designator);
	}

	private void DisableDragDropCutAndPaste(TextBox textBox)
	{
		DataObject.AddCopyingHandler(textBox, PreventCopy);
		DataObject.AddPastingHandler(textBox, PreventPaste);
	}

	private void PreventCopy(object sender, DataObjectCopyingEventArgs e)
	{
		e.CancelCommand();
	}

	private void PreventPaste(object sender, DataObjectPastingEventArgs e)
	{
		e.CancelCommand();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
