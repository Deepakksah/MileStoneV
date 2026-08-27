namespace VideoOS.SqlUpdate.Core.Configuration;

public interface IWindowsIdentityWrapper
{
	bool IsRunningAsAdministrator();
}
