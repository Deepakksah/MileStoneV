using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;

public class GetAccessControlDoorsParams
{
	private Guid _accessControlDoorId;

	private string _excludedAccessControlSystemIds;

	private string _excludedAccessControlDoorIds;

	private string _excludedAccessControlDoorCategoryIds;

	private string _excludedAccessControlDoorTypeIds;

	private uint _getAccessControlDoorsParamMask;

	public Guid AccessControlDoorId
	{
		get
		{
			return _accessControlDoorId;
		}
		set
		{
			_accessControlDoorId = value;
			_getAccessControlDoorsParamMask |= 1u;
		}
	}

	public string ExcludedAccessControlSystemIds
	{
		get
		{
			return _excludedAccessControlSystemIds;
		}
		set
		{
			_excludedAccessControlSystemIds = value;
			_getAccessControlDoorsParamMask |= 2u;
		}
	}

	public string ExcludedAccessControlDoorIds
	{
		get
		{
			return _excludedAccessControlDoorIds;
		}
		set
		{
			_excludedAccessControlDoorIds = value;
			_getAccessControlDoorsParamMask |= 4u;
		}
	}

	public string ExcludedAccessControlDoorCategoryIds
	{
		get
		{
			return _excludedAccessControlDoorCategoryIds;
		}
		set
		{
			_excludedAccessControlDoorCategoryIds = value;
			_getAccessControlDoorsParamMask |= 8u;
		}
	}

	public string ExcludedAccessControlDoorTypeIds
	{
		get
		{
			return _excludedAccessControlDoorTypeIds;
		}
		set
		{
			_excludedAccessControlDoorTypeIds = value;
			_getAccessControlDoorsParamMask |= 16u;
		}
	}

	public GetAccessControlDoorsParams()
	{
		InternalInit();
	}

	public GetAccessControlDoorsParams(Command command, uint maskToCheck = 31u)
	{
		InternalInit();
		InitFromCommand(command, maskToCheck);
	}

	public GetAccessControlDoorsParams InitFromCommand(Command command, uint maskToCheck = 31u)
	{
		try
		{
			ParamsHelper.Convertion<Guid> convFunc = (string x) => (!Guid.TryParse(x, out var result)) ? Guid.Empty : result;
			_getAccessControlDoorsParamMask = 0u;
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "AccessControlDoorId", ref _accessControlDoorId, GetAccessControlDoorsParamMask.AccessControlDoorId, convFunc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExcludedAccessControlSystemIds", ref _excludedAccessControlSystemIds, GetAccessControlDoorsParamMask.ExcludedAccessControlSystemIds, ParamsHelper.StringConvertion, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExcludedAccessControlDoorIds", ref _excludedAccessControlDoorIds, GetAccessControlDoorsParamMask.ExcludedAccessControlDoorIds, ParamsHelper.StringConvertion, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExcludedAccessControlDoorCategoryIds", ref _excludedAccessControlDoorCategoryIds, GetAccessControlDoorsParamMask.ExcludedAccessControlDoorCategoryIds, ParamsHelper.StringConvertion, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExcludedAccessControlDoorTypeIds", ref _excludedAccessControlDoorTypeIds, GetAccessControlDoorsParamMask.ExcludedAccessControlDoorTypeIds, ParamsHelper.StringConvertion, maskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GetAccessControlDoorsParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	public GetAccessControlDoorsParams ConvertToCommand(ref Command command)
	{
		try
		{
			command.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "AccessControlDoorId", _accessControlDoorId, GetAccessControlDoorsParamMask.AccessControlDoorId);
			ConvertParameterTo(dictionary, "ExcludedAccessControlSystemIds", _excludedAccessControlSystemIds, GetAccessControlDoorsParamMask.ExcludedAccessControlSystemIds);
			ConvertParameterTo(dictionary, "ExcludedAccessControlDoorIds", _excludedAccessControlDoorIds, GetAccessControlDoorsParamMask.ExcludedAccessControlDoorIds);
			ConvertParameterTo(dictionary, "ExcludedAccessControlDoorCategoryIds", _excludedAccessControlDoorCategoryIds, GetAccessControlDoorsParamMask.ExcludedAccessControlDoorCategoryIds);
			ConvertParameterTo(dictionary, "ExcludedAccessControlDoorTypeIds", _excludedAccessControlDoorTypeIds, GetAccessControlDoorsParamMask.ExcludedAccessControlDoorTypeIds);
			command.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GetAccessControlDoorsParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckGetAccessControlDoorParams(uint maskToCheck)
	{
		return ParamsHelper.CheckParams(maskToCheck, _getAccessControlDoorsParamMask);
	}

	private void InternalInit()
	{
		_getAccessControlDoorsParamMask = 0u;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, GetAccessControlDoorsParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _getAccessControlDoorsParamMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, GetAccessControlDoorsParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _getAccessControlDoorsParamMask, converter);
	}
}
