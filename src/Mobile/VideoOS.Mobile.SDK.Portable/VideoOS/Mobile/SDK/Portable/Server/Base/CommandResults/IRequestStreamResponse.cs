using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public interface IRequestStreamResponse
{
	Guid VideoId { get; }

	StreamParamsHelper.ByteOrder ByteOrder { get; }
}
