using System;
using System.Collections.Generic;
using System.IO;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Audio;

public class PushToTalk : PushStream<AudioParams, MicrophoneCapabilities, AudioStreamControl, PushStreamData>
{
	public Action<Dictionary<Guid, int>> GetSpeakerState;

	public PushToTalk(RequestStreamResponseUpload response, IConnection iConnection, IOnVideoStream onVideoStream = null, string alias = "XProtectMobile/Audio")
		: base(response, iConnection, onVideoStream, alias)
	{
		_pushStreamSub.GetDeviceState = GetDeviceState;
	}

	public override AudioStreamControl CreateStreamControl(IConnection connection, Guid streamId)
	{
		return new AudioStreamControl(connection, streamId, null);
	}

	public void GetDeviceState(Stream responseStream)
	{
		byte[] array = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			responseStream.CopyTo(memoryStream);
			array = memoryStream.ToArray();
		}
		if (array.Length == 0)
		{
			return;
		}
		ByteBuffer buffer = new ByteBuffer(array, 0, array.Length);
		MainHeaderExt tHeader = FrameExtractor.ExtractMainHeader(buffer);
		HeadersExtractor headersExtractor = FrameExtractor.ExtractHeader(buffer, tHeader);
		Dictionary<Guid, int> dictionary = new Dictionary<Guid, int>();
		foreach (HeaderTypeDeviceStateInfo deviceStateInfo in headersExtractor.DeviceStateInfoList)
		{
			dictionary.Add(deviceStateInfo.ItemID, (int)deviceStateInfo.ErrorCode);
		}
		GetSpeakerState?.Invoke(dictionary);
	}
}
