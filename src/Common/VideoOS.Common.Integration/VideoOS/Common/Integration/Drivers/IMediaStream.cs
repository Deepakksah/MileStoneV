using System;
using VideoOS.Common.Integration.Definition;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Drivers;

public interface IMediaStream
{
	StreamHwid StreamHwid { get; }

	StreamState State { get; }

	double SettingsFps { get; }

	double KeyframeInterval { get; }

	event EventHandler<StreamEventArgs> StreamStateEvent;

	event EventHandler<DriverEventArgs> DeviceDriverEvent;

	void StartFeed();

	void StopFeed();

	ReadOnlySharedBuffer GetNextFrame();

	void ReleaseFrameThread();

	void FlushStream(bool waitForData);
}
