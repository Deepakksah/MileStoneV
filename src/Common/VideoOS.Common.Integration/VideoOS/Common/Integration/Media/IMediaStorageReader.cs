using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Media;

public interface IMediaStorageReader : IDisposable
{
	bool Connected { get; }

	bool MoveTo(DateTime dt, RequestMediaSearchCriteria mediaSearchCriteria);

	bool MoveFirst();

	bool MoveLast();

	bool MoveNext();

	bool MovePrevious();

	bool MoveNextSequence();

	bool MovePreviousSequence();

	IMediaResponse ReadMediaData();

	GenericGop GetGop();

	DateTime FindEarliestMediaTime();

	List<SequenceEntry> GetSequences(RequestMediaDataType mediaDataType, RequestMediaSearchCriteria mediaSearchCriteria, DateTime minTime, DateTime maxTime, int maxCount);

	void SetPreferredTrack(Guid trackDefinitionId);
}
