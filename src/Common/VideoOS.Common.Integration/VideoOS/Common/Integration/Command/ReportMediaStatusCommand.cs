using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class ReportMediaStatusCommand : CommonCommandBase
{
	public MediaStatusEntry2[] MediaStatus { get; private set; }

	public TrackStatusEntry[] TrackStatus { get; private set; }

	public ReportMediaStatusCommand(MediaStatusEntry2[] mediaStatus, TrackStatusEntry[] trackStatus)
	{
		if (mediaStatus == null)
		{
			throw new ArgumentNullException("mediaStatus");
		}
		if (trackStatus == null)
		{
			throw new ArgumentNullException("trackStatus");
		}
		MediaStatus = mediaStatus;
		TrackStatus = trackStatus;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
