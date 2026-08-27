using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using VideoOS.DataCollector.Service.Grpc;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.WebService.Common;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Server.Webservice.Shared.DataCollectorService;

public class LiveDataCollectionManager : WebServiceManagerBase<LiveDataCollectionClient, ILiveDataCollection>, ILiveDataCollection, ILiveDataCollectionManager, IDisposable
{
	private readonly bool _secure;

	private readonly string _hostName;

	private bool _disposedValue;

	private GrpcChannel _channel;

	private readonly LiveDataCollectorService.LiveDataCollectorServiceClient _client;

	private static readonly HttpClient HttpClient = new HttpClient((HttpMessageHandler)(object)new GrpcWebHandler((HttpMessageHandler)new HttpClientHandler()));

	public LiveDataCollectionManager(string hostName, bool secure, ITokenCache tokenCache)
		: base(new UriBuilder(secure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, 7609, "DataCollector/").Uri, "ManagementServer/DataCollectorServiceOAuth.svc", tokenCache, (IWebServiceManagerExceptionHandler)null)
	{
		_secure = secure;
		_hostName = hostName;
		_client = CreateGrpcClient();
	}

	public Task<int> GetDataCollectorId()
	{
		return Task.FromResult(_client.GetDataCollectorId(new Empty(), CreateAuthorizationHeader()).Value);
	}

	public string[] GetPerformanceCategories()
	{
		return _client.GetPerformanceCategories(new Empty(), CreateAuthorizationHeader()).Values.ToArray();
	}

	public VideoOS.WebService.Common.DataCollectorService.PerformanceCounterInstance[] GetPerformanceCounterInstances(string categoryName)
	{
		GetPerformanceCounterInstancesRequest request = new GetPerformanceCounterInstancesRequest
		{
			CategoryName = categoryName
		};
		return _client.GetPerformanceCounterInstances(request, CreateAuthorizationHeader()).Values.Select((VideoOS.DataCollector.Service.Grpc.PerformanceCounterInstance instance) => new VideoOS.WebService.Common.DataCollectorService.PerformanceCounterInstance
		{
			CategoryName = instance.CategoryName,
			CategoryType = (System.Diagnostics.PerformanceCounterCategoryType)instance.CategoryType,
			CounterType = (System.Diagnostics.PerformanceCounterType)instance.CounterType,
			CounterDirection = instance.CounterDirection,
			CounterName = instance.CounterName,
			EnableHistory = instance.EnableHistory,
			InstanceName = instance.InstanceName,
			SampleFrequency = instance.SampleFrequency,
			TriggerStatePerValue = instance.TriggerStatePerValue
		}).ToArray();
	}

	public VideoOS.WebService.Common.DataCollectorService.PerformanceCounterValue[] GetPerformanceCounterValues(string categoryName, string counterName, string instanceName, int sampleFrequency)
	{
		GetPerformanceCounterValuesRequest request = new GetPerformanceCounterValuesRequest
		{
			CategoryName = categoryName,
			CounterName = counterName,
			InstanceName = instanceName,
			SampleFrequency = sampleFrequency
		};
		return _client.GetPerformanceCounterValues(request, CreateAuthorizationHeader()).Values.Select((VideoOS.DataCollector.Service.Grpc.PerformanceCounterValue counterValue) => new VideoOS.WebService.Common.DataCollectorService.PerformanceCounterValue
		{
			CategoryName = counterValue.CategoryName,
			CalculatedValue = counterValue.CalculatedValue,
			CollectionTime = counterValue.CollectionTime.ToDateTime(),
			CounterName = counterValue.CounterName,
			InstanceName = counterValue.InstanceName,
			RawValue = counterValue.RawValue,
			SendImmediately = counterValue.SendImmediately
		}).ToArray();
	}

	public Task<bool> IsAlive()
	{
		return Task.FromResult(_client.IsAlive(new Empty(), CreateAuthorizationHeader()).Value);
	}

	public void PostUpdatedCounterStateRuleConfigurations(VideoOS.WebService.Common.DataCollectorService.CounterStateRulesConfigurationUpdate[] ruleConfigUpdates)
	{
		if (ruleConfigUpdates == null)
		{
			throw new ArgumentNullException("ruleConfigUpdates");
		}
		PostUpdatedCounterStateRuleConfigurationsRequest postUpdatedCounterStateRuleConfigurationsRequest = new PostUpdatedCounterStateRuleConfigurationsRequest();
		foreach (VideoOS.WebService.Common.DataCollectorService.CounterStateRulesConfigurationUpdate counterStateRulesConfigurationUpdate in ruleConfigUpdates)
		{
			VideoOS.DataCollector.Service.Grpc.CounterStateRulesConfigurationUpdate item = new VideoOS.DataCollector.Service.Grpc.CounterStateRulesConfigurationUpdate
			{
				Deleted = counterStateRulesConfigurationUpdate.Deleted,
				RuleConfig = new VideoOS.DataCollector.Service.Grpc.CounterStateRuleConfiguration
				{
					CategoryName = counterStateRulesConfigurationUpdate.RuleConfig.CategoryName,
					CounterName = counterStateRulesConfigurationUpdate.RuleConfig.CounterName,
					CounterPath = (counterStateRulesConfigurationUpdate.RuleConfig.CounterPath ?? ""),
					CounterRuleId = counterStateRulesConfigurationUpdate.RuleConfig.CounterRuleId,
					CriticalThreshold = counterStateRulesConfigurationUpdate.RuleConfig.CriticalThreshold,
					Name = counterStateRulesConfigurationUpdate.RuleConfig.Name,
					TimeWindow = counterStateRulesConfigurationUpdate.RuleConfig.TimeWindow,
					WarningThreshold = counterStateRulesConfigurationUpdate.RuleConfig.WarningThreshold
				}
			};
			postUpdatedCounterStateRuleConfigurationsRequest.RuleConfigUpdates.Add(item);
		}
		_client.PostUpdatedCounterStateRuleConfigurations(postUpdatedCounterStateRuleConfigurationsRequest, CreateAuthorizationHeader());
	}

	private LiveDataCollectorService.LiveDataCollectorServiceClient CreateGrpcClient()
	{
		UriBuilder uriBuilder = new UriBuilder(_secure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, _hostName, 7609);
		ServicePointManager.FindServicePoint(uriBuilder.Uri).ConnectionLeaseTimeout = 120000;
		GrpcChannelOptions channelOptions = new GrpcChannelOptions
		{
			HttpClient = HttpClient,
			DisposeHttpClient = false
		};
		_channel = GrpcChannel.ForAddress(uriBuilder.Uri, channelOptions);
		return new LiveDataCollectorService.LiveDataCollectorServiceClient(_channel);
	}

	private Metadata CreateAuthorizationHeader()
	{
		return new Metadata { 
		{
			"Authorization",
			"Bearer " + base.TokenCache.RefreshToken().GetAwaiter().GetResult()
				.RawData
		} };
	}

	protected override LiveDataCollectionClient CreateClientObject(Binding binding, EndpointAddress remoteAddress)
	{
		return new LiveDataCollectionClient(binding, remoteAddress);
	}

	protected override void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_channel?.Dispose();
			}
			_disposedValue = true;
		}
		base.Dispose(disposing);
	}
}
