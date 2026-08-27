using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Drivers;

namespace VideoOS.Common.Integration.Media;

public sealed class DriverMediaStorageReader : IMediaStorageReader, IDisposable
{
	private IDriverMediaStorageReader driverMediaStorageReader;

	private IConnectionStatus connectionStatus;

	bool IMediaStorageReader.Connected => connectionStatus.Connected;

	public DriverMediaStorageReader(IDriverMediaStorageReader driverMediaStorageReader, IConnectionStatus connectionStatus)
	{
		this.driverMediaStorageReader = driverMediaStorageReader;
		this.connectionStatus = connectionStatus;
	}

	bool IMediaStorageReader.MoveTo(DateTime dt, RequestMediaSearchCriteria mediaSearchCriteria)
	{
		return driverMediaStorageReader.MoveTo(dt, mediaSearchCriteria);
	}

	bool IMediaStorageReader.MoveFirst()
	{
		return driverMediaStorageReader.MoveFirst();
	}

	bool IMediaStorageReader.MoveLast()
	{
		return driverMediaStorageReader.MoveLast();
	}

	bool IMediaStorageReader.MoveNext()
	{
		return driverMediaStorageReader.MoveNext();
	}

	bool IMediaStorageReader.MovePrevious()
	{
		return driverMediaStorageReader.MovePrevious();
	}

	bool IMediaStorageReader.MoveNextSequence()
	{
		return driverMediaStorageReader.MoveNextSequence();
	}

	bool IMediaStorageReader.MovePreviousSequence()
	{
		return driverMediaStorageReader.MovePreviousSequence();
	}

	void IMediaStorageReader.SetPreferredTrack(Guid trackDefinitionId)
	{
	}

	private MediaDrvResponse ReadMedia()
	{
		MediaDrvResponse mediaDrvResponse = null;
		DriverBrowseMediaResponse driverBrowseMediaResponse = driverMediaStorageReader.ReadMediaData();
		if (driverBrowseMediaResponse != null)
		{
			bool[] array = new bool[driverBrowseMediaResponse.FrameResponses.Length];
			List<GenericFrame> list = new List<GenericFrame>(driverBrowseMediaResponse.FrameResponses.Length);
			for (int i = 0; i < driverBrowseMediaResponse.FrameResponses.Length; i++)
			{
				DriverBrowseFrameResponse driverBrowseFrameResponse = driverBrowseMediaResponse.FrameResponses[i];
				list.Add(new GenericFrame(driverBrowseFrameResponse.Data, driverBrowseFrameResponse.Time));
				array[i] = driverBrowseFrameResponse.Motion.HasValue && driverBrowseFrameResponse.Motion == true;
			}
			mediaDrvResponse = new MediaDrvResponse();
			mediaDrvResponse.Gop = new GenericGop(list.ToArray());
			mediaDrvResponse.Gop.IncrementReferenceCount();
			TimeSlice dtPrev = TimeSlice.Invalid;
			if (driverBrowseMediaResponse.DateTimePrevious != mediaDrvResponse.Gop.BeginTimestamp)
			{
				DateTime endTimestamp = ((mediaDrvResponse.Gop.FrameCount == 1) ? driverBrowseMediaResponse.DateTimePrevious : mediaDrvResponse.Gop.BeginTimestamp.AddMilliseconds(-1.0));
				dtPrev = new TimeSlice(driverBrowseMediaResponse.DateTimePrevious, endTimestamp);
			}
			mediaDrvResponse.DtPrev = dtPrev;
			dtPrev = TimeSlice.Invalid;
			if (driverBrowseMediaResponse.DateTimeNext != mediaDrvResponse.Gop.BeginTimestamp)
			{
				DateTime endTimestamp2 = ((mediaDrvResponse.Gop.FrameCount == 1) ? driverBrowseMediaResponse.DateTimeNext : driverBrowseMediaResponse.DateTimeNext.AddMilliseconds(1.0));
				dtPrev = new TimeSlice(driverBrowseMediaResponse.DateTimeNext, endTimestamp2);
			}
			mediaDrvResponse.DtNext = dtPrev;
			mediaDrvResponse.RecordSeqNo = driverBrowseMediaResponse.RecordSeqNo;
			if (driverBrowseMediaResponse.FrameResponses[0].Motion.HasValue)
			{
				mediaDrvResponse.MotionPrFrame = array;
			}
			if (driverBrowseMediaResponse.Signature != null)
			{
				mediaDrvResponse.Signature = driverBrowseMediaResponse.Signature;
			}
		}
		return mediaDrvResponse;
	}

