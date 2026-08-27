using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Net;

public class WebClient : IWebClient
{
	private readonly HttpClient _webClient;

	private readonly ManualResetEvent _handle = new ManualResetEvent(initialState: false);

	public WebClient()
	{
		_webClient = new HttpClient();
		_webClient.DefaultRequestHeaders.Connection.Clear();
		_webClient.DefaultRequestHeaders.Connection.Add("Close");
	}

	public string DownloadString(string location)
	{
		string result = null;
		bool processed = false;
		Task.Run(async delegate
		{
			try
			{
				result = await _webClient.GetStringAsync(location);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, GetCallerName("DownloadString"), ex.Message);
			}
			processed = true;
		});
		while (!processed)
		{
			_handle.WaitOne(10);
		}
		return result;
	}

	private static string GetCallerName([CallerMemberName] string methodName = "")
	{
		return methodName;
	}
}
