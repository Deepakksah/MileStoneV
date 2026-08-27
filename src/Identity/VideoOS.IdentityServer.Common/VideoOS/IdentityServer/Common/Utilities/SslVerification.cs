using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.IdentityServer.Common.Utilities;

public sealed class SslVerification : ISslVerification
{
	private const int MillisecondsPerSecond = 1000;

	private const int DefaultTimeoutSixtySeconds = 60;

	private const int MinimumTimeoutSeconds = 1;

	private const int TimedOut = 10060;

	private const int ConnectionRefused = 10061;

	private const int HostNotFound = 11001;

	public int TimeoutInSeconds { get; }

	public SslVerification()
		: this(60)
	{
	}

	public SslVerification(int timeoutInSeconds)
	{
		TimeoutInSeconds = timeoutInSeconds;
	}

	public SslEndpointVerificationResult VerifyEndpointSupportsSsl(Uri uri)
	{
		return VerifyEndpointSupportsSsl(uri, TimeoutInSeconds);
	}

	public SslEndpointVerificationResult VerifyEndpointSupportsSsl(Uri uri, int timeoutInSeconds)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (!uri.IsAbsoluteUri)
		{
			throw new ArgumentException("Partial Uri can not be validated", "uri");
		}
		if (timeoutInSeconds < 1)
		{
			throw new ArgumentException($"Timeout must be greater or equal to than {1}", "timeoutInSeconds");
		}
		IAsyncResult asyncResult = null;
		TcpClient tcpClient = null;
		try
		{
			tcpClient = new TcpClient
			{
				ReceiveTimeout = timeoutInSeconds * 1000,
				SendTimeout = timeoutInSeconds * 1000
			};
			asyncResult = tcpClient.BeginConnect(uri.Host, uri.Port, null, null);
			if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeoutInSeconds)))
			{
				throw new SocketException(10060);
			}
			if (!tcpClient.Connected)
			{
				throw new SocketException(10061);
			}
			RemoteCertificateValidationCallback userCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => sslPolicyErrors == SslPolicyErrors.None;
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
		finally
		{
			if (tcpClient != null)
			{
				if (tcpClient.Connected && asyncResult != null)
				{
					tcpClient.EndConnect(asyncResult);
				}
				tcpClient.Close();
			}
		}
		return SslEndpointVerificationResult.SslSupported;
	}
}
