using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VideoOS.Mobile.MP4Muxing.FMP4Parser;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.VideoChannel;
using VideoOS.Mobile.VideoChannel.MediaFoundation;
using VideoOS.Mobile.VideoChannel.Muxing.MediaFoundation.Classes;

namespace VideoOS.Mobile.VideoChannel.Muxing;

public class MfMp4Muxer : IMp4Muxer, IControl
{
	private IMFMediaSink _mediaSink;

	private ComStream _comStream;

	private IMFByteStream _byteStream;

	private IMFMediaType _mediaType;

	private IMFSinkWriter _sinkWriter;

	private ulong _startTime;

	private uint _fpsNumerator;

	private uint _fpsDenominator;

	private bool _fragmented;

	private bool _shouldJoinFragments;

	private readonly Dictionary<uint, IMp4MuxerCodecImpl> _codecImplementers = new Dictionary<uint, IMp4MuxerCodecImpl>();

	private static readonly byte[] _startCode = new byte[4] { 0, 0, 0, 1 };

	public MfMp4Muxer(bool fragmented, bool joinFragments, IReadOnlyCollection<IMp4MuxerCodecImpl> codecImplementers)
	{
		_fpsNumerator = 0u;
		_fpsDenominator = 0u;
		_startTime = 0uL;
		_fragmented = fragmented;
		_shouldJoinFragments = joinFragments;
		foreach (IMp4MuxerCodecImpl codecImplementer in codecImplementers)
		{
			_codecImplementers.Add(codecImplementer.CodecType, codecImplementer);
		}
	}

	private void SetDurations(uint lengthMs, uint frameCount)
	{
		try
		{
			MFHelper.MFAverageTimePerFrameToFrameRate(lengthMs / frameCount * 10000, out _fpsNumerator, out _fpsDenominator);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			throw;
		}
	}

	private void SetMediaType(IFmp4FragmentInfo fmp4FragmentInfo, byte[] spsPps)
	{
		_mediaType = MFHelper.MFCreateMediaType();
		_mediaType.SetGUID(Constants.MF_MT_MAJOR_TYPE, Constants.MFMediaType_Video);
		_mediaType.SetGUID(Constants.MF_MT_SUBTYPE, Constants.MFVideoFormat_H264);
		_mediaType.SetUINT32(Constants.MF_MT_INTERLACE_MODE, 2u);
		_mediaType.SetUINT64(Constants.MF_MT_FRAME_SIZE, MFHelper.Pack(fmp4FragmentInfo.CodecOutputParams.Width, fmp4FragmentInfo.CodecOutputParams.Height));
		_mediaType.SetUINT64(Constants.MF_MT_PIXEL_ASPECT_RATIO, MFHelper.Pack(1u, 1u));
		_mediaType.SetUINT64(Constants.MF_MT_FRAME_RATE, MFHelper.Pack(_fpsNumerator, _fpsDenominator));
		if (spsPps != null)
		{
			_mediaType.SetBlob(Constants.MF_MT_MPEG_SEQUENCE_HEADER, spsPps, (uint)spsPps.Length);
		}
	}

	private void CreateMp4MediaSink()
	{
		if (_comStream == null)
		{
			return;
		}
		try
		{
			_mediaSink = null;
			_byteStream = null;
			MFHelper.MFCreateMFByteStreamOnStream(_comStream, out _byteStream);
			_mediaSink = MFHelper.MFCreateMPEG4MediaSink(_byteStream, _mediaType, null, _fragmented);
		}
		catch (Exception)
		{
			_byteStream = null;
			throw;
		}
	}

	private IMFAttributes CreateSinkWriterAttributes()
	{
		MFHelper.MFCreateAttributes(out var attributes, 2u);
		Guid guidValue = (_fragmented ? Constants.MFTranscodeContainerType_FMPEG4 : Constants.MFTranscodeContainerType_MPEG4);
		attributes.SetGUID(Constants.MF_TRANSCODE_CONTAINERTYPE, guidValue);
		attributes.SetUINT32(Constants.MF_READWRITE_ENABLE_HARDWARE_TRANSFORMS, 1u);
		return attributes;
	}

	private void CreateSinkWriter()
	{
		if (_mediaSink != null)
		{
			_sinkWriter = MFHelper.MFCreateSinkWriterFromMediaSink(_mediaSink, CreateSinkWriterAttributes());
		}
	}

