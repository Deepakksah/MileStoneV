using System;
using System.ComponentModel;
using System.Security;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class UserInformation : IDisposable, IUserInformation, INotifyPropertyChanged
{
	private string _userName;

	private SecureString _password;

	private bool _disposed;

	public SecureString Password
	{
		get
		{
			return _password;
		}
		set
		{
			_password = value;
			OnPropertyChanged("Password");
		}
	}

	public string UserName
	{
		get
		{
			return _userName;
		}
		set
		{
			_userName = value;
			OnPropertyChanged("UserName");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public UserInformation(string userName, SecureString password)
	{
		_userName = userName;
		_password = password;
	}

	protected void OnPropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (!_disposed && _password != null)
			{
				_password.Dispose();
				_password = null;
			}
			_disposed = true;
		}
	}
}
