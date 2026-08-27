using System;
using VideoOS.Platform.Util.Error;

namespace VideoOS.Platform.Data.MediaDataHelpers;

public class AudioRecorderToMediaData
{
	private readonly IAudioRecorder _audioRecorder;

	private readonly AudioMediaDataQueue _audioQueue = new AudioMediaDataQueue();

	public bool InIdle
	{
		get
		{
			return !_audioRecorder.IsRecording();
		}
		set
		{
			if (!value)
			{
				_audioQueue.Reset();
				_audioRecorder.StartRecording();
			}
			else
			{
				_audioRecorder.StopRecording();
			}
		}
	}

	public IErrorHandler<Exception> ErrorHandler => _audioQueue.ErrorHandler;

	public AudioRecorderToMediaData(IAudioRecorder audioRecorder)
	{
		_audioRecorder = audioRecorder;
		_audioRecorder.Initialize();
		_audioRecorder.AudioDataReadyEvent += AudioRecorderOnAudioDataReadyEvent;
	}

	private void AudioRecorderOnAudioDataReadyEvent(object sender, IAudioRecorder.AudioDataEventArgs args)
	{
		if (args?.AudioData != null)
		{
			MediaData mediaData = new MediaData
			{
				Bytes = args.AudioData
			};
			_audioQueue.OnData(mediaData);
		}
	}

	public MediaData GetMediaData(MediaProviderSession session)
	{
		return _audioQueue.GetMediaData(session);
	}

	public void Close()
	{
		_audioRecorder.AudioDataReadyEvent -= AudioRecorderOnAudioDataReadyEvent;
		_audioRecorder.StopRecording();
		_audioRecorder.Free();
	}
}
