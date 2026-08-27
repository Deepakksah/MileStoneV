using System;
using System.Diagnostics;
using System.Reflection;
using VideoOS.IdentityServer.Common.Utilities;

namespace VideoOS.IdentityServer.Common;

public sealed class IdpServerInformation : IIdpServerInformation
{
	public Uri UriValidated { get; }

	public bool IsValid => IdpConfiguration != null;

	public bool IsCurrentVersion
	{
		get
		{
			if (!IsValid)
			{
				throw new InvalidOperationException();
			}
			if (string.IsNullOrEmpty(IdpConfiguration.ServerVersion))
			{
				return false;
			}
			Version version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
			Version version2 = new Version(IdpConfiguration.ServerVersion);
			if (version2.Major == version.Major)
			{
				return version2.Minor == version.Minor;
			}
			return false;
		}
	}

	public ServerVersionCompareResult ServerVersionCompare
	{
		get
		{
			if (!IsValid || string.IsNullOrEmpty(IdpConfiguration.ServerVersion))
			{
				return ServerVersionCompareResult.Unknown;
			}
			Version version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
			version = new Version(version.Major, version.Minor, 0, 0);
			Version version2 = new Version(IdpConfiguration.ServerVersion);
			version2 = new Version(version2.Major, version2.Minor, 0, 0);
			return version2.CompareTo(version) switch
			{
				-1 => ServerVersionCompareResult.OlderThanClient, 
				0 => ServerVersionCompareResult.EqualToClient, 
				1 => ServerVersionCompareResult.NewerThanClient, 
				_ => ServerVersionCompareResult.Unknown, 
			};
		}
	}

	public bool IsOAuthSupported
	{
		get
		{
			if (IdpConfiguration != null)
			{
				return !string.IsNullOrEmpty(IdpConfiguration.ServerVersion);
			}
			return false;
		}
	}

	public bool ValidCertificate { get; set; }

	public bool SecureIdpValidated { get; set; }

	public bool SecureIdpAvailable { get; set; }

	public bool UnsecureIdpAvailable { get; set; }

	public bool UnsecureIdpValidated { get; set; }

	public Uri ValidatedServerUri { get; set; }

	public IIdpOpenConfiguration IdpConfiguration { get; set; }

	public IdpDiscoveryType RequestedIdpDiscoveryType { get; set; }

	public bool ContainsCustomPort { get; set; }

	public SslEndpointVerificationResult SecureSslVerification { get; set; }

	public SslEndpointVerificationResult UnsecureSslVerification { get; set; }

	public IdpServerInformation(IdpDiscoveryType requestedIdpDiscoveryType, Uri uri)
	{
		UriValidated = uri ?? throw new ArgumentNullException("uri");
		RequestedIdpDiscoveryType = requestedIdpDiscoveryType;
		SecureSslVerification = SslEndpointVerificationResult.NotVerified;
		UnsecureSslVerification = SslEndpointVerificationResult.NotVerified;
	}
}
