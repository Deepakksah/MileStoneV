using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class PrepareMoveHardwareResponse : CommonResponseBase
{
	public IEnumerable<DeviceTrackId> DeviceTracksWithLiveTable { get; private set; }

	public PrepareMoveHardwareResponse(IEnumerable<DeviceTrackId> deviceTracksWithLiveTable)
	{
		DeviceTracksWithLiveTable = deviceTracksWithLiveTable;
	}

	public override string ToString()
	{
		return GetType().Name;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
