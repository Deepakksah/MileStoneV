using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using VideoOS.Common.Interfaces.Communication;

namespace VideoOS.Common.Integration.Communication;

public sealed class HttpPost : IHttpPost
{
	private const string MethodPost = "POST";

	public string Post(Uri url, string dataToPost, int timeout, NameValueCollection additionalHeaders, string userAgent, string contentType)
	{
		if (url == null)
		{
			throw new ArgumentNullException("url");
		}
		if (!url.IsAbsoluteUri)
		{
			throw new ArgumentException("url must be absolute", "url");
		}
		if (dataToPost == null)
		{
			throw new ArgumentNullException("dataToPost");
		}
		if (timeout < 1 || timeout > 1800)
		{
			throw new ArgumentOutOfRangeException("timeout", "timeout must be at least 1 second");
		}
		if (string.IsNullOrEmpty(userAgent))
		{
			throw new ArgumentNullException("userAgent");
		}
		if (string.IsNullOrEmpty(contentType))
		{
			throw new ArgumentNullException("contentType");
		}
		return PostData(url, dataToPost, timeout, additionalHeaders, userAgent, contentType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string PostData(Uri url, string dataToPost, int timeout, NameValueCollection additionalHeaders, string userAgent, string contentType)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(dataToPost);
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.Method = "POST";
		httpWebRequest.Timeout = timeout * 1000;
		httpWebRequest.UserAgent = userAgent;
		httpWebRequest.ContentType = contentType;
		httpWebRequest.ContentLength = bytes.Length;
		if (additionalHeaders != null)
		{
			foreach (string additionalHeader in additionalHeaders)
			{
				httpWebRequest.Headers.Add(additionalHeader, additionalHeaders[additionalHeader]);
			}
		}
		using (Stream stream = httpWebRequest.GetRequestStream())
		{
			stream.Write(bytes, 0, bytes.Length);
		}
		using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
		return streamReader.ReadToEnd();
	}
}
