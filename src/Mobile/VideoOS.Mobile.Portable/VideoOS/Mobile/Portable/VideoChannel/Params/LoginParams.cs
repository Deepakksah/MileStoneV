using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class LoginParams
{
	private bool _processingMessages;

	private bool _supportsResampling;

	private bool _extendedResampling;

	private bool _supportsCarousel;

	private bool _supportsFootages;

	private bool _supportsAudioIn;

	private bool _supportsAudioOut;

	private bool _supportsSpeakerAudioIn;

	private bool _supportsAdaptiveStreaming;

	private bool _supportsSmartMap;

	private uint _paramMask;

	public uint ParamMask => _paramMask;

	public bool ProcessingMessages
	{
		get
		{
			return _processingMessages;
		}
		set
		{
			_processingMessages = value;
			_paramMask |= 1u;
		}
	}

	public bool SupportsResampling
	{
		get
		{
			return _supportsResampling;
		}
		set
		{
			_supportsResampling = value;
			_paramMask |= 2u;
		}
	}

	public bool ExtendedResampling
	{
		get
		{
			return _extendedResampling;
		}
		set
		{
			_extendedResampling = value;
			_paramMask |= 4u;
		}
	}

	public bool SupportsCarousel
	{
		get
		{
			return _supportsCarousel;
		}
		set
		{
			_supportsCarousel = value;
			_paramMask |= 8u;
		}
	}

	public bool SupportsFootages
	{
		get
		{
			return _supportsFootages;
		}
		set
		{
			_supportsFootages = value;
			_paramMask |= 32u;
		}
	}

	public bool SupportsAudioIn
	{
		get
		{
			return _supportsAudioIn;
		}
		set
		{
			_supportsAudioIn = value;
			_paramMask |= 64u;
		}
	}

	public bool SupportsAudioOut
	{
		get
		{
			return _supportsAudioOut;
		}
		set
		{
			_supportsAudioOut = value;
			_paramMask |= 128u;
		}
	}

	public bool SupportsSpeakerAudioIn
	{
		get
		{
			return _supportsSpeakerAudioIn;
		}
		set
		{
			_supportsSpeakerAudioIn = value;
			_paramMask |= 256u;
		}
	}

	public bool SupportsAdaptiveStreaming
	{
		get
		{
			return _supportsAdaptiveStreaming;
		}
		set
		{
			_supportsAdaptiveStreaming = value;
			_paramMask |= 512u;
		}
	}

	public bool SupportsSmartMap
	{
		get
		{
			return _supportsSmartMap;
		}
		set
		{
			_supportsSmartMap = value;
			_paramMask |= 2048u;
		}
	}

	public LoginParams()
	{
		InternalInit();
	}

	public LoginParams(Command tCommand)
	{
		InternalInit();
		InitFromCommand(tCommand);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_processingMessages = true;
		_supportsResampling = false;
		_extendedResampling = false;
		_supportsCarousel = false;
		_supportsFootages = false;
		_supportsAudioIn = false;
		_supportsAudioOut = false;
		_supportsSpeakerAudioIn = false;
		_supportsAdaptiveStreaming = false;
		_supportsSmartMap = false;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, LoginParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public LoginParams InitFromCommand(Command tCommand, uint unMaskToCheck = 65535u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "ProcessingMessage", ref _processingMessages, LoginParamsHelper.ParamMask.ProcessingMessage, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SupportsResampling", ref _supportsResampling, LoginParamsHelper.ParamMask.SupportsResampling, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SupportsExtendedResamplingFactor", ref _extendedResampling, LoginParamsHelper.ParamMask.ExtendedResampling, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SupportsCarousel", ref _supportsCarousel, LoginParamsHelper.ParamMask.SupportsCarousel, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "Footages", ref _supportsFootages, LoginParamsHelper.ParamMask.SupportsFootages, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SupportsAudioIn", ref _supportsAudioIn, LoginParamsHelper.ParamMask.SupportsAudioIn, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SupportsAudioOut", ref _supportsAudioOut, LoginParamsHelper.ParamMask.SupportsAudioOut, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SupportsSpeakerAudioIn", ref _supportsSpeakerAudioIn, LoginParamsHelper.ParamMask.SupportsSpeakerAudioIn, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SupportsAdaptiveStreaming", ref _supportsAdaptiveStreaming, LoginParamsHelper.ParamMask.SupportsAdaptiveStreaming, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SupportsSmartMap", ref _supportsSmartMap, LoginParamsHelper.ParamMask.SupportsSmartMap, ParamsHelper.BoolConvertion, unMaskToCheck);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "LoginParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, LoginParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public LoginParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "ProcessingMessage", _processingMessages, LoginParamsHelper.ParamMask.ProcessingMessage, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SupportsResampling", _supportsResampling, LoginParamsHelper.ParamMask.SupportsResampling, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SupportsExtendedResamplingFactor", _extendedResampling, LoginParamsHelper.ParamMask.ExtendedResampling, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SupportsCarousel", _supportsCarousel, LoginParamsHelper.ParamMask.SupportsCarousel, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "Footages", _supportsFootages, LoginParamsHelper.ParamMask.SupportsFootages, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SupportsAudioIn", _supportsAudioIn, LoginParamsHelper.ParamMask.SupportsAudioIn, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SupportsAudioOut", _supportsAudioOut, LoginParamsHelper.ParamMask.SupportsAudioOut, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SupportsSpeakerAudioIn", _supportsSpeakerAudioIn, LoginParamsHelper.ParamMask.SupportsSpeakerAudioIn, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SupportsAdaptiveStreaming", _supportsAdaptiveStreaming, LoginParamsHelper.ParamMask.SupportsAdaptiveStreaming, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SupportsSmartMap", _supportsSmartMap, LoginParamsHelper.ParamMask.SupportsSmartMap, ParamsHelper.BoolConvertionTo);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "LoginParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
