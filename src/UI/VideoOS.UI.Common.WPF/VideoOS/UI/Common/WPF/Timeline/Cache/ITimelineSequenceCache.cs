using System;
using System.Collections.Generic;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline.Cache;

public interface ITimelineSequenceCache
{
	string Name { get; }

	bool SupportsMultithreading { get; }

	event EventHandler<SequenceSourcesEventArgs> NewSequencesAvailable;

	void InitConfig(TimelineCacheSettings timelineCacheSettings);

	void SetCurrentTimeInterval(TimeInterval interval);

	ISequenceSource[] GetTimelineData(IEnumerable<ISequenceSource> sequenceSources, DateTime beginTime, TimeSpan interval, int count);

	IEnumerable<Sequence> GetSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime);

	void SetSequenceSources(IEnumerable<ISequenceSource> sequenceSources);

	void InvalidateSequences(DateTime fromTime, DateTime toTime);

	void InvalidateSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime);

	void InvalidateSequences(IEnumerable<Guid> sourceIds, DateTime fromTime, DateTime toTime);
}
