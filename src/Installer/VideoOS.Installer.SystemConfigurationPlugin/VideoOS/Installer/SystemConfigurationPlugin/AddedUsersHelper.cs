using System;
using System.Globalization;
using System.Security;
using VideoOS.Management.Common;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class AddedUsersHelper : IAddedUsersHelper, IDisposable
{
	private bool _disposed;

	public string UserName { get; set; }

	public string RoleName { get; set; }

	public string UserTypeDisplayName { get; set; }

	public SecureString Password { get; set; }

	public Identity Identity { get; set; }

	public UserPageViewModel.UserType UserType { get; set; }

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (!_disposed && Password != null)
			{
				Password.Dispose();
				Password = null;
			}
			_disposed = true;
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is AddedUsersHelper addedUsersHelper))
		{
			return false;
		}
		if (UserName.ToUpper(CultureInfo.CurrentCulture).Equals(addedUsersHelper.UserName.ToUpper(CultureInfo.CurrentCulture)))
		{
			return UserType.Equals(addedUsersHelper.UserType);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return UserName.ToUpper(CultureInfo.CurrentCulture).GetHashCode() ^ UserType.GetHashCode();
	}
}
