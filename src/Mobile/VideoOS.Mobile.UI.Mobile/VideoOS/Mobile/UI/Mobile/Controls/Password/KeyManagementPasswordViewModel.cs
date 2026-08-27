using System;
using System.ComponentModel;
using System.Security;
using System.Windows;
using VideoOS.Utilities.Security;

namespace VideoOS.Mobile.UI.Mobile.Controls.Password;

public class KeyManagementPasswordViewModel : IDisposable, INotifyPropertyChanged
{
	private static SecureString _emptySecureString = new SecureString();

	private bool _disposed;

	private bool _doNotUsePassword;

	private SecureString _password = _emptySecureString;

	private SecureString _repeatedPassword;

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

	public string HeaderText { get; set; }

	public string PageInfoText { get; set; } = Resources.KeyManagementPasswordPageInfo;

	public string DoNotUsePasswordText => Resources.KeyManagementDoNotAssignPassword;

	public string PageInfoWarningText => Resources.KeyManagementPasswordPageInfoWarning;

	public string PasswordLabelText { get; set; } = Resources.KeyManagementPassword;

	public string ConfirmPasswordLabelText { get; set; } = Resources.KeyManagementConfirmPassword;

	public string MessageDialogText { get; set; } = Resources.KeyManagementPasswordPageHeader;

	public bool IsHeaderVisible => !string.IsNullOrEmpty(HeaderText);

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

	public bool Validate()
	{
		bool result = true;
		if (!DoNotUsePassword)
		{
			PasswordValidationResult passwordValidationResult = new PasswordValidator().ValidateNewPassword(Password, RepeatedPassword);
			if (passwordValidationResult != PasswordValidationResult.Ok)
			{
				ShowErrorMessage(ToErrorMessage(passwordValidationResult));
				result = false;
			}
		}
		return result;
	}

	private static string ToErrorMessage(PasswordValidationResult result)
	{
		switch (result)
		{
		case PasswordValidationResult.Ok:
			return string.Empty;
		case PasswordValidationResult.PasswordsDiffers:
			return Resources.NewPasswordPasswordsDoNotMatch;
		case PasswordValidationResult.PasswordIsEmpty:
			return Resources.NewPasswordEmpty;
		case PasswordValidationResult.TooShort:
			return Resources.NewPasswordTooShort;
		case PasswordValidationResult.TooLong:
			return Resources.NewPasswordTooLong;
		case PasswordValidationResult.ComplexityPolicyNotMet:
		case PasswordValidationResult.LocalPasswordFilterNotMet:
			return Resources.NewPasswordDoesNotMeetComplexityPolicy;
		case PasswordValidationResult.OtherError:
			return Resources.NewPasswordInvalid;
		default:
			return Resources.NewPasswordInvalid;
		}
	}

	public virtual void ShowErrorMessage(string message)
	{
		MessageBox.Show(message, MessageDialogText, MessageBoxButton.OK, MessageBoxImage.Hand);
	}
}
