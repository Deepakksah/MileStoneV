using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common;

public class HttpGet : IHttpGet
{
	private class HttpWebRequestThread : AsyncThread<IHttpGetResult>
	{
		private const string ThreadName = "Http Get Web Request";

		private Uri _uri;

		private int _timeout;

		public HttpWebRequestThread(Uri idpUri, int timeout)
			: base(ThreadPriority.AboveNormal, "Http Get Web Request")
		{
			_uri = idpUri;
			_timeout = timeout;
		}

		public override Task<IHttpGetResult> Run()
		{
			return Run(CancellationToken.None);
		}

		public override Task<IHttpGetResult> Run(CancellationToken cancellationToken)
		{
			return Run(delegate
			{
				bool isValidCertificate = false;
				HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(_uri);
				obj.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
				{
					isValidCertificate = sslPolicyErrors == SslPolicyErrors.None;
					return true;
				};
				obj.Method = "GET";
				obj.Timeout = _timeout;
				using HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
				cancellationToken.ThrowIfCancellationRequested();
				using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
				cancellationToken.ThrowIfCancellationRequested();
				return new HttpGetResult(streamReader.ReadToEnd(), isValidCertificate);
			}, cancellationToken);
		}
	}

	private const int DefaultTimeout = 60000;

	public CancellationToken CancelToken { get; set; }

	public HttpGet()
	{
		CancelToken = CancellationToken.None;
	}

	public HttpGet(CancellationToken cancellationToken)
	{
		CancelToken = cancellationToken;
	}

	public Task<IHttpGetResult> GetResponseAsync(Uri uri)
	{
		return GetResponseAsync(uri, 60000);
	}

	public async Task<IHttpGetResult> GetResponseAsync(Uri uri, int timeout)
	{
		HttpWebRequestThread httpWebRequestThread = new HttpWebRequestThread(uri, timeout);
		try
		{
			return await httpWebRequestThread.Run(CancelToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (AggregateException ex)
		{
			ex.Handle((Exception h) => true);
			return null;
		}
		catch (Exception ex2)
		{
			if (ex2 is WebException)
			{
				return null;
			}
			throw;
		}
	}
}
