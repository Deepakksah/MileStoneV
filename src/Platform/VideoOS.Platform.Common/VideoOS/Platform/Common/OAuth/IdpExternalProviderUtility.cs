#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.Platform.Common.OAuth;

public class IdpExternalProviderUtility
{
	private int Timeout = 5;

	public async Task<List<IdpExternalProviderInfo>> ExternalProviders(string address)
	{
		DiscoveryClientFactory discoveryClientFactory = new DiscoveryClientFactory();
		Uri item = new Uri(new Uri(address), "Idp");
		IDiscoveryClient discoveryClient = ((IDiscoveryClientFactory)discoveryClientFactory).Create((IEnumerable<Uri>)new List<Uri> { item }, (HttpMessageHandler)null);
		using CancellationTokenSource source = new CancellationTokenSource(Timeout * 1000);
		ExternalProviderDiscoveryResponse externalProviderDiscoveryResponse = await ((DiscoveryClient)discoveryClient).GetExternalProviderAsync(source.Token);
		if (externalProviderDiscoveryResponse.IsError)
		{
			if (externalProviderDiscoveryResponse.ErrorResponses != null)
			{
				string text = string.Join("\n", externalProviderDiscoveryResponse.ErrorResponses.Select((DiscoveryResponseBase r) => r.Error));
				Trace.WriteLine("ExternalProviders discovery failed. ServerUrl: " + address + ", Error(s): " + text);
			}
			return null;
		}
		ExternalProviderModel[] externalProviders = externalProviderDiscoveryResponse.ExternalProviders;
		if (externalProviders == null)
		{
			return null;
		}
		List<IdpExternalProviderInfo> list = new List<IdpExternalProviderInfo>();
		ExternalProviderModel[] array = externalProviders;
		foreach (ExternalProviderModel externalProviderModel in array)
		{
			list.Add(new IdpExternalProviderInfo
			{
				DisplayName = externalProviderModel.DisplayName,
				LoginProviderId = externalProviderModel.LoginProviderId
			});
		}
		return list;
	}
}
