using System.Windows.Input;
using VideoOS.Platform.UI.Commands;

namespace VideoOS.Platform.UI.Controls;

public class VideoOSDialogActionCheckBox : VideoOSDialogActionBase
{
	private bool _isChecked;

	private string __text;

	public override bool CanResize => true;

	public override double DesiredWidthPadding => 1.0;

	public bool IsChecked
	{
		get
		{
			return _isChecked;
		}
		set
		{
			if (_isChecked != value)
			{
				_isChecked = value;
				OnPropertyChanged("IsChecked");
			}
		}
	}

	public string Text
	{
		get
		{
			return __text;
		}
		set
		{
			if (__text != value)
			{
				__text = value;
				OnPropertyChanged("Text");
			}
		}
	}

	public ICommand CheckedUncheckedCommand { get; }

	public VideoOSDialogActionCheckBox()
	{
		CheckedUncheckedCommand = new DelegateCommand(HandleCheckedChanged);
	}

	private void HandleCheckedChanged(object obj)
	{
		EventsToCommandMappingArgs obj2 = (EventsToCommandMappingArgs)obj;
		if (obj2.EventName == "Checked")
		{
			IsChecked = true;
		}
		if (obj2.EventName == "Unchecked")
		{
			IsChecked = false;
		}
	}
}
