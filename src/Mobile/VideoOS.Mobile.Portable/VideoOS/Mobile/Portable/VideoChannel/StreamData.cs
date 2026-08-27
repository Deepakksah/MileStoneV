using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.VideoChannel;

public class StreamData : RawData, IStreamData, IRawData
{
	public StreamParamsHelper.StreamType StreamType { get; set; }

	public DateTime RequestedTimeUTC { get; set; }

	public DateTime BeginTimeUtc { get; set; }

	public DateTime EndTimeUtc { get; set; }

	public DateTime PrevTimeUtc { get; set; }
}
