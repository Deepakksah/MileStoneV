using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Utilities.Network;

public static class SslUtilities
{
	private const int MillisecondsPerSeconds = 1000;

	private const int DefaultTimeoutFiveSeconds = 5;

	private const int MinimumTimeoutSeconds = 1;

	private const int MaximumTimeoutSeconds = 300;

	private const int TimedOut = 10060;

	private const int ConnectionRefused = 10061;

	private const int HostNotFound = 11001;

	public static SslEndpointVerificationResult EndpointSupportsSsl(Uri uri)
	{
		return EndpointSupportsSsl(uri, 5);
	}

	public static SslEndpointVerificationResult EndpointSupportsSsl(Uri uri, int timeoutInSeconds)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (!uri.IsAbsoluteUri)
		{
			throw new ArgumentException("Partial Uri can not be validated", "uri");
		}
		if (timeoutInSeconds < 1 || timeoutInSeconds > 300)
		{
			throw new ArgumentException($"Timeout must be between {1} and {300}", "timeoutInSeconds");
		}
		try
		{
			TcpClient tcpClient = new TcpClient(uri.Host, uri.Port)
			{
				ReceiveTimeout = timeoutInSeconds * 1000,
				SendTimeout = timeoutInSeconds * 1000
			};
			RemoteCertificateValidationCallback userCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => false;
			SslStream sslStream = new SslStream(tcpClient.GetStream(), leaveInnerStreamOpen: false, userCertificateValidationCallback, null);
			try
			{
				sslStream.AuthenticateAsClient(uri.Host);
			}
			catch (SocketException ex)
			{
				if (ex.ErrorCode == 10060)
				{
					return SslEndpointVerificationResult.TimedOut;
				}
				return SslEndpointVerificationResult.UnknownResponse;
			}
			catch (IOException ex2)
			{
				if (ex2.InnerException != null && ex2.InnerException is SocketException { ErrorCode: 10060 })
				{
					return SslEndpointVerificationResult.TimedOut;
				}
				return SslEndpointVerificationResult.SslNotSupported;
			}
			catch (AuthenticationException)
			{
				return SslEndpointVerificationResult.SslSupportedInvalidCertificate;
			}
			finally
			{
				tcpClient.Close();
			}
		}
		catch (SocketException ex5)
		{
			return ex5.ErrorCode switch
			{
				10060 => SslEndpointVerificationResult.TimedOut, 
				11001 => SslEndpointVerificationResult.InvalidHostName, 
				10061 => SslEndpointVerificationResult.EndpointNotFound, 
				_ => SslEndpointVerificationResult.UnknownResponse, 
			};
		}
		return SslEndpointVerificationResult.SslSupported;
	}
}
