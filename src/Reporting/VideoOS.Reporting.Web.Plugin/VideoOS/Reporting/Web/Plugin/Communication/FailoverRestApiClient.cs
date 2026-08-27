using System;
using System.Net.Http;
using System.Net.Http.Headers;
using VideoOS.Management.Common;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business;

namespace VideoOS.Reporting.Web.Plugin.Communication;

public static class FailoverRestApiClient
{
	public static FailoverStatusDto GetFailoverStatus(Uri uri)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			string rawData = ReportSecurity.FailoverTokenCache.RefreshToken().GetAwaiter().GetResult()
				.RawData;
			HttpClient val = new HttpClient();
			try
			{
				val.BaseAddress = uri;
				HttpRequestMessage val2 = new HttpRequestMessage(HttpMethod.Get, "api/failover/getfailoverstatus");
				try
				{
					val2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", rawData);
					HttpResponseMessage result = val.SendAsync(val2).GetAwaiter().GetResult();
					try
					{
						if (result.IsSuccessStatusCode)
						{
							return result.Content.ReadAsAsync<FailoverStatusDto>().GetAwaiter().GetResult();
						}
					}
					finally
					{
						((IDisposable)result)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			throw new ReportServerException("Communication error: " + ex.Message, ex);
		}
		throw new ReportServerException("Unexpected communication result (failed response)");
	}
}
