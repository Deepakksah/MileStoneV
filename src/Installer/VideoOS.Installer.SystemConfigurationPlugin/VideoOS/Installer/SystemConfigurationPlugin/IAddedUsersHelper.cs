using System;
using System.Security;
using VideoOS.Management.Common;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface IAddedUsersHelper : IDisposable
{
	Identity Identity { get; set; }

	SecureString Password { get; set; }

	string RoleName { get; set; }

	string UserName { get; set; }

	UserPageViewModel.UserType UserType { get; set; }

	string UserTypeDisplayName { get; set; }

	new bool Equals(object addedUsersHelper);

	new int GetHashCode();
}
