using System;
using System.ComponentModel;

namespace VideoOS.Installer.Setup;

public class SetupViewModel : INotifyPropertyChanged
{
	private int _progressBarProgress;

	private string _statusProgressState;

	public IProgress<int> ProgressBar { get; }

	public IProgress<string> StatusProgress { get; }

	public int ProgressBarProgress
	{
		get
		{
			return _progressBarProgress;
		}
		set
		{
			_progressBarProgress = value;
			OnPropertyChanged("ProgressBarProgress");
		}
	}

	public string StatusProgressState
	{
		get
		{
			return _statusProgressState;
		}
		set
		{
			_statusProgressState = value;
			OnPropertyChanged("StatusProgressState");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public SetupViewModel()
	{
		ProgressBar = new Progress<int>(delegate(int procent)
		{
			ProgressBarProgress = procent;
		});
		StatusProgress = new Progress<string>(delegate(string status)
		{
			StatusProgressState = status;
		});
	}

	private void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
