using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IRecordingsAsync
{
	IAsyncResult GetSequences(SequenceParams sequenceParams, OnResponse<GetSequencesResponse> onSuccess = null, OnResponse<GetSequencesResponse> onFail = null);

	IAsyncResult GetRecordingTime(PlaybackParams playbackParams, SequenceParams sequenceParams, OnResponse<GetRecordingTimeResponse> onSuccess = null, OnResponse<GetRecordingTimeResponse> onFail = null);
}
