using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.VideoChannel;

public interface IStreamData : IRawData
{
	StreamParamsHelper.StreamType StreamType { get; }

	DateTime RequestedTimeUTC { get; }

	DateTime BeginTimeUtc { get; }

	DateTime EndTimeUtc { get; }

	DateTime PrevTimeUtc { get; }
}
