using System;
using System.Collections.Generic;
using VideoOS.UI.Common.WPF.Timeline.Cache;
using VideoOS.UI.Common.WPF.Timeline.QueryDispatcher;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline;

public static class TimelineFactory
{
	public static readonly TimelineMockLevel MockLevel = TimelineMockLevel.RealData;

	private static Dictionary<Type, C5TimelineCache> _theCacheInstances = new Dictionary<Type, C5TimelineCache>();

	public static ITimelineSequenceCache BuildCache()
	{
		return BuildCache(MockLevel);
	}

	public static ITimelineSequenceCache BuildCache(TimelineMockLevel mockLevel)
	{
		return mockLevel switch
		{
			TimelineMockLevel.RealData => BuildRealDataCache(), 
			TimelineMockLevel.MockServerData => BuildMockServerDataCache(), 
			TimelineMockLevel.MockSequenceSources => BuildMockServerDataCache(), 
			TimelineMockLevel.MockSourceQueryDispatcher => BuildMockRetrieverCache(), 
			TimelineMockLevel.MockCache => BuildMockCache(), 
			TimelineMockLevel.MockSimpleCache => BuildMockSimpleCache(), 
			_ => throw new Exception($"Enumeration value not supported: {MockLevel}"), 
		};
	}

	public static ISequenceRetriever GetDummySequenceRetriever()
	{
		return new DummySequenceRetriever();
	}

	private static ITimelineSequenceCache BuildRealDataCache()
	{
		return GetRealCache(new SourceQueryDispatcher());
	}

	private static ITimelineSequenceCache BuildMockServerDataCache()
	{
		return GetRealCache(new SourceQueryDispatcher());
	}

	private static ITimelineSequenceCache BuildMockRetrieverCache()
	{
		return GetRealCache(new DummyQueryDispatcher());
	}

	private static ITimelineSequenceCache GetRealCache(ISourceQueryDispatcher queryDispatcher)
	{
		if (!_theCacheInstances.TryGetValue(queryDispatcher.GetType(), out var value))
		{
			value = new C5TimelineCache(queryDispatcher);
			_theCacheInstances.Add(queryDispatcher.GetType(), value);
		}
		return value.CreateAccessHandle();
	}

	private static ITimelineSequenceCache BuildMockCache()
	{
		return new DummyC5TimelineSequenceCache();
	}

	private static ITimelineSequenceCache BuildMockSimpleCache()
	{
		return new DummySimpleTimelineSequenceCache();
	}
}
