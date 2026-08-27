using System;
using VideoOS.Management.Common;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class RecorderWatchItem
{
	public Guid RecorderId { get; }

	public int Position { get; }

	public CommunicationErrors CommunicationErrors { get; }

	public RecorderWatchItem(Guid recorderId, int position, CommunicationErrors communicationErrors)
	{
		RecorderId = recorderId;
		Position = position;
		CommunicationErrors = communicationErrors;
	}

	public RecorderWatchItem Clone()
	{
		return new RecorderWatchItem(RecorderId, Position, CommunicationErrors);
	}
}
