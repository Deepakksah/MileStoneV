using System.Runtime.CompilerServices;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Grpc;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;

public record GetDataCollectorIdQuery() : ReceiveRequestBase<GetDataCollectorIdResponse>((LogSettings?)null)
{
	[CompilerGenerated]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	[CompilerGenerated]
	protected GetDataCollectorIdQuery(GetDataCollectorIdQuery original)
		: base((ReceiveRequestBase<GetDataCollectorIdResponse>)original)
	{
	}
}
