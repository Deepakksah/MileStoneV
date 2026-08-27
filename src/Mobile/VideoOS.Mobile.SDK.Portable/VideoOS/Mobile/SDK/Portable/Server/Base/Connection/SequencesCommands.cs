using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class SequencesCommands : IRecordingsSync, IRecordingsAsync
{
	private readonly IConnection _connection;

	public SequencesCommands(IConnection connection)
	{
		_connection = connection;
	}

	public GetSequencesResponse GetSequences(SequenceParams sequenceParams, TimeSpan timeout)
	{
		IAsyncResult sequences = GetSequences(sequenceParams);
		return new GetSequencesResponse(_connection.ProcessResult(sequences, timeout));
	}

	public GetRecordingTimeResponse GetRecordingTime(PlaybackParams playbackParams, SequenceParams sequenceParams, TimeSpan timeout)
	{
		IAsyncResult recordingTime = GetRecordingTime(playbackParams, sequenceParams);
		return new GetRecordingTimeResponse(_connection.ProcessResult(recordingTime, timeout));
	}

	public IAsyncResult GetSequences(SequenceParams sequenceParams, OnResponse<GetSequencesResponse> onSuccess = null, OnResponse<GetSequencesResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetSequences);
		sequenceParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetSequencesResponse(response));
	}

	public IAsyncResult GetRecordingTime(PlaybackParams playbackParams, SequenceParams sequenceParams, OnResponse<GetRecordingTimeResponse> onSuccess = null, OnResponse<GetRecordingTimeResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetRecordingTime);
		playbackParams.ConvertToCommand(ref tCommand);
		sequenceParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetRecordingTimeResponse(response));
	}
}
