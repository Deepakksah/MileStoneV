using System.ComponentModel;

namespace VideoOS.UI.Common.WPF.SplitButton;

public class ListBoxItemWrapper : INotifyPropertyChanged
{
	private bool _selected;

	public SplitButtonItem SplitButtonItem { get; set; }

	public bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			if (value != _selected)
			{
				_selected = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Selected"));
				}
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;
}
