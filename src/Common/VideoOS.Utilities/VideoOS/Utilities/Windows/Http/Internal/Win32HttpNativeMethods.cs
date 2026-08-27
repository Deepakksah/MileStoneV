using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace VideoOS.Utilities.Windows.Http.Internal;

public class Win32HttpNativeMethods : IWin32Http
{
	private class InitializationToken : IDisposable
	{
		private int _disposeCount;

		private InitFlag Flags { get; }

		public InitializationToken(InitFlag flags)
		{
			Flags = flags;
		}

		~InitializationToken()
		{
			PInvoke_HttpTerminate(Flags, IntPtr.Zero);
		}

		public void Dispose()
		{
			if (Interlocked.Increment(ref _disposeCount) <= 1)
			{
				PInvoke_HttpTerminate(Flags, IntPtr.Zero);
				GC.SuppressFinalize(this);
			}
		}
	}

	private struct IpAddressWithPort(ushort port, IPAddress address)
	{
		public static readonly IpAddressWithPort Invalid = new IpAddressWithPort(0, null);

		public ushort Port = port;

		public IPAddress Address = address;

		public bool IsValid
		{
			get
			{
				if (Port != 0)
				{
					return Address != null;
				}
				return false;
			}
		}
	}

	private struct sockaddr_in
	{
		public ushort Family;

		public ushort Port;

		public uint Addr;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Zero;
	}

	private struct sockaddr_in6
	{
		public ushort Family;

		public ushort Port;

		public uint FlowInfo;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] Addr;

