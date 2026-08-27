using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class SequenceDataSource : DataSource
{
	private static readonly ISet<Guid> SupportedKinds = new HashSet<Guid>
	{
		Kind.Camera,
		Kind.Metadata,
		Kind.Microphone,
		Kind.Speaker
	};

	public SequenceDataSource(Item item)
		: base(item)
	{
		if (item == null)
		{
			throw new MIPException("SequenceDataSource: item parameter cannot be null");
		}
		if (!SupportedKinds.Contains(item.FQID.Kind))
		{
			throw new ImplementationMIPException("Item kind not supported by SequenceDataSource");
		}
		new TrackPageViewData(typeof(SequenceDataSource)).Send();
	}

	public override List<DataType> GetTypes()
	{
		return (from sequenceType in InternalCommandService.Instance.SequencesGetTypes(Item.FQID)
			select new DataType
			{
				Id = sequenceType.Id,
				Name = sequenceType.Name
			}).ToList();
	}

	public override List<object> GetData(DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		return GetData(dateTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, DataType.SequenceTypeGuids.RecordingSequence);
	}

	public override List<object> GetData(DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid sequenceTypeGuid)
	{
		return GetData(dateTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, sequenceTypeGuid, throwExceptions: false);
	}

	public override List<object> GetData(DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid sequenceTypeGuid, bool throwExceptions)
	{
		SequenceEntry[] seqs = InternalCommandService.Instance.SequencesGetAroundWithSpan(Item.FQID, sequenceTypeGuid, dateTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, throwExceptions);
		return ConvertSequenceEntriesToSequenceData(seqs).Cast<object>().ToList();
	}

	public override void GetDataAsync(object asyncState, DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, AsyncCallbackHandler resultCallbackHandler)
	{
		InternalCommandService.Instance.AsyncSequencesGetAroundWithSpan(Item.FQID, new AsyncState
		{
			CallerAsyncState = asyncState,
			CallerCallbackHandler = resultCallbackHandler
		}, DataType.SequenceTypeGuids.RecordingSequence, dateTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, AsyncResultHandler);
	}

	public override void GetDataAsync(object asyncState, DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid typeOfData, AsyncCallbackHandler resultCallbackHandler)
	{
		InternalCommandService.Instance.AsyncSequencesGetAroundWithSpan(Item.FQID, new AsyncState
		{
			CallerAsyncState = asyncState,
			CallerCallbackHandler = resultCallbackHandler
		}, typeOfData, dateTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, AsyncResultHandler);
	}

	private void AsyncResultHandler(object result, object state)
	{
		if (result is SequenceEntry[] seqs)
		{
			SequenceData[] result2 = ConvertSequenceEntriesToSequenceData(seqs).ToArray();
			((AsyncState)state).CallerCallbackHandler(result2, ((AsyncState)state).CallerAsyncState);
		}
	}

	public override string Command(object objectData, string command, string[] args)
	{
		return "Not Supported";
	}

	private IEnumerable<SequenceData> ConvertSequenceEntriesToSequenceData(IEnumerable<SequenceEntry> seqs)
	{
		return from se in seqs
			let eventSource = new EventSource
			{
				FQID = Item.FQID,
				Name = Item.Name
			}
			let eventHeader = new EventHeader
			{
				Class = "Sequence",
				Timestamp = se.TimeTriggedField,
				ID = Guid.NewGuid(),
				Name = Item.Name,
				Source = eventSource
			}
			let eventSequence = new EventSequence
			{
				StartDateTime = se.TimeBeginField,
				EndDateTime = se.TimeEndField
			}
			select new SequenceData
			{
				EventHeader = eventHeader,
				EventSequence = eventSequence
			};
	}
}
