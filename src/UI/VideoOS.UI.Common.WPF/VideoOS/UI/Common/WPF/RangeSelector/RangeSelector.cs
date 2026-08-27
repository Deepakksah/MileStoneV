using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.UI.Common.WPF.RangeSelector;

[TemplateVisualState(Name = "MinPressed", GroupName = "CommonStates")]
[TemplateVisualState(Name = "MaxPressed", GroupName = "CommonStates")]
[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
[TemplatePart(Name = "MinThumb", Type = typeof(Thumb))]
[TemplatePart(Name = "MaxThumb", Type = typeof(Thumb))]
[TemplatePart(Name = "ContainerCanvas", Type = typeof(Canvas))]
[TemplatePart(Name = "ControlGrid", Type = typeof(Grid))]
[DebuggerDisplay("{MinValue} {RangeMin}-{RangeMax} {MaxValue}")]
public class RangeSelector : Control, INotifyPropertyChanged
{
	private enum DragEntity
	{
		None,
		RangeSingle,
		RangeMin,
		RangeMax
	}

	private const string _vstateMinPressed = "MinPressed";

	private const string _vstateMaxPressed = "MaxPressed";

	private const string _vstateDisabled = "Disabled";

	private const string _vstateNormal = "Normal";

	private const double _maxValueDefault = 100.0;

	public const double _tolerance = 0.0001;

	private static readonly string _decimalSeparator;

	private static readonly Regex _rangeEditRegex;

	private double _dragPosition = double.NaN;

	public Canvas _containerCanvas;

	public Thumb _minThumb;

	public Thumb _maxThumb;

	public TextBox _rangeEdit;

	public TextBox _rangeMinEdit;

	public TextBox _rangeMaxEdit;

	public Rectangle _selectedRange;

	public Border _entireRange;

	private double _dragOriginX = double.NaN;

	private DragEntity _dragEntity;

	private double _dragThumbOffset;

	private bool _isRangeMinEqualMax;

	private bool _isThumbsOverlapping;

	private double _rangeSingle;

	private Tuple<double, double> _rangeSelectionState;

	public static readonly DependencyProperty EditFieldStyleProperty;

	public static readonly DependencyProperty SliderThumbStyleProperty;

	public static readonly DependencyProperty MinValueProperty;

	public static readonly DependencyProperty MaxValueProperty;

	public static readonly DependencyProperty RangeMaxProperty;

	public static readonly DependencyProperty RangeMinProperty;

	public static readonly DependencyProperty TickFrequencyProperty;

	public static readonly DependencyProperty IsMinRangeShownProperty;

	public Style EditFieldStyle
	{
		get
		{
			return (Style)GetValue(EditFieldStyleProperty);
		}
		set
		{
			SetValue(EditFieldStyleProperty, value);
		}
	}

	public Style SliderThumbStyle
	{
		get
		{
			return (Style)GetValue(SliderThumbStyleProperty);
		}
		set
		{
			SetValue(SliderThumbStyleProperty, value);
		}
	}

	public double MinValue
	{
		get
		{
			return (double)GetValue(MinValueProperty);
		}
		set
		{
			SetValue(MinValueProperty, value);
		}
	}

	public double MaxValue
	{
		get
		{
			return (double)GetValue(MaxValueProperty);
		}
		set
		{
			if (!value.IsEqualTo(MaxValue, 0.0001))
			{
				SetValue(MaxValueProperty, value);
			}
		}
	}

	public double RangeMax
	{
		get
		{
			return (double)GetValue(RangeMaxProperty);
		}
		set
		{
			if (!value.IsEqualTo(RangeMax, 0.0001))
			{
				SetValue(RangeMaxProperty, value);
			}
		}
	}

	public double RangeMin
	{
		get
		{
			return (double)GetValue(RangeMinProperty);
		}
		set
		{
			if (!value.IsEqualTo(RangeMin, 0.0001))
			{
				SetValue(RangeMinProperty, value);
			}
		}
	}

	public double TickFrequency
	{
		get
		{
			return (double)GetValue(TickFrequencyProperty);
		}
		set
		{
			if (!value.IsEqualTo(TickFrequency, 0.0001))
			{
				SetValue(TickFrequencyProperty, value);
			}
		}
	}

	public bool IsMinRangeShown
	{
		get
		{
			return (bool)GetValue(IsMinRangeShownProperty);
		}
		set
		{
			SetValue(IsMinRangeShownProperty, value);
		}
	}

	public double RangeSingle
	{
		get
		{
			return _rangeSingle;
		}
		set
		{
			double num = CoerceTicks(value, IsDragging);
			if (num.IsEqualTo(_rangeSingle, 0.0001))
			{
				return;
			}
			_rangeSingle = num;
			if (_rangeSingle > MaxValue)
			{
				_rangeSingle = MaxValue;
			}
			if (_rangeSingle < MinValue)
			{
				_rangeSingle = MinValue;
			}
			OnPropertyChanged("RangeSingle");
			if (RangeMax.IsEqualTo(RangeSingle, 0.0001))
			{
				return;
			}
			bool flag = false;
			if (RangeSingle < RangeMin)
			{
				if (!RangeMin.IsEqualTo(RangeSingle, 0.0001))
				{
					RangeMin = RangeSingle;
					OnPropertyChanged("RangeMin");
					flag = true;
				}
			}
			else if (!RangeMax.IsEqualTo(RangeSingle, 0.0001))
			{
				RangeMax = RangeSingle;
				OnPropertyChanged("RangeMax");
				flag = true;
			}
			Keyboard.ClearFocus();
			if (flag)
			{
				FireRangeChanged(RangeMin, RangeMax);
			}
		}
	}

	public bool IsMouseOverRangeBar
	{
		get
		{
			if (_containerCanvas == null)
			{
				return false;
			}
			Point position = Mouse.GetPosition(_containerCanvas);
			if (!(_maxThumb.TranslatePoint(new Point(_maxThumb.ActualWidth, 0.0), _containerCanvas).Y <= position.Y) || !(position.Y <= _maxThumb.ActualHeight))
			{
				return IsDragging;
			}
			return true;
		}
	}

	public bool IsRangeMinEqualMax
	{
		get
		{
			return _isRangeMinEqualMax;
		}
		private set
		{
			_isRangeMinEqualMax = value;
			OnPropertyChanged("IsRangeMinEqualMax");
		}
	}

	public bool IsThumbsOverlapping
	{
		get
		{
			return _isThumbsOverlapping;
		}
		private set
		{
			if (_isThumbsOverlapping != value)
			{
				_isThumbsOverlapping = value;
				OnPropertyChanged("IsThumbsOverlapping");
			}
		}
	}

	private bool IsDragging => !double.IsNaN(_dragOriginX);

	public event PropertyChangedEventHandler PropertyChanged;

	public event EventHandler<RangeChangedEventArgs> RangeChanged;

	private static object CoerceMinValueCallback(DependencyObject d, object basevalue)
	{
		RangeSelector rangeSelector = (RangeSelector)d;
		double num = (double)basevalue;
		if (num > rangeSelector.MaxValue)
		{
			rangeSelector.MaxValue = num;
		}
		if (num > rangeSelector.RangeMax)
		{
			rangeSelector.RangeMax = num;
		}
		if (num > rangeSelector.RangeMin)
		{
			rangeSelector.RangeMin = num;
		}
		return num;
	}

	private static object CoerceMaxValueCallback(DependencyObject d, object basevalue)
	{
		RangeSelector rangeSelector = (RangeSelector)d;
		double num = (double)basevalue;
		if (num < rangeSelector.MinValue)
		{
			rangeSelector.MinValue = num;
		}
		if (!rangeSelector.IsMinRangeShown || rangeSelector.RangeMin > num)
		{
			rangeSelector.RangeMin = num;
		}
		if (!rangeSelector.IsMinRangeShown || rangeSelector.RangeMax > num)
		{
			rangeSelector.RangeMax = num;
		}
		return num;
	}

	private static object CoerceRangeMaxCallback(DependencyObject d, object basevalue)
	{
		RangeSelector rangeSelector = (RangeSelector)d;
		double num = (double)basevalue;
		if (num > rangeSelector.MaxValue)
		{
			num = rangeSelector.MaxValue;
		}
		if (num < rangeSelector.MinValue)
		{
			num = rangeSelector.MinValue;
		}
		if (num < rangeSelector.RangeMin)
		{
			num = rangeSelector.RangeMin;
		}
		return num;
	}

	private static object CoerceRangeMinCallback(DependencyObject d, object basevalue)
	{
		RangeSelector rangeSelector = (RangeSelector)d;
		double num = (double)basevalue;
		if (!rangeSelector.IsMinRangeShown)
		{
			return rangeSelector.MinValue;
		}
		if (num < rangeSelector.MinValue)
		{
			num = rangeSelector.MinValue;
		}
		if (num > rangeSelector.MaxValue)
		{
			num = rangeSelector.MaxValue;
		}
		if (num > rangeSelector.RangeMax)
		{
			num = rangeSelector.RangeMax;
		}
		return num;
	}

	private static void TickFrequencyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is RangeSelector rangeSelector)
		{
			double num = (double)e.NewValue;
			if (num > rangeSelector.MaxValue - rangeSelector.MinValue)
			{
				num = rangeSelector.MaxValue - rangeSelector.MinValue;
			}
			rangeSelector.TickFrequency = num;
			rangeSelector.SyncThumbs();
		}
	}

	private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is RangeSelector rangeSelector && !((double)e.OldValue).IsEqualTo((double)e.NewValue, 0.0001))
		{
			rangeSelector.SyncThumbs();
			rangeSelector.FireRangeChanged(rangeSelector.RangeMin, rangeSelector.RangeMax);
		}
	}

	static RangeSelector()
	{
		_decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		_rangeEditRegex = new Regex("^-$|^-?(0|[1-9]\\d*)(\\" + _decimalSeparator + "\\d*)?$");
		EditFieldStyleProperty = DependencyProperty.Register("EditFieldStyle", typeof(Style), typeof(RangeSelector), new PropertyMetadata((object)null));
		SliderThumbStyleProperty = DependencyProperty.Register("SliderThumbStyle", typeof(Style), typeof(RangeSelector), new PropertyMetadata((object)null));
		MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, PropertyChangedCallback, CoerceMinValueCallback));
		MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(RangeSelector), new PropertyMetadata(100.0, PropertyChangedCallback, CoerceMaxValueCallback));
		RangeMaxProperty = DependencyProperty.Register("RangeMax", typeof(double), typeof(RangeSelector), new PropertyMetadata(100.0, PropertyChangedCallback, CoerceRangeMaxCallback));
		RangeMinProperty = DependencyProperty.Register("RangeMin", typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, PropertyChangedCallback, CoerceRangeMinCallback));
		TickFrequencyProperty = DependencyProperty.Register("TickFrequency", typeof(double), typeof(RangeSelector), new PropertyMetadata(1.0, TickFrequencyChangedCallback));
		IsMinRangeShownProperty = DependencyProperty.Register("IsMinRangeShown", typeof(bool), typeof(RangeSelector), new PropertyMetadata(true));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSelector), new FrameworkPropertyMetadata(typeof(RangeSelector)));
	}

	public RangeSelector()
	{
		base.Loaded += delegate
		{
			SyncThumbs();
		};
	}

	public override void OnApplyTemplate()
	{
		if (_containerCanvas != null)
		{
			_containerCanvas.PreviewMouseLeftButtonDown -= ContainerCanvas_OnPreviewMouseLeftButtonDown;
			_containerCanvas.MouseLeftButtonUp -= ContainerCanvas_OnMouseLeftButtonUp;
			_containerCanvas.SizeChanged -= ContainerCanvas_OnSizeChanged;
			_containerCanvas.MouseLeave -= ContainerCanvas_OnMouseLeave;
			_containerCanvas.MouseDown -= ContainerCanvas_OnMouseDown;
			_containerCanvas.MouseMove -= ContainerCanvas_OnMouseMove;
		}
		UnSubscribeEditboxEvents(_rangeMinEdit);
		UnSubscribeEditboxEvents(_rangeMaxEdit);
		if (_rangeEdit != null)
		{
			_rangeEdit.GotKeyboardFocus -= SaveRangeSelectionState;
			_rangeEdit.LostFocus -= RangeEdit_OnLostFocus;
			_rangeEdit.KeyDown -= RangeEdit_OnKeyDown;
			_rangeEdit.TextChanged -= RangeEditSingle_TextChanged;
			_rangeEdit.PreviewTextInput -= RangeEdit_PreviewTextInput;
			_rangeEdit.PreviewKeyDown -= RangeEdit_PreviewKeyDown;
			DataObject.RemovePastingHandler(_rangeEdit, RangeEdit_Pasting);
		}
		base.IsEnabledChanged -= OnIsEnabledChanged;
		base.PreviewMouseMove -= OnPreviewMouseMove;
		EnforceDataInvariant();
		_containerCanvas = GetTemplateChild("ContainerCanvas") as Canvas;
		_minThumb = GetTemplateChild("MinThumb") as Thumb;
		_maxThumb = GetTemplateChild("MaxThumb") as Thumb;
		_rangeEdit = GetTemplateChild("RangeEdit") as TextBox;
		_rangeMinEdit = GetTemplateChild("RangeMinEdit") as TextBox;
		_rangeMaxEdit = GetTemplateChild("RangeMaxEdit") as TextBox;
		_selectedRange = GetTemplateChild("SelectedRangeBar") as Rectangle;
		_entireRange = GetTemplateChild("BackgroundElement") as Border;
		if (SliderThumbStyle == null)
		{
			SetValue(SliderThumbStyleProperty, (Style)FindResource("SliderThumbStyle"));
		}
		if (EditFieldStyle == null)
		{
			SetValue(EditFieldStyleProperty, (Style)FindResource("EditFieldStyle"));
		}
		_minThumb.Style = SliderThumbStyle;
		_maxThumb.Style = SliderThumbStyle;
		_rangeMinEdit.Style = EditFieldStyle;
		_rangeMaxEdit.Style = EditFieldStyle;
		if (!IsMinRangeShown)
		{
			_minThumb.Visibility = Visibility.Collapsed;
		}
		if (_containerCanvas != null)
		{
			_containerCanvas.PreviewMouseLeftButtonDown += ContainerCanvas_OnPreviewMouseLeftButtonDown;
			_containerCanvas.MouseLeftButtonUp += ContainerCanvas_OnMouseLeftButtonUp;
			_containerCanvas.SizeChanged += ContainerCanvas_OnSizeChanged;
			_containerCanvas.MouseLeave += ContainerCanvas_OnMouseLeave;
			_containerCanvas.MouseDown += ContainerCanvas_OnMouseDown;
			_containerCanvas.MouseMove += ContainerCanvas_OnMouseMove;
		}
		if (_rangeEdit != null)
		{
			_rangeEdit.GotKeyboardFocus += SaveRangeSelectionState;
			_rangeEdit.LostFocus += RangeEdit_OnLostFocus;
			_rangeEdit.KeyDown += RangeEdit_OnKeyDown;
			_rangeEdit.TextChanged += RangeEditSingle_TextChanged;
			_rangeEdit.PreviewTextInput += RangeEdit_PreviewTextInput;
			_rangeEdit.PreviewKeyDown += RangeEdit_PreviewKeyDown;
			DataObject.AddPastingHandler(_rangeEdit, RangeEdit_Pasting);
		}
		SubscribeEditboxEvents(_rangeMinEdit);
		SubscribeEditboxEvents(_rangeMaxEdit);
		base.IsEnabledChanged += OnIsEnabledChanged;
		base.PreviewMouseMove += OnPreviewMouseMove;
		VisualStateManager.GoToState(this, base.IsEnabled ? "Normal" : "Disabled", useTransitions: false);
		base.OnApplyTemplate();
	}

	private void SubscribeEditboxEvents(TextBox rangeEdit)
	{
		if (rangeEdit != null)
		{
			rangeEdit.TextChanged += RangeEditMinMax_OnTextChanged;
			rangeEdit.GotKeyboardFocus += SaveRangeSelectionState;
			rangeEdit.LostFocus += RangeEdit_OnLostFocus;
			rangeEdit.KeyDown += RangeEdit_OnKeyDown;
			rangeEdit.PreviewTextInput += RangeEdit_PreviewTextInput;
			rangeEdit.PreviewKeyDown += RangeEdit_PreviewKeyDown;
			DataObject.AddPastingHandler(rangeEdit, RangeEdit_Pasting);
		}
	}

	private void UnSubscribeEditboxEvents(TextBox rangeEdit)
	{
		if (rangeEdit != null)
		{
			rangeEdit.TextChanged -= RangeEditMinMax_OnTextChanged;
			rangeEdit.GotKeyboardFocus -= SaveRangeSelectionState;
			rangeEdit.LostFocus -= RangeEdit_OnLostFocus;
			rangeEdit.KeyDown -= RangeEdit_OnKeyDown;
			rangeEdit.PreviewTextInput -= RangeEdit_PreviewTextInput;
			rangeEdit.PreviewKeyDown -= RangeEdit_PreviewKeyDown;
			DataObject.RemovePastingHandler(rangeEdit, RangeEdit_Pasting);
		}
	}

	private void RangeEdit_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		e.Handled = !ValidateRangeEditTextChange((TextBox)sender, e.Text);
	}

	private void RangeEdit_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Space)
		{
			e.Handled = true;
		}
	}

	private void RangeEdit_Pasting(object sender, DataObjectPastingEventArgs e)
	{
		string text = (string)e.DataObject.GetData(typeof(string));
		TextBox rangeEdit = sender as TextBox;
		if (ValidateRangeEditTextChange(rangeEdit, text))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < text.Length; i++)
		{
			if (ValidateRangeEditTextChange(rangeEdit, stringBuilder.ToString() + text[i]))
			{
				stringBuilder.Append(text[i]);
			}
		}
		DataObject dataObject = new DataObject();
		dataObject.SetData(DataFormats.Text, stringBuilder.ToString());
		e.DataObject = dataObject;
	}

	private bool ValidateRangeEditTextChange(TextBox rangeEdit, string textToAdd)
	{
		if (string.IsNullOrWhiteSpace(textToAdd))
		{
			return false;
		}
		StringBuilder stringBuilder = new StringBuilder(rangeEdit.Text);
		if (!string.IsNullOrEmpty(rangeEdit.SelectedText))
		{
			stringBuilder.Replace(rangeEdit.SelectedText, textToAdd, rangeEdit.SelectionStart, rangeEdit.SelectionLength);
		}
		else
		{
			stringBuilder.Insert(rangeEdit.SelectionStart, textToAdd);
		}
		return _rangeEditRegex.IsMatch(stringBuilder.ToString());
	}

	private void RangeEdit_OnKeyDown(object sender, KeyEventArgs args)
	{
		if (args.Key == Key.Return)
		{
			RangeMin = CoerceTicks(RangeMin, updateDragPosition: false);
			RangeMax = CoerceTicks(RangeMax, updateDragPosition: false);
			if (!MakeState().Equals(_rangeSelectionState))
			{
				FireRangeChanged(RangeMin, RangeMax);
				_rangeSelectionState = MakeState();
			}
			if (IsRangeMinEqualMax)
			{
				Keyboard.ClearFocus();
			}
			((TextBox)sender).SelectAll();
			Refresh(_rangeMinEdit);
			Refresh(_rangeMaxEdit);
			SyncThumbs();
		}
	}

	private void RangeEdit_OnLostFocus(object sender, RoutedEventArgs args)
	{
		RangeMin = CoerceTicks(RangeMin, updateDragPosition: false);
		RangeMax = CoerceTicks(RangeMax, updateDragPosition: false);
		if (!MakeState().Equals(_rangeSelectionState))
		{
			FireRangeChanged(RangeMin, RangeMax);
		}
		if (!IsDragging)
		{
			_rangeSelectionState = null;
		}
		Refresh(_rangeMinEdit);
		Refresh(_rangeMaxEdit);
		SyncThumbs();
	}

	private void SaveRangeSelectionState(object sender, KeyboardFocusChangedEventArgs args)
	{
		_rangeSelectionState = MakeState();
	}

	private void RangeEditMinMax_OnTextChanged(object sender, TextChangedEventArgs e)
	{
		SyncThumbs();
	}

	private void RangeEditSingle_TextChanged(object sender, TextChangedEventArgs e)
	{
		FormattedText formattedText = RenderText(_rangeEdit.Text, _rangeEdit);
		double length = CalcEditOffset(Math.Max(formattedText.Width, _rangeEdit.MinWidth), RangeMin, RangeMax, 0.0, _containerCanvas.ActualWidth - Math.Max(formattedText.Width, _rangeEdit.MinWidth), 0.0, calculateLowBound: false);
		Canvas.SetLeft(_rangeEdit, length);
	}

	public void OnPreviewMouseMove(object s, MouseEventArgs e)
	{
		OnPropertyChanged("IsMouseOverRangeBar");
	}

	private void EnforceDataInvariant()
	{
		if (MinValue > MaxValue)
		{
			MinValue = MaxValue;
		}
		if (RangeMin < MinValue)
		{
			RangeMin = MinValue;
		}
		if (RangeMax < RangeMin)
		{
			RangeMax = RangeMin;
		}
	}

	private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		VisualStateManager.GoToState(this, base.IsEnabled ? "Normal" : "Disabled", useTransitions: true);
	}

	private void ContainerCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
		{
			double value = OffsetToValue(Math.Max(Mouse.GetPosition(_containerCanvas).X - _minThumb.ActualWidth / 2.0, 0.0));
			value = CoerceTicks(value, updateDragPosition: false);
			if (IsMinRangeShown && ((IsRangeMinEqualMax && value < RangeMin) || (!IsRangeMinEqualMax && Math.Abs(value - RangeMin) < Math.Abs(value - RangeMax))))
			{
				RangeMin = value;
			}
			else
			{
				RangeMax = value;
			}
			FireRangeChanged(RangeMin, RangeMax);
			SyncThumbs();
			e.Handled = true;
		}
	}

	private void ContainerCanvas_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (_rangeMinEdit.IsKeyboardFocusWithin)
		{
			PrepareDrag(_rangeMinEdit);
		}
		else if (_rangeMaxEdit.IsKeyboardFocusWithin)
		{
			PrepareDrag(_rangeMaxEdit);
		}
		if (IsMouseHit(_minThumb, e) || IsMouseHit(_maxThumb, e))
		{
			StartDrag(IsMouseHit(_maxThumb, e) ? _maxThumb : _minThumb);
			e.Handled = true;
		}
	}

	private static void PrepareDrag(TextBox edit)
	{
		BindingOperations.GetBindingExpression(edit, TextBox.TextProperty)?.UpdateSource();
		Keyboard.ClearFocus();
	}

	private static bool IsMouseHit(UIElement elem, MouseButtonEventArgs e)
	{
		return VisualTreeHelper.HitTest(elem, e.GetPosition(elem)) != null;
	}

	private void StartDrag(Thumb thumb)
	{
		Mouse.Capture(_containerCanvas, CaptureMode.Element);
		_dragThumbOffset = Mouse.GetPosition(thumb).X;
		_dragOriginX = (_dragPosition = Mouse.GetPosition(_containerCanvas).X);
		if (IsRangeMinEqualMax && IsMinRangeShown)
		{
			_dragEntity = DragEntity.RangeSingle;
		}
		else if (thumb == _minThumb)
		{
			_dragEntity = DragEntity.RangeMin;
		}
		else
		{
			_dragEntity = DragEntity.RangeMax;
		}
		_rangeSelectionState = MakeState();
		SyncThumbs();
		VisualStateManager.GoToState(this, (thumb == _minThumb) ? "MinPressed" : "MaxPressed", useTransitions: true);
		Keyboard.ClearFocus();
	}

	private void ContainerCanvas_OnMouseMove(object sender, MouseEventArgs e)
	{
		if (!IsDragging)
		{
			return;
		}
		double x = e.GetPosition(_containerCanvas).X;
		_dragPosition = x - _dragThumbOffset;
		_dragPosition = Math.Min(_dragPosition, DragWidth());
		_dragPosition = Math.Max(_dragPosition, 0.0);
		double num = CoerceTicks(OffsetToValue(_dragPosition));
		if (_dragEntity == DragEntity.RangeSingle && Math.Abs(_dragPosition - _dragOriginX) > 1.0)
		{
			if (_dragPosition > _dragOriginX)
			{
				_dragEntity = DragEntity.RangeMax;
				RangeMax = num;
			}
			else if (_dragPosition < _dragOriginX - _minThumb.ActualWidth)
			{
				_dragEntity = DragEntity.RangeMin;
				RangeMin = num;
			}
		}
		else if (_dragEntity == DragEntity.RangeMin)
		{
			RangeMin = num;
		}
		else if (_dragEntity == DragEntity.RangeMax)
		{
			RangeMax = num;
		}
		SyncThumbs();
	}

	private void ContainerCanvas_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (IsDragging)
		{
			StopDrag();
			e.Handled = true;
		}
	}

	private void ContainerCanvas_OnMouseLeave(object sender, MouseEventArgs e)
	{
		VisualStateManager.GoToState(this, "Normal", useTransitions: false);
	}

	private void StopDrag()
	{
		Mouse.Capture(null);
		SyncThumbs();
		_dragPosition = (_dragOriginX = double.NaN);
		_dragEntity = DragEntity.None;
		VisualStateManager.GoToState(this, "Normal", useTransitions: true);
		OnPropertyChanged("IsMouseOverRangeBar");
		_rangeMinEdit?.SelectAll();
		_rangeMaxEdit?.SelectAll();
		if (!MakeState().Equals(_rangeSelectionState))
		{
			FireRangeChanged(RangeMin, RangeMax);
		}
		_rangeSelectionState = null;
	}

	private void ContainerCanvas_OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		OnPropertyChanged("IsMouseOverRangeBar");
		SyncThumbs();
	}

	private double DragWidth()
	{
		return _containerCanvas.ActualWidth - _maxThumb.Width;
	}

	private double OffsetToValue(double offsetX)
	{
		return MinValue + (MaxValue - MinValue) * offsetX / DragWidth();
	}

	private double ValueToDragPosition(double value)
	{
		double num = (value - MinValue) / (MaxValue - MinValue) * DragWidth();
		if (!double.IsNaN(num))
		{
			return num;
		}
		return 0.0;
	}

	private double CoerceTicks(double value, bool updateDragPosition = true)
	{
		value = Math.Round(value / TickFrequency, MidpointRounding.AwayFromZero) * TickFrequency;
		if (updateDragPosition)
		{
			_dragPosition = ValueToDragPosition(value);
		}
		return value;
	}

	private void SyncThumbs()
	{
		if (_containerCanvas != null)
		{
			double num = ValueToDragPosition(RangeMin);
			double num2 = ValueToDragPosition(RangeMax);
			Canvas.SetLeft(_minThumb, num);
			Canvas.SetLeft(_maxThumb, num2);
			IsThumbsOverlapping = num + _minThumb.ActualWidth > num2;
			double num3 = ValueToDragPosition((RangeMax + RangeMin) / 2.0);
			double num4 = CalcEditOffset(_rangeMinEdit.ActualWidth, RangeMin, RangeMax, 0.0, _containerCanvas.ActualWidth - _rangeMaxEdit.ActualWidth - _rangeMinEdit.ActualWidth - 1.0, num3 - _rangeMinEdit.ActualWidth + _minThumb.ActualWidth / 2.0 - 1.0, calculateLowBound: true);
			Canvas.SetLeft(_rangeMinEdit, num4);
			double minAllowedOffset = (IsMinRangeShown ? (_rangeMinEdit.ActualWidth + 1.0) : 0.0);
			double repelPosition = (IsMinRangeShown ? (num3 + _maxThumb.ActualWidth / 2.0) : 0.0);
			double num5 = CalcEditOffset(_rangeMaxEdit.ActualWidth, RangeMax, RangeMin, minAllowedOffset, _containerCanvas.ActualWidth - _rangeMaxEdit.ActualWidth, repelPosition, calculateLowBound: false);
			Canvas.SetLeft(_rangeMaxEdit, num5);
			double length = num4;
			if (num3 > DragWidth() / 2.0)
			{
				length = num5;
			}
			Canvas.SetLeft(_rangeEdit, length);
			IsRangeMinEqualMax = RangeMax == RangeMin;
			_rangeEdit.Visibility = ((!IsRangeMinEqualMax || !IsMinRangeShown) ? Visibility.Hidden : Visibility.Visible);
			_rangeMaxEdit.Visibility = ((IsRangeMinEqualMax && IsMinRangeShown) ? Visibility.Hidden : Visibility.Visible);
			_rangeMinEdit.Visibility = (IsMinRangeShown ? _rangeMaxEdit.Visibility : Visibility.Collapsed);
			if (IsRangeMinEqualMax)
			{
				RangeSingle = RangeMin;
			}
			_selectedRange.Width = num2 - num;
			_selectedRange.Margin = new Thickness(num + _minThumb.ActualWidth * 0.5, 0.0, 0.0, 0.0);
		}
	}

	private double CalcEditOffset(double rangeEditWidth, double value, double otherValue, double minAllowedOffset, double maxAllowedOffset, double repelPosition, bool calculateLowBound)
	{
		double num = ValueToDragPosition(value);
		double num2 = (rangeEditWidth + 1.0) / 2.0;
		double num3 = (_maxThumb.ActualWidth + 1.0) / 2.0;
		num = num - num2 + num3;
		if (Math.Abs(value - otherValue) > 0.0001)
		{
			num = ((!calculateLowBound) ? Math.Max(Math.Max(minAllowedOffset, repelPosition), num) : Math.Min(num, Math.Min(maxAllowedOffset, repelPosition)));
		}
		num = Math.Min(num, maxAllowedOffset);
		return Math.Max(minAllowedOffset, num);
	}

	private Tuple<double, double> MakeState()
	{
		return new Tuple<double, double>(RangeMin, RangeMax);
	}

	protected virtual void FireRangeChanged(double rangeMin, double rangeMax)
	{
		RangeChangedEventArgs e = new RangeChangedEventArgs(rangeMin, rangeMax);
		this.RangeChanged?.Invoke(this, e);
	}

	protected virtual void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private FormattedText RenderText(string text, TextBox styler)
	{
		Typeface typeface = new Typeface(styler.FontFamily, styler.FontStyle, styler.FontWeight, styler.FontStretch);
		return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, styler.FontSize, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
	}

	private void Refresh(UIElement elem)
	{
		elem.Dispatcher.Invoke(DispatcherPriority.Render, (Action)delegate
		{
		});
	}

	[Conditional("DEBUG")]
	private void __InvariantCheck()
	{
		_ = IsDragging;
	}
}
