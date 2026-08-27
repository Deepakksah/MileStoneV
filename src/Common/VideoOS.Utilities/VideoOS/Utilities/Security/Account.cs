using System;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace VideoOS.Utilities.Security;

[CLSCompliant(true)]
public static class Account
{
	public static bool HasAccountAccountRight(string systemName, string accountName, string accountRight)
	{
		IntPtr policyHandle = new IntPtr(0);
		bool flag = true;
		try
		{
			LookupAccountName(systemName, accountName, out var sid, out var _);
			OpenPolicy(systemName, out policyHandle);
			return HasAccountRight(ref policyHandle, ref sid, accountRight);
		}
		catch (Win32Exception)
		{
			flag = false;
			throw;
		}
		finally
		{
			if (policyHandle != IntPtr.Zero)
			{
				Win32Api.LsaClose(policyHandle);
			}
		}
	}

	public static bool AddAccountRightToAccount(string systemName, string accountName, string accountRight, out string domainName)
	{
		IntPtr policyHandle = new IntPtr(0);
		bool result = true;
		domainName = string.Empty;
		try
		{
			LookupAccountName(systemName, accountName, out var sid, out domainName);
			OpenPolicy(systemName, out policyHandle);
			AddAccountRight(ref policyHandle, ref sid, accountRight);
			return result;
		}
		catch (Win32Exception)
		{
			result = false;
			throw;
		}
		finally
		{
			if (policyHandle != IntPtr.Zero)
			{
				Win32Api.LsaClose(policyHandle);
			}
		}
	}

