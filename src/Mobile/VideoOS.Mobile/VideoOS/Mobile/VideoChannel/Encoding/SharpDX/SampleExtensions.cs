using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.MediaFoundation;
using VideoOS.Mobile.VideoChannel.MediaFoundation;
using VideoOS.Mobile.VideoChannel.Muxing.MediaFoundation.Classes;

namespace VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

public static class SampleExtensions
{
	public const int HudredNanoInS = 10000000;

	public static Sample GetMediaSample(byte[] dataBytes)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Sample val = MediaFactory.CreateSample();
		MediaBuffer val2 = new MediaBuffer(Marshal.GetComInterfaceForObject(MediaBufferHelper.CreateMediaBuffer(dataBytes), typeof(IMFMediaBuffer)));
		try
		{
			val.AddBuffer(val2);
			return val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static Sample GetMediaSample(byte[] dataBytes, MediaType mediaType)
	{
		Sample obj = MediaFactory.CreateSample();
		MediaBuffer val = default(MediaBuffer);
		MediaFactory.CreateMediaBufferFromMediaType(mediaType, (long)dataBytes.Length, dataBytes.Length, 1, ref val);
		MediaBufferHelper.PutDataIntoMediaBuffer(val, dataBytes);
		obj.AddBuffer(val);
		((DisposeBase)val).Dispose();
		return obj;
	}

	public static void SetAudioSampleProps(this Sample sample, IAudioSampleSettings audioEncoderSettings, ref long currentTime)
	{
		int num = 0;
		for (int i = 0; i < sample.BufferCount; i++)
		{
			MediaBuffer bufferByIndex = sample.GetBufferByIndex(i);
			try
			{
				num += bufferByIndex.CurrentLength;
			}
			finally
			{
				((IDisposable)bufferByIndex)?.Dispose();
			}
		}
		long num2 = (long)((float)(num / (audioEncoderSettings.ChannelsCount * audioEncoderSettings.BitsPerSample / 8)) / (float)audioEncoderSettings.SampleRate * 10000000f);
		sample.SampleTime = currentTime;
		sample.SampleDuration = num2;
		((MediaAttributes)sample).Set<int>(((MediaAttributeKey)SampleAttributeKeys.CleanPoint).Guid, 1);
		currentTime += num2;
	}

	public static void InitOutputMediaSample(out Sample outputSample, out TOutputDataBuffer outputBuffer)
	{
		InitOutputMediaSampleInternal(out outputSample, out outputBuffer, GetMediaSample);
	}

	public static void InitOutputMediaSample(out Sample outputSample, out TOutputDataBuffer outputBuffer, MediaType outMediaType)
	{
		InitOutputMediaSampleInternal(out outputSample, out outputBuffer, (byte[] data) => GetMediaSample(data, outMediaType));
	}

	private static void InitOutputMediaSampleInternal(out Sample outputSample, out TOutputDataBuffer outputBuffer, Func<byte[], Sample> sampleFactory)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		outputSample = sampleFactory(new byte[16384]);
		outputBuffer = default(TOutputDataBuffer);
		outputBuffer.DwStreamID = 0;
		outputBuffer.PSample = ((CppObject)outputSample).NativePointer;
		outputBuffer.DwStatus = 0;
		outputBuffer.PEvents = IntPtr.Zero;
	}
}
