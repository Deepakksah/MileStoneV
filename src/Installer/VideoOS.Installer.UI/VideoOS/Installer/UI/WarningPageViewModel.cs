using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WarningPageViewModel : INotifyPropertyChanged
{
	private bool checkBoxChecked;

	public string LabelHeadline => Resources.WarningPageHeader;

	public string LabelInfo => Resources.WarningPageHeaderInfo;

	public string CheckBoxInfo => Resources.WarningPageCheckBoxInfo;

	public bool CheckBoxChecked
	{
		get
		{
			return checkBoxChecked;
		}
		set
		{
			checkBoxChecked = value;
			OnPropertyChanged("CheckBoxChecked");
		}
	}

	public IEnumerable<WarningPageEntryViewModel> CommonPageItems { get; }

	public event PropertyChangedEventHandler PropertyChanged;

	public WarningPageViewModel(IEnumerable<WarningPageEntryViewModel> commonPageItems)
	{
		CommonPageItems = commonPageItems;
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
