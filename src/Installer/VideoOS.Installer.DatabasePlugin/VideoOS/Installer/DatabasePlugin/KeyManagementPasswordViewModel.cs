using System;
using System.ComponentModel;
using System.Security;
using System.Windows;
using VideoOS.Installer.Common;
using VideoOS.Installer.DatabasePlugin.Properties;

namespace VideoOS.Installer.DatabasePlugin;

public class KeyManagementPasswordViewModel : IDisposable, INotifyPropertyChanged
{
	private static SecureString _emptySecureString = new SecureString();

	private bool _disposed;

	private SystemPasswordMode _systemPasswordMode;

	private bool _doNotUsePassword;

	private SecureString _password = _emptySecureString;

	private SecureString _repeatedPassword;

	public SystemPasswordMode SystemPasswordMode
	{
		get
		{
			return _systemPasswordMode;
		}
		set
		{
			_systemPasswordMode = value;
			ReloadResources();
		}
	}

	public bool DoNotUsePassword
	{
		get
		{
			return _doNotUsePassword;
		}
		set
		{
			if (value)
			{
				ClearPasswordEntries();
			}
			_doNotUsePassword = value;
			OnPropertyChanged("DoNotUsePassword");
		}
	}

	public string HeaderText
	{
		get
		{
			if (ShowEnterExistingPassword)
			{
				return Resources.KeyManagementConfirmPasswordPageHeader;
			}
			return Resources.KeyManagementPasswordPageHeader;
		}
	}

	public string PageInfoText
	{
		get
		{
			if (ShowEnterExistingPassword)
			{
				return Resources.KeyManagementConfirmPasswordPageInfo;
			}
			return Resources.KeyManagementPasswordPageInfo;
		}
	}

	public string DoNotUsePasswordText
	{
		get
		{
			if (ShowEnterExistingPassword)
			{
				return Resources.KeyManagementNoExistingPassword;
			}
			return Resources.KeyManagementDoNotAssignPassword;
		}
	}

	public string PageInfoWarningText => Resources.KeyManagementPasswordPageInfoWarning;

	public string PasswordLabelText => Resources.KeyManagementPassword;

	public string ConfirmPasswordLabelText => Resources.KeyManagementConfirmPassword;

	public bool ShowEnterExistingPassword => _systemPasswordMode == SystemPasswordMode.EnterExistingPassword;

	public SecureString Password
	{
		get
		{
			return _password;
		}
		set
		{
			SecureString secureString = (DoNotUsePassword ? _emptySecureString : value);
			if (_password != secureString)
			{
				_password = secureString;
				OnPropertyChanged("Password");
			}
		}
	}

	public SecureString RepeatedPassword
	{
		get
		{
			return _repeatedPassword;
		}
		set
		{
			SecureString secureString = (DoNotUsePassword ? _emptySecureString : value);
			if (_repeatedPassword != secureString)
			{
				_repeatedPassword = secureString;
				OnPropertyChanged("RepeatedPassword");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void ClearPasswordEntries()
	{
		Password = new SecureString();
		RepeatedPassword = new SecureString();
	}

	protected void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public void ReloadResources()
	{
		OnPropertyChanged("HeaderText");
		OnPropertyChanged("PageInfoText");
		OnPropertyChanged("PageInfoWarningText");
		OnPropertyChanged("PasswordLabelText");
		OnPropertyChanged("ConfirmPasswordLabelText");
		OnPropertyChanged("ShowEnterExistingPassword");
		OnPropertyChanged("DoNotUsePasswordText");
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
			if (!_disposed)
			{
				_emptySecureString.Dispose();
			}
			_disposed = true;
		}
	}

	public virtual void ShowErrorMessage(string message)
	{
		MessageBox.Show(message, Resources.KeyManagementPasswordPageHeader, MessageBoxButton.OK, MessageBoxImage.Hand);
	}
}
