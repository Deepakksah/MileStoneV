using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoOS.IM.Common.GlobalDefinitions.Configuration;

namespace VideoOS.IM.Common.Service.Common;

public class ManagerOptions : INotifyPropertyChanged
{
	private bool _GlobalDefaultAccessTypeAllowEverything;

	private IncidentProperties _IncidentExtendedPropsConfig;

	public bool GlobalDefaultAccessTypeAllowEverything
	{
		get
		{
			return _GlobalDefaultAccessTypeAllowEverything;
		}
		set
		{
			if (_GlobalDefaultAccessTypeAllowEverything != value)
			{
				_GlobalDefaultAccessTypeAllowEverything = value;
				NotifyPropertyChanged("GlobalDefaultAccessTypeAllowEverything");
			}
		}
	}

	public IncidentProperties IncidentExtendedPropsConfig
	{
		get
		{
			return _IncidentExtendedPropsConfig;
		}
		set
		{
			if (_IncidentExtendedPropsConfig != value)
			{
				_IncidentExtendedPropsConfig = value;
				NotifyPropertyChanged("IncidentExtendedPropsConfig");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public ManagerOptions()
	{
		GlobalDefaultAccessTypeAllowEverything = false;
	}

	private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
