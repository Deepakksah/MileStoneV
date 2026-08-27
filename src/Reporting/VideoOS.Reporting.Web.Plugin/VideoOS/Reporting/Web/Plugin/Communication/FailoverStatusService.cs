using System;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Plugin.Communication;

public static class FailoverStatusService
{
	public static FailoverStatus GetFailoverStatus(Uri uri)
	{
		FailoverStatusDto failoverStatus = FailoverRestApiClient.GetFailoverStatus(uri);
		if (!Enum.TryParse<ConnectStatus>(failoverStatus.ConnectStatus, out var result))
		{
			result = ConnectStatus.Unknown;
		}
		if (!Enum.TryParse<VideoOS.Reporting.Web.Plugin.Business.Model.ServiceStatus>(failoverStatus.Status, out var result2))
		{
			result2 = VideoOS.Reporting.Web.Plugin.Business.Model.ServiceStatus.Unknown;
		}
		if (!Enum.TryParse<WatchingState>(failoverStatus.WatchingState, out var result3))
		{
			result3 = WatchingState.Unknown;
		}
		return new FailoverStatus(result2, result, result3, failoverStatus.RecorderWatchItems.Select((RecorderWatchItemDto r) => new RecorderWatchItem(r.RecorderId, r.Position, r.Errors)), failoverStatus.LocalRecorderCommunicationErrors);
	}
}
