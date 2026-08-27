using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ConnectParams
{
	private Guid _deviceId;

	private string _publicKey;

	private PrimeLength _primeLength;

	private EncodingPadding _encodingPadding;

	private bool _requireCommandEncryption;

	private uint _paramMask;

	public uint ParamMask => _paramMask;

	public string PublicKey
	{
		get
		{
			return _publicKey;
		}
		set
		{
			_publicKey = value;
			_paramMask |= 2u;
		}
	}

	public Guid DeviceId
	{
		get
		{
			return _deviceId;
		}
		set
		{
			_deviceId = value;
			_paramMask |= 1u;
		}
	}

	public bool PeriodicLiveMessage { get; set; }

	public PrimeLength PrimeLength
	{
		get
		{
			return _primeLength;
		}
		set
		{
			_primeLength = value;
			_paramMask |= 8u;
		}
	}

	public bool RequireCommandEncryption
	{
		get
		{
			return _requireCommandEncryption;
		}
		set
		{
			_requireCommandEncryption = value;
			_paramMask |= 16u;
		}
	}

	public EncodingPadding EncodingPadding
	{
		get
		{
			return _encodingPadding;
		}
		set
		{
			_encodingPadding = value;
			_paramMask |= 4u;
		}
	}

	public ConnectParams()
	{
		InternalInit();
	}

	public ConnectParams(Command tCommand)
	{
		InternalInit();
		InitFromCommand(tCommand);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_deviceId = Guid.Empty;
		_publicKey = string.Empty;
		_requireCommandEncryption = false;
		_primeLength = PrimeLength.Prime1024;
		_encodingPadding = EncodingPadding.Pkcs7;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, ConnectParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public ConnectParams InitFromCommand(Command tCommand, uint unMaskToCheck = 31u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "PublicKey", ref _publicKey, ConnectParamsHelper.ParamMask.PublicKey, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "DeviceId", ref _deviceId, ConnectParamsHelper.ParamMask.DeviceId, ParamsHelper.GuidConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "RequireCommandEncryption", ref _requireCommandEncryption, ConnectParamsHelper.ParamMask.RequireCommandEncryption, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "EncryptionPadding", ref _encodingPadding, ConnectParamsHelper.ParamMask.EncryptionPadding, EnumsHelper.EnumConverter<EncodingPadding>, unMaskToCheck);
			ConvertParameter(dictionary, "PrimeLength", ref _primeLength, ConnectParamsHelper.ParamMask.PrimeLength, EnumsHelper.EnumConverter<PrimeLength>, unMaskToCheck);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ConnectParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, ConnectParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public ConnectParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "DeviceId", _deviceId, ConnectParamsHelper.ParamMask.DeviceId);
			ConvertParameterTo(dictionary, "PublicKey", _publicKey, ConnectParamsHelper.ParamMask.PublicKey);
			ConvertParameterTo(dictionary, "RequireCommandEncryption", _requireCommandEncryption, ConnectParamsHelper.ParamMask.RequireCommandEncryption, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "EncryptionPadding", _encodingPadding, ConnectParamsHelper.ParamMask.EncryptionPadding, (EncodingPadding x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "PrimeLength", _primeLength, ConnectParamsHelper.ParamMask.PrimeLength, (PrimeLength x) => EnumsHelper.GetDescription(x));
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ConnectParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}

	public bool CheckForMask(ConnectParamsHelper.ParamMask maskToCheck)
	{
		return (ParamMask & (uint)maskToCheck) == (uint)maskToCheck;
	}
}
