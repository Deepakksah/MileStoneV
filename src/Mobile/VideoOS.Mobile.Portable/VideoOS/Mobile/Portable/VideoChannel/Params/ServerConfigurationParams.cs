using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ServerConfigurationParams : ICloneable, IComparable, IComparable<ServerConfigurationParams>
{
	public const string HostToken = "*HostName*";

	private uint _paramMask;

	private Guid _serverId;

	private Guid _siteId;

	private string _slc;

	private string _deviceId;

	private bool _isAlive;

	private bool _isAbsEnabled;

	private int _timeout;

	private List<ServerEndpoint> _endpointList;

	private DateTime _timeStamp;

	private string _localIp;

	private string _serverName;

	public uint ParamMask => _paramMask;

	public string DeviceId
	{
		get
		{
			return _deviceId;
		}
		set
		{
			_deviceId = value;
			_paramMask |= 128u;
		}
	}

	public string ServerName
	{
		get
		{
			return _serverName;
		}
		set
		{
			_serverName = value;
			_paramMask |= 512u;
		}
	}

	public string LocalIp
	{
		get
		{
			return _localIp;
		}
		set
		{
			_localIp = value;
			_paramMask |= 256u;
		}
	}

	public string Slc
	{
		get
		{
			return _slc;
		}
		set
		{
			_slc = value;
			_paramMask |= 1u;
		}
	}

	public Guid SiteId
	{
		get
		{
			return _siteId;
		}
		set
		{
			_siteId = value;
			_paramMask |= 2u;
		}
	}

	public Guid ServerId
	{
		get
		{
			return _serverId;
		}
		set
		{
			_serverId = value;
			_paramMask |= 4u;
		}
	}

	public bool IsAlive
	{
		get
		{
			return _isAlive;
		}
		set
		{
			_isAlive = value;
			_paramMask |= 32u;
		}
	}

	public bool AbsEnabled
	{
		get
		{
			return _isAbsEnabled;
		}
		set
		{
			_isAbsEnabled = value;
			_paramMask |= 1024u;
		}
	}

	public int Timeout
	{
		get
		{
			return _timeout;
		}
		set
		{
			_timeout = value;
			_paramMask |= 16u;
		}
	}

	public DateTime TimeStamp
	{
		get
		{
			return _timeStamp;
		}
		set
		{
			_timeStamp = value;
			_paramMask |= 64u;
		}
	}

	public List<ServerEndpoint> ServerAccesses
	{
		get
		{
			return _endpointList;
		}
		set
		{
			_endpointList = value;
			_paramMask |= 8u;
		}
	}

	public ServerConfigurationParams()
	{
		InternalInit();
	}

	public ServerConfigurationParams(Command tCommand, uint unMaskToCheck = 2047u, Command.ParamsType paramsType = Command.ParamsType.Input)
	{
		InternalInit();
		InitFromCommand(tCommand, unMaskToCheck, paramsType);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_slc = string.Empty;
		_siteId = Guid.Empty;
		_serverId = Guid.Empty;
		_isAlive = false;
		_isAbsEnabled = true;
		_timeout = 0;
		_endpointList = new List<ServerEndpoint>();
		_timeStamp = DateTime.MinValue;
		_deviceId = string.Empty;
		_localIp = string.Empty;
		_serverName = string.Empty;
	}

	public ErrorCodes CheckParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, ServerConfigurationParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public ServerConfigurationParams InitFromCommand(Command tCommand, uint unMaskToCheck = 2047u, Command.ParamsType paramsType = Command.ParamsType.Input)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(paramsType);
			ConvertParameter(dictionary, "Slc", ref _slc, ServerConfigurationParamsHelper.ParamMask.Slc, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "DeviceId", ref _deviceId, ServerConfigurationParamsHelper.ParamMask.DeviceId, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "ServerName", ref _serverName, ServerConfigurationParamsHelper.ParamMask.ServerName, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "LocalIp", ref _localIp, ServerConfigurationParamsHelper.ParamMask.LocalIp, ParamsHelper.StringConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SiteId", ref _siteId, ServerConfigurationParamsHelper.ParamMask.SiteId, ParamsHelper.GuidConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "ServerId", ref _serverId, ServerConfigurationParamsHelper.ParamMask.ServerId, ParamsHelper.GuidConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "Timeout", ref _timeout, ServerConfigurationParamsHelper.ParamMask.Timeout, Convert.ToInt32, unMaskToCheck);
			ConvertParameter(dictionary, "IsAlive", ref _isAlive, ServerConfigurationParamsHelper.ParamMask.IsAlive, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "SmartConnectEnabled", ref _isAbsEnabled, ServerConfigurationParamsHelper.ParamMask.AbsEnabled, ParamsHelper.BoolConvertion, unMaskToCheck);
			ConvertParameter(dictionary, "Timestamp", ref _timeStamp, ServerConfigurationParamsHelper.ParamMask.DateTimeStamp, TimeConverter.FromString, unMaskToCheck);
			if ((unMaskToCheck & 8) == 8)
			{
				_endpointList = SerializableServerEndpoint.GetServerEndPoints(tCommand.Items);
				_paramMask |= 8u;
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ServerConfigurationParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, ServerConfigurationParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public ServerConfigurationParams ConvertToCommand(ref Command tCommand, Command.ParamsType paramsType = Command.ParamsType.Input)
	{
		try
		{
			tCommand.ConvertToDictionary(paramsType);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(paramsType);
			ConvertParameterTo(dictionary, "Slc", _slc, ServerConfigurationParamsHelper.ParamMask.Slc);
			ConvertParameterTo(dictionary, "DeviceId", _deviceId, ServerConfigurationParamsHelper.ParamMask.DeviceId);
			ConvertParameterTo(dictionary, "ServerName", _serverName, ServerConfigurationParamsHelper.ParamMask.ServerName);
			ConvertParameterTo(dictionary, "LocalIp", _localIp, ServerConfigurationParamsHelper.ParamMask.LocalIp);
			ConvertParameterTo(dictionary, "SiteId", _siteId, ServerConfigurationParamsHelper.ParamMask.SiteId);
			ConvertParameterTo(dictionary, "ServerId", _serverId, ServerConfigurationParamsHelper.ParamMask.ServerId);
			ConvertParameterTo(dictionary, "Timeout", _timeout, ServerConfigurationParamsHelper.ParamMask.Timeout);
			ConvertParameterTo(dictionary, "IsAlive", _isAlive, ServerConfigurationParamsHelper.ParamMask.IsAlive, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "SmartConnectEnabled", _isAbsEnabled, ServerConfigurationParamsHelper.ParamMask.AbsEnabled, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "Timestamp", _timeStamp, ServerConfigurationParamsHelper.ParamMask.DateTimeStamp, TimeConverter.ToMilisecString);
			tCommand.ConvertFromDictionary(paramsType);
			tCommand.Items = SerializableServerEndpoint.GetItems(_endpointList);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ServerConfigurationParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	private void SetField<T>(ref T tToSet, ref T tField, ServerConfigurationParamsHelper.ParamMask tCheckMask, ref uint tSetMask, ParamsHelper.CopyMethod<T> copyFunc = null)
	{
		ParamsHelper.SetField(ref tToSet, ref tField, ParamMask, (uint)tCheckMask, ref tSetMask, copyFunc);
	}

	public ServerConfigurationParams InitFromAnother(ServerConfigurationParams tAnother)
	{
		tAnother.SetField(ref _slc, ref tAnother._slc, ServerConfigurationParamsHelper.ParamMask.Slc, ref _paramMask);
		tAnother.SetField(ref _deviceId, ref tAnother._deviceId, ServerConfigurationParamsHelper.ParamMask.DeviceId, ref _paramMask);
		tAnother.SetField(ref _serverName, ref tAnother._serverName, ServerConfigurationParamsHelper.ParamMask.ServerName, ref _paramMask);
		tAnother.SetField(ref _localIp, ref tAnother._localIp, ServerConfigurationParamsHelper.ParamMask.LocalIp, ref _paramMask);
		tAnother.SetField(ref _siteId, ref tAnother._siteId, ServerConfigurationParamsHelper.ParamMask.SiteId, ref _paramMask);
		tAnother.SetField(ref _serverId, ref tAnother._serverId, ServerConfigurationParamsHelper.ParamMask.ServerId, ref _paramMask);
		tAnother.SetField(ref _isAlive, ref tAnother._isAlive, ServerConfigurationParamsHelper.ParamMask.IsAlive, ref _paramMask);
		tAnother.SetField(ref _isAbsEnabled, ref tAnother._isAbsEnabled, ServerConfigurationParamsHelper.ParamMask.AbsEnabled, ref _paramMask);
		tAnother.SetField(ref _timeout, ref tAnother._timeout, ServerConfigurationParamsHelper.ParamMask.Timeout, ref _paramMask);
		tAnother.SetField(ref _timeStamp, ref tAnother._timeStamp, ServerConfigurationParamsHelper.ParamMask.DateTimeStamp, ref _paramMask);
		tAnother.SetField(ref _endpointList, ref tAnother._endpointList, ServerConfigurationParamsHelper.ParamMask.Endpoints, ref _paramMask);
		return this;
	}

	private void CompareCheck<T>(ref int nResult, T first, T second, ParamsHelper.CompareFunc<T> func, uint unMaskToCheck, ServerConfigurationParamsHelper.ParamMask tMask)
	{
		ParamsHelper.CompareCheck(ref nResult, first, second, func, unMaskToCheck, (uint)tMask);
	}

	private static void CompareBoolE(ref int nResult, bool first, bool second)
	{
		VideoOS.Mobile.Portable.Utilities.Math.CompareE(ref nResult, first, second);
	}

	public int CompareTo(ServerConfigurationParams other, uint unMaskToCheck)
	{
		if (other == null)
		{
			return 1;
		}
		if ((_paramMask & unMaskToCheck) != (other._paramMask & unMaskToCheck))
		{
			return 1;
		}
		int nResult = 0;
		CompareCheck(ref nResult, Slc, other.Slc, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.Slc);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, DeviceId, other.DeviceId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.DeviceId);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, ServerName, other.ServerName, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.ServerName);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, LocalIp, other.LocalIp, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.LocalIp);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, SiteId, other.SiteId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.SiteId);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, ServerId, other.ServerId, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.ServerId);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, IsAlive, other.IsAlive, CompareBoolE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.IsAlive);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, AbsEnabled, other.AbsEnabled, CompareBoolE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.AbsEnabled);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, Timeout, other.Timeout, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.Timeout);
		if (nResult != 0)
		{
			return nResult;
		}
		CompareCheck(ref nResult, TimeStamp, other.TimeStamp, VideoOS.Mobile.Portable.Utilities.Math.CompareE, unMaskToCheck, ServerConfigurationParamsHelper.ParamMask.Timeout);
		if (nResult != 0)
		{
			return nResult;
		}
		if (_endpointList != null || other._endpointList != null)
		{
			if (_endpointList == null || other._endpointList == null)
			{
				return 1;
			}
			if (!_endpointList.SequenceEqual(other._endpointList))
			{
				return 1;
			}
		}
		return nResult;
	}

	public int CompareTo(ServerConfigurationParams other)
	{
		return CompareTo(other, 2047u);
	}

	public int CompareTo(object obj)
	{
		return CompareTo(obj as ServerConfigurationParams);
	}

	public object Clone()
	{
		ServerConfigurationParams obj = (ServerConfigurationParams)MemberwiseClone();
		obj._endpointList = new List<ServerEndpoint>();
		return obj;
	}

	public bool CheckForMask(ServerConfigurationParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}

	public ErrorCodes CheckParams(ServerConfigurationParamsHelper.ParamMask tMaskToCheck)
	{
		return ParamsHelper.CheckParams((uint)tMaskToCheck, ParamMask);
	}
}