		public uint ScopeId;
	}

	private enum InitFlag
	{
		HTTP_INITIALIZE_SERVER = 1,
		HTTP_INITIALIZE_CONFIG
	}

	private enum HTTP_SERVICE_CONFIG_QUERY_TYPE
	{
		HttpServiceConfigQueryExact,
		HttpServiceConfigQueryNext
	}

	private struct HTTP_SERVICE_CONFIG_SSL_KEY
	{
		public IntPtr pIpPort;
	}

	private struct HTTP_SERVICE_CONFIG_SSL_QUERY
	{
		public HTTP_SERVICE_CONFIG_QUERY_TYPE QueryDesc;

		public HTTP_SERVICE_CONFIG_SSL_KEY KeyDesc;

		public int dwToken;
	}

	private struct HTTP_SERVICE_CONFIG_SSL_SET
	{
		public HTTP_SERVICE_CONFIG_SSL_KEY KeyDesc;

		public HTTP_SERVICE_CONFIG_SSL_PARAM ParamDesc;
	}

	private struct HTTP_SERVICE_CONFIG_SSL_PARAM
	{
		public int SslHashLength;

		public IntPtr pSslHash;

		public Guid AppId;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string pSslCertStoreName;

		public ClientCertCheckMode CertCheckMode;

		public uint RevocationFreshnessTime;

		public uint RevocationUrlRetrievalTimeout;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string pSslCtlIdentifier;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string pSslCtlStoreName;

		public SslConfigFlag Flags;
	}

	private struct HTTP_SERVICE_CONFIG_URLACL_KEY
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pUrlPrefix;
	}

	private struct HTTP_SERVICE_CONFIG_URLACL_PARAM
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pStringSecurityDescriptor;
	}

	private struct HTTP_SERVICE_CONFIG_URLACL_SET
	{
		public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;

		public HTTP_SERVICE_CONFIG_URLACL_PARAM ParamDesc;
	}

	private struct HTTP_SERVICE_CONFIG_URLACL_QUERY
	{
		public HTTP_SERVICE_CONFIG_QUERY_TYPE QueryDesc;

		public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;

		public int dwToken;
	}

	private struct HTTPAPI_VERSION(ushort majorVersion, ushort minorVersion)
	{
		public ushort major = majorVersion;

		public ushort minor = minorVersion;
	}

	private enum HTTP_SERVICE_CONFIG_ID
	{
		HttpServiceConfigIPListenList,
		HttpServiceConfigSSLCertInfo,
		HttpServiceConfigUrlAclInfo
	}

	private const int HTTP_CODES_NO_ERROR = 0;

	private const int HTTP_CODES_ERROR_FILE_NOT_FOUND = 2;

	private const int HTTP_CODES_ERROR_INVALID_DATA = 13;

	private const int HTTP_CODES_ERROR_HANDLE_EOF = 38;

	private const int HTTP_CODES_ERROR_INVALID_PARAMETER = 87;

	private const int HTTP_CODES_ERROR_INSUFFICIENT_BUFFER = 122;

	private const int HTTP_CODES_ERROR_MORE_DATA = 234;

	private const int HTTP_CODES_ERROR_NO_MORE_ITEMS = 259;

	private const int HTTP_CODES_ERROR_INVALID_DLL = 1154;

	private const int HTTP_CODES_ERROR_NOT_FOUND = 1168;

	private const string HttpApiDll = "Httpapi.dll";

	public static IWin32Http Instance { get; } = new Win32HttpNativeMethods();

	private Win32HttpNativeMethods()
	{
	}

	public IDisposable InitializeConfig()
	{
		EnsureSuccess(PInvoke_HttpInitialize(new HTTPAPI_VERSION(1, 0), InitFlag.HTTP_INITIALIZE_CONFIG, IntPtr.Zero));
		return new InitializationToken(InitFlag.HTTP_INITIALIZE_CONFIG);
	}

	public IEnumerable<SslConfiguration> QuerySslConfigurations()
	{
		HTTP_SERVICE_CONFIG_SSL_QUERY query = new HTTP_SERVICE_CONFIG_SSL_QUERY
		{
			QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryNext
		};
		int inputSize = Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_SSL_QUERY));
		int outputSize = 0;
		IntPtr pInput = Marshal.AllocHGlobal(inputSize);
		IntPtr pResult = IntPtr.Zero;
		try
		{
			query.dwToken = 0;
			while (true)
			{
				Marshal.StructureToPtr(query, pInput, fDeleteOld: false);
				int writtenDataLength = 0;
				int num = HttpQueryServiceConfigurationWithBufferAdjustments(HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo, pInput, inputSize, ref pResult, ref outputSize, out writtenDataLength);
				switch (num)
				{
				default:
					throw new Win32Exception(num, "Failed to query Http Service Configuration for SSL");
				case 0:
				{
					HTTP_SERVICE_CONFIG_SSL_SET hTTP_SERVICE_CONFIG_SSL_SET = (HTTP_SERVICE_CONFIG_SSL_SET)Marshal.PtrToStructure(pResult, typeof(HTTP_SERVICE_CONFIG_SSL_SET));
					IpAddressWithPort ipAddressWithPort = ParseSocketAddress(hTTP_SERVICE_CONFIG_SSL_SET.KeyDesc.pIpPort);
					if (ipAddressWithPort.IsValid)
					{
						byte[] array = new byte[hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.SslHashLength];
						Marshal.Copy(hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslHash, array, 0, array.Length);
						yield return new SslConfiguration(ipAddressWithPort.Port, ipAddressWithPort.Address, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.AppId, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslCertStoreName, array, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.RevocationFreshnessTime, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.RevocationUrlRetrievalTimeout, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslCtlIdentifier, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslCtlStoreName, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.CertCheckMode, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.Flags);
					}
					query.dwToken++;
					break;
				}
				case 259:
					yield break;
				}
			}
		}
		finally
		{
			if (pResult != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pResult);
			}
			Marshal.FreeHGlobal(pInput);
		}
	}

	public void SetSslConfiguration(SslConfiguration configuration)
	{
		IntPtr intPtr = IntPtr.Zero;
		HTTP_SERVICE_CONFIG_SSL_SET structure = default(HTTP_SERVICE_CONFIG_SSL_SET);
		try
		{
			structure.KeyDesc.pIpPort = IPAddressAndPortToPtr(configuration.Address, configuration.Port);
			byte[] certificateHash = configuration.CertificateHash;
			if (certificateHash != null)
			{
				structure.ParamDesc.pSslHash = Marshal.AllocHGlobal(certificateHash.Length);
				Marshal.Copy(certificateHash, 0, structure.ParamDesc.pSslHash, certificateHash.Length);
				structure.ParamDesc.SslHashLength = certificateHash.Length;
			}
			structure.ParamDesc.AppId = configuration.AppId;
			structure.ParamDesc.pSslCertStoreName = configuration.CertStoreName;
			structure.ParamDesc.RevocationFreshnessTime = configuration.RevocationFreshnessTime;
			structure.ParamDesc.RevocationUrlRetrievalTimeout = configuration.RevocationUrlRetrievalTimeout;
			structure.ParamDesc.pSslCtlIdentifier = configuration.SslCtlIdentifier;
			structure.ParamDesc.pSslCtlStoreName = configuration.SslCtlStoreName;
			structure.ParamDesc.CertCheckMode = configuration.CertCheckMode;
			intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			int num = PInvoke_HttpSetServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo, intPtr, Marshal.SizeOf(structure), IntPtr.Zero);
			if (num != 0)
			{
				throw new Win32Exception(num, "Setting the SSL configuration failed.");
			}
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.DestroyStructure(intPtr, typeof(HTTP_SERVICE_CONFIG_SSL_SET));
				Marshal.FreeHGlobal(intPtr);
			}
			if (structure.KeyDesc.pIpPort != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(structure.KeyDesc.pIpPort);
			}
			if (structure.ParamDesc.pSslHash != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(structure.ParamDesc.pSslHash);
			}
		}
	}

	public SslConfiguration QuerySslConfiguration(SslConfigurationKey configurationKey)
	{
		if (configurationKey == null)
		{
			throw new ArgumentNullException("configurationKey");
		}
		HTTP_SERVICE_CONFIG_SSL_QUERY structure = new HTTP_SERVICE_CONFIG_SSL_QUERY
		{
			QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryExact
		};
		int num = Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_SSL_QUERY));
		int outputSize = 0;
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		IntPtr pOutput = IntPtr.Zero;
		try
		{
			structure.KeyDesc.pIpPort = IPAddressAndPortToPtr(configurationKey.Address, configurationKey.Port);
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			int writtenDataLength = 0;
			int num2 = HttpQueryServiceConfigurationWithBufferAdjustments(HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo, intPtr, num, ref pOutput, ref outputSize, out writtenDataLength);
			switch (num2)
			{
			case 2:
				return null;
			default:
				throw new Win32Exception(num2, "Failed to query Http Service Configuration for SSL");
			case 0:
			{
				HTTP_SERVICE_CONFIG_SSL_SET hTTP_SERVICE_CONFIG_SSL_SET = (HTTP_SERVICE_CONFIG_SSL_SET)Marshal.PtrToStructure(pOutput, typeof(HTTP_SERVICE_CONFIG_SSL_SET));
				IpAddressWithPort ipAddressWithPort = ParseSocketAddress(hTTP_SERVICE_CONFIG_SSL_SET.KeyDesc.pIpPort);
				if (!ipAddressWithPort.IsValid)
				{
					return null;
				}
				byte[] array = new byte[hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.SslHashLength];
				Marshal.Copy(hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslHash, array, 0, array.Length);
				return new SslConfiguration(ipAddressWithPort.Port, ipAddressWithPort.Address, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.AppId, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslCertStoreName, array, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.RevocationFreshnessTime, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.RevocationUrlRetrievalTimeout, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslCtlIdentifier, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.pSslCtlStoreName, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.CertCheckMode, hTTP_SERVICE_CONFIG_SSL_SET.ParamDesc.Flags);
			}
			}
		}
		finally
		{
			if (pOutput != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pOutput);
			}
			if (structure.KeyDesc.pIpPort != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(structure.KeyDesc.pIpPort);
			}
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public void DeleteSslConfiguration(SslConfigurationKey configurationKey)
	{
		if (configurationKey == null)
		{
			throw new ArgumentNullException("configurationKey");
		}
		IntPtr intPtr = IntPtr.Zero;
		HTTP_SERVICE_CONFIG_SSL_SET structure = default(HTTP_SERVICE_CONFIG_SSL_SET);
		try
		{
			structure.KeyDesc.pIpPort = IPAddressAndPortToPtr(configurationKey.Address, configurationKey.Port);
			intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			int num = PInvoke_HttpDeleteServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo, intPtr, Marshal.SizeOf(structure), IntPtr.Zero);
			if (num == 2 || num == 0)
			{
				return;
			}
			throw new Win32Exception(num, "Deleting the SSL configuration failed.");
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.DestroyStructure(intPtr, typeof(HTTP_SERVICE_CONFIG_SSL_SET));
				Marshal.FreeHGlobal(intPtr);
			}
			if (structure.KeyDesc.pIpPort != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(structure.KeyDesc.pIpPort);
			}
		}
	}

	public IEnumerable<UrlAclInfo> QueryUrlAclInfos()
	{
		HTTP_SERVICE_CONFIG_URLACL_QUERY query = new HTTP_SERVICE_CONFIG_URLACL_QUERY
		{
			QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryNext
		};
		int inputSize = Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_URLACL_QUERY));
		int outputSize = 0;
		IntPtr pInput = Marshal.AllocHGlobal(inputSize);
		IntPtr pResult = IntPtr.Zero;
		try
		{
			query.dwToken = 0;
			while (true)
			{
				Marshal.StructureToPtr(query, pInput, fDeleteOld: false);
				int writtenDataLength = 0;
				int num = HttpQueryServiceConfigurationWithBufferAdjustments(HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, pInput, inputSize, ref pResult, ref outputSize, out writtenDataLength);
				switch (num)
				{
				default:
					throw new Win32Exception(num, "Failed to query Http Service Configuration for URL ACL information.");
				case 0:
				{
					HTTP_SERVICE_CONFIG_URLACL_SET hTTP_SERVICE_CONFIG_URLACL_SET = (HTTP_SERVICE_CONFIG_URLACL_SET)Marshal.PtrToStructure(pResult, typeof(HTTP_SERVICE_CONFIG_URLACL_SET));
					yield return new UrlAclInfo(hTTP_SERVICE_CONFIG_URLACL_SET.KeyDesc.pUrlPrefix, hTTP_SERVICE_CONFIG_URLACL_SET.ParamDesc.pStringSecurityDescriptor);
					query.dwToken++;
					break;
				}
				case 259:
					yield break;
				}
			}
		}
		finally
		{
			if (pResult != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pResult);
			}
			Marshal.FreeHGlobal(pInput);
		}
	}

	public UrlAclInfo QueryUrlAclInfo(string urlPrefix)
	{
		HTTP_SERVICE_CONFIG_URLACL_QUERY structure = new HTTP_SERVICE_CONFIG_URLACL_QUERY
		{
			QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryExact,
			KeyDesc = 
			{
				pUrlPrefix = urlPrefix
			}
		};
		int num = Marshal.SizeOf(structure);
		int outputSize = 0;
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		IntPtr pOutput = IntPtr.Zero;
		try
		{
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			int writtenDataLength = 0;
			int num2 = HttpQueryServiceConfigurationWithBufferAdjustments(HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, intPtr, num, ref pOutput, ref outputSize, out writtenDataLength);
			switch (num2)
			{
			case 2:
				return null;
			default:
				throw new Win32Exception(num2, "Failed to query Http Service Configuration for URL ACL information.");
			case 0:
			{
				HTTP_SERVICE_CONFIG_URLACL_SET hTTP_SERVICE_CONFIG_URLACL_SET = (HTTP_SERVICE_CONFIG_URLACL_SET)Marshal.PtrToStructure(pOutput, typeof(HTTP_SERVICE_CONFIG_URLACL_SET));
				return new UrlAclInfo(hTTP_SERVICE_CONFIG_URLACL_SET.KeyDesc.pUrlPrefix, hTTP_SERVICE_CONFIG_URLACL_SET.ParamDesc.pStringSecurityDescriptor);
			}
			}
		}
		finally
		{
			if (pOutput != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pOutput);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.DestroyStructure(intPtr, typeof(HTTP_SERVICE_CONFIG_URLACL_QUERY));
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public void DeleteUrlAclInfo(string urlPrefix)
	{
		if (string.IsNullOrEmpty(urlPrefix))
		{
			throw new ArgumentException("Missing URL Prefix data.", "urlPrefix");
		}
		IntPtr intPtr = IntPtr.Zero;
		HTTP_SERVICE_CONFIG_URLACL_SET structure = new HTTP_SERVICE_CONFIG_URLACL_SET
		{
			KeyDesc = 
			{
				pUrlPrefix = urlPrefix
			}
		};
		try
		{
			intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			int num = PInvoke_HttpDeleteServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, intPtr, Marshal.SizeOf(structure), IntPtr.Zero);
			if (num != 0)
			{
				throw new Win32Exception(num, "Setting the SSL configuration failed.");
			}
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.DestroyStructure(intPtr, typeof(HTTP_SERVICE_CONFIG_URLACL_SET));
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	public void SetUrlAclInfo(UrlAclInfo aclInfo)
	{
		if (aclInfo == null)
		{
			throw new ArgumentNullException("aclInfo");
		}
		if (string.IsNullOrEmpty(aclInfo.UrlPrefix))
		{
			throw new ArgumentException("Milling URL Prefix on the ACL info", "aclInfo");
		}
		if (string.IsNullOrEmpty(aclInfo.Permissions))
		{
			throw new ArgumentException("Milling permissions SDDL string on the ACL info", "aclInfo");
		}
		IntPtr intPtr = IntPtr.Zero;
		HTTP_SERVICE_CONFIG_URLACL_SET structure = new HTTP_SERVICE_CONFIG_URLACL_SET
		{
			KeyDesc = 
			{
				pUrlPrefix = aclInfo.UrlPrefix
			},
			ParamDesc = 
			{
				pStringSecurityDescriptor = aclInfo.Permissions
			}
		};
		try
		{
			intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			int num = PInvoke_HttpSetServiceConfiguration(IntPtr.Zero, HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo, intPtr, Marshal.SizeOf(structure), IntPtr.Zero);
			if (num != 0)
			{
				throw new Win32Exception(num, "Setting the SSL configuration failed.");
			}
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.DestroyStructure(intPtr, typeof(HTTP_SERVICE_CONFIG_URLACL_SET));
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}

	private static int HttpQueryServiceConfigurationWithBufferAdjustments(HTTP_SERVICE_CONFIG_ID configId, IntPtr pInput, int inputSize, ref IntPtr pOutput, ref int outputSize, out int writtenDataLength)
	{
		int num;
		while (true)
		{
			num = PInvoke_HttpQueryServiceConfiguration(IntPtr.Zero, configId, pInput, inputSize, pOutput, outputSize, out writtenDataLength, IntPtr.Zero);
			if (num != 122 && num != 234)
			{
				return num;
			}
			if (writtenDataLength <= outputSize)
			{
				break;
			}
			if (pOutput != IntPtr.Zero)
			{
				IntPtr hglobal = pOutput;
				pOutput = IntPtr.Zero;
				Marshal.FreeHGlobal(hglobal);
			}
			pOutput = Marshal.AllocHGlobal(writtenDataLength);
			outputSize = writtenDataLength;
		}
		return num;
	}

	private static IntPtr IPAddressAndPortToPtr(IPAddress inputAddress, ushort port)
	{
		IntPtr zero = IntPtr.Zero;
		switch (inputAddress.AddressFamily)
		{
		case AddressFamily.InterNetworkV6:
		{
			sockaddr_in6 structure2 = new sockaddr_in6
			{
				Family = 23,
				Port = (ushort)IPAddress.HostToNetworkOrder((short)port),
				Addr = inputAddress.GetAddressBytes(),
				ScopeId = (uint)IPAddress.HostToNetworkOrder(inputAddress.ScopeId)
			};
			zero = Marshal.AllocHGlobal(Marshal.SizeOf(structure2));
			Marshal.StructureToPtr(structure2, zero, fDeleteOld: false);
			return zero;
		}
		case AddressFamily.InterNetwork:
		{
			sockaddr_in structure = new sockaddr_in
			{
				Family = 2,
				Port = (ushort)IPAddress.HostToNetworkOrder((short)port),
				Addr = BitConverter.ToUInt32(inputAddress.GetAddressBytes(), 0),
				Zero = new byte[8]
			};
			zero = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, zero, fDeleteOld: false);
			return zero;
		}
		default:
			throw new ArgumentException("Unsupported address family in SSL configuration passed.", "inputAddress");
		}
	}

	private static void EnsureSuccess(int result)
	{
		if (result == 0)
		{
			return;
		}
		throw new Win32Exception(result);
	}

	private static IpAddressWithPort ParseSocketAddress(IntPtr sockaddr)
	{
		switch ((AddressFamily)Marshal.ReadInt16(sockaddr))
		{
		case AddressFamily.InterNetwork:
		{
			sockaddr_in sockaddr_in8 = (sockaddr_in)Marshal.PtrToStructure(sockaddr, typeof(sockaddr_in));
			return new IpAddressWithPort((ushort)IPAddress.NetworkToHostOrder((short)sockaddr_in8.Port), new IPAddress(sockaddr_in8.Addr));
		}
		case AddressFamily.InterNetworkV6:
		{
			sockaddr_in6 sockaddr_in7 = (sockaddr_in6)Marshal.PtrToStructure(sockaddr, typeof(sockaddr_in6));
			return new IpAddressWithPort((ushort)IPAddress.NetworkToHostOrder((short)sockaddr_in7.Port), new IPAddress(sockaddr_in7.Addr, IPAddress.NetworkToHostOrder(sockaddr_in7.ScopeId)));
		}
		default:
			return IpAddressWithPort.Invalid;
		}
	}

	[DllImport("Httpapi.dll", EntryPoint = "HttpInitialize")]
	private static extern int PInvoke_HttpInitialize(HTTPAPI_VERSION version, InitFlag flags, IntPtr reserved);

	[DllImport("Httpapi.dll", EntryPoint = "HttpTerminate")]
	private static extern int PInvoke_HttpTerminate(InitFlag flags, IntPtr pReserved);

	[DllImport("Httpapi.dll", EntryPoint = "HttpQueryServiceConfiguration")]
	private static extern int PInvoke_HttpQueryServiceConfiguration(IntPtr serviceHandle, HTTP_SERVICE_CONFIG_ID configId, IntPtr pInputConfigInfo, int inputConfigInfoLength, IntPtr pOutputConfigInfo, int outputConfigInfoLength, out int pReturnLength, IntPtr pOverlapped);

	[DllImport("Httpapi.dll", EntryPoint = "HttpSetServiceConfiguration")]
	private static extern int PInvoke_HttpSetServiceConfiguration(IntPtr serviceHandle, HTTP_SERVICE_CONFIG_ID configId, IntPtr pConfigInformation, int configInformationLength, IntPtr pOverlapped);

	[DllImport("Httpapi.dll", EntryPoint = "HttpDeleteServiceConfiguration")]
	private static extern int PInvoke_HttpDeleteServiceConfiguration(IntPtr serviceHandle, HTTP_SERVICE_CONFIG_ID configId, IntPtr pConfigInformation, int configInformationLength, IntPtr pOverlapped);
}
