using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;

public record GetConfigurationServerQuery(int DataCollectorId) : SendRequestBase<Result<DataCollectorConfiguration>>((RetrySettings?)null, (LogSettings?)null)
{
	[CompilerGenerated]
	public override int GetHashCode()
	{
		return base.GetHashCode() * -1521134295 + EqualityComparer<int>.Default.GetHashCode(DataCollectorId);
	}

	[CompilerGenerated]
	protected GetConfigurationServerQuery(GetConfigurationServerQuery original)
		: base((SendRequestBase<Result<DataCollectorConfiguration>>)original)
	{
		DataCollectorId = original.DataCollectorId;
	}
}
