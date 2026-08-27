using System;

namespace VideoOS.Platform;

public interface ISourceData
{
	DateTime DateTime { get; set; }

	bool IsNextAvailable { get; set; }

	DateTime NextDateTime { get; set; }

	bool IsPreviousAvailable { get; set; }

	DateTime PreviousDateTime { get; set; }
}
