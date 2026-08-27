using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CarouselParams
{
	private CarouselParamsHelper.CarouselCommands _carouselCommand;

	private uint _paramMask;

	private Guid _videoId;

	public CarouselParamsHelper.CarouselCommands CarouselCommand
	{
		get
		{
			return _carouselCommand;
		}
		set
		{
			_carouselCommand = value;
			_paramMask |= 2u;
		}
	}

	public Guid VideoId
	{
		get
		{
			return _videoId;
		}
		set
		{
			_videoId = value;
			_paramMask |= 1u;
		}
	}

	public uint ParamMask => _paramMask;

	public CarouselParams()
	{
		InternalInit();
	}

	public CarouselParams(Command Command, uint unMaskToCheck = 3u)
	{
		InternalInit();
		InitFromCommand(Command, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_carouselCommand = CarouselParamsHelper.CarouselCommands.Unknown;
	}

	public CarouselParams InitFromCommand(Command command, uint unMaskToCheck = 3u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "CarouselCommand", ref _carouselCommand, CarouselParamsHelper.ParamMask.CarouselCommand, EnumsHelper.EnumConverter<CarouselParamsHelper.CarouselCommands>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "VideoId", ref _videoId, CarouselParamsHelper.ParamMask.VideoId, ParamsHelper.GuidConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CarouselParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	public CarouselParams ConvertToCommand(ref Command command)
	{
		try
		{
			command.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "VideoId", _videoId, CarouselParamsHelper.ParamMask.VideoId);
			ConvertParameterTo(dictionary, "CarouselCommand", _carouselCommand, CarouselParamsHelper.ParamMask.CarouselCommand);
			command.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CarouselParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, CarouselParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, CarouselParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public bool CheckForMask(CarouselParamsHelper.ParamMask mask)
	{
		return (ParamMask & (uint)mask) == (uint)mask;
	}
}
