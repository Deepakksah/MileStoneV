using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public enum PerformanceCounterCategoryType
{
	[OriginalName("SINGLE_INSTANCE")]
	SingleInstance = 0,
	[OriginalName("MULTI_INSTANCE")]
	MultiInstance = 1,
	[OriginalName("UNKNOWN")]
	Unknown = -1
}
