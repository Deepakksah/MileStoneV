using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params.AccessControlParams;

public class GetAccessControlEventsParams
{
	private int _pageSize;

	private int _page;

	private DateTime _startTimestamp;

	private DateTime _endTimestamp;

	private bool _includeEventsCount;

	private Guid _accessControlEventId;

	private Guid _accessControlRuleId;

	private string _excludedAccessControlSystemIds;

	private string _excludedAccessControlDoorIds;

	private string _excludedAccessControlEventCategoryIds;

	private uint _getAccessControlEventsParamMask;

	public int PageSize
	{
		get
		{
			return _pageSize;
		}
		set
		{
			_pageSize = value;
			_getAccessControlEventsParamMask |= 1u;
		}
	}

	public int Page
	{
		get
		{
			return _page;
		}
		set
		{
			_page = value;
			_getAccessControlEventsParamMask |= 2u;
		}
	}

	public DateTime StartTimestamp
	{
		get
		{
			return _startTimestamp;
		}
		set
		{
			_startTimestamp = value;
			_getAccessControlEventsParamMask |= 4u;
		}
	}

	public DateTime EndTimestamp
	{
		get
		{
			return _endTimestamp;
		}
		set
		{
			_endTimestamp = value;
			_getAccessControlEventsParamMask |= 8u;
		}
	}

	public bool IncludeEventsCount
	{
		get
		{
			return _includeEventsCount;
		}
		set
		{
			_includeEventsCount = value;
			_getAccessControlEventsParamMask |= 16u;
		}
	}

	public Guid AccessControlEventId
	{
		get
		{
			return _accessControlEventId;
		}
		set
		{
			_accessControlEventId = value;
			_getAccessControlEventsParamMask |= 32u;
		}
	}

	public Guid AccessControlRuleId
	{
		get
		{
			return _accessControlRuleId;
		}
		set
		{
			_accessControlRuleId = value;
			_getAccessControlEventsParamMask |= 64u;
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
			_getAccessControlEventsParamMask |= 128u;
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
			_getAccessControlEventsParamMask |= 256u;
		}
	}

	public string ExcludedAccessControlEventCategoryIds
	{
		get
		{
			return _excludedAccessControlEventCategoryIds;
		}
		set
		{
			_excludedAccessControlEventCategoryIds = value;
			_getAccessControlEventsParamMask |= 512u;
		}
	}

	public GetAccessControlEventsParams()
	{
		InternalInit();
	}

	public GetAccessControlEventsParams(Command command, uint maskToCheck = 1023u)
	{
		InternalInit();
		InitFromCommand(command, maskToCheck);
	}

	public GetAccessControlEventsParams InitFromCommand(Command command, uint maskToCheck = 1023u)
	{
		try
		{
			ParamsHelper.Convertion<int> convFunc = (string x) => int.TryParse(x, out var result) ? result : 0;
			ParamsHelper.Convertion<Guid> convFunc2 = (string x) => (!Guid.TryParse(x, out var result)) ? Guid.Empty : result;
			_getAccessControlEventsParamMask = 0u;
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "AccessControlPageSize", ref _pageSize, GetAccessControlEventsParamMask.PageSize, convFunc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlPage", ref _page, GetAccessControlEventsParamMask.Page, convFunc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlStartTimestamp", ref _startTimestamp, GetAccessControlEventsParamMask.StartTimestamp, TimeConverter.FromStringUtc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlEndTimestamp", ref _endTimestamp, GetAccessControlEventsParamMask.EndTimestamp, TimeConverter.FromStringUtc, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlIncludeEventsCount", ref _includeEventsCount, GetAccessControlEventsParamMask.IncludeEventsCount, ParamsHelper.BoolConvertion, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlEventId", ref _accessControlEventId, GetAccessControlEventsParamMask.AccessControlEventId, convFunc2, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "AccessControlRuleId", ref _accessControlRuleId, GetAccessControlEventsParamMask.AccessControlRuleId, convFunc2, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExcludedAccessControlSystemIds", ref _excludedAccessControlSystemIds, GetAccessControlEventsParamMask.ExcludedAccessControlSystemIds, ParamsHelper.StringConvertion, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExcludedAccessControlDoorIds", ref _excludedAccessControlDoorIds, GetAccessControlEventsParamMask.ExcludedAccessControlDoorIds, ParamsHelper.StringConvertion, maskToCheck, bLog: false);
			ConvertParameter(dictionary, "ExcludedAccessControlEventCategoryIds", ref _excludedAccessControlEventCategoryIds, GetAccessControlEventsParamMask.ExcludedAccessControlEventCategoryIds, ParamsHelper.StringConvertion, maskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GetAccessControlEventsParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	public GetAccessControlEventsParams ConvertToCommand(ref Command command)
	{
		try
		{
			command.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "AccessControlPageSize", _pageSize, GetAccessControlEventsParamMask.PageSize);
			ConvertParameterTo(dictionary, "AccessControlPage", _page, GetAccessControlEventsParamMask.Page);
			ConvertParameterTo(dictionary, "AccessControlStartTimestamp", _startTimestamp, GetAccessControlEventsParamMask.StartTimestamp, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "AccessControlEndTimestamp", _endTimestamp, GetAccessControlEventsParamMask.EndTimestamp, TimeConverter.ToMilisecString);
			ConvertParameterTo(dictionary, "AccessControlIncludeEventsCount", _includeEventsCount, GetAccessControlEventsParamMask.IncludeEventsCount, ParamsHelper.BoolConvertionTo);
			ConvertParameterTo(dictionary, "AccessControlEventId", _accessControlEventId, GetAccessControlEventsParamMask.AccessControlEventId);
			ConvertParameterTo(dictionary, "AccessControlRuleId", _accessControlRuleId, GetAccessControlEventsParamMask.AccessControlRuleId);
			ConvertParameterTo(dictionary, "ExcludedAccessControlSystemIds", _excludedAccessControlSystemIds, GetAccessControlEventsParamMask.ExcludedAccessControlSystemIds);
			ConvertParameterTo(dictionary, "ExcludedAccessControlDoorIds", _excludedAccessControlDoorIds, GetAccessControlEventsParamMask.ExcludedAccessControlDoorIds);
			ConvertParameterTo(dictionary, "ExcludedAccessControlEventCategoryIds", _excludedAccessControlEventCategoryIds, GetAccessControlEventsParamMask.ExcludedAccessControlEventCategoryIds);
			command.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GetAccessControlEventsParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckGetAccessControlEventsParams(uint maskToCheck)
	{
		if (maskToCheck == 1023)
		{
			ErrorCodes errorCodes = ParamsHelper.CheckParams(11u, _getAccessControlEventsParamMask);
			if (errorCodes == ErrorCodes.Ok)
			{
				return errorCodes;
			}
			errorCodes = ParamsHelper.CheckParams(32u, _getAccessControlEventsParamMask);
			if (errorCodes == ErrorCodes.Ok)
			{
				return errorCodes;
			}
		}
		return ParamsHelper.CheckParams(maskToCheck, _getAccessControlEventsParamMask);
	}

	private void InternalInit()
	{
		_getAccessControlEventsParamMask = 0u;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, GetAccessControlEventsParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _getAccessControlEventsParamMask, bLog);
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, GetAccessControlEventsParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _getAccessControlEventsParamMask, converter);
	}
}
