using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.Integration.Definition;
using VideoOS.EventServer.Events.RestApi.Client;
using VideoOS.Management.Server.MIP.Communication.Client;
using VideoOS.Platform;
using VideoOS.Platform.EventsData;
using VideoOS.Platform.Messaging;

namespace VideoOS.Management.Server;

public class VideoWallChangedEventSender : IDisposable
{
	private class VideoWallChangedEventData
	{
		public Guid ObjectId { get; set; }

		public Guid ObjectParentId { get; set; }

		public Guid ObjectType { get; set; }

		public EventAction Action { get; set; }
	}

	private readonly EventsRestApiCommunicationManager _eventsRestApiCommunicationManager;

	private readonly object _lockObject = new object();

	private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private readonly List<VideoWallChangedEventData> _requestsQueue;

	public VideoWallChangedEventSender()
	{
		VideoWallChangedEventSender videoWallChangedEventSender = this;
		CancellationToken cancellationToken = _cancellationTokenSource.Token;
		_eventsRestApiCommunicationManager = new EventsRestApiCommunicationManager();
		_requestsQueue = new List<VideoWallChangedEventData>();
		System.Threading.Tasks.Task.Run(async delegate
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				videoWallChangedEventSender.DequeueAndSend();
				await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
			}
		}, cancellationToken);
	}

	private void DequeueAndSend()
	{
		IEnumerable<VideoWallChangedEventData> source;
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
			List<Task<PostEventBody>> source2 = source.Select(async (VideoWallChangedEventData x) => await CreatePostFreeFormEventBodyAsync(requestContext, x)).ToList();
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

	private static async Task<PostEventBody> CreatePostFreeFormEventBodyAsync(IGeminiRequestContext requestContext, VideoWallChangedEventData videoWallChangedEventData)
	{
		return await System.Threading.Tasks.Task.Factory.StartNew((Func<PostEventBody>)(() => new PostFreeFormEventBody
		{
			Source = GeminiApplicationCore.SiteManager.HomeSiteId.ToString(),
			Type = BuiltInEventTypes.EventIDs.SmartWallChanged,
			Data = new SmartWallEventChangedData
			{
				Action = ((videoWallChangedEventData.Action == EventAction.Added) ? "created" : ((videoWallChangedEventData.Action == EventAction.Deleted) ? "deleted" : "updated")),
				RelatedItem = new SmartWallChangedEventDataRelatedItem
				{
					Id = videoWallChangedEventData.ObjectId,
					ParentId = videoWallChangedEventData.ObjectParentId,
					Kind = videoWallChangedEventData.ObjectType,
					FolderType = ((videoWallChangedEventData.ObjectType == Kind.SmartWall) ? FolderType.UserDefined : FolderType.No),
					ViewItemInformation = ((videoWallChangedEventData.ObjectType == Kind.View) ? ViewItemsInformationProvider.GetViewItemsInformation(requestContext, videoWallChangedEventData.ObjectId) : Enumerable.Empty<ViewItemInformation>())
				}
			},
			Time = DateTimeOffset.UtcNow
		}));
	}

	public void Send(Guid objectId, Guid objectParentId, Guid objectType, EventAction action)
	{
		lock (_lockObject)
		{
			_requestsQueue.Add(new VideoWallChangedEventData
			{
				Action = action,
				ObjectId = objectId,
				ObjectParentId = objectParentId,
				ObjectType = objectType
			});
		}
	}

	public void Dispose()
	{
		_cancellationTokenSource.Cancel();
	}
}
