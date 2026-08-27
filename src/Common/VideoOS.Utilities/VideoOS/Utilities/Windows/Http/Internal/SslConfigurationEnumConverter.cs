using System;

namespace VideoOS.Utilities.Windows.Http.Internal;

public static class SslConfigurationEnumConverter
{
	public static ClientCertificateCheckMode ToManaged(this ClientCertCheckMode value)
	{
		return value switch
		{
			ClientCertCheckMode.Enabled => ClientCertificateCheckMode.EnableRevocationCheck, 
			ClientCertCheckMode.NoVerifyRevocation => ClientCertificateCheckMode.NoRevocationCheck, 
			ClientCertCheckMode.CachedRevocationOnly => ClientCertificateCheckMode.OnlyCachedCertRevocation, 
			ClientCertCheckMode.UseRevocationFreshnessTime => ClientCertificateCheckMode.UseDefaultRevocationFreshnessTime, 
			ClientCertCheckMode.NoUsageCheck => ClientCertificateCheckMode.NoUsageCheck, 
			_ => throw new ArgumentOutOfRangeException("Unsupported value of the certificate check mode.", (Exception)null), 
		};
	}

	public static ClientCertCheckMode ToNative(this ClientCertificateCheckMode value)
	{
		return value switch
		{
			ClientCertificateCheckMode.EnableRevocationCheck => ClientCertCheckMode.Enabled, 
			ClientCertificateCheckMode.NoRevocationCheck => ClientCertCheckMode.NoVerifyRevocation, 
			ClientCertificateCheckMode.OnlyCachedCertRevocation => ClientCertCheckMode.CachedRevocationOnly, 
			ClientCertificateCheckMode.UseDefaultRevocationFreshnessTime => ClientCertCheckMode.UseRevocationFreshnessTime, 
			ClientCertificateCheckMode.NoUsageCheck => ClientCertCheckMode.NoUsageCheck, 
			_ => throw new ArgumentOutOfRangeException("Unsupported value of the certificate check mode.", (Exception)null), 
		};
	}

	public static SslConfigOptions ToManaged(this SslConfigFlag value)
	{
		SslConfigOptions sslConfigOptions = SslConfigOptions.None;
		SslConfigFlag sslConfigFlag = SslConfigFlag.None;
		if ((value & SslConfigFlag.DoNotRouteToRawIsapiFilters) == SslConfigFlag.DoNotRouteToRawIsapiFilters)
		{
			sslConfigFlag |= SslConfigFlag.DoNotRouteToRawIsapiFilters;
			sslConfigOptions |= SslConfigOptions.NoRawFilter;
		}
		if ((value & SslConfigFlag.NegotiateClientCertificates) == SslConfigFlag.NegotiateClientCertificates)
		{
			sslConfigFlag |= SslConfigFlag.NegotiateClientCertificates;
			sslConfigOptions |= SslConfigOptions.NegotiateClientCertificate;
		}
		if ((value & SslConfigFlag.UseDSMapper) == SslConfigFlag.UseDSMapper)
		{
			sslConfigFlag |= SslConfigFlag.UseDSMapper;
			sslConfigOptions |= SslConfigOptions.UseDSMapper;
		}
		return sslConfigOptions;
	}

	public static SslConfigFlag ToNative(this SslConfigOptions value)
	{
		SslConfigFlag sslConfigFlag = SslConfigFlag.None;
		SslConfigOptions sslConfigOptions = SslConfigOptions.None;
		if ((value & SslConfigOptions.NoRawFilter) == SslConfigOptions.NoRawFilter)
		{
			sslConfigOptions |= SslConfigOptions.NoRawFilter;
			sslConfigFlag |= SslConfigFlag.DoNotRouteToRawIsapiFilters;
		}
		if ((value & SslConfigOptions.NegotiateClientCertificate) == SslConfigOptions.NegotiateClientCertificate)
		{
			sslConfigOptions |= SslConfigOptions.NegotiateClientCertificate;
			sslConfigFlag |= SslConfigFlag.NegotiateClientCertificates;
		}
		if ((value & SslConfigOptions.UseDSMapper) == SslConfigOptions.UseDSMapper)
		{
			sslConfigOptions |= SslConfigOptions.UseDSMapper;
			sslConfigFlag |= SslConfigFlag.UseDSMapper;
		}
		return sslConfigFlag;
	}
}
