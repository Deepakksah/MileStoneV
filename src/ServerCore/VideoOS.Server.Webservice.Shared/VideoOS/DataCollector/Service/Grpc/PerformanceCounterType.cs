using Google.Protobuf.Reflection;

namespace VideoOS.DataCollector.Service.Grpc;

public enum PerformanceCounterType
{
	[OriginalName("NUMBER_OF_ITEMS_HEX32")]
	NumberOfItemsHex32 = 0,
	[OriginalName("NUMBER_OF_ITEMS_HEX64")]
	NumberOfItemsHex64 = 256,
	[OriginalName("NUMBER_OF_ITEMS_32")]
	NumberOfItems32 = 65536,
	[OriginalName("NUMBER_OF_ITEMS_64")]
	NumberOfItems64 = 65792,
	[OriginalName("COUNTER_DELTA_32")]
	CounterDelta32 = 4195328,
	[OriginalName("COUNTER_DELTA_64")]
	CounterDelta64 = 4195584,
	[OriginalName("SAMPLE_COUNTER")]
	SampleCounter = 4260864,
	[OriginalName("COUNT_PER_TIME_INTERVAL_32")]
	CountPerTimeInterval32 = 4523008,
	[OriginalName("COUNT_PER_TIME_INTERVAL_64")]
	CountPerTimeInterval64 = 4523264,
	[OriginalName("RATE_OF_COUNTS_PER_SECOND_32")]
	RateOfCountsPerSecond32 = 272696320,
	[OriginalName("RATE_OF_COUNTS_PER_SECOND_64")]
	RateOfCountsPerSecond64 = 272696576,
	[OriginalName("RAW_FRACTION")]
	RawFraction = 537003008,
	[OriginalName("COUNTER_TIMER")]
	CounterTimer = 541132032,
	[OriginalName("TIMER_100_NS")]
	Timer100Ns = 542180608,
	[OriginalName("SAMPLE_FRACTION")]
	SampleFraction = 549585920,
	[OriginalName("COUNTER_TIMER_INVERSE")]
	CounterTimerInverse = 557909248,
	[OriginalName("TIMER_100_NS_INVERSE")]
	Timer100NsInverse = 558957824,
	[OriginalName("COUNTER_MULTI_TIMER")]
	CounterMultiTimer = 574686464,
	[OriginalName("COUNTER_MULTI_TIMER_100_NS")]
	CounterMultiTimer100Ns = 575735040,
	[OriginalName("COUNTER_MULTI_TIMER_INVERSE")]
	CounterMultiTimerInverse = 591463680,
	[OriginalName("COUNTER_MULTI_TIMER_100_NS_INVERSE")]
	CounterMultiTimer100NsInverse = 592512256,
	[OriginalName("AVERAGE_TIMER_32")]
	AverageTimer32 = 805438464,
	[OriginalName("ELAPSED_TIME")]
	ElapsedTime = 807666944,
	[OriginalName("AVERAGE_COUNT_64")]
	AverageCount64 = 1073874176,
	[OriginalName("SAMPLE_BASE")]
	SampleBase = 1073939457,
	[OriginalName("AVERAGE_BASE")]
	AverageBase = 1073939458,
	[OriginalName("RAW_BASE")]
	RawBase = 1073939459,
	[OriginalName("COUNTER_MULTI_BASE")]
	CounterMultiBase = 1107494144
}
