using System.Security.Principal;

namespace VideoOS.SqlUpdate.Core.Configuration;

public class WindowsIdentityWrapper : IWindowsIdentityWrapper
{
	public bool IsRunningAsAdministrator()
	{
		return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
	}
}
