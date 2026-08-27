using System.Diagnostics;
using System.Security.Principal;

namespace VideoOS.Mobile.Helpers;

public static class SystemImpersonsation
{
	private static bool impersonationEnabled;

	public const uint STANDARD_RIGHTS_REQUIRED = 983040u;

	public const uint STANDARD_RIGHTS_READ = 131072u;

	public const uint TOKEN_ASSIGN_PRIMARY = 1u;

	public const uint TOKEN_DUPLICATE = 2u;

	public const uint TOKEN_IMPERSONATE = 4u;

	public const uint TOKEN_QUERY = 8u;

	public const uint TOKEN_QUERY_SOURCE = 16u;

	public const uint TOKEN_ADJUST_PRIVILEGES = 32u;

	public const uint TOKEN_ADJUST_GROUPS = 64u;

	public const uint TOKEN_ADJUST_DEFAULT = 128u;

	public const uint TOKEN_ADJUST_SESSIONID = 256u;

	public const uint TOKEN_READ = 131080u;

	public const uint TOKEN_ALL_ACCESS = 983551u;

	public static void ImpersonateSystem()
	{
		Process[] processes = Process.GetProcesses();
		foreach (Process process in processes)
		{
			if (process.ProcessName == "winlogon")
			{
				if (!NativeMethods.OpenProcessToken(process.Handle, 131086u, out var TokenHandle))
				{
					break;
				}
				new WindowsIdentity(TokenHandle).Impersonate();
				NativeMethods.CloseHandle(TokenHandle);
				impersonationEnabled = true;
			}
		}
	}

	public static void StopImpersonation()
	{
		if (impersonationEnabled)
		{
			NativeMethods.RevertToSelf();
			impersonationEnabled = false;
		}
	}
}
