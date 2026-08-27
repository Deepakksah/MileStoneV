using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults.Processors;

public class ReconnectStreamAsyncResult : CommandAsyncResultBase
{
	private readonly IBaseStream _video;

	public ReconnectStreamAsyncResult(IConnection connection, IBaseStream video)
		: base(connection)
	{
		_video = video;
	}

	public override BaseCommandResponse OnCommand(Command request, Command response)
	{
		BaseCommandResponse baseCommandResponse = null;
		if (response.GetCommandKind() == Command.CommandKinds.RequestStream)
		{
			request.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = request.GetDictionary(Command.ParamsType.Input);
			string text = dictionary["SignalType"];
			EnumsHelper.GetDescription(StreamParamsHelper.StreamType.Transcoded);
			if (dictionary.ContainsKey("StreamType"))
			{
				_ = dictionary["StreamType"];
			}
			StreamParamsHelper.ByteOrder byteOrder = StreamParamsHelper.ByteOrder.LittleEndian;
			if (text == EnumsHelper.GetDescription(StreamParamsHelper.SignalType.Live))
			{
				RequestStreamResponseLive requestStreamResponseLive = new RequestStreamResponseLive(response, byteOrder);
				_video?.Initialize(requestStreamResponseLive);
				baseCommandResponse = requestStreamResponseLive;
			}
			else if (text == EnumsHelper.GetDescription(StreamParamsHelper.SignalType.Playback))
			{
				RequestStreamResponsePlayback requestStreamResponsePlayback = new RequestStreamResponsePlayback(response, byteOrder);
				_video?.Initialize(requestStreamResponsePlayback);
				baseCommandResponse = requestStreamResponsePlayback;
			}
			else
			{
				if (!(text == EnumsHelper.GetDescription(StreamParamsHelper.SignalType.Upload)))
				{
					throw new NotImplementedException();
				}
				RequestStreamResponseUpload requestStreamResponseUpload = new RequestStreamResponseUpload(response, byteOrder);
				(_video as IVideoUpload)?.Initialize(requestStreamResponseUpload);
				baseCommandResponse = requestStreamResponseUpload;
			}
		}
		else
		{
			baseCommandResponse = new BaseCommandResponse(response);
			Logger.Instance.Log(LogContent.Error, "ReconnectStreamAsyncResult.OnCommand", "Wrong processing response object attached to this command kind: " + response.GetCommandKind());
		}
		return baseCommandResponse;
	}
}
