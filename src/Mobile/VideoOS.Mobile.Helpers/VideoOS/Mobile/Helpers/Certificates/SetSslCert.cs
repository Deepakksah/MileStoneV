using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Mobile.Helpers.Certificates;

public class SetSslCert
{
	private enum HTTP_SERVICE_CONFIG_ID
	{
		HttpServiceConfigIPListenList,
		HttpServiceConfigSSLCertInfo,
		HttpServiceConfigUrlAclInfo,
		HttpServiceConfigMax
	}

	public struct HTTP_SERVICE_CONFIG_IP_LISTEN_PARAM
	{
		public ushort AddrLength;

		public IntPtr pAddress;
	}

	public struct HTTP_SERVICE_CONFIG_SSL_SET
	{
		public HTTP_SERVICE_CONFIG_SSL_KEY KeyDesc;

		public HTTP_SERVICE_CONFIG_SSL_PARAM ParamDesc;
	}

	public struct HTTP_SERVICE_CONFIG_SSL_KEY
	{
		public IntPtr pIpPort;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct HTTP_SERVICE_CONFIG_SSL_PARAM
	{
		public int SslHashLength;

		public IntPtr pSslHash;

		public Guid AppId;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string pSslCertStoreName;

		public uint DefaultCertCheckMode;

		public int DefaultRevocationFreshnessTime;

		public int DefaultRevocationUrlRetrievalTimeout;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string pDefaultSslCtlIdentifier;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string pDefaultSslCtlStoreName;

		public uint DefaultFlags;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	[ExcludeFromCodeCoverage]
	public struct HTTPAPI_VERSION(ushort majorVersion, ushort minorVersion)
	{
		public ushort HttpApiMajorVersion = majorVersion;

		public ushort HttpApiMinorVersion = minorVersion;
	}

	private struct HTTP_SERVICE_CONFIG_SSL_QUERY
	{
		public HTTP_SERVICE_CONFIG_QUERY_TYPE QueryDesc;

		public HTTP_SERVICE_CONFIG_SSL_KEY KeyDesc;

		public uint dwToken;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	[ExcludeFromCodeCoverage]
	public struct HTTP_SERVICE_CONFIG_URLACL_KEY(string urlPrefix)
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pUrlPrefix = urlPrefix;
	}

	public enum HTTP_SERVICE_CONFIG_QUERY_TYPE
	{
		HttpServiceConfigQueryExact,
		HttpServiceConfigQueryNext,
		HttpServiceConfigQueryMax
	}

	public const uint HTTP_INITIALIZE_CONFIG = 2u;

	public const uint HTTP_SERVICE_CONFIG_SSL_FLAG_USE_DS_MAPPER = 1u;

	public const uint HTTP_SERVICE_CONFIG_SSL_FLAG_NEGOTIATE_CLIENT_CERT = 2u;

	public const uint HTTP_SERVICE_CONFIG_SSL_FLAG_NO_RAW_FILTER = 4u;

	public static int NOERROR = 0;

	public static int ERROR_CANNOT_FIND_SPECIFIED_FILE = 2;

	public static int ERROR_ALREADY_EXISTS = 183;

	public static int ERROR_INSUFFICIENT_BUFFER = 122;

	private const int CertCheckMode_EnabledClientCertRevocationCheck = 0;

	private const int CertCheckMode_ClientCertNotVerifiedForRevocation = 1;

	private const int CertCheckMode_OnlyCachedCertRevocationUsed = 2;

	private const int CertCheckMode_DefaultRevocationFreshnessTimeEnabled = 4;

	private const int CertCheckMode_Disabled = 65536;

	[DllImport("httpapi.dll", SetLastError = true)]
	public static extern uint HttpInitialize(HTTPAPI_VERSION Version, uint Flags, IntPtr pReserved);

	[DllImport("httpapi.dll", SetLastError = true)]
	private static extern uint HttpQueryServiceConfiguration(IntPtr ServiceIntPtr, HTTP_SERVICE_CONFIG_ID ConfigId, IntPtr pInputConfigInfo, int InputConfigInfoLength, IntPtr pOutputConfigInfo, int OutputConfigInfoLength, [Optional] out int pReturnLength, IntPtr pOverlapped);

	[DllImport("httpapi.dll", SetLastError = true)]
	private static extern uint HttpSetServiceConfiguration(IntPtr ServiceIntPtr, HTTP_SERVICE_CONFIG_ID ConfigId, IntPtr pConfigInformation, int ConfigInformationLength, IntPtr pOverlapped);

	[DllImport("httpapi.dll", SetLastError = true)]
	private static extern uint HttpDeleteServiceConfiguration(IntPtr ServiceIntPtr, HTTP_SERVICE_CONFIG_ID ConfigId, IntPtr pConfigInformation, int ConfigInformationLength, IntPtr pOverlapped);

	[DllImport("httpapi.dll", SetLastError = true)]
	public static extern uint HttpTerminate(uint Flags, IntPtr pReserved);

	public static void UnBindCertificate(string ipAddress, int port, byte[] hash, string appId)
	{
		BindUnbindCertificate(ipAddress, port, hash, appId, HttpDeleteServiceConfiguration);
	}

	public static void BindCertificate(string ipAddress, int port, byte[] hash, string appId)
	{
		BindUnbindCertificate(ipAddress, port, hash, appId, HttpSetServiceConfiguration);
	}

	private static void BindUnbindCertificate(string ipAddress, int port, byte[] hash, string appId, Func<IntPtr, HTTP_SERVICE_CONFIG_ID, IntPtr, int, IntPtr, uint> function)
	{
		uint nOERROR = (uint)NOERROR;
		nOERROR = HttpInitialize(new HTTPAPI_VERSION(1, 0), 2u, IntPtr.Zero);
		if (NOERROR == (int)nOERROR)
		{
			HTTP_SERVICE_CONFIG_SSL_SET structure = default(HTTP_SERVICE_CONFIG_SSL_SET);
			HTTP_SERVICE_CONFIG_SSL_KEY keyDesc = default(HTTP_SERVICE_CONFIG_SSL_KEY);
			HTTP_SERVICE_CONFIG_SSL_PARAM paramDesc = default(HTTP_SERVICE_CONFIG_SSL_PARAM);
			SocketAddress socketAddress = new IPEndPoint(IPAddress.Parse(ipAddress), port).Serialize();
			byte[] array = new byte[socketAddress.Size];
			GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			for (int i = 0; i < socketAddress.Size; i++)
			{
				array[i] = socketAddress[i];
			}
			keyDesc.pIpPort = gCHandle.AddrOfPinnedObject();
			GCHandle gCHandle2 = GCHandle.Alloc(hash, GCHandleType.Pinned);
			paramDesc.AppId = new Guid(appId);
			paramDesc.DefaultCertCheckMode = 65536u;
			paramDesc.DefaultFlags = 4u;
			paramDesc.DefaultRevocationFreshnessTime = 0;
			paramDesc.DefaultRevocationUrlRetrievalTimeout = 0;
			paramDesc.pSslCertStoreName = StoreName.My.ToString();
			paramDesc.pSslHash = gCHandle2.AddrOfPinnedObject();
			if (hash != null)
			{
				paramDesc.SslHashLength = hash.Length;
			}
			structure.ParamDesc = paramDesc;
			structure.KeyDesc = keyDesc;
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_SSL_SET)));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			nOERROR = function(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo, intPtr, Marshal.SizeOf(structure), IntPtr.Zero);
			Marshal.FreeCoTaskMem(intPtr);
			HttpTerminate(2u, IntPtr.Zero);
		}
		if (NOERROR != (int)nOERROR)
		{
			throw new Win32Exception(Convert.ToInt32(nOERROR));
		}
	}

	public static byte[] GetCertHashByPort(string ipAddress, int port)
	{
		HTTPAPI_VERSION version = new HTTPAPI_VERSION(1, 0);
		uint nOERROR = (uint)NOERROR;
		HTTP_SERVICE_CONFIG_SSL_SET hTTP_SERVICE_CONFIG_SSL_SET = default(HTTP_SERVICE_CONFIG_SSL_SET);
		nOERROR = HttpInitialize(version, 2u, IntPtr.Zero);
		if (NOERROR == (int)nOERROR)
		{
			HTTP_SERVICE_CONFIG_SSL_KEY keyDesc = default(HTTP_SERVICE_CONFIG_SSL_KEY);
			SocketAddress socketAddress = new IPEndPoint(IPAddress.Parse(ipAddress), port).Serialize();
			byte[] array = new byte[socketAddress.Size];
			GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			for (int i = 0; i < socketAddress.Size; i++)
			{
				array[i] = socketAddress[i];
			}
			keyDesc.pIpPort = gCHandle.AddrOfPinnedObject();
			HTTP_SERVICE_CONFIG_SSL_QUERY structure = new HTTP_SERVICE_CONFIG_SSL_QUERY
			{
				QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryExact,
				KeyDesc = keyDesc,
				dwToken = 0u
			};
			Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_SSL_QUERY));
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_SSL_QUERY)));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: true);
			IntPtr intPtr2 = Marshal.AllocCoTaskMem(500);
			int pReturnLength = 0;
			Marshal.SizeOf(structure);
			nOERROR = HttpQueryServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo, intPtr, Marshal.SizeOf(structure), intPtr2, pReturnLength, out pReturnLength, IntPtr.Zero);
			if (ERROR_INSUFFICIENT_BUFFER == nOERROR)
			{
				Marshal.FreeCoTaskMem(intPtr2);
				intPtr2 = Marshal.AllocCoTaskMem(Convert.ToInt32(pReturnLength));
				nOERROR = HttpQueryServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo, intPtr, Marshal.SizeOf(structure), intPtr2, pReturnLength, out pReturnLength, IntPtr.Zero);
			}
			if (NOERROR == nOERROR)
			{
				hTTP_SERVICE_CONFIG_SSL_SET = (HTTP_SERVICE_CONFIG_SSL_SET)Marshal.PtrToStructure(intPtr2, typeof(HTTP_SERVICE_CONFIG_SSL_SET));
			}
			Marshal.FreeCoTaskMem(intPtr2);
			HttpTerminate(2u, IntPtr.Zero);
		}
		byte[] array2 = null;
		if (NOERROR == (int)nOERROR)
		{
			if (hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.SslHashLength > 0 && hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslHash != IntPtr.Zero)
			{
				array2 = new byte[hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.SslHashLength];
				Marshal.Copy(hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslHash, array2, 0, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.SslHashLength);
			}
		}
		else if (ERROR_CANNOT_FIND_SPECIFIED_FILE == (int)nOERROR)
		{
			return null;
		}
		return array2;
	}
}