	private void ProcessSamples(IReadOnlyList<GbdVideoData> pFrames)
	{
		if (_sinkWriter == null)
		{
			return;
		}
		_startTime = 0uL;
		_sinkWriter.BeginWriting();
		foreach (GbdVideoData pFrame in pFrames)
		{
			if (pFrame != null)
			{
				IMFMediaBuffer pBuffer = MediaBufferHelper.CreateMediaBuffer(pFrame.ByteBuffer);
				GC.AddMemoryPressure(pFrame.ByteBuffer.lenWritten);
				IMFSample iMFSample = MFHelper.MFCreateSample();
				ulong num = (ulong)pFrame.Duration * 10000uL;
				iMFSample.AddBuffer(pBuffer);
				iMFSample.SetSampleTime(_startTime);
				iMFSample.SetSampleDuration(num);
				if (pFrame.IsSyncFrame)
				{
					iMFSample.SetUINT32(Constants.MFSampleExtension_CleanPoint, 1u);
				}
				else
				{
					iMFSample.SetUINT32(Constants.MFSampleExtension_CleanPoint, 0u);
				}
				_sinkWriter.WriteSample(0u, iMFSample);
				GC.RemoveMemoryPressure(pFrame.ByteBuffer.lenWritten);
				_startTime += num;
			}
		}
		_sinkWriter.DoFinalize();
	}

	private void WriteOutput(out byte[] content)
	{
		content = new byte[_comStream.Length];
		_comStream.Position = 0L;
		_comStream.Read(content, 0, (int)_comStream.Length);
	}

	private byte[] ExtractSpsPps(GbdVideoData firstFrame, IFmp4FragmentInfo fmp4FragmentInfo)
	{
		SampleEntry sampleEntry = _codecImplementers[fmp4FragmentInfo.CodecType].CreateVisualSampleEntry(firstFrame, fmp4FragmentInfo.CodecOutputParams);
		if (sampleEntry == null || !sampleEntry.Children.Any())
		{
			return null;
		}
		AvcDecoderConfigurationBox obj = sampleEntry.Children[0] as AvcDecoderConfigurationBox;
		byte[] spsData = obj.SpsData;
		byte[] ppsData = obj.PpsData;
		return _startCode.Concat(spsData).Concat(_startCode).Concat(ppsData)
			.ToArray();
	}

	public byte[] Mux(IReadOnlyList<GbdVideoData> h264Input, Fmp4FragmentInfo fmp4FragmentInfo)
	{
		if (!_codecImplementers.ContainsKey(fmp4FragmentInfo.CodecType))
		{
			return null;
		}
		byte[] spsPps = ExtractSpsPps(h264Input[0], fmp4FragmentInfo);
		byte[] array = MuxContentInternal(h264Input, fmp4FragmentInfo, spsPps);
		if (array == null)
		{
			return null;
		}
		Fmp4FragmentInfoParser.Do(array, fmp4FragmentInfo);
		if (_fragmented)
		{
			array = Fmp4Remuxer.Do(array, _shouldJoinFragments);
		}
		return array;
	}

	private byte[] MuxContentInternal(IReadOnlyList<GbdVideoData> h264Input, IFmp4FragmentInfo fmp4FragmentInfo, byte[] spsPps)
	{
		byte[] content = null;
		MemoryStream stream = new MemoryStream();
		_comStream = new ComStream(stream);
		SetDurations((uint)fmp4FragmentInfo.WholeSegmentDuration.TotalMilliseconds, (uint)h264Input.Count);
		SetMediaType(fmp4FragmentInfo, spsPps);
		CreateMp4MediaSink();
		CreateSinkWriter();
		ProcessSamples(h264Input);
		WriteOutput(out content);
		Release();
		return content;
	}

	private void Release()
	{
		if (_mediaSink != null)
		{
			_mediaSink.Shutdown();
			_mediaSink = null;
		}
		if (_comStream != null)
		{
			_comStream.Flush();
			_comStream = null;
		}
		if (_byteStream != null)
		{
			_byteStream.Flush();
			_byteStream = null;
		}
		_sinkWriter = null;
	}

	public void Start()
	{
		MfLib.Instance.StartMfNoSockets();
	}

	public void Stop()
	{
		MfLib.Instance.ShutdownMf();
	}
}
