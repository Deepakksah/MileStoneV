using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public class WindowsAccountHelper
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

	private const int NO_ERROR = 0;

	private const int ERROR_INSUFFICIENT_BUFFER = 122;

	private const int ERROR_INVALID_FLAGS = 1004;

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool LookupAccountSid(string lpSystemName, [MarshalAs(UnmanagedType.LPArray)] byte[] sid, StringBuilder lpName, ref uint cchName, StringBuilder referencedDomainName, ref uint cchReferencedDomainName, out SID_NAME_USE peUse);

	public static bool IsPredefinedGroup(string hostName, string sid)
	{
		bool result = false;
		Chronometer chronometer = new Chronometer();
		chronometer.Reset();
		try
		{
			SID_NAME_USE userType = GetUserType(sid, hostName);
			if (userType == SID_NAME_USE.SidTypeWellKnownGroup || userType == SID_NAME_USE.SidTypeAlias)
			{
				result = true;
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		chronometer.MeasureAndPrintTime(MethodBase.GetCurrentMethod().Name, $"sid: {sid}, hostName: {hostName}");
		return result;
	}

	public static SID_NAME_USE GetUserType(string userSid, string machineName)
	{
		SID_NAME_USE result = SID_NAME_USE.SidTypeUnknown;
		Chronometer chronometer = new Chronometer();
		chronometer.Reset();
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			uint cchName = (uint)stringBuilder.Capacity;
			StringBuilder stringBuilder2 = new StringBuilder();
			uint cchReferencedDomainName = (uint)stringBuilder2.Capacity;
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(userSid);
			byte[] array = new byte[securityIdentifier.BinaryLength];
			securityIdentifier.GetBinaryForm(array, 0);
			int num = 0;
			if (!LookupAccountSid(machineName, array, stringBuilder, ref cchName, stringBuilder2, ref cchReferencedDomainName, out var peUse))
			{
				num = Marshal.GetLastWin32Error();
				if (num == 122)
				{
					stringBuilder.EnsureCapacity((int)cchName);
					stringBuilder2.EnsureCapacity((int)cchReferencedDomainName);
					num = 0;
					if (!LookupAccountSid(null, array, stringBuilder, ref cchName, stringBuilder2, ref cchReferencedDomainName, out peUse))
					{
						num = Marshal.GetLastWin32Error();
					}
				}
			}
			if (num == 0)
			{
				result = peUse;
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		chronometer.MeasureAndPrintTime(MethodBase.GetCurrentMethod().Name, $"sid: {userSid}, hostName: {machineName}");
		return result;
	}

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool LookupAccountName(string lpSystemName, string lpAccountName, [MarshalAs(UnmanagedType.LPArray)] byte[] Sid, ref uint cbSid, StringBuilder referencedDomainName, ref uint cchReferencedDomainName, out SID_NAME_USE peUse);

	[DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool ConvertSidToStringSid([MarshalAs(UnmanagedType.LPArray)] byte[] pSID, out IntPtr ptrSid);

	[DllImport("kernel32.dll")]
	private static extern IntPtr LocalFree(IntPtr hMem);

	public static bool GetSid(string machineName, string name, out SID_NAME_USE sidNameUse, out string sidString)
	{
		bool result = false;
		byte[] array = null;
		uint cbSid = 0u;
		StringBuilder stringBuilder = new StringBuilder();
		uint cchReferencedDomainName = (uint)stringBuilder.Capacity;
		sidNameUse = SID_NAME_USE.SidTypeUnknown;
		sidString = string.Empty;
		try
		{
			int num = 0;
			if (!LookupAccountName(machineName, name, array, ref cbSid, stringBuilder, ref cchReferencedDomainName, out sidNameUse))
			{
				num = Marshal.GetLastWin32Error();
				if (num == 122 || num == 1004)
				{
					array = new byte[cbSid];
					stringBuilder.EnsureCapacity((int)cchReferencedDomainName);
					num = 0;
					if (!LookupAccountName(null, name, array, ref cbSid, stringBuilder, ref cchReferencedDomainName, out sidNameUse))
					{
						num = Marshal.GetLastWin32Error();
					}
				}
			}
			sidString = string.Empty;
			if (num == 0)
			{
				if (!ConvertSidToStringSid(array, out var ptrSid))
				{
					num = Marshal.GetLastWin32Error();
					LogImplementer.Instance.Log(LogContent.Error, MethodBase.GetCurrentMethod().Name, "Failed to convert sid to string");
				}
				else
				{
					sidString = Marshal.PtrToStringAuto(ptrSid);
					LocalFree(ptrSid);
					LogImplementer.Instance.Log(LogContent.Error, MethodBase.GetCurrentMethod().Name, $"Found sid {sidNameUse} : {sidString}");
					result = true;
				}
			}
		}
		catch (Exception ex)
		{
			LogImplementer.Instance.Log(LogContent.Error, MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		return result;
	}

	public static bool IsDomainName(string machineName, string domainName)
	{
		bool result = false;
		if (GetSid(machineName, domainName, out var sidNameUse, out var _) && sidNameUse == SID_NAME_USE.SidTypeDomain)
		{
			result = true;
		}
		return result;
	}
}
