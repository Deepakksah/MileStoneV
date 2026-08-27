using System;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.Installer.SQLExpressAdapter;

public static class NativeMethods
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

	public const short SQL_HANDLE_ENV = 1;

	public const short SQL_HANDLE_DBC = 2;

	public const int SQL_ATTR_ODBC_VERSION = 200;

	public const int SQL_OV_ODBC3 = 3;

	public const short SQL_SUCCESS = 0;

	public const short SQL_NEED_DATA = 99;

	public const short DEFAULT_RESULT_SIZE = 1024;

	public const string SQL_DRIVER_STR = "DRIVER=SQL SERVER";

	public const int NO_ERROR = 0;

	public const int ERROR_INSUFFICIENT_BUFFER = 122;

	public const int ERROR_INVALID_FLAGS = 1004;

	[DllImport("odbc32.dll")]
	public static extern short SQLAllocHandle(short hType, IntPtr inputHandle, out IntPtr outputHandle);

	[DllImport("odbc32.dll")]
	public static extern short SQLSetEnvAttr(IntPtr henv, int attribute, IntPtr valuePtr, int strLength);

	[DllImport("odbc32.dll")]
	public static extern short SQLFreeHandle(short hType, IntPtr handle);

	[DllImport("odbc32.dll", CharSet = CharSet.Ansi)]
	public static extern short SQLBrowseConnect(IntPtr hconn, StringBuilder inString, short inStringLength, StringBuilder outString, short outStringLength, out short outLengthNeeded);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern bool LookupAccountSidW([In][MarshalAs(UnmanagedType.LPTStr)] string systemName, IntPtr sid, [Out][MarshalAs(UnmanagedType.LPTStr)] StringBuilder name, ref int cbName, StringBuilder referencedDomainName, ref int cbReferencedDomainName, out int use);

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool LookupAccountNameW([In][MarshalAs(UnmanagedType.LPTStr)] string systemName, [In][MarshalAs(UnmanagedType.LPTStr)] string accountName, IntPtr sid, ref int cbSid, StringBuilder referencedDomainName, ref int cbReferencedDomainName, out int use);

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool ConvertSidToStringSidW(IntPtr sid, [In][Out][MarshalAs(UnmanagedType.LPTStr)] ref string pStringSid);

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool ConvertStringSidToSidW([In][MarshalAs(UnmanagedType.LPTStr)] string pStringSid, ref IntPtr sid);

	[DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool IsWow64Process(IntPtr hProcess, out bool Wow64Process);
}
