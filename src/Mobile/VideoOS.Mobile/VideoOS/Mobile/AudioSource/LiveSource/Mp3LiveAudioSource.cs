using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.VideoChannel.Encoding;
using VideoOS.Platform;

namespace VideoOS.Mobile.AudioSource.LiveSource;

public class Mp3LiveAudioSource : BasePcmLiveAudioSource
{
	private AudioEncoder _audioEncoder;

	public Mp3LiveAudioSource(Item microphone, IAudioEncoderSettingsExt audioEncoderSettingsExt, IStreamSourceSettings streamSourceSettings)
		: base(microphone, audioEncoderSettingsExt.PcmSourceSettings, streamSourceSettings)
	{
		_audioEncoder = new AudioEncoder(audioEncoderSettingsExt);
		_audioEncoder.OnTransformedData = OnEncodedData;
	}

	private void OnEncodedData(byte[] bytes)
	{
		NotifySubscribers(bytes);
	}

	public override void Start()
	{
		_audioEncoder.Start();
		base.Start();
	}

	public override void Stop()
	{
		base.Stop();
		_audioEncoder?.Stop();
	}

	protected override void OnPcmData(byte[] pcmData)
	{
		_audioEncoder.OnInputData(pcmData);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		Disposing.DisposeMember(ref _audioEncoder);
	}
}
