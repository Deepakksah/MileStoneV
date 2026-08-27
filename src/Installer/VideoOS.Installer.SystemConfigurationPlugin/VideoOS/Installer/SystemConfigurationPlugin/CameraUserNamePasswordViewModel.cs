using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class CameraUserNamePasswordViewModel : IDisposable, INotifyPropertyChanged
{
	private bool _disposed;

	private ObservableCollection<IUserInformation> _userInfo;

	private bool _useHttps = true;

	private bool _useHttp;

	public ObservableCollection<IUserInformation> UserInfo => _userInfo;

	public bool UseHttps
	{
		get
		{
			return _useHttps;
		}
		set
		{
			_useHttps = value;
			OnPropertyChanged("UseHttps");
		}
	}

	public bool UseHttp
	{
		get
		{
			return _useHttp;
		}
		set
		{
			_useHttp = value;
			OnPropertyChanged("UseHttp");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public CameraUserNamePasswordViewModel(ObservableCollection<IUserInformation> userInformation)
	{
		_userInfo = userInformation;
	}

	public void SetUsers(IEnumerable<IUserInformation> users)
	{
		if (users == null)
		{
			throw new ArgumentNullException("users");
		}
		UserInfo.Clear();
		foreach (IUserInformation user in users)
		{
			UserInfo.Add(user);
		}
	}

	protected void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		if (!_disposed && _userInfo != null)
		{
			foreach (IUserInformation item in _userInfo)
			{
				item.Dispose();
			}
			_userInfo = null;
		}
		_disposed = true;
	}
}
