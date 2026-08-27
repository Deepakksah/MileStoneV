using System;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class StreamPtz : Ptz, IStreamPtzAsync, IPtzAsync, IStreamPtzSync, IPtzSync, IStreamPtzAwait, IPtzAwait
{
	public StreamPtz(IConnection connection, Guid videoId)
		: base(connection, videoId)
	{
	}
}
