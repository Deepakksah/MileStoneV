using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.TimePicker;

public class StringTextBox : TextBoxBase
{
	private int _selectedIndex;

	private string[] _outcome;

	public string[] Outcome
	{
		get
		{
			return _outcome;
		}
		set
		{
			_outcome = value;
		}
	}

	public StringTextBox()
	{
		base.PreviewKeyDown += StringTextBox_PreviewKeyDown;
	}

	private void StringTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		string text = ((char)KeyInterop.VirtualKeyFromKey(e.Key)).ToString();
		string[] outcome = Outcome;
		foreach (string text2 in outcome)
		{
			if (text2.Substring(0, 1).ToLower() == text.ToLower())
			{
				SetSelected(text2);
				break;
			}
		}
		if (e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Tab)
		{
			e.Handled = true;
		}
	}

	public override void Increase()
	{
		_selectedIndex++;
		if (_selectedIndex >= _outcome.Length)
		{
			_selectedIndex = 0;
		}
		UpdateText();
	}

	public override void Decrease()
	{
		_selectedIndex--;
		if (_selectedIndex < 0)
		{
			_selectedIndex = _outcome.Length - 1;
		}
		UpdateText();
	}

	public void SetSelected(string selected)
	{
		if (_outcome == null)
		{
			return;
		}
		for (int i = 0; i < _outcome.Length; i++)
		{
			if (_outcome[i].Equals(selected))
			{
				_selectedIndex = i;
				UpdateText();
				break;
			}
		}
	}

	private void UpdateText()
	{
		if (_outcome != null && _selectedIndex < _outcome.Length && _selectedIndex >= 0)
		{
			base.Text = _outcome[_selectedIndex];
			FireOnValidated();
		}
	}
}