	public static string LookupSid(string systemName, string accountName)
	{
		string domainName = string.Empty;
		LookupAccountName(systemName, accountName, out var sid, out domainName);
		if (!Win32Api.ConvertSidToStringSid(sid, out var ptrSid))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to convert SID to a string.");
		}
		string result = Marshal.PtrToStringAuto(ptrSid);
		Win32Api.LocalFree(ptrSid);
		return result;
	}

	public static string LookupAccountName(string systemName, string accountName)
	{
		string domainName = string.Empty;
		LookupAccountName(systemName, accountName, out var _, out domainName);
		if (!accountName.Contains("\\") && !string.IsNullOrEmpty(domainName))
		{
			return domainName + "\\" + accountName;
		}
		return accountName;
	}

	private static void LookupAccountName(string systemName, string accountName, out byte[] sid, out string domainName)
	{
		sid = null;
		uint cbSid = 0u;
		StringBuilder stringBuilder = new StringBuilder();
		uint cchReferencedDomainName = (uint)stringBuilder.Capacity;
		domainName = string.Empty;
		int num = 0;
		if (!Win32Api.LookupAccountName(systemName, accountName, sid, ref cbSid, stringBuilder, ref cchReferencedDomainName, out var peUse))
		{
			num = Marshal.GetLastWin32Error();
			if (num != 122)
			{
				throw new Win32Exception(num);
			}
			sid = new byte[cbSid];
			stringBuilder.EnsureCapacity((int)cchReferencedDomainName);
			if (!Win32Api.LookupAccountName(systemName, accountName, sid, ref cbSid, stringBuilder, ref cchReferencedDomainName, out peUse))
			{
				num = Marshal.GetLastWin32Error();
				throw new Win32Exception(num, "LookupAccountName() failed");
			}
			domainName = stringBuilder.ToString();
		}
	}

	private static void OpenPolicy(string systemName, out IntPtr policyHandle)
	{
		Win32Api.LSA_UNICODE_STRING SystemName = default(Win32Api.LSA_UNICODE_STRING);
		SystemName.SetTo(systemName);
		uint desiredAccess = 8191u;
		policyHandle = IntPtr.Zero;
		Win32Api.LSA_OBJECT_ATTRIBUTES ObjectAttributes = new Win32Api.LSA_OBJECT_ATTRIBUTES
		{
			Length = 0,
			RootDirectory = IntPtr.Zero,
			Attributes = 0u,
			SecurityDescriptor = IntPtr.Zero,
			SecurityQualityOfService = IntPtr.Zero
		};
		int num = (int)Win32Api.LsaNtStatusToWinError(Win32Api.LsaOpenPolicy(ref SystemName, ref ObjectAttributes, desiredAccess, out policyHandle));
		SystemName.Clean();
		if (num != 0)
		{
			throw new Win32Exception(num, "LsaOpenPolicy() failed");
		}
	}

	private static bool HasAccountRight(ref IntPtr policyHandle, ref byte[] sid, string privilegeName)
	{
		uint CountOfRights = 0u;
		IntPtr UserRights;
		int num = (int)Win32Api.LsaNtStatusToWinError(Win32Api.LsaEnumerateAccountRights(policyHandle, sid, out UserRights, out CountOfRights));
		switch (num)
		{
		case 2:
			return false;
		default:
			throw new Win32Exception(num, "LsaEnumerateAccountRights() failed");
		case 0:
		{
			_ = string.Empty;
			for (int i = 0; i < CountOfRights; i++)
			{
				if (((Win32Api.LSA_UNICODE_STRING)Marshal.PtrToStructure((IntPtr)(UserRights.ToInt64() + i * Marshal.SizeOf(typeof(Win32Api.LSA_UNICODE_STRING))), typeof(Win32Api.LSA_UNICODE_STRING))/*cast due to constrained. prefix*/).ToString() == privilegeName)
				{
					return true;
				}
			}
			return false;
		}
		}
	}

	private static void AddAccountRight(ref IntPtr policyHandle, ref byte[] sid, string privilegeName)
	{
		Win32Api.LSA_UNICODE_STRING[] array = new Win32Api.LSA_UNICODE_STRING[1] { default(Win32Api.LSA_UNICODE_STRING) };
		array[0].Buffer = Marshal.StringToHGlobalUni(privilegeName);
		array[0].Length = (ushort)(privilegeName.Length * 2);
		array[0].MaximumLength = (ushort)((privilegeName.Length + 1) * 2);
		int num = (int)Win32Api.LsaNtStatusToWinError(Win32Api.LsaAddAccountRights(policyHandle, sid, array, 1u));
		array[0].Clean();
		if (num != 0)
		{
			throw new Win32Exception(num, "LsaAddAccountRights() failed");
		}
	}

	public static bool IsUserAdministrator(string userName)
	{
		bool result = false;
		IntPtr siPtr = IntPtr.Zero;
		uint prefmaxlen = uint.MaxValue;
		uint entriesread = 0u;
		uint totalentries = 0u;
		Win32Api.NetLocalGroupGetMembers(IntPtr.Zero, "administrators", 2u, ref siPtr, prefmaxlen, ref entriesread, ref totalentries, IntPtr.Zero);
		for (int i = 0; i < totalentries; i++)
		{
			if (Marshal.PtrToStringAuto(((Win32Api.LOCALGROUP_MEMBERS_INFO_2)Marshal.PtrToStructure(new IntPtr(siPtr.ToInt64() + Marshal.SizeOf(typeof(Win32Api.LOCALGROUP_MEMBERS_INFO_2)) * i), typeof(Win32Api.LOCALGROUP_MEMBERS_INFO_2))).lgrmi2_domainandname).Equals(userName, StringComparison.OrdinalIgnoreCase))
			{
				result = true;
				break;
			}
		}
		Win32Api.NetApiBufferFree(siPtr);
		return result;
	}

	public static bool IsCurrentUserAdministrator()
	{
		using WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
		return IsUserAdministrator(windowsIdentity.Name);
	}

	public static bool IsServiceAccountValid(string userName, string domain, string password)
	{
		IntPtr phToken = IntPtr.Zero;
		return Win32Api.LogonUser(userName, domain, password, 5, 0, ref phToken);
	}

	public static bool IsAccountValid(string userName, string domain, string password)
	{
		IntPtr phToken = IntPtr.Zero;
		return Win32Api.LogonUser(userName, domain, password, 2, 0, ref phToken);
	}

	public static string GetServiceLogOnUser(string serviceName)
	{
		using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(new SelectQuery("SELECT StartName FROM Win32_Service WHERE Name='" + serviceName + "'"));
		try
		{
			ManagementObject managementObject = managementObjectSearcher.Get().Cast<ManagementObject>().FirstOrDefault();
			if (managementObject != null)
			{
				string text = managementObject["StartName"].ToString();
				if (text.StartsWith(".", StringComparison.Ordinal))
				{
					text = Environment.MachineName + text.Substring(1);
				}
				if (text == "LocalSystem")
				{
					text = "NT AUTHORITY\\SYSTEM";
				}
				return text;
			}
		}
		catch (Exception innerException)
		{
			throw new ManagementException("Failed to read service user from WMI.", innerException);
		}
		return null;
	}

	public static string GetProcessOwner(int processId)
	{
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ProcessID = " + processId))
		{
			try
			{
				foreach (ManagementObject item in managementObjectSearcher.Get().Cast<ManagementObject>())
				{
					object[] array = new object[2]
					{
						string.Empty,
						string.Empty
					};
					if (Convert.ToInt32(item.InvokeMethod("GetOwner", array)) == 0)
					{
						return array[1]?.ToString() + "\\" + array[0];
					}
				}
			}
			catch (Exception innerException)
			{
				throw new ManagementException("Failed to read service user from WMI.", innerException);
			}
		}
		return null;
	}
}
