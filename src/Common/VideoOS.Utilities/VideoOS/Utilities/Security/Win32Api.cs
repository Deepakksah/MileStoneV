using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace VideoOS.Utilities.Security;

public static class Win32Api
{
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

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct LOCALGROUP_MEMBERS_INFO_2
	{
		public IntPtr lgrmi2_sid;

		public IntPtr lgrmi2_sidusage;

		public IntPtr lgrmi2_domainandname;
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

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct NET_VALIDATE_OUTPUT_ARG
	{
		public NET_VALIDATE_PERSISTED_FIELDS ChangedPersistedFields;

		public NET_API_STATUS ValidationStatus;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct NET_VALIDATE_PASSWORD_HASH
	{
		public uint Length;

		public IntPtr Hash;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct NET_VALIDATE_PASSWORD_CHANGE_INPUT_ARG
	{
		public NET_VALIDATE_PERSISTED_FIELDS InputPersistedFields;

		public IntPtr ClearPassword;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string UserAccountName;

		public NET_VALIDATE_PASSWORD_HASH HashedPassword;

		[MarshalAs(UnmanagedType.I1)]
		public bool PasswordMatched;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct NET_VALIDATE_PERSISTED_FIELDS
	{
		public uint PresentFields;

		public System.Runtime.InteropServices.ComTypes.FILETIME PasswordLastSet;

		public System.Runtime.InteropServices.ComTypes.FILETIME BadPasswordTime;

		public System.Runtime.InteropServices.ComTypes.FILETIME LockoutTime;

		public uint BadPasswordCount;

		public uint PasswordHistoryLength;

		public IntPtr PasswordHistory;
	}

	public enum NET_API_STATUS : uint
	{
		ERROR_ACCESS_DENIED = 5u,
		ERROR_NOT_ENOUGH_MEMORY = 8u,
		ERROR_INVALID_PARAMETER = 87u,
		ERROR_INVALID_NAME = 123u,
		ERROR_INVALID_LEVEL = 124u,
		ERROR_SESSION_CREDENTIAL_CONFLICT = 1219u,
		NERR_Success = 0u,
		NERR_InvalidComputer = 2351u,
		NERR_BadPassword = 2203u,
		NERR_UserNotFound = 2221u,
		NERR_AccountLockedOut = 2702u,
		NERR_PasswordTooRecent = 2246u,
		NERR_PasswordHistConflict = 2244u,
		NERR_PasswordTooShort = 2245u,
		NERR_PasswordTooLong = 2703u,
		NERR_PasswordNotComplexEnough = 2704u,
		NERR_PasswordFilterError = 2705u,
		NERR_PasswordMustChange = 2701u,
		NERR_PasswordExpired = 2242u
	}

	public enum NET_VALIDATE_PASSWORD_TYPE
	{
		NetValidateAuthentication = 1,
		NetValidatePasswordChange,
		NetValidatePasswordReset
	}

	[DllImport("kernel32", SetLastError = true)]
	public static extern bool GetVersionEx(ref OSVERSIONINFOEX osvi);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool LookupAccountName(string lpSystemName, string lpAccountName, [MarshalAs(UnmanagedType.LPArray)] byte[] Sid, ref uint cbSid, StringBuilder ReferencedDomainName, ref uint cchReferencedDomainName, out SID_NAME_USE peUse);

	[DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
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

	[DllImport("netapi32.dll")]
	public static extern int NetApiBufferFree(IntPtr bufptr);

	[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int NetGetJoinInformation([In][MarshalAs(UnmanagedType.LPWStr)] string server, out IntPtr domain, out NetJoinStatus status);

	[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern NET_API_STATUS NetValidatePasswordPolicy([MarshalAs(UnmanagedType.LPWStr)] string ServerName, IntPtr Qualifier, NET_VALIDATE_PASSWORD_TYPE ValidationType, IntPtr InputArg, ref IntPtr OutputArg);

	[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern NET_API_STATUS NetValidatePasswordPolicyFree(ref IntPtr OutputArg);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "LogonUserW", ExactSpelling = true, SetLastError = true)]
	public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
}
