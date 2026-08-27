using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace VideoOS.Server.Admin.Utilities;

public static class NativeMethods
{
	public enum COMPUTER_NAME_FORMAT
	{
		ComputerNameNetBIOS,
		ComputerNameDnsHostname,
		ComputerNameDnsDomain,
		ComputerNameDnsFullyQualified,
		ComputerNamePhysicalNetBIOS,
		ComputerNamePhysicalDnsHostname,
		ComputerNamePhysicalDnsDomain,
		ComputerNamePhysicalDnsFullyQualified,
		ComputerNameMax
	}

	public struct CryptoApiBlob(int dataLength, IntPtr data)
	{
		public int DataLength = dataLength;

		public IntPtr Data = data;
	}

	public struct CryptKeyProviderInformation
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ContainerName;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProviderName;

		public int ProviderType;

		public int Flags;

		public int ProviderParameterCount;

		public IntPtr ProviderParameters;

		public int KeySpec;
	}

	public struct SystemTime
	{
		public short Year;

		public short Month;

		public short DayOfWeek;

		public short Day;

		public short Hour;

		public short Minute;

		public short Second;

		public short Milliseconds;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct DomainControllerInfo
	{
		public string DomainControllerName;

		public string DomainControllerAddress;

		public int DomainControllerAddressType;

		public Guid DomainGuid;

		public string DomainName;

		public string DnsForestName;

		public int Flags;

		public string DcSiteName;

		public string ClientSiteName;
	}

	[Flags]
	private enum DSGETDCNAME_FLAGS : uint
	{
		DS_FORCE_REDISCOVERY = 1u,
		DS_DIRECTORY_SERVICE_REQUIRED = 0x10u,
		DS_DIRECTORY_SERVICE_PREFERRED = 0x20u,
		DS_GC_SERVER_REQUIRED = 0x40u,
		DS_PDC_REQUIRED = 0x80u,
		DS_BACKGROUND_ONLY = 0x100u,
		DS_IP_REQUIRED = 0x200u,
		DS_KDC_REQUIRED = 0x400u,
		DS_TIMESERV_REQUIRED = 0x800u,
		DS_WRITABLE_REQUIRED = 0x1000u,
		DS_GOOD_TIMESERV_PREFERRED = 0x2000u,
		DS_AVOID_SELF = 0x4000u,
		DS_ONLY_LDAP_NEEDED = 0x8000u,
		DS_IS_FLAT_NAME = 0x10000u,
		DS_IS_DNS_NAME = 0x20000u,
		DS_RETURN_DNS_NAME = 0x40000000u,
		DS_RETURN_FLAT_NAME = 0x80000000u
	}

	[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CertAddCertificateContextToStore(IntPtr certificateStoreHandle, IntPtr certificateContext, int addDisposition, out IntPtr storeContextPtr);

	[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CertCloseStore(IntPtr certificateStoreHandle, int flags);

	[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr CertCreateSelfSignCertificate(IntPtr providerHandle, [In] ref CryptoApiBlob subjectIssuerBlob, int flags, [In] ref CryptKeyProviderInformation keyProviderInformation, IntPtr signatureAlgorithm, [In] ref SystemTime startTime, [In] ref SystemTime endTime, IntPtr extensions);

	[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CertFreeCertificateContext(IntPtr certificateContext);

	[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr CertOpenStore([MarshalAs(UnmanagedType.LPStr)] string storeProvider, int messageAndCertificateEncodingType, IntPtr cryptProvHandle, int flags, IntPtr parameters);

	[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CertSetCertificateContextProperty(IntPtr certificateContext, int propertyId, int flags, [In] ref CryptKeyProviderInformation data);

	[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CertStrToNameW(int certificateEncodingType, IntPtr x500, int strType, IntPtr reserved, [Out][MarshalAs(UnmanagedType.LPArray)] byte[] encoded, ref int encodedLength, out IntPtr errorString);

	[DllImport("AdvApi32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CryptAcquireContextW(out IntPtr providerContext, [MarshalAs(UnmanagedType.LPWStr)] string container, [MarshalAs(UnmanagedType.LPWStr)] string provider, int providerType, int flags);

	[DllImport("AdvApi32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CryptDestroyKey(IntPtr cryptKeyHandle);

	[DllImport("AdvApi32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CryptGenKey(IntPtr providerContext, int algorithmId, int flags, out IntPtr cryptKeyHandle);

	[DllImport("AdvApi32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool CryptReleaseContext(IntPtr providerContext, int flags);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "DnsHostnameToComputerNameW", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool DnsHostnameToComputerName(string Hostname, StringBuilder ComputerName, ref int nSize);

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool FileTimeToSystemTime([In] ref long fileTime, out SystemTime systemTime);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetComputerNameExW", SetLastError = true)]
	public static extern int GetComputerNameEx(COMPUTER_NAME_FORMAT NameType, [Out] StringBuilder lpBuffer, ref uint lpnSize);

	[DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "LookupAccountSidW", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool LookupAccountSid(string systemName, byte[] psid, StringBuilder accountName, ref uint nameLength, StringBuilder domainName, ref uint domainLength, out AccountType accountType);

	[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool PFXExportCertStoreEx(IntPtr certificateStoreHandle, ref CryptoApiBlob pfxBlob, IntPtr password, IntPtr reserved, int flags);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

	[DllImport("Netapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DsGetDcNameW")]
	private static extern int DsGetDcName([In] string computerName, [In] string domainName, [In] IntPtr domainGuid, [In] string siteName, [In] DSGETDCNAME_FLAGS flags, out IntPtr domainControllerInfo);

	[DllImport("Netapi32.dll")]
	private static extern int NetApiBufferFree([In] IntPtr buffer);

	public static string GetNetBIOSDomainName(string domainFqdn)
	{
		IntPtr domainControllerInfo = IntPtr.Zero;
		try
		{
			if (DsGetDcName(string.Empty, domainFqdn, IntPtr.Zero, string.Empty, DSGETDCNAME_FLAGS.DS_IS_DNS_NAME | DSGETDCNAME_FLAGS.DS_RETURN_FLAT_NAME, out domainControllerInfo) == 0)
			{
				DomainControllerInfo obj = (DomainControllerInfo)Marshal.PtrToStructure(domainControllerInfo, typeof(DomainControllerInfo));
				domainControllerInfo = IntPtr.Zero;
				return obj.DomainName;
			}
		}
		finally
		{
			NetApiBufferFree(domainControllerInfo);
		}
		return null;
	}

	public static string GetComputerNameEx(COMPUTER_NAME_FORMAT format)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		uint lpnSize = (uint)stringBuilder.Capacity;
		if (GetComputerNameEx(format, stringBuilder, ref lpnSize) != 0)
		{
			return stringBuilder.ToString();
		}
		return null;
	}

	public static string DnsHostnameToComputerName(string hostname)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		int nSize = stringBuilder.Capacity;
		if (DnsHostnameToComputerName(hostname, stringBuilder, ref nSize))
		{
			return stringBuilder.ToString();
		}
		return null;
	}

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

	public static byte[] CreateSelfSignCertificatePfx(string x500, DateTime startTime, DateTime endTime, string insecurePassword)
	{
		SecureString secureString = null;
		try
		{
			if (!string.IsNullOrEmpty(insecurePassword))
			{
				secureString = new SecureString();
				foreach (char c in insecurePassword)
				{
					secureString.AppendChar(c);
				}
				secureString.MakeReadOnly();
			}
			return CreateSelfSignCertificatePfx(x500, startTime, endTime, secureString);
		}
		finally
		{
			secureString?.Dispose();
		}
	}

	public static byte[] CreateSelfSignCertificatePfx(string x500, DateTime startTime, DateTime endTime, SecureString password)
	{
		if (x500 == null)
		{
			x500 = "";
		}
		SystemTime startTime2 = ToSystemTime(startTime);
		SystemTime endTime2 = ToSystemTime(endTime);
		string text = Guid.NewGuid().ToString();
		GCHandle gCHandle = default(GCHandle);
		IntPtr providerContext = IntPtr.Zero;
		IntPtr cryptKeyHandle = IntPtr.Zero;
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		IntPtr storeContextPtr = IntPtr.Zero;
		IntPtr intPtr3 = IntPtr.Zero;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			Check(CryptAcquireContextW(out providerContext, text, null, 1, 8));
			Check(CryptGenKey(providerContext, 1, 134217729, out cryptKeyHandle));
			int encodedLength = 0;
			gCHandle = GCHandle.Alloc(x500, GCHandleType.Pinned);
			if (!CertStrToNameW(65537, gCHandle.AddrOfPinnedObject(), 3, IntPtr.Zero, null, ref encodedLength, out var errorString))
			{
				throw new ArgumentException(Marshal.PtrToStringUni(errorString));
			}
			byte[] array = new byte[encodedLength];
			if (!CertStrToNameW(65537, gCHandle.AddrOfPinnedObject(), 3, IntPtr.Zero, array, ref encodedLength, out errorString))
			{
				throw new ArgumentException(Marshal.PtrToStringUni(errorString));
			}
			gCHandle.Free();
			gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			CryptoApiBlob subjectIssuerBlob = new CryptoApiBlob(array.Length, gCHandle.AddrOfPinnedObject());
			CryptKeyProviderInformation data = new CryptKeyProviderInformation
			{
				ContainerName = text,
				ProviderType = 1,
				KeySpec = 1
			};
			intPtr = CertCreateSelfSignCertificate(providerContext, ref subjectIssuerBlob, 0, ref data, IntPtr.Zero, ref startTime2, ref endTime2, IntPtr.Zero);
			Check(intPtr != IntPtr.Zero);
			gCHandle.Free();
			intPtr2 = CertOpenStore("Memory", 0, IntPtr.Zero, 8192, IntPtr.Zero);
			Check(intPtr2 != IntPtr.Zero);
			Check(CertAddCertificateContextToStore(intPtr2, intPtr, 1, out storeContextPtr));
			CertSetCertificateContextProperty(storeContextPtr, 2, 0, ref data);
			if (password != null)
			{
				intPtr3 = Marshal.SecureStringToCoTaskMemUnicode(password);
			}
			CryptoApiBlob pfxBlob = default(CryptoApiBlob);
			Check(PFXExportCertStoreEx(intPtr2, ref pfxBlob, intPtr3, IntPtr.Zero, 7));
			byte[] array2 = new byte[pfxBlob.DataLength];
			gCHandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
			pfxBlob.Data = gCHandle.AddrOfPinnedObject();
			Check(PFXExportCertStoreEx(intPtr2, ref pfxBlob, intPtr3, IntPtr.Zero, 7));
			gCHandle.Free();
			return array2;
		}
		finally
		{
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.ZeroFreeCoTaskMemUnicode(intPtr3);
			}
			if (gCHandle.IsAllocated)
			{
				gCHandle.Free();
			}
			if (intPtr != IntPtr.Zero)
			{
				CertFreeCertificateContext(intPtr);
			}
			if (storeContextPtr != IntPtr.Zero)
			{
				CertFreeCertificateContext(storeContextPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				CertCloseStore(intPtr2, 0);
			}
			if (cryptKeyHandle != IntPtr.Zero)
			{
				CryptDestroyKey(cryptKeyHandle);
			}
			if (providerContext != IntPtr.Zero)
			{
				CryptReleaseContext(providerContext, 0);
				CryptAcquireContextW(out providerContext, text, null, 1, 16);
			}
		}
	}

	private static SystemTime ToSystemTime(DateTime dateTime)
	{
		long fileTime = dateTime.ToFileTime();
		Check(FileTimeToSystemTime(ref fileTime, out var systemTime));
		return systemTime;
	}

	private static void Check(bool nativeCallSucceeded)
	{
		if (!nativeCallSucceeded)
		{
			Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
		}
	}
}
