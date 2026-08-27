using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VideoOS.DataCollector.Service.Common;
using VideoOS.DataCollector.Service.Common.Models;
using VideoOS.DataCollector.Service.Grpc;

namespace VideoOS.DataCollector.Service.Sample.Queries;

public record GetPerformanceCounterValuesQuery(string CategoryName, string CounterName, string InstanceName, int SampleFrequency) : ReceiveRequestBase<GetPerformanceCounterValuesResponse>((LogSettings?)null)
{
	[CompilerGenerated]
	public override int GetHashCode()
	{
		return (((base.GetHashCode() * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CategoryName)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CounterName)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceName)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(SampleFrequency);
	}

	[CompilerGenerated]
	protected GetPerformanceCounterValuesQuery(GetPerformanceCounterValuesQuery original)
		: base((ReceiveRequestBase<GetPerformanceCounterValuesResponse>)original)
	{
		CategoryName = original.CategoryName;
		CounterName = original.CounterName;
		InstanceName = original.InstanceName;
		SampleFrequency = original.SampleFrequency;
	}
}
