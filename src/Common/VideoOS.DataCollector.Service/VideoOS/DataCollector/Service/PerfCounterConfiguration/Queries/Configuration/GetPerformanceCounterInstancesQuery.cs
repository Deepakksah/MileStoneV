using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Grpc;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;

public record GetPerformanceCounterInstancesQuery(string CategoryName) : ReceiveRequestBase<GetPerformanceCounterInstancesResponse>((LogSettings?)null)
{
	[CompilerGenerated]
	public override int GetHashCode()
	{
		return base.GetHashCode() * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CategoryName);
	}

	[CompilerGenerated]
	protected GetPerformanceCounterInstancesQuery(GetPerformanceCounterInstancesQuery original)
		: base((ReceiveRequestBase<GetPerformanceCounterInstancesResponse>)original)
	{
		CategoryName = original.CategoryName;
	}
}
