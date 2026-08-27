using System;
using System.Runtime.InteropServices;

namespace VideoOS.DatabaseUtility;

public interface IDatabaseMetaData : IDisposable
{
	[return: MarshalAs(UnmanagedType.U1)]
	bool Open();

	[return: MarshalAs(UnmanagedType.U1)]
	bool IsOpen();

	[return: MarshalAs(UnmanagedType.U1)]
	bool Close();

	TimelineResponse[] GetTimeLineInformation(MediaType mediaType, TimeLineInformationType activityType, DateTime beginTime, TimeSpan resolution, int count);

	SequencesResponse[] SequencesGetAroundWithSpan(SequenceType sequenceType, DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter);
}
