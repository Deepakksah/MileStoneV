using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace VideoOS.Installer.Common.ServiceAccountHelper;

public static class NativeMethods
{
	[Flags]
	public enum SERVICE_ACCESS : uint
	{
		STANDARD_RIGHTS_REQUIRED = 0xF0000u,
		SERVICE_QUERY_CONFIG = 1u,
		SERVICE_CHANGE_CONFIG = 2u,
		SERVICE_QUERY_STATUS = 4u,
		SERVICE_ENUMERATE_DEPENDENTS = 8u,
		SERVICE_START = 0x10u,
		SERVICE_STOP = 0x20u,
		SERVICE_PAUSE_CONTINUE = 0x40u,
		SERVICE_INTERROGATE = 0x80u,
		SERVICE_USER_DEFINED_CONTROL = 0x100u,
		SERVICE_ALL_ACCESS = 0xF01FFu
	}

	public enum SID_NAME_USE
	{
		SidTypeUser = 1,
		SidTypeGroup,
		SidTypeDomain,
		SidTypeAlias,
		SidTypeWellKnownGroup,
		SidTypeDeletedAccount,
		SidTypeInvalid,
		SidTypeUnknown,
		SidTypeComputer
	}

	public enum LSA_AccessPolicy : long
	{
		POLICY_VIEW_LOCAL_INFORMATION = 1L,
		POLICY_VIEW_AUDIT_INFORMATION = 2L,
		POLICY_GET_PRIVATE_INFORMATION = 4L,
		POLICY_TRUST_ADMIN = 8L,
		POLICY_CREATE_ACCOUNT = 0x10L,
		POLICY_CREATE_SECRET = 0x20L,
		POLICY_CREATE_PRIVILEGE = 0x40L,
		POLICY_SET_DEFAULT_QUOTA_LIMITS = 0x80L,
		POLICY_SET_AUDIT_REQUIREMENTS = 0x100L,
		POLICY_AUDIT_LOG_ADMIN = 0x200L,
		POLICY_SERVER_ADMIN = 0x400L,
		POLICY_LOOKUP_NAMES = 0x800L,
		POLICY_NOTIFICATION = 0x1000L
	}

	public enum NetJoinStatus
	{
		NetSetupUnknownStatus,
		NetSetupUnjoined,
		NetSetupWorkgroupName,
		NetSetupDomainName
	}

	public enum LogonTypes
	{
		LOGON32_PROVIDER_DEFAULT = 0,
		LOGON32_LOGON_INTERACTIVE = 2,
		LOGON32_LOGON_NETWORK = 3,
		LOGON32_LOGON_BATCH = 4,
		LOGON32_LOGON_SERVICE = 5,
		LOGON32_LOGON_UNLOCK = 7,
		LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
		LOGON32_LOGON_NEW_CREDENTIALS = 9
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct LOCALGROUP_MEMBERS_INFO_2
	{
		public IntPtr lgrmi2_sid;

		public IntPtr lgrmi2_sidusage;

		public IntPtr lgrmi2_domainandname;
	}

	public struct OSVERSIONINFOEX
	{
		public enum ProductType : byte
		{
			VER_NT_WORKSTATION = 1,
			VER_NT_DOMAIN_CONTROLLER,
			VER_NT_SERVER
		}

		public uint dwOSVersionInfoSize;

		public uint dwMajorVersion;

		public uint dwMinorVersion;

		public uint dwBuildNumber;

		public uint dwPlatformId;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string szCSDVersion;

		public ushort wServicePackMajor;

		public ushort wServicePackMinor;

		public ushort wSuiteMask;

		public ProductType wProductType;

		public byte wReserved;
	}

	public struct SECURITY_DESCRIPTOR
	{
		public byte revision;

		public byte size;

		public short control;

		public IntPtr owner;

		public IntPtr group;

		public IntPtr sacl;

		public IntPtr dacl;
	}

	public static class Win32ErrorCodes
	{
		public const int NO_ERROR = 0;

		public const int ERROR_FILE_NOT_FOUND = 2;

		public const int ERROR_ACCESS_DENIED = 5;

		public const int ERROR_INSUFFICIENT_BUFFER = 122;

		public const int ERROR_NONE_MAPPED = 1332;
	}

	public struct LSA_OBJECT_ATTRIBUTES
	{
		public int Length;

		public IntPtr RootDirectory;

		public LSA_UNICODE_STRING ObjectName;

		public uint Attributes;

		public IntPtr SecurityDescriptor;

		public IntPtr SecurityQualityOfService;
	}

	public struct LSA_UNICODE_STRING
	{
		public ushort Length;

		public ushort MaximumLength;

		public IntPtr Buffer;

		public void SetTo(string str)
		{
			Buffer = Marshal.StringToHGlobalUni(str);
			Length = (ushort)((str.Length + 1) * 2);
			MaximumLength = Length;
		}

		public override string ToString()
		{
			return Marshal.PtrToStringUni(Buffer);
		}

		public void Clean()
		{
			if (Buffer != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(Buffer);
			}
			Buffer = IntPtr.Zero;
			Length = 0;
			MaximumLength = 0;
		}
	}

	public const int HttpServiceConfigUrlAclInfo = 2;

	public const int HTTP_INITIALIZE_CONFIG = 2;

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool QueryServiceObjectSecurity(SafeHandle serviceHandle, SecurityInfos secInfo, byte[] lpSecDesrBuf, uint bufSize, out uint bufSizeNeeded);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetServiceObjectSecurity(SafeHandle serviceHandle, SecurityInfos secInfos, byte[] lpSecDesrBuf);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool LookupAccountName(string lpSystemName, string lpAccountName, [MarshalAs(UnmanagedType.LPArray)] byte[] Sid, ref uint cbSid, StringBuilder ReferencedDomainName, ref uint cchReferencedDomainName, out SID_NAME_USE peUse);

	[DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool ConvertSidToStringSid([MarshalAs(UnmanagedType.LPArray)] byte[] pSID, out IntPtr ptrSid);

	[DllImport("advapi32.dll", SetLastError = true)]
	public static extern uint LsaOpenPolicy(ref LSA_UNICODE_STRING SystemName, ref LSA_OBJECT_ATTRIBUTES ObjectAttributes, uint DesiredAccess, out IntPtr PolicyHandle);

	[DllImport("advapi32.dll", SetLastError = true)]
	public static extern uint LsaAddAccountRights(IntPtr PolicyHandle, [MarshalAs(UnmanagedType.LPArray)] byte[] AccountSid, LSA_UNICODE_STRING[] UserRights, uint CountOfRights);

	[DllImport("advapi32.dll", SetLastError = true)]
	public static extern uint LsaEnumerateAccountRights(IntPtr PolicyHandle, [MarshalAs(UnmanagedType.LPArray)] byte[] AccountSid, out IntPtr UserRights, out uint CountOfRights);

	[DllImport("netapi32.dll")]
	public static extern uint NetLocalGroupGetMembers(IntPtr ServerName, [MarshalAs(UnmanagedType.LPWStr)] string GrouprName, uint level, ref IntPtr siPtr, uint prefmaxlen, ref uint entriesread, ref uint totalentries, IntPtr resumeHandle);

	[DllImport("advapi32.dll")]
	public static extern uint LsaNtStatusToWinError(uint status);

	[DllImport("advapi32.dll")]
	public static extern uint LsaClose(IntPtr ObjectHandle);

	[DllImport("kernel32.dll")]
	public static extern IntPtr LocalFree(IntPtr hMem);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "LogonUserW", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

	[DllImport("netapi32.dll")]
	public static extern uint NetApiBufferFree(IntPtr bufptr);
}
