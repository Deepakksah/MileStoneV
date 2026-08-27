using System;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Reflection;
using System.Security.Principal;

namespace VideoOS.TrayControllerControls;

public class ProcessHelper
{
	public static bool IsProcessRunningByCurrentUser(Assembly mainAssembly)
	{
		string text = mainAssembly.GetName().Name + ".exe";
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(FormattableString.Invariant($"Select * from Win32_Process Where Name = \"{text}\" AND ProcessId != \"{Process.GetCurrentProcess().Id}\"")))
		{
			using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
			foreach (ManagementObject item in managementObjectCollection)
			{
				string[] array = new string[1] { string.Empty };
				object[] args = array;
				if (Convert.ToInt32(item.InvokeMethod("GetOwnerSid", args), CultureInfo.InvariantCulture) == 0)
				{
					SecurityIdentifier securityIdentifier = new SecurityIdentifier(array[0]);
					if (WindowsIdentity.GetCurrent().User == securityIdentifier)
					{
						return true;
					}
					return false;
				}
			}
		}
		return false;
	}
}
