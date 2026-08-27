using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows.Input;
using System.Windows.Media;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Management.Common;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;
using VideoOS.Utilities.Security;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class UserPageViewModel : ViewModelBase, IDisposable
{
	public enum UserType
	{
		WindowsUser,
		BasicUser
	}

	private Identity _identity;

	private bool _disposed;

	private SolidColorBrush ErrorColorBrush = (SolidColorBrush)new BrushConverter().ConvertFrom(ErrorColor);

	private readonly Dictionary<UserType, string> _userTypeMapping = new Dictionary<UserType, string>
	{
		{
			UserType.WindowsUser,
			Resources.UserPageUserTypeWindows
		},
		{
			UserType.BasicUser,
			Resources.UserPageUserTypeBasic
		}
	};

	private string _selectedRole;

	private SolidColorBrush _passwordBorderBrush;

	private SolidColorBrush _passworRepeatBorderBrush;

	private SolidColorBrush _userNameBorderBrush;

	private UserType _selectedUserType;

	private string _errorMessage;

	private string _userName;

	private string _labelHeadline;

	private string _labelInfo;

	private string _userTypeLabel;

	private string _userNameLabel;

	private string _passwordLabel;

	private string _roleLabel;

	private string _repeatPasswordLabel;

	private string _addButton;

	private string _addedUserTypeLabel;

	private string _addedUserNameLabel;

	private string _addedUserRoleLabel;

	public static string ErrorColor => "#FFDE0000";

	public ObservableCollection<string> Roles { get; } = new ObservableCollection<string>();

	public ObservableCollection<IAddedUsersHelper> AddedUsers { get; } = new ObservableCollection<IAddedUsersHelper>();

	public ICommand AddUserCommand => new DelegateCommand(delegate(object o)
	{
		AddUser(o);
	}, (object o) => IsAddButtonEnabled(o));

	public ICommand RemoveUserCommand => new DelegateCommand(delegate(object o)
	{
		RemoveUser(o);
	});

	public ICommand PasswordChangedCommand => new DelegateCommand(delegate
	{
		PasswordChanged();
	});

	public ICommand UserNameChangedCommand => new DelegateCommand(delegate
	{
		UserNameChanged();
	});

	public ICommand OpenObjectPickerCommand => new DelegateCommand(delegate
	{
		OpenObjectPicker();
	});

	public Dictionary<UserType, string> UserTypeMapping => _userTypeMapping;

	public bool PasswordFieldsEnabled => SelectedUserType == UserType.BasicUser;

	public bool UserButtonVisible => SelectedUserType == UserType.WindowsUser;

	public bool IsUserAdded => AddedUsers.Count != 0;

	public static int SelectedUserTypeIndex => 0;

	public ICommand UserTypeSelectionChangedCommand => new DelegateCommand(delegate(object o)
	{
		ClearFields(o);
	});

	public string SelectedRole
	{
		get
		{
			return _selectedRole;
		}
		set
		{
			if (!(_selectedRole == value))
			{
				_selectedRole = value;
				OnPropertyChanged("SelectedRole");
			}
		}
	}

	public SolidColorBrush PasswordBorderBrush
	{
		get
		{
			return _passwordBorderBrush;
		}
		set
		{
			if (_passwordBorderBrush != value)
			{
				_passwordBorderBrush = value;
				OnPropertyChanged("PasswordBorderBrush");
			}
		}
	}

	public SolidColorBrush PasswordRepeatBorderBrush
	{
		get
		{
			return _passworRepeatBorderBrush;
		}
		set
		{
			if (_passworRepeatBorderBrush != value)
			{
				_passworRepeatBorderBrush = value;
				OnPropertyChanged("PasswordRepeatBorderBrush");
			}
		}
	}

	public SolidColorBrush UserNameBorderBrush
	{
		get
		{
			return _userNameBorderBrush;
		}
		set
		{
			if (_userNameBorderBrush != value)
			{
				_userNameBorderBrush = value;
				OnPropertyChanged("UserNameBorderBrush");
			}
		}
	}

	public UserType SelectedUserType
	{
		get
		{
			return _selectedUserType;
		}
		set
		{
			if (_selectedUserType != value)
			{
				_selectedUserType = value;
				OnPropertyChanged("SelectedUserType");
				OnPropertyChanged("PasswordFieldsEnabled");
				OnPropertyChanged("UserButtonVisible");
			}
		}
	}

	public string ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			_errorMessage = value;
			OnPropertyChanged("ErrorMessage");
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

	public string LabelHeadline
	{
		get
		{
			return _labelHeadline;
		}
		set
		{
			_labelHeadline = value;
			OnPropertyChanged("LabelHeadline");
		}
	}

	public string LabelInfo
	{
		get
		{
			return _labelInfo;
		}
		set
		{
			_labelInfo = value;
			OnPropertyChanged("LabelInfo");
		}
	}

	public string UserTypeLabel
	{
		get
		{
			return _userTypeLabel;
		}
		set
		{
			_userTypeLabel = value;
			OnPropertyChanged("UserTypeLabel");
		}
	}

	public string UserNameLabel
	{
		get
		{
			return _userNameLabel;
		}
		set
		{
			_userNameLabel = value;
			OnPropertyChanged("UserNameLabel");
		}
	}

	public string PasswordLabel
	{
		get
		{
			return _passwordLabel;
		}
		set
		{
			_passwordLabel = value;
			OnPropertyChanged("PasswordLabel");
		}
	}

	public string RoleLabel
	{
		get
		{
			return _roleLabel;
		}
		set
		{
			_roleLabel = value;
			OnPropertyChanged("RoleLabel");
		}
	}

	public string RepeatPasswordLabel
	{
		get
		{
			return _repeatPasswordLabel;
		}
		set
		{
			_repeatPasswordLabel = value;
			OnPropertyChanged("RepeatPasswordLabel");
		}
	}

	public string AddButton
	{
		get
		{
			return _addButton;
		}
		set
		{
			_addButton = value;
			OnPropertyChanged("AddButton");
		}
	}

	public string AddedUserTypeLabel
	{
		get
		{
			return _addedUserTypeLabel;
		}
		set
		{
			_addedUserTypeLabel = value;
			OnPropertyChanged("AddedUserTypeLabel");
		}
	}

	public string AddedUserNameLabel
	{
		get
		{
			return _addedUserNameLabel;
		}
		set
		{
			_addedUserNameLabel = value;
			OnPropertyChanged("AddedUserNameLabel");
		}
	}

	public string AddedUserRoleLabel
	{
		get
		{
			return _addedUserRoleLabel;
		}
		set
		{
			_addedUserRoleLabel = value;
			OnPropertyChanged("AddedUserRoleLabel");
		}
	}

	public UserPageViewModel()
	{
	}

	public UserPageViewModel(Identity identity)
	{
		_identity = identity;
	}

	public void LoadResources()
	{
		LabelHeadline = Resources.UserPageHeader;
		LabelInfo = Resources.UserPageHeaderInfo;
		UserTypeLabel = Resources.UserPageUserTypeLabel;
		UserNameLabel = Resources.UserPageUserNameLabel;
		PasswordLabel = Resources.UserPagePasswordLabel;
		RoleLabel = Resources.UserPageRoleLabel;
		RepeatPasswordLabel = Resources.UserPageRepeatPasswordLabel;
		AddButton = Resources.UserPageAddButton;
		AddedUserTypeLabel = Resources.UserPageUserTypeLabel;
		AddedUserNameLabel = Resources.UserPageUserNameLabel;
		AddedUserRoleLabel = Resources.UserPageRoleLabel;
	}

	public void LoadRoles(ICollection<string> roles)
	{
		Roles.Clear();
		AddedUsers.Clear();
		OnPropertyChanged("IsUserAdded");
		if (roles == null || roles.Count == 0)
		{
			ErrorMessage = Resources.UserPageCouldNotLoadRoles;
			return;
		}
		ClearErrorMessage();
		foreach (string role in roles)
		{
			Roles.Add(role);
		}
		SelectedRole = Roles[0];
	}

	private bool ValidateBasicUserName(out string errorMessage)
	{
		errorMessage = null;
		if (UserName.Length > 256)
		{
			errorMessage = Resources.UserPageUserNameTooLong;
			return false;
		}
		char[] anyOf = "?/#[]{}\\".ToCharArray();
		if (UserName.IndexOfAny(anyOf) != -1)
		{
			errorMessage = Resources.UserPageUserInValidCharaters;
			return false;
		}
		return true;
	}

	private void AddUser(object parameter)
	{
		ClearErrorMessage();
		AddedUsersHelper addedUsersHelper = new AddedUsersHelper();
		SecureString password = null;
		addedUsersHelper.RoleName = SelectedRole;
		addedUsersHelper.Password = password;
		addedUsersHelper.UserName = UserName;
		addedUsersHelper.UserTypeDisplayName = UserTypeMapping[SelectedUserType];
		addedUsersHelper.UserType = SelectedUserType;
		if (SelectedUserType == UserType.WindowsUser)
		{
			addedUsersHelper.Identity = _identity;
		}
		if (AddedUsers.Contains(addedUsersHelper))
		{
			UserNameBorderBrush = ErrorColorBrush;
			ErrorMessage = Resources.UserPageUserAlreadyAdded;
			return;
		}
		if (SelectedUserType == UserType.BasicUser)
		{
			if (!ValidateBasicUserName(out var errorMessage))
			{
				UserNameBorderBrush = ErrorColorBrush;
				ErrorMessage = errorMessage;
				return;
			}
			if (!GetSecurePasswords(parameter, out password, out var passwordRepeat))
			{
				return;
			}
			if (!ValidatePassword(UserName, password, passwordRepeat, out var errorMessage2, out var passwordBrush, out var passwordRepeatBrush))
			{
				PasswordBorderBrush = passwordBrush;
				PasswordRepeatBorderBrush = passwordRepeatBrush;
				ErrorMessage = errorMessage2;
				return;
			}
		}
		ClearFields(parameter);
		addedUsersHelper.Password = password;
		AddedUsers.Add(addedUsersHelper);
		OnPropertyChanged("IsUserAdded");
	}

	private void OpenObjectPicker()
	{
		_identity = WindowsUserObjectPicker.AddWindowsUser();
		if (_identity != null)
		{
			UserName = _identity.DomainAccountName;
		}
		else
		{
			UserName = string.Empty;
		}
	}

	private void PasswordChanged()
	{
		ClearErrorMessage();
	}

	private void UserNameChanged()
	{
		ClearErrorMessage();
	}

	private void RemoveUser(object parameter)
	{
		if (parameter is AddedUsersHelper item)
		{
			AddedUsers.Remove(item);
			OnPropertyChanged("IsUserAdded");
		}
	}

	private void ClearFields(object parameter)
	{
		if (parameter is IHavePassword havePassword)
		{
			UserName = string.Empty;
			havePassword.PasswordBox.Clear();
			havePassword.PasswordRepeatBox.Clear();
			ClearErrorMessage();
		}
	}

	private void ClearErrorMessage()
	{
		ErrorMessage = string.Empty;
		PasswordBorderBrush = null;
		PasswordRepeatBorderBrush = null;
		UserNameBorderBrush = null;
	}

	private bool IsAddButtonEnabled(object parameter)
	{
		if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(SelectedRole))
		{
			return false;
		}
		if (SelectedUserType == UserType.BasicUser)
		{
			if (!GetSecurePasswords(parameter, out var password, out var passwordRepeat))
			{
				return false;
			}
			if (password.Length != 0)
			{
				return passwordRepeat.Length != 0;
			}
			return false;
		}
		return true;
	}

	private bool ValidatePassword(string username, SecureString password, SecureString passwordRepeat, out string errorMessage, out SolidColorBrush passwordBrush, out SolidColorBrush passwordRepeatBrush)
	{
		errorMessage = null;
		passwordBrush = null;
		passwordRepeatBrush = null;
		if (password.Length > 256)
		{
			passwordBrush = ErrorColorBrush;
			errorMessage = Resources.UserPagePasswordTooLong;
			return false;
		}
		if (!Password.ValidatePassword(username, password))
		{
			passwordBrush = ErrorColorBrush;
			errorMessage = Resources.UserPagePasswordComplexityNotMet;
			return false;
		}
		if (!password.IsEqualTo(passwordRepeat))
		{
			passwordRepeatBrush = ErrorColorBrush;
			errorMessage = Resources.UserPagePasswordDoNotMatch;
			return false;
		}
		return true;
	}

	private static bool GetSecurePasswords(object parameter, out SecureString password, out SecureString passwordRepeat)
	{
		password = null;
		passwordRepeat = null;
		if (!(parameter is IHavePassword havePassword))
		{
			return false;
		}
		password = havePassword.PasswordBox.SecurePassword;
		passwordRepeat = havePassword.PasswordRepeatBox.SecurePassword;
		return true;
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
		if (!_disposed && AddedUsers != null)
		{
			foreach (IAddedUsersHelper addedUser in AddedUsers)
			{
				addedUser.Dispose();
			}
			AddedUsers.Clear();
		}
		_disposed = true;
	}
}
