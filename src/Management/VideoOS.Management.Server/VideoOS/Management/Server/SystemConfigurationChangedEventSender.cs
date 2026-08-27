using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Definition;
using VideoOS.EventServer.Events.RestApi.Client;
using VideoOS.Management.Server.MIP.Communication.Client;
using VideoOS.Platform.EventsData;

namespace VideoOS.Management.Server;

public class SystemConfigurationChangedEventSender : IDisposable
{
	private readonly EventsRestApiCommunicationManager _eventsRestApiCommunicationManager;

	private readonly object _lockObject = new object();

	private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private readonly List<SystemConfigurationChangedEventData> _requestsQueue;

	public SystemConfigurationChangedEventSender()
	{
		SystemConfigurationChangedEventSender systemConfigurationChangedEventSender = this;
		CancellationToken cancellationToken = _cancellationTokenSource.Token;
		_eventsRestApiCommunicationManager = new EventsRestApiCommunicationManager();
		_requestsQueue = new List<SystemConfigurationChangedEventData>();
		System.Threading.Tasks.Task.Run(async delegate
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				systemConfigurationChangedEventSender.DequeueAndSend();
				await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
			}
		}, cancellationToken);
	}

	private void DequeueAndSend()
	{
		IEnumerable<SystemConfigurationChangedEventData> source;
		lock (_lockObject)
		{
			source = _requestsQueue.ToList();
			_requestsQueue.Clear();
		}
		if (!source.Any())
		{
			return;
		}
		GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		try
		{
			List<Task<PostEventBody>> source2 = source.Select(async (SystemConfigurationChangedEventData x) => await CreatePostFreeFormEventBodyAsync(requestContext, x)).ToList();
			_eventsRestApiCommunicationManager.SentEventsToConsumer(source2.Select((Task<PostEventBody> x) => x.Result));
		}
		finally
		{
			if (requestContext != null)
			{
				((IDisposable)requestContext).Dispose();
			}
		}
	}

	private static async Task<PostEventBody> CreatePostFreeFormEventBodyAsync(IGeminiRequestContext requestContext, SystemConfigurationChangedEventData systemConfigurationChangedEventData)
	{
		return await System.Threading.Tasks.Task.Factory.StartNew((Func<PostEventBody>)(() => new PostFreeFormEventBody
		{
			Source = GeminiApplicationCore.SiteManager.HomeSiteId.ToString(),
			Data = new SystemConfigurationChangedEventData
			{
				Action = GetActionName(systemConfigurationChangedEventData.Action),
				RelatedItem = systemConfigurationChangedEventData.RelatedItem
			},
			Type = BuiltInEventTypes.EventIDs.SystemConfigurationChanged,
			Time = DateTimeOffset.UtcNow
		}));
	}

	private static string GetActionName(string vmsActionName)
	{
		if (vmsActionName == EventAction.Added.ToString() || vmsActionName == "Created")
		{
			return "created";
		}
		if (vmsActionName == EventAction.Deleted.ToString())
		{
			return "deleted";
		}
		return "updated";
	}

	public void Send(Guid objectId, string name, string itemType, Guid parentId, string parentItemType, EventAction action)
	{
		string restResource = new SendEventRestResource(objectId, itemType, parentId, parentItemType).ToString();
		Send(restResource, name, action);
	}

	public void Send(Guid objectId, string name, string itemType, EventAction action)
	{
		string restResource = new SendEventRestResource(objectId, itemType).ToString();
		Send(restResource, name, action);
	}

	private void Send(string restResource, string name, EventAction action)
	{
		lock (_lockObject)
		{
			_requestsQueue.Add(new SystemConfigurationChangedEventData
			{
				Action = ((action == EventAction.Added) ? "Created" : action.ToString()),
				RelatedItem = new SystemConfigurationChangedEventDataRelatedItem
				{
					RelatedItemRestResource = restResource,
					Name = name
				}
			});
		}
	}

	public void Dispose()
	{
		_cancellationTokenSource.Cancel();
	}
}
