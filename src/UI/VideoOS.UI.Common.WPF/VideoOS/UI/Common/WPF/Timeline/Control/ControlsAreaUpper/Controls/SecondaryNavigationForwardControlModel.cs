using System.ComponentModel;

namespace VideoOS.UI.Common.WPF.Timeline.Control.ControlsAreaUpper.Controls;

public class SecondaryNavigationForwardControlModel : INotifyPropertyChanged
{
	private bool _navigationControlsEnabled;

	public bool NavigationControlsEnabled
	{
		get
		{
			return _navigationControlsEnabled;
		}
		set
		{
			if (_navigationControlsEnabled != value)
			{
				_navigationControlsEnabled = value;
				FirePropChanged("NavigationControlsEnabled");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void FirePropChanged(string propName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}
}
