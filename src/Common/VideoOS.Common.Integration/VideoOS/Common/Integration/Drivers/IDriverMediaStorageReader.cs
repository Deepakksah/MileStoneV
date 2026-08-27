using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IDriverMediaStorageReader : IDisposable
{
	bool IsConnected { get; }

	bool MoveTo(DateTime dateTime, RequestMediaSearchCriteria mediaSearchCriteria);

	bool MoveFirst();

	bool MoveLast();

	bool MoveNext();

	bool MovePrevious();

	bool MoveNextSequence();

	bool MovePreviousSequence();

	DriverBrowseMediaResponse ReadMediaData();

	DriverSequenceEntry[] GetSequences(DriverRequestSequenceType requestSequenceType, DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter);
}
