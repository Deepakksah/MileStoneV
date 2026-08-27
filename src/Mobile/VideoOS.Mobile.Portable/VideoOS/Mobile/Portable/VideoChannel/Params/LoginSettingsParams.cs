using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class LoginSettingsParams
{
	private UserType _loginType;

	private string _username;

	private string _password;

	private string _idpClientId;

	private string _identityToken;

	private string _refreshToken;

	private string _accessToken;

	private string _cllientType;

	private uint _paramMask;

	public uint ParamMask => _paramMask;

	public UserType LoginType
	{
		get
		{
			return _loginType;
		}
		set
		{
			_loginType = value;
			_paramMask |= 1u;
		}
	}

	public string UserName
	{
		get
		{
			return _username;
		}
		set
		{
			_username = value;
			_paramMask |= 2u;
		}
	}

	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			_password = value;
			_paramMask |= 4u;
		}
	}

	public string IdpClientId
	{
		get
		{
			return _idpClientId;
		}
		set
		{
			_idpClientId = value;
			_paramMask |= 8u;
		}
	}

	public string IdentityToken
	{
		get
		{
			return _identityToken;
		}
		set
		{
			_identityToken = value;
			_paramMask |= 16u;
		}
	}

	public string AccessToken
	{
		get
		{
			return _accessToken;
		}
		set
		{
			_accessToken = value;
			_paramMask |= 32u;
		}
	}

	public string RefreshToken
	{
		get
		{
			return _refreshToken;
		}
		set
		{
			_refreshToken = value;
			_paramMask |= 64u;
		}
	}

	public string ClientType
	{
		get
		{
			return _cllientType;
		}
		set
		{
			_cllientType = value;
			_paramMask |= 128u;
		}
	}

	public LoginSettingsParams()
	{
		InternalInit();
	}

	public LoginSettingsParams(Command tCommand)
	{
		InternalInit();
		InitFromCommand(tCommand);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_loginType = UserType.Unknown;
		_username = string.Empty;
		_password = string.Empty;
		_identityToken = string.Empty;
		_accessToken = string.Empty;
		_refreshToken = string.Empty;
		_cllientType = ClientTypes.MobileClient;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, LoginSettingsParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	private void InitFromCommand(Command tCommand, uint unMaskToCheck = 65535u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "LoginType", ref _loginType, LoginSettingsParamsHelper.ParamMask.LoginType, EnumsHelper.EnumConverter<UserType>, unMaskToCheck);
			ConvertParameter(dictionary, "Username", ref _username, LoginSettingsParamsHelper.ParamMask.UserName, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "Password", ref _password, LoginSettingsParamsHelper.ParamMask.Password, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "IdpClientId", ref _idpClientId, LoginSettingsParamsHelper.ParamMask.IdpClientId, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "IdentityToken", ref _identityToken, LoginSettingsParamsHelper.ParamMask.IdentityToken, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "AccessToken", ref _accessToken, LoginSettingsParamsHelper.ParamMask.AccessToken, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "RefreshToken", ref _refreshToken, LoginSettingsParamsHelper.ParamMask.RefreshToken, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "ClientType", ref _cllientType, LoginSettingsParamsHelper.ParamMask.ClientType, ParamsHelper.StringConvertion, unMaskToCheck);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "LoginSettingsParams.InitFromCommand", ex.Message);
		}
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, LoginSettingsParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public void ConvertToCommand(ref Command tCommand, bool isEncrypted, DHEncryption encryption)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "LoginType", _loginType, LoginSettingsParamsHelper.ParamMask.LoginType, (UserType x) => EnumsHelper.GetDescription(x));
			string input = _username;
			string input2 = _password;
			if (isEncrypted)
			{
				input = encryption.Encrypt(_username);
				input2 = encryption.Encrypt(_password);
			}
			ConvertParameterTo(dictionary, "Username", input, LoginSettingsParamsHelper.ParamMask.UserName, ParamsHelper.StringConvertion);
			ConvertParameterTo(dictionary, "Password", input2, LoginSettingsParamsHelper.ParamMask.Password, ParamsHelper.StringConvertion);
			ConvertParameterTo(dictionary, "IdpClientId", _idpClientId, LoginSettingsParamsHelper.ParamMask.IdpClientId, ParamsHelper.StringConvertion);
			ConvertParameterTo(dictionary, "AccessToken", _accessToken, LoginSettingsParamsHelper.ParamMask.AccessToken, ParamsHelper.StringConvertion);
			ConvertParameterTo(dictionary, "IdentityToken", _identityToken, LoginSettingsParamsHelper.ParamMask.IdentityToken, ParamsHelper.StringConvertion);
			ConvertParameterTo(dictionary, "RefreshToken", _refreshToken, LoginSettingsParamsHelper.ParamMask.RefreshToken, ParamsHelper.StringConvertion);
			ConvertParameterTo(dictionary, "ClientType", _cllientType, LoginSettingsParamsHelper.ParamMask.ClientType, ParamsHelper.StringConvertion);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "LoginParams.ConvertToCommand", ex.Message);
		}
	}

	public ErrorCodes CheckParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
