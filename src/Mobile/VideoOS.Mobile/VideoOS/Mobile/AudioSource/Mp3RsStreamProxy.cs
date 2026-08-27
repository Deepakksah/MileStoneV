using System.Collections.Generic;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel;
using VideoOS.Mobile.VideoChannel.Encoding;
using VideoOS.Mobile.VideoSource;

namespace VideoOS.Mobile.AudioSource;

public class Mp3RsStreamProxy : RsStreamProxy<AudioData>, IControl
{
	private AudioEncoder _audioEncoder;

	public override OnStreamData<AudioData> OnData { get; set; }

	public Mp3RsStreamProxy(IRsStream<AudioData> proxy, IAudioEncoderSettingsExt audioEncoderSettingsExt)
		: this(audioEncoderSettingsExt)
	{
		SetSource(proxy);
	}

	public Mp3RsStreamProxy(IAudioEncoderSettingsExt audioEncoderSettingsExt)
	{
		_audioEncoder = new AudioEncoder(audioEncoderSettingsExt);
		_audioEncoder.OnTransformedData = OnEncodedData;
	}

	public override void SetSource(IRsStream<AudioData> proxy)
	{
		base.SetSource(proxy);
		proxy.OnData = OnProxyData;
	}

	private void OnProxyData(IEnumerable<AudioData> data)
	{
		if (data == null)
		{
			return;
		}
		foreach (AudioData datum in data)
		{
			_audioEncoder.OnInputData(datum.Content);
		}
	}

	private void OnEncodedData(byte[] encodedBytes)
	{
		OnData?.Invoke(new AudioData[1]
		{
			new AudioData(encodedBytes)
		});
	}

	public void Start()
	{
		_audioEncoder.Start();
	}

	public void Stop()
	{
		_audioEncoder?.Stop();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		Disposing.DisposeMember(ref _audioEncoder);
	}
}
