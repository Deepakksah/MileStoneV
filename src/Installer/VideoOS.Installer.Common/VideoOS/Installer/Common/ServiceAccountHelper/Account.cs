using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

public static class Account
{
	public const string SEInteractiveLogOnName = "SeInteractiveLogonRight";

	public const string SEServiceLogOnName = "SeServiceLogonRight";

	public static ServiceAccountErrorString ServiceAccountPasswordValidation(string acountName, string accountPass)
	{
		ServiceAccountErrorString result = ServiceAccountErrorString.Empty;
		try
		{
			string empty = string.Empty;
			empty = LookupAccountName(string.Empty, acountName);
			if (!acountName.Contains("\\"))
			{
				acountName = empty;
			}
			string[] array = empty.Split(new string[1] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
			bool flag = false;
			if (!((array.Length <= 1) ? IsAccountValid(empty, string.Empty, accountPass) : IsAccountValid(array[1], array[0], accountPass)))
			{
				return ServiceAccountErrorString.PasswordInvalid;
			}
			return result;
		}
		catch (Win32Exception ex)
		{
			result = ServiceAccountErrorString.AddRightGrantedFailed;
			switch (ex.NativeErrorCode)
			{
			case 1332:
				result = ServiceAccountErrorString.NameNotFound;
				break;
			case 5:
				result = ServiceAccountErrorString.AccessDenied;
				break;
			}
			return result;
		}
	}

	public static ServiceAccountErrorString SetServiceAccountRights(string accountRight, string accountName, string accountPass)
	{
		ServiceAccountErrorString result = ServiceAccountErrorString.Empty;
		string domainName = string.Empty;
		string empty = string.Empty;
		empty = LookupAccountName(string.Empty, accountName);
		if (!string.IsNullOrEmpty(accountName) && !accountName.Contains("\\"))
		{
			accountName = empty;
		}
		try
		{
			if (!(accountRight == "SeInteractiveLogonRight"))
			{
				if (accountRight == "SeServiceLogonRight")
				{
					AddAccountRightToAccount(string.Empty, accountName, "SeServiceLogonRight", out domainName);
				}
			}
			else
			{
				AddAccountRightToAccount(string.Empty, accountName, "SeInteractiveLogonRight", out domainName);
				result = TestLogonRight(empty, accountPass);
			}
		}
		catch (Win32Exception ex)
		{
			result = ServiceAccountErrorString.AddRightGrantedFailed;
			switch (ex.NativeErrorCode)
			{
			case 1332:
				result = ServiceAccountErrorString.NameNotFound;
				break;
			case 5:
				result = ServiceAccountErrorString.AccessDenied;
				break;
			}
			return result;
		}
		return result;
	}

	public static ServiceAccountErrorString SetServicePermissions(string service, string userName)
	{
		ServiceAccountErrorString result = ServiceAccountErrorString.Empty;
		try
		{
			using ServiceController serviceController = new ServiceController(service);
			byte[] array = new byte[0];
			bool flag = NativeMethods.QueryServiceObjectSecurity(serviceController.ServiceHandle, SecurityInfos.DiscretionaryAcl, array, 0u, out var bufSizeNeeded);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (!flag && (lastWin32Error == 0 || lastWin32Error == 122))
			{
				array = new byte[bufSizeNeeded];
				flag = NativeMethods.QueryServiceObjectSecurity(serviceController.ServiceHandle, SecurityInfos.DiscretionaryAcl, array, bufSizeNeeded, out bufSizeNeeded);
			}
			if (!flag)
			{
				throw CreateWin32Exception(lastWin32Error, "Failed to GET service permissions");
			}
			RawSecurityDescriptor rawSecurityDescriptor = new RawSecurityDescriptor(array, 0);
			DiscretionaryAcl discretionaryAcl = new DiscretionaryAcl(isContainer: false, isDS: false, rawSecurityDescriptor.DiscretionaryAcl);
			string empty = string.Empty;
			empty = LookupAccountName(string.Empty, userName);
			if (!string.IsNullOrEmpty(userName) && !userName.Contains("\\"))
			{
				userName = empty;
			}
			SecurityIdentifier sid = (SecurityIdentifier)new NTAccount(userName).Translate(typeof(SecurityIdentifier));
			discretionaryAcl.AddAccess(AccessControlType.Allow, sid, 16, InheritanceFlags.None, PropagationFlags.None);
			discretionaryAcl.AddAccess(AccessControlType.Allow, sid, 32, InheritanceFlags.None, PropagationFlags.None);
			discretionaryAcl.AddAccess(AccessControlType.Allow, sid, 4, InheritanceFlags.None, PropagationFlags.None);
			byte[] binaryForm = new byte[discretionaryAcl.BinaryLength];
			discretionaryAcl.GetBinaryForm(binaryForm, 0);
			rawSecurityDescriptor.DiscretionaryAcl = new RawAcl(binaryForm, 0);
			byte[] array2 = new byte[rawSecurityDescriptor.BinaryLength];
			rawSecurityDescriptor.GetBinaryForm(array2, 0);
			flag = NativeMethods.SetServiceObjectSecurity(serviceController.ServiceHandle, SecurityInfos.DiscretionaryAcl, array2);
			lastWin32Error = Marshal.GetLastWin32Error();
			if (!flag)
			{
				throw CreateWin32Exception(lastWin32Error, "SetServiceObjectSecurity() failed");
			}
			return result;
		}
		catch (Win32Exception ex)
		{
			result = ServiceAccountErrorString.AddRightGrantedFailed;
			switch (ex.NativeErrorCode)
			{
			case 1332:
				result = ServiceAccountErrorString.NameNotFound;
				break;
			case 5:
				result = ServiceAccountErrorString.AccessDenied;
				break;
			}
			return result;
		}
	}

	public static bool IsAccountValid(string username, string domain, string password)
	{
		IntPtr phToken = IntPtr.Zero;
		return NativeMethods.LogonUser(username, domain, password, 2, 0, ref phToken);
	}

	public static string LookupAccountName(string systemName, string accountName)
	{
		string domainName = string.Empty;
		LookupAccountName(systemName, accountName, out var _, out domainName);
		if (!accountName.Contains("\\") && domainName != string.Empty)
		{
			return domainName + "\\" + accountName;
		}
		return accountName;
	}

	public static void LookupAccountName(string systemName, string accountName, out byte[] sid, out string domainName)
	{
		sid = null;
		uint cbSid = 0u;
		StringBuilder stringBuilder = new StringBuilder();
		uint cchReferencedDomainName = (uint)stringBuilder.Capacity;
		domainName = string.Empty;
		int num = 0;
		if (!NativeMethods.LookupAccountName(systemName, accountName, sid, ref cbSid, stringBuilder, ref cchReferencedDomainName, out var peUse))
		{
			num = Marshal.GetLastWin32Error();
			if (num != 122)
			{
				throw new Win32Exception(num);
			}
			sid = new byte[cbSid];
			stringBuilder.EnsureCapacity((int)cchReferencedDomainName);
			if (!NativeMethods.LookupAccountName(systemName, accountName, sid, ref cbSid, stringBuilder, ref cchReferencedDomainName, out peUse))
			{
				num = Marshal.GetLastWin32Error();
				throw CreateWin32Exception(num, "LookupAccountName() failed");
			}
			domainName = stringBuilder.ToString();
		}
	}

	public static bool HasAccountAccountRight(string systemName, string accountName, string accountRight)
	{
		IntPtr policyHandle = IntPtr.Zero;
		bool flag = true;
		try
		{
			LookupAccountName(systemName, accountName, out var sid, out var _);
			OpenPolicy(systemName, out policyHandle);
			return HasAccountRight(ref policyHandle, ref sid, accountRight);
		}
		catch (Win32Exception ex)
		{
			flag = false;
			throw CreateWin32Exception(ex.NativeErrorCode, "HasAccountAccountRight() failed");
		}
		finally
		{
			if (policyHandle != IntPtr.Zero)
			{
				NativeMethods.LsaClose(policyHandle);
			}
		}
	}

	public static bool AddAccountRightToAccount(string systemName, string accountName, string accountRight, out string domainName)
	{
		IntPtr policyHandle = IntPtr.Zero;
		bool result = true;
		domainName = string.Empty;
		try
		{
			LookupAccountName(systemName, accountName, out var sid, out domainName);
			OpenPolicy(systemName, out policyHandle);
			AddAccountRight(ref policyHandle, ref sid, accountRight);
			return result;
		}
		catch (Win32Exception ex)
		{
			result = false;
			throw CreateWin32Exception(ex.NativeErrorCode, "AddAccountRightToAccount() failed");
		}
		finally
		{
			if (policyHandle != IntPtr.Zero)
			{
				NativeMethods.LsaClose(policyHandle);
			}
		}
	}

	public static string LookupSid(string systemName, string accountName)
	{
		string domainName = string.Empty;
		LookupAccountName(systemName, accountName, out var sid, out domainName);
		if (!NativeMethods.ConvertSidToStringSid(sid, out var ptrSid))
		{
			throw CreateWin32Exception(Marshal.GetLastWin32Error(), "Unable to convert SID to a string.");
		}
		string result = Marshal.PtrToStringAuto(ptrSid);
		NativeMethods.LocalFree(ptrSid);
		return result;
	}

	private static void OpenPolicy(string systemName, out IntPtr policyHandle)
	{
		NativeMethods.LSA_UNICODE_STRING SystemName = default(NativeMethods.LSA_UNICODE_STRING);
		SystemName.SetTo(systemName);
		uint desiredAccess = 8191u;
		policyHandle = IntPtr.Zero;
		NativeMethods.LSA_OBJECT_ATTRIBUTES ObjectAttributes = new NativeMethods.LSA_OBJECT_ATTRIBUTES
		{
			Length = 0,
			RootDirectory = IntPtr.Zero,
			Attributes = 0u,
			SecurityDescriptor = IntPtr.Zero,
			SecurityQualityOfService = IntPtr.Zero
		};
		int num = (int)NativeMethods.LsaNtStatusToWinError(NativeMethods.LsaOpenPolicy(ref SystemName, ref ObjectAttributes, desiredAccess, out policyHandle));
		SystemName.Clean();
		if (num != 0)
		{
			throw CreateWin32Exception(num, "LsaOpenPolicy() failed");
		}
	}

	private static bool HasAccountRight(ref IntPtr policyHandle, ref byte[] sid, string privilegeName)
	{
		uint CountOfRights = 0u;
		IntPtr UserRights;
		switch ((int)NativeMethods.LsaNtStatusToWinError(NativeMethods.LsaEnumerateAccountRights(policyHandle, sid, out UserRights, out CountOfRights)))
		{
		case 2:
			return false;
		default:
			throw CreateWin32Exception("LsaEnumerateAccountRights() failed");
		case 0:
		{
			_ = string.Empty;
			for (int i = 0; i < CountOfRights; i++)
			{
				if (((NativeMethods.LSA_UNICODE_STRING)Marshal.PtrToStructure(UserRights + Marshal.SizeOf(typeof(NativeMethods.LSA_UNICODE_STRING)) * i, typeof(NativeMethods.LSA_UNICODE_STRING))/*cast due to constrained. prefix*/).ToString() == privilegeName)
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
		NativeMethods.LSA_UNICODE_STRING[] array = new NativeMethods.LSA_UNICODE_STRING[1] { default(NativeMethods.LSA_UNICODE_STRING) };
		array[0].Buffer = Marshal.StringToHGlobalUni(privilegeName);
		array[0].Length = (ushort)(privilegeName.Length * 2);
		array[0].MaximumLength = (ushort)((privilegeName.Length + 1) * 2);
		int num = (int)NativeMethods.LsaNtStatusToWinError(NativeMethods.LsaAddAccountRights(policyHandle, sid, array, 1u));
		array[0].Clean();
		if (num != 0)
		{
			throw CreateWin32Exception(num, "LsaAddAccountRights() failed");
		}
	}

	public static bool IsUserAdministrator(string userName)
	{
		bool result = false;
		IntPtr siPtr = IntPtr.Zero;
		uint prefmaxlen = uint.MaxValue;
		uint entriesread = 0u;
		uint totalentries = 0u;
		uint num = NativeMethods.NetLocalGroupGetMembers(IntPtr.Zero, "administrators", 2u, ref siPtr, prefmaxlen, ref entriesread, ref totalentries, IntPtr.Zero);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "NetLocalGroupGetMembers() result: {0}", num));
		for (int i = 0; i < totalentries; i++)
		{
			if (Marshal.PtrToStringAuto(((NativeMethods.LOCALGROUP_MEMBERS_INFO_2)Marshal.PtrToStructure(siPtr + Marshal.SizeOf(typeof(NativeMethods.LOCALGROUP_MEMBERS_INFO_2)) * i, typeof(NativeMethods.LOCALGROUP_MEMBERS_INFO_2))).lgrmi2_domainandname).Equals(userName, StringComparison.OrdinalIgnoreCase))
			{
				result = true;
				break;
			}
		}
		NativeMethods.NetApiBufferFree(siPtr);
		return result;
	}

	public static bool IsServiceAccountValid(string userName, string domain, string password)
	{
		IntPtr phToken = IntPtr.Zero;
		return NativeMethods.LogonUser(userName, domain, password, 5, 0, ref phToken);
	}

	private static Win32Exception CreateWin32Exception([Localizable(false)] string message)
	{
		return new Win32Exception(message);
	}

	private static Win32Exception CreateWin32Exception(int win32Error, [Localizable(false)] string message)
	{
		return new Win32Exception(win32Error, message);
	}

	private static ServiceAccountErrorString TestLogonRight(string domainUserName, string accountPass)
	{
		ServiceAccountErrorString result = ServiceAccountErrorString.Empty;
		string[] array = domainUserName.Split(new string[1] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
		bool flag = false;
		if (!((array == null || array.Length <= 1) ? IsAccountValid(domainUserName, string.Empty, accountPass) : IsAccountValid(array[1], array[0], accountPass)))
		{
			result = ServiceAccountErrorString.PasswordInvalid;
		}
		return result;
	}
}
