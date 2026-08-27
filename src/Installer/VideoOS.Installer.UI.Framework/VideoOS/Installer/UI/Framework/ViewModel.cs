using System.ComponentModel;

namespace VideoOS.Installer.UI.Framework;

public class ViewModel : INotifyPropertyChanged
{
	private bool _isProgressvis;

	private bool _isSucVis;

	private bool _isWarningVis;

	private bool _isProcessSuccessIndicatorVisible;

	private string _warningMessageText;

	private string _upgradeLicenseMessage;

	private string _availableLicenseMessage;

	public bool IsProgressIndicatorVisible
	{
		get
		{
			return _isProgressvis;
		}
		set
		{
			_isProgressvis = value;
			NotifyPropertyChanged("IsProgressIndicatorVisible");
		}
	}

	public bool IsSuccessIndicatorVisible
	{
		get
		{
			return _isSucVis;
		}
		set
		{
			_isSucVis = value;
			NotifyPropertyChanged("IsSuccessIndicatorVisible");
		}
	}

	public bool IsWarningIndicatorVisible
	{
		get
		{
			return _isWarningVis;
		}
		set
		{
			_isWarningVis = value;
			NotifyPropertyChanged("IsWarningIndicatorVisible");
		}
	}

	public bool IsProcessSuccessIndicatorVisible
	{
		get
		{
			return _isProcessSuccessIndicatorVisible;
		}
		set
		{
			_isProcessSuccessIndicatorVisible = value;
			NotifyPropertyChanged("IsProcessSuccessIndicatorVisible");
		}
	}

	public string WarningMessageText
	{
		get
		{
			return _warningMessageText;
		}
		set
		{
			_warningMessageText = value;
			NotifyPropertyChanged("WarningMessageText");
		}
	}

	public string UpgradeLicenseMessage
	{
		get
		{
			return _upgradeLicenseMessage;
		}
		set
		{
			_upgradeLicenseMessage = value;
			NotifyPropertyChanged("UpgradeLicenseMessage");
		}
	}

	public string AvailableLicenseMessage
	{
		get
		{
			return _availableLicenseMessage;
		}
		set
		{
			_availableLicenseMessage = value;
			NotifyPropertyChanged("AvailableLicenseMessage");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void NotifyPropertyChanged(string info)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(info));
		}
	}
}
