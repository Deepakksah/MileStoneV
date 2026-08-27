using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.NumericUpDownControl;

public partial class NumericUpDown : UserControl, IComponentConnector
{
	public static readonly DependencyProperty NumericValueProperty = DependencyProperty.Register("NumericValue", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, NumericValuePropertyChanged, null, isAnimationProhibited: false, UpdateSourceTrigger.PropertyChanged));

	public static readonly DependencyProperty NumericMaxValueProperty = DependencyProperty.Register("NumericMaxValue", typeof(int), typeof(NumericUpDown));

	public static readonly DependencyProperty NumericMinValueProperty = DependencyProperty.Register("NumericMinValue", typeof(int), typeof(NumericUpDown));

	public int NumericValue
	{
		get
		{
			return (int)GetValue(NumericValueProperty);
		}
		set
		{
			SetValue(NumericValueProperty, value);
			NumericUpDownInput.Text = value.ToString(CultureInfo.CurrentCulture);
		}
	}

	public int NumericMaxValue
	{
		get
		{
			return (int)GetValue(NumericMaxValueProperty);
		}
		set
		{
			SetValue(NumericMaxValueProperty, value);
		}
	}

	public int NumericMinValue
	{
		get
		{
			return (int)GetValue(NumericMinValueProperty);
		}
		set
		{
			SetValue(NumericMinValueProperty, value);
		}
	}

	public NumericUpDown()
	{
		InitializeComponent();
		NumericMinValue = 1;
		NumericMaxValue = 99999;
	}

	private static void NumericValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
	{
		if (target is NumericUpDown numericUpDown)
		{
			numericUpDown.NumericUpDownInput.Text = e.NewValue.ToString();
		}
	}

	private void NumericUpDownTextChanged(object sender, TextChangedEventArgs e)
	{
		if (!NumericUpDownInput.IsFocused)
		{
			return;
		}
		if (int.TryParse(NumericUpDownInput.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			if (result < NumericMinValue)
			{
				NumericValue = NumericMinValue;
			}
			else if (result > NumericMaxValue)
			{
				NumericValue = NumericMaxValue;
			}
			else
			{
				NumericValue = result;
			}
		}
		else if (!NumericUpDownInput.Text.Equals(string.Empty) && !NumericUpDownInput.Text.Equals("-"))
		{
			NumericUpDownInput.Text = NumericValue.ToString(CultureInfo.CurrentCulture);
		}
	}

	private void IncreaseButtonClicked(object sender, RoutedEventArgs e)
	{
		if (NumericValue < NumericMaxValue)
		{
			NumericValue++;
		}
	}

	private void DecreaseButtonClicked(object sender, RoutedEventArgs e)
	{
		if (NumericValue > NumericMinValue)
		{
			NumericValue--;
		}
	}

	private void NumericPreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		Regex regex = new Regex("-?\\d+$");
		e.Handled = !regex.IsMatch(e.Text) && !e.Text.Equals("-");
	}

	private void NumericPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.IsDown && e.Key == Key.Up && NumericValue < NumericMaxValue)
		{
			NumericValue++;
		}
		else if (e.IsDown && e.Key == Key.Down && NumericValue > NumericMinValue)
		{
			NumericValue--;
		}
	}
}
