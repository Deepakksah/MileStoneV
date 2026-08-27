using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class GenerateCertificates : IConfigurationStep
{
	private const string CaUserName = "CA_admin";

	public string CaPassword { get; set; }

	public string SaveToFolder { get; set; }

	private ILogger Log { get; }

	public async Task ExecuteAsync()
	{
		await Task.Run(delegate
		{
			GetAuthorityCertificate();
		});
	}

	public GenerateCertificates(ILogger log)
	{
		Log = log;
	}

	private WebRequest PrepareWebRequest(string Uri, string user, string pass, string message)
	{
		HttpWebRequest httpWebRequest = WebRequest.CreateHttp(Uri);
		httpWebRequest.Credentials = new NetworkCredential(user, pass);
		httpWebRequest.Method = "POST";
		httpWebRequest.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(httpWebRequest.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true));
		StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream(), new ASCIIEncoding());
		try
		{
			streamWriter.Write(message);
		}
		catch (ObjectDisposedException ex)
		{
			Log.LogError("PrepareWebRequest failed with ObjectDisposedException : {ExceptionMessage}", ex.Message);
		}
		catch (NotSupportedException ex2)
		{
			Log.LogError("PrepareWebRequest failed with NotSupportedException : {ExceptionMessage}", ex2.Message);
		}
		catch (IOException ex3)
		{
			Log.LogError("PrepareWebRequest failed with IOException : {ExceptionMessage}", ex3.Message);
		}
		finally
		{
			streamWriter.Dispose();
		}
		return httpWebRequest;
	}

	private static HttpWebResponse ProcessRequest(WebRequest request)
	{
		try
		{
			return (HttpWebResponse)request.GetResponse();
		}
		catch (WebException ex)
		{
			if (ex.Response is HttpWebResponse httpWebResponse)
			{
				throw new ScriptErrorException(string.Format(CultureInfo.InvariantCulture, "Remote server call {0} {1} resulted in a http error {2} {3}.", "Process Request", request.RequestUri, httpWebResponse.StatusCode, httpWebResponse.StatusDescription), ex);
			}
			throw new ScriptErrorException(string.Format(CultureInfo.InvariantCulture, "Remote server call {0} {1} resulted in an error.", "Process Request", request.RequestUri), ex);
		}
		catch (Exception ex2)
		{
			throw new ScriptErrorException("Unknown exception occurred when generating certificates. " + ex2.Message);
		}
	}

	private void GetAuthorityCertificate()
	{
		string uri = "https://" + Environment.MachineName + ":9001/certs/cacert.crt";
		string path = SaveToFolder + "\\serverCert.crt";
		HttpWebResponse httpWebResponse = ProcessRequest(PrepareWebRequest(uri, "CA_admin", CaPassword, ""));
		using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
		byte[] buffer = new byte[16384];
		int num;
		do
		{
			num = httpWebResponse.GetResponseStream().Read(buffer, 0, 16384);
			fileStream.Write(buffer, 0, num);
		}
		while (num > 0);
	}
}