	IMediaResponse IMediaStorageReader.ReadMediaData()
	{
		return ReadMedia();
	}

	GenericGop IMediaStorageReader.GetGop()
	{
		GenericGop result = null;
		MediaDrvResponse mediaDrvResponse = ReadMedia();
		if (mediaDrvResponse != null)
		{
			using (mediaDrvResponse)
			{
				result = mediaDrvResponse.Gop;
				mediaDrvResponse.Gop = null;
			}
		}
		return result;
	}

	public DateTime FindEarliestMediaTime()
	{
		DateTime result = DateTime.MinValue;
		if (((IMediaStorageReader)this).MoveFirst())
		{
			using GenericGop genericGop = ((IMediaStorageReader)this).GetGop();
			if (genericGop != null)
			{
				result = genericGop.BeginTimestamp;
			}
		}
		else if (driverMediaStorageReader.IsConnected)
		{
			result = DateTime.UtcNow;
		}
		return result;
	}

	List<SequenceEntry> IMediaStorageReader.GetSequences(RequestMediaDataType mediaDataType, RequestMediaSearchCriteria mediaSearchCriteria, DateTime minTime, DateTime maxTime, int maxCount)
	{
		DriverRequestSequenceType requestSequenceType = DriverRequestSequenceType.Recording;
		switch (mediaDataType)
		{
		case RequestMediaDataType.Recording:
			requestSequenceType = DriverRequestSequenceType.Recording;
			break;
		case RequestMediaDataType.Motion:
			requestSequenceType = DriverRequestSequenceType.Motion;
			break;
		}
		bool flag = false;
		switch (mediaSearchCriteria)
		{
		case RequestMediaSearchCriteria.Before:
			flag = false;
			break;
		case RequestMediaSearchCriteria.AtOrBefore:
			flag = false;
			break;
		case RequestMediaSearchCriteria.After:
			flag = true;
			break;
		case RequestMediaSearchCriteria.AtOrAfter:
			flag = true;
			break;
		}
		TimeSpan timeSpan = maxTime - minTime;
		int num = ((maxCount < int.MaxValue) ? (maxCount + 1) : maxCount);
		DateTime searchTime;
		TimeSpan maxTimeBefore;
		int maxCountBefore;
		TimeSpan maxTimeAfter;
		int maxCountAfter;
		if (flag)
		{
			searchTime = minTime;
			maxTimeBefore = TimeSpan.Zero;
			maxCountBefore = 0;
			maxTimeAfter = timeSpan;
			maxCountAfter = num;
		}
		else
		{
			searchTime = maxTime;
			maxTimeBefore = timeSpan;
			maxCountBefore = num;
			maxTimeAfter = TimeSpan.Zero;
			maxCountAfter = 0;
		}
		List<DriverSequenceEntry> list = new List<DriverSequenceEntry>(from e in driverMediaStorageReader.GetSequences(requestSequenceType, searchTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter)
			where e.StartTime <= e.EndTime && e.EndTime < DateTime.MaxValue
			select e);
		List<DriverSequenceEntry> list2 = null;
		switch (mediaSearchCriteria)
		{
		case RequestMediaSearchCriteria.Before:
			list2 = new List<DriverSequenceEntry>(list.Where((DriverSequenceEntry e) => e.EndTime < searchTime));
			break;
		case RequestMediaSearchCriteria.AtOrBefore:
			list2 = new List<DriverSequenceEntry>(list.Where((DriverSequenceEntry e) => e.StartTime < searchTime));
			break;
		case RequestMediaSearchCriteria.After:
			list2 = new List<DriverSequenceEntry>(list.Where((DriverSequenceEntry e) => searchTime < e.StartTime));
			break;
		case RequestMediaSearchCriteria.AtOrAfter:
			list2 = new List<DriverSequenceEntry>(list.Where((DriverSequenceEntry e) => searchTime < e.EndTime));
			break;
		}
		list.Sort(new DriverSequenceEntryComparer());
		while (list2.Count > maxCount)
		{
			if (flag)
			{
				list2.RemoveAt(list.Count - 1);
			}
			else
			{
				list2.RemoveAt(0);
			}
		}
		return new List<SequenceEntry>(list2.Select((DriverSequenceEntry s) => new SequenceEntry(s.StartTime, s.EndTime)));
	}

	public void Dispose()
	{
		if (driverMediaStorageReader != null)
		{
			driverMediaStorageReader.Dispose();
			driverMediaStorageReader = null;
		}
	}
}
