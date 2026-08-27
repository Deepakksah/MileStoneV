using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class FailoverStatus
{
	public bool Unknown { get; private set; }

	public bool ServerUnavailable { get; private set; }

	public ServiceStatus ServiceStatus { get; }

	public ConnectStatus ConnectStatus { get; }

	public WatchingState WatchingState { get; }

	public ReadOnlyCollection<RecorderWatchItem> RecorderWatchItems { get; }

	public CommunicationErrors LocalRecorderCommunicationErrors { get; }

	public static FailoverStatus StatusUnknown => new FailoverStatus
	{
		Unknown = true
	};

	public static FailoverStatus StatusServerUnavailable => new FailoverStatus
	{
		ServerUnavailable = true
	};

	private FailoverStatus()
	{
		RecorderWatchItems = new List<RecorderWatchItem>().AsReadOnly();
	}

	public FailoverStatus(ServiceStatus serviceStatus, ConnectStatus connectStatus, WatchingState watchingState, IEnumerable<RecorderWatchItem> recorderWatchItems, CommunicationErrors localRecorderCommunicationErrors)
	{
		ServiceStatus = serviceStatus;
		ConnectStatus = connectStatus;
		WatchingState = watchingState;
		RecorderWatchItems = recorderWatchItems.ToList().AsReadOnly();
		LocalRecorderCommunicationErrors = localRecorderCommunicationErrors;
	}
}
