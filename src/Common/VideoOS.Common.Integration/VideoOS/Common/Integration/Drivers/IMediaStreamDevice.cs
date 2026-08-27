using System;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Drivers;

public interface IMediaStreamDevice
{
	bool StableMediaFeedExpected { get; }

	event EventHandler<StreamEventArgs> StreamStateEvent;

	IMediaStream CreateStream(StreamHwid streamHwid, string deviceName, string streamName);

	void ReleaseStream(StreamHwid streamHwid);

	StreamState GetCombinedStreamState();

	bool EdgeStorageSupported();

	IEdgeMediaStream CreateEdgeStream(int deviceIndex, int streamIndex);
}
