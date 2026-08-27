using System;
using VideoOS.Mobile.Portable.Interfaces;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public interface IAudioResampler : IControl, IDisposable
{
	Action<byte[]> OnTransformedData { get; set; }

	void OnInputData(byte[] input);

	void SetOutputSampleRate(int outputRate);
}
