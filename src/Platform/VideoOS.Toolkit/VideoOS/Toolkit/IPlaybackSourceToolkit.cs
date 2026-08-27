using System;

namespace VideoOS.Toolkit;

public interface IPlaybackSourceToolkit : ISourceToolkit
{
	public enum MoveStatus
	{
		MOVED_OK,
		NOT_MOVED
	}

	public enum SearchCriteria
	{
		NEAREST,
		BEFORE,
		AT_OR_BEFORE,
		AFTER,
		AT_OR_AFTER
	}

	public enum GetDataStatus
	{
		DATA_RETRIEVED,
		NO_DATA_AVAILABLE
	}

	MoveStatus MoveFirst();

	MoveStatus MoveLast();

	MoveStatus MoveNext();

	MoveStatus MovePrevious();

	MoveStatus MoveNextSequence();

	MoveStatus MovePreviousSequence();

	MoveStatus MoveTo(DateTime timeStamp, SearchCriteria searchCriteria);

	GetDataStatus GetData(out IData data);
}
