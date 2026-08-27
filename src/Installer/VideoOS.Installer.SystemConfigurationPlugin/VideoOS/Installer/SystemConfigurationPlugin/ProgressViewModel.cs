using System.ComponentModel;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ProgressViewModel : INotifyPropertyChanged
{
	private double _currentProgress;

	private string _header;

	public string Header
	{
		get
		{
			return _header;
		}
		set
		{
			_header = value;
			OnPropertyChanged("Header");
		}
	}

	public double CurrentProgress
	{
		get
		{
			return _currentProgress;
		}
		private set
		{
			_currentProgress = value;
			OnPropertyChanged("CurrentProgress");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public ProgressViewModel()
	{
		_currentProgress = 0.0;
		_header = "";
	}

	public void SetProgress(int value)
	{
		CurrentProgress = value;
	}

	protected void OnPropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
