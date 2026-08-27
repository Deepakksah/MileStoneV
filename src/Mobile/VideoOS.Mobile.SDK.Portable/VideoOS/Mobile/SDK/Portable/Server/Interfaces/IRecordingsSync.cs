using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IRecordingsSync
{
	GetSequencesResponse GetSequences(SequenceParams sequenceParams, TimeSpan timeout);

	GetRecordingTimeResponse GetRecordingTime(PlaybackParams playbackParams, SequenceParams sequenceParams, TimeSpan timeout);
}
