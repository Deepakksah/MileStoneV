using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.TimePicker;

public class IntTextBox : TextBoxBase
{
	private readonly int _maxNumberOfDigits = 2;

	private int _maxValue;

	private int _minValue;

	private string _lastKeyPressed = "";

	public int MaxValue
	{
		get
		{
			return _maxValue;
		}
		set
		{
			_maxValue = value;
			SetValue(Text);
			Invalidate();
		}
	}

	public int MinValue
	{
		get
		{
			return _minValue;
		}
		set
		{
			_minValue = value;
			SetValue(Text);
		}
	}

	public new string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			SetValue(value);
		}
	}

	public int Value
	{
		get
		{
			return GetValue(Text);
		}
		set
		{
			Text = value.ToString();
		}
	}

	public event EventHandler OnHasLoopedAround;

	public IntTextBox()
	{
		base.MaxLength = _maxNumberOfDigits;
		base.TextChanged += TimeTextBox_TextChanged;
		base.LostFocus += TimeTextBox_LostFocus;
		base.KeyDown += TimeTextBox_KeyDown;
		base.PreviewKeyDown += IntTextBox_PreviewKeyDown;
	}

	private void IntTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		switch (e.Key)
		{
		case Key.Back:
		case Key.Tab:
		case Key.End:
		case Key.Home:
		case Key.Left:
		case Key.Up:
		case Key.Right:
		case Key.Down:
		case Key.Delete:
		case Key.NumPad0:
		case Key.NumPad1:
		case Key.NumPad2:
		case Key.NumPad3:
		case Key.NumPad4:
		case Key.NumPad5:
		case Key.NumPad6:
		case Key.NumPad7:
		case Key.NumPad8:
		case Key.NumPad9:
			return;
		case Key.Next:
			Decrease(10);
			return;
		case Key.Prior:
			Increase(10);
			return;
		}
		if (!char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.Return && e.Key != Key.Return)
		{
			e.Handled = true;
		}
	}

	private void TimeTextBox_KeyDown(object sender, KeyEventArgs e)
	{
		switch (e.Key)
		{
		case Key.NumPad0:
			_lastKeyPressed = "0";
			break;
		case Key.NumPad1:
			_lastKeyPressed = "1";
			break;
		case Key.NumPad2:
			_lastKeyPressed = "2";
			break;
		case Key.NumPad3:
			_lastKeyPressed = "3";
			break;
		case Key.NumPad4:
			_lastKeyPressed = "4";
			break;
		case Key.NumPad5:
			_lastKeyPressed = "5";
			break;
		case Key.NumPad6:
			_lastKeyPressed = "6";
			break;
		case Key.NumPad7:
			_lastKeyPressed = "7";
			break;
		case Key.NumPad8:
			_lastKeyPressed = "8";
			break;
		case Key.NumPad9:
			_lastKeyPressed = "9";
			break;
		case Key.Tab:
		{
			string value = Validate(GetValue(Text), useKeyDown: true).ToString();
			value = FormatString(value);
			if (base.Text != value)
			{
				SetText(value);
			}
			FireOnValidated();
			Select(Text.Length, 0);
			break;
		}
		default:
			_lastKeyPressed = ((char)KeyInterop.VirtualKeyFromKey(e.Key)).ToString();
			break;
		}
	}

	private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
	{
		string value = Validate(GetValue(Text), useKeyDown: true).ToString();
		value = FormatString(value);
		if (base.Text != value)
		{
			SetText(value);
		}
		FireOnValidated();
	}

	private void TimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (!_isSettingTextInternal)
		{
			string text = Validate(GetValue(Text), useKeyDown: true).ToString();
			if (GetValue(base.Text) != GetValue(text))
			{
				SetText(text);
			}
			FireOnValidated();
			Select(Text.Length, 0);
		}
	}

	public override void Increase()
	{
		Increase(1);
	}

	public override void Decrease()
	{
		Decrease(1);
	}

	private void SetValue(string value)
	{
		SetValue(GetValue(value));
	}

	private void SetValue(int value)
	{
		value = Validate(value, useKeyDown: false);
		string value2 = value.ToString();
		string text = FormatString(value2);
		if (GetValue(base.Text) != GetValue(text))
		{
			SetText(text);
			FireOnValidated();
		}
	}

	private string FormatString(string value)
	{
		if (value.Length == 0)
		{
			return "00";
		}
		if (value.Length == 1)
		{
			return "0" + value;
		}
		return value;
	}

	private void Increase(int value)
	{
		int value2 = GetValue(Text);
		value2 += value;
		SetValue(value2);
	}

	private void Decrease(int value)
	{
		int value2 = GetValue(Text);
		value2 -= value;
		SetValue(value2);
	}

	private int Validate(int value, bool useKeyDown)
	{
		if (value > _maxValue)
		{
			if (useKeyDown)
			{
				value = GetValue(_lastKeyPressed);
			}
			else
			{
				value = _minValue;
				if (this.OnHasLoopedAround != null)
				{
					this.OnHasLoopedAround(this, new EventArgs());
				}
			}
		}
		else if (value < _minValue)
		{
			if (useKeyDown)
			{
				value = GetValue(_lastKeyPressed);
			}
			else
			{
				value = _maxValue;
				if (this.OnHasLoopedAround != null)
				{
					this.OnHasLoopedAround(this, new EventArgs());
				}
			}
		}
		return value;
	}

	private int GetValue(string value)
	{
		if (int.TryParse(value, out var result))
		{
			return result;
		}
		return 0;
	}
}
