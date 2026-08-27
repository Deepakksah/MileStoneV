using System;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Drivers;

public interface IEdgeMediaStream
{
	bool OpenStream(int mediaType);

	void CloseStream();

	bool Retrieve(DateTime utcStartTime, DateTime utcEndTime);

	RetrieveStatus RetrieveStatus();

	ReadOnlySharedBuffer GetNextFrame();
}
