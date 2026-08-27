using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace VideoOS.Management.Server;

public static class NativeMethods
{
	public enum SystemErrorCodes
	{
		ERROR_NONE_MAPPED = 1332,
		ERROR_TRUST_FAILURE = 1789
	}

	public sealed class SafeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		protected override bool ReleaseHandle()
		{
			return CloseHandle(handle);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		public SafeThreadHandle()
			: base(ownsHandle: true)
		{
		}

		public void InitialSetHandle(IntPtr h)
		{
			SetHandle(h);
		}
	}

	[DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "LookupAccountSidW", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool LookupAccountSid(string systemName, byte[] psid, StringBuilder accountName, ref uint nameLength, StringBuilder domainName, ref uint domainLength, out AccountType accountType);

	public static string SidToName(byte[] sid, out string domainName, out AccountType accountType)
	{
		uint nameLength = 512u;
		uint domainLength = 512u;
		StringBuilder stringBuilder = new StringBuilder((int)nameLength);
		StringBuilder stringBuilder2 = new StringBuilder((int)domainLength);
		if (!LookupAccountSid(null, sid, stringBuilder, ref nameLength, stringBuilder2, ref domainLength, out accountType))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		domainName = stringBuilder2.ToString();
		return stringBuilder.ToString();
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	public static extern void OutputDebugString(string s);

	[DllImport("kernel32.dll")]
	public static extern int GetCurrentThreadId();

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern SafeThreadHandle OpenThread(int access, bool inherit, int threadId);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool QueryThreadCycleTime(SafeThreadHandle threadHandle, out ulong cycleTime);

	public static long GetCPUTime()
	{
		ulong cycleTime = 0uL;
		try
		{
			using SafeThreadHandle safeThreadHandle = OpenThread(64, inherit: false, GetCurrentThreadId());
			if (!safeThreadHandle.IsInvalid)
			{
				QueryThreadCycleTime(safeThreadHandle, out cycleTime);
			}
		}
		catch (Exception)
		{
		}
		return (long)cycleTime;
	}
}
