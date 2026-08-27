using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Http;

public class HttpRacePing
{
	public static Uri FindBestAddress(IEnumerable<Uri> raceAddresses, TimeSpan timeout)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		Uri uri = null;
		try
		{
			if (raceAddresses == null)
			{
				return null;
			}
			HttpClient val = new HttpClient((HttpMessageHandler)new HttpClientHandler
			{
				AllowAutoRedirect = false
			});
			try
			{
				List<Task<HttpResponseMessage>> list = new List<Task<HttpResponseMessage>>();
				foreach (Uri raceAddress in raceAddresses)
				{
					list.Add(val.GetAsync(raceAddress));
				}
				try
				{
					Task.WhenAll(list).Wait(timeout);
				}
				catch (Exception ex)
				{
					if (list.Any((Task<HttpResponseMessage> t) => t.Status.Equals(TaskStatus.RanToCompletion)))
					{
						Log.Trace(MethodBase.GetCurrentMethod().Name, ex.Message);
					}
					else
					{
						Log.Exception(ex);
					}
				}
				List<Task<HttpResponseMessage>> tasks = list.Where((Task<HttpResponseMessage> t) => t.IsCompleted && t.Status == TaskStatus.RanToCompletion && t.Result.IsSuccessStatusCode).ToList();
				uri = tasks.FindUriByScheme(Uri.UriSchemeHttps);
				if (uri == null)
				{
					uri = tasks.FindUriByScheme(Uri.UriSchemeHttp);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return uri;
	}
}
