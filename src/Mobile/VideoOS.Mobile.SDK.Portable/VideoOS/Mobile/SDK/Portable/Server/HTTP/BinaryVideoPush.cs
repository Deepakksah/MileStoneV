using System;
using System.IO;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.SDK.Portable.Server.Base;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class BinaryVideoPush<TData> where TData : PushStreamData, new()
{
	public delegate void OnCalculatedLength(long totalLength);

	private MainHeader _binaryHeader;

	private MainHeaderExt _binaryHeaderExt;

	private HeaderExtensionLocationInfo _binaryHeaderLocationInfo;

	private uint _framesCount;

	public BinaryVideoPush(Guid videoId)
	{
		_binaryHeader = default(MainHeader);
		_binaryHeader.VideoID = videoId;
		_binaryHeaderExt = default(MainHeaderExt);
		_binaryHeaderExt.HeaderVersion = 0;
		_binaryHeaderExt.VideoID = videoId;
		_binaryHeaderLocationInfo = default(HeaderExtensionLocationInfo);
		_framesCount = 0u;
	}

	public void Send(TData frame, Stream stream, OnCalculatedLength onCalculatedLength)
	{
		if (frame.FrameData == null)
		{
			frame.FrameData = new byte[0];
		}
		_binaryHeaderExt.TimeStampUtcMs = (ulong)DateTime.UtcNow.ToLong();
		_binaryHeaderExt.FrameCount = _framesCount++;
		_binaryHeaderExt.FrameSizeBytes = (uint)frame.FrameData.Count();
		_binaryHeaderExt.HeaderSizeBytes = 36u;
		_binaryHeaderExt.HeaderExtensionFlags = 0;
		bool flag = false;
		PushVideoFrame pushVideoFrame = frame as PushVideoFrame;
		if (pushVideoFrame != null)
		{
			flag = BinaryFrameGenerator.GenerateFromLocationInfo(ref _binaryHeaderLocationInfo, pushVideoFrame.LocationInfo);
			BinaryFrameGenerator.AddExtension(ref _binaryHeaderExt, flag, 32, 48u);
		}
		long totalLength = _binaryHeader.FrameSizeBytes + _binaryHeader.HeaderSizeBytes;
		onCalculatedLength?.Invoke(totalLength);
		byte[] array = null;
		if (_binaryHeaderExt.HeaderSizeBytes <= 65535)
		{
			BinaryExtensions.InitFromExtended(ref _binaryHeader, _binaryHeaderExt);
			array = BinaryFrameHeaderHelper.Instance.WriteStruct(_binaryHeader, 36, BinaryFrameInvertor.Invert);
		}
		else
		{
			array = BinaryFrameHeaderHelper.Instance.WriteStruct(_binaryHeaderExt, 40, BinaryFrameInvertor.Invert);
		}
		stream.Write(array, 0, array.Count());
		if (pushVideoFrame != null)
		{
			WriteStruct(flag, stream, _binaryHeaderLocationInfo, BinaryFrameInvertor.Invert, invert: false, 48);
		}
		stream.Write(frame.FrameData, 0, frame.FrameData.Count());
	}

	private static void WriteStruct<T>(bool condition, Stream stream, T tObject, BinaryFrameHeaderHelper.Inverter<T> inverter, bool invert, int sizeOfT)
	{
		WriteStruct(condition, stream, tObject, BinaryFrameHeaderHelper.Instance.WriteStruct, inverter, invert, sizeOfT);
	}

	private static void WriteStruct<T>(bool condition, Stream stream, T tObject, BinaryFrameHeaderHelper.Writer<T> writer, BinaryFrameHeaderHelper.Inverter<T> inverter, bool invert, int sizeOfT)
	{
		if (condition)
		{
			byte[] array = writer(tObject, sizeOfT, inverter, invert);
			stream.Write(array, 0, array.Count());
		}
	}
}
