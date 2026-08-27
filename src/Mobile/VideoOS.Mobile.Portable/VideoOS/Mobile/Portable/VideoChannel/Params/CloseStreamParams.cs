using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CloseStreamParams
{
	private readonly List<Guid> _videoIds = new List<Guid>();

	public IEnumerable<Guid> VideoIds
	{
		get
		{
			return _videoIds;
		}
		set
		{
			foreach (Guid item in value)
			{
				_videoIds.Add(item);
			}
		}
	}

	public CloseStreamParams()
	{
	}

	public CloseStreamParams(Command tCommand)
	{
		InitFromCommand(tCommand);
	}

	public CloseStreamParams InitFromCommand(Command tCommand)
	{
		try
		{
			foreach (Parameter inputParam in tCommand.InputParams)
			{
				if (inputParam.ParamName == "VideoId")
				{
					try
					{
						_videoIds.Add(new Guid(inputParam.ParamValue));
					}
					catch (Exception ex)
					{
						Logger.Instance.Log(LogContent.Error, "CloseStreamParams.InitFromCommand - (0)", ex.Message);
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Logger.Instance.Log(LogContent.Error, "CloseStreamParams.InitFromCommand", ex2.Message);
		}
		return this;
	}

	public CloseStreamParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			foreach (Guid videoId in _videoIds)
			{
				tCommand.InputParams.Add(new Parameter("VideoId", videoId.ToString()));
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CloseStreamParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckParams()
	{
		if (_videoIds.Count > 0)
		{
			return ErrorCodes.Ok;
		}
		return ErrorCodes.MissingInputParameter;
	}
}
