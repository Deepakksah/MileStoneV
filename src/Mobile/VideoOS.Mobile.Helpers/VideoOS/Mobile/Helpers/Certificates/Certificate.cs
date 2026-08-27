using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace VideoOS.Mobile.Helpers.Certificates;

public class Certificate : ICertificateSelectionControlModel
{
	private struct SystemTime
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

	private struct CryptoApiBlob(int dataLength, IntPtr data)
	{
		public int DataLength = dataLength;

		public IntPtr Data = data;
	}

	public struct BLOB
	{
		public int cbData;

		public IntPtr pbData;

		public void Dispose()
		{
			if (!pbData.Equals(IntPtr.Zero))
			{
				Marshal.FreeHGlobal(pbData);
			}
		}
	}

	public struct CRYPT_ALGORITHM_IDENTIFIER
	{
		[MarshalAs(UnmanagedType.LPStr)]
		public string pszObjId;

		public BLOB Parameters;

		public void Dispose()
		{
			Parameters.Dispose();
		}
	}

	private struct CryptKeyProviderInformation
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

	private static class NativeMethods
	{
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FileTimeToSystemTime([In] ref long fileTime, out SystemTime systemTime);

		[DllImport("AdvApi32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptAcquireContextW(out IntPtr providerContext, [MarshalAs(UnmanagedType.LPWStr)] string container, [MarshalAs(UnmanagedType.LPWStr)] string provider, int providerType, int flags);

		[DllImport("AdvApi32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptReleaseContext(IntPtr providerContext, int flags);

		[DllImport("AdvApi32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptGenKey(IntPtr providerContext, int algorithmId, int flags, out IntPtr cryptKeyHandle);

		[DllImport("AdvApi32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptDestroyKey(IntPtr cryptKeyHandle);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CertStrToNameW(int certificateEncodingType, IntPtr x500, int strType, IntPtr reserved, [Out][MarshalAs(UnmanagedType.LPArray)] byte[] encoded, ref int encodedLength, out IntPtr errorString);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		public static extern IntPtr CertCreateSelfSignCertificate(IntPtr providerHandle, [In] ref CryptoApiBlob subjectIssuerBlob, int flags, [In] ref CryptKeyProviderInformation keyProviderInformation, [In] ref CRYPT_ALGORITHM_IDENTIFIER signatureAlgorithm, [In] ref SystemTime startTime, [In] ref SystemTime endTime, IntPtr extensions);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CertFreeCertificateContext(IntPtr certificateContext);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		public static extern IntPtr CertOpenStore([MarshalAs(UnmanagedType.LPStr)] string storeProvider, int messageAndCertificateEncodingType, IntPtr cryptProvHandle, int flags, IntPtr parameters);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CertCloseStore(IntPtr certificateStoreHandle, int flags);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CertAddCertificateContextToStore(IntPtr certificateStoreHandle, IntPtr certificateContext, int addDisposition, out IntPtr storeContextPtr);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CertSetCertificateContextProperty(IntPtr certificateContext, int propertyId, int flags, [In] ref CryptKeyProviderInformation data);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CertSetCertificateContextProperty(IntPtr certificateContext, int propertyId, int flags, ref CryptoApiBlob data);

		[DllImport("Crypt32.dll", ExactSpelling = true, SetLastError = true)]
		[DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PFXExportCertStoreEx(IntPtr certificateStoreHandle, ref CryptoApiBlob pfxBlob, IntPtr password, IntPtr reserved, int flags);
	}

	public const string szOID_RSA_SHA256RSA = "1.2.840.113549.1.1.11";

	public const string szOID_RSA_SHA384RSA = "1.2.840.113549.1.1.12";

	public const string szOID_RSA_SHA512RSA = "1.2.840.113549.1.1.13";

	public const string XCN_OID_SUBJECT_ALT_NAME2 = "2.5.29.17";

	public const int PROV_RSA_FULL = 1;

	public const int PROV_RSA_AES = 24;

	private static string _certificateName = "Mobile Certificate";

	public const X509KeyStorageFlags DefaultX509KeyStorageFlags = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet;

	public static readonly int PublicKeyLengthBits = 2048;

	private static readonly Regex _dnsRegex = new Regex("^(.*)=(.*)");

	public static string DefaultCertificateName
	{
		get
		{
			return _certificateName;
		}
		set
		{
			_certificateName = value;
		}
	}

	public static string AlgorithmRsaSha256 => "1.2.840.113549.1.1.11";

	public static string AlgorithmRsaSha384 => "1.2.840.113549.1.1.12";

	public static string AlgorithmRsaSha512 => "1.2.840.113549.1.1.13";

	public static void AddToStore(StoreName storeName, StoreLocation storeLocation, byte[] certificatePxf)
	{
		using X509Certificate2 certificate = new X509Certificate2(certificatePxf);
		AddToStore(storeName, storeLocation, certificate);
	}

	public static void AddToStore(StoreName storeName, StoreLocation storeLocation, X509Certificate2 certificate)
	{
		X509Store x509Store = new X509Store(storeName, storeLocation);
		x509Store.Open(OpenFlags.ReadWrite);
		x509Store.Add(certificate);
		x509Store.Close();
	}

	public static void RemoveFromStoreByConcreteSubjectName(StoreName storeName, StoreLocation storeLocation, string subjName)
	{
		X509Store x509Store = new X509Store(storeName, storeLocation);
		x509Store.Open(OpenFlags.ReadWrite);
		X509Certificate2Collection certificates = x509Store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, subjName, validOnly: false);
		x509Store.RemoveRange(certificates);
		x509Store.Close();
	}

	public static bool IsCeritificateValid(X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		return certificate.HasPrivateKey;
	}

	public IEnumerable<CertificateData> GetCertificates(StoreName storeName, StoreLocation storeLocation)
	{
		using X509Store x509Store = new X509Store(storeName, storeLocation);
		x509Store.Open(OpenFlags.ReadOnly);
		return (from X509Certificate2 i in x509Store.Certificates
			select new CertificateData(i)).ToArray();
	}

	public static X509Certificate2Collection GetCertificateCollection(StoreName storeName, StoreLocation storeLocation)
	{
		using X509Store x509Store = new X509Store(storeName, storeLocation);
		x509Store.Open(OpenFlags.ReadOnly);
		return x509Store.Certificates;
	}

	public static X509Certificate2 TryFindCertificateByPort(int port)
	{
		string sslCertificateHashBindedOnPort = GetSslCertificateHashBindedOnPort(IPAddress.Any.ToString(), port.ToString(CultureInfo.InvariantCulture));
		if (string.IsNullOrEmpty(sslCertificateHashBindedOnPort))
		{
			return null;
		}
		return TryFindCertificateByHash(StoreName.My, StoreLocation.LocalMachine, sslCertificateHashBindedOnPort);
	}

	public static List<string> GetSubjectAlternativeNames(X509Certificate2 cert)
	{
		cert = cert ?? throw new ArgumentNullException("cert");
		List<string> list = new List<string>();
		string text = (from X509Extension n in cert.Extensions
			where n.Oid.Value == "2.5.29.17"
			select new AsnEncodedData(n.Oid, n.RawData) into n
			select n.Format(multiLine: true)).FirstOrDefault();
		if (text != null)
		{
			string[] array = text.Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string input in array)
			{
				GroupCollection groups = _dnsRegex.Match(input).Groups;
				if (groups.Count > 0 && !string.IsNullOrEmpty(groups[2].Value))
				{
					list.Add(groups[2].Value);
				}
			}
		}
		return list;
	}

	public static X509Certificate2 TryFindCertificateByHash(StoreName storeName, StoreLocation storeLocation, string certHash)
	{
		try
		{
			X509Certificate2Collection x509Certificate2Collection = GetCertificateCollection(storeName, storeLocation).Find(X509FindType.FindByThumbprint, certHash, validOnly: false);
			if (x509Certificate2Collection.Count > 0)
			{
				return x509Certificate2Collection[0];
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	public static string GetSslCertificateHashBindedOnPort(string ip, string port)
	{
		return HttpApiDll.GetSslCertificateHashBindedOnPort(ip, port);
	}

	public static bool TryBindCertificateToPort(string ip, string port, string certHash, string appId, out string errorMsg)
	{
		return HttpApiDll.TryBindCertificateToPort(ip, port, certHash, appId, out errorMsg);
	}

	public static bool TryUnbindCertificateFromPort(string ip, string port, string appId, out string errorMsg)
	{
		return HttpApiDll.TryUnbindCertificateFromPort(ip, port, appId, out errorMsg);
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
			Check(NativeMethods.CryptAcquireContextW(out providerContext, text, null, 1, 8));
			Check(NativeMethods.CryptGenKey(providerContext, 1, 1 | (PublicKeyLengthBits << 16), out cryptKeyHandle));
			int encodedLength = 0;
			gCHandle = GCHandle.Alloc(x500, GCHandleType.Pinned);
			if (!NativeMethods.CertStrToNameW(65537, gCHandle.AddrOfPinnedObject(), 3, IntPtr.Zero, null, ref encodedLength, out var errorString))
			{
				throw new ArgumentException(Marshal.PtrToStringUni(errorString));
			}
			byte[] array = new byte[encodedLength];
			if (!NativeMethods.CertStrToNameW(65537, gCHandle.AddrOfPinnedObject(), 3, IntPtr.Zero, array, ref encodedLength, out errorString))
			{
				throw new ArgumentException(Marshal.PtrToStringUni(errorString));
			}
			gCHandle.Free();
			gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			CryptoApiBlob subjectIssuerBlob = new CryptoApiBlob(array.Length, gCHandle.AddrOfPinnedObject());
			CryptKeyProviderInformation data = new CryptKeyProviderInformation
			{
				ContainerName = text,
				ProviderType = 24,
				KeySpec = 1
			};
			CRYPT_ALGORITHM_IDENTIFIER signatureAlgorithm = new CRYPT_ALGORITHM_IDENTIFIER
			{
				pszObjId = "1.2.840.113549.1.1.11"
			};
			intPtr = NativeMethods.CertCreateSelfSignCertificate(providerContext, ref subjectIssuerBlob, 0, ref data, ref signatureAlgorithm, ref startTime2, ref endTime2, IntPtr.Zero);
			Check(intPtr != IntPtr.Zero);
			gCHandle.Free();
			intPtr2 = NativeMethods.CertOpenStore("Memory", 0, IntPtr.Zero, 8192, IntPtr.Zero);
			Check(intPtr2 != IntPtr.Zero);
			Check(NativeMethods.CertAddCertificateContextToStore(intPtr2, intPtr, 1, out storeContextPtr));
			NativeMethods.CertSetCertificateContextProperty(storeContextPtr, 2, 0, ref data);
			byte[] array2 = new byte[DefaultCertificateName.Length * 2];
			Buffer.BlockCopy(DefaultCertificateName.ToCharArray(), 0, array2, 0, array2.Length);
			GCHandle gCHandle2 = GCHandle.Alloc(array2, GCHandleType.Pinned);
			CryptoApiBlob data2 = new CryptoApiBlob(array2.Length, gCHandle2.AddrOfPinnedObject());
			NativeMethods.CertSetCertificateContextProperty(storeContextPtr, 11, 0, ref data2);
			gCHandle2.Free();
			if (password != null)
			{
				intPtr3 = Marshal.SecureStringToCoTaskMemUnicode(password);
			}
			CryptoApiBlob pfxBlob = default(CryptoApiBlob);
			Check(NativeMethods.PFXExportCertStoreEx(intPtr2, ref pfxBlob, intPtr3, IntPtr.Zero, 7));
			byte[] array3 = new byte[pfxBlob.DataLength];
			gCHandle = GCHandle.Alloc(array3, GCHandleType.Pinned);
			pfxBlob.Data = gCHandle.AddrOfPinnedObject();
			Check(NativeMethods.PFXExportCertStoreEx(intPtr2, ref pfxBlob, intPtr3, IntPtr.Zero, 7));
			gCHandle.Free();
			return array3;
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
				NativeMethods.CertFreeCertificateContext(intPtr);
			}
			if (storeContextPtr != IntPtr.Zero)
			{
				NativeMethods.CertFreeCertificateContext(storeContextPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				NativeMethods.CertCloseStore(intPtr2, 0);
			}
			if (cryptKeyHandle != IntPtr.Zero)
			{
				NativeMethods.CryptDestroyKey(cryptKeyHandle);
			}
			if (providerContext != IntPtr.Zero)
			{
				NativeMethods.CryptReleaseContext(providerContext, 0);
				NativeMethods.CryptAcquireContextW(out providerContext, text, null, 1, 16);
			}
		}
	}

	private static SystemTime ToSystemTime(DateTime dateTime)
	{
		long fileTime = dateTime.ToFileTime();
		Check(NativeMethods.FileTimeToSystemTime(ref fileTime, out var systemTime));
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
