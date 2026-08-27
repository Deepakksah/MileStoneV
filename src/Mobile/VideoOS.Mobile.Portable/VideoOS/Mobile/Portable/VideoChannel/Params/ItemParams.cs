using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ItemParams
{
	private uint _paramMask;

	private ItemKind _itemKind;

	private Hierarchy _hierarchy;

	private bool _includeRelatedDevices;

	public ItemKind Kind
	{
		get
		{
			return _itemKind;
		}
		set
		{
			_itemKind = value;
			_paramMask |= 1u;
		}
	}

	public Hierarchy Hierarchy
	{
		get
		{
			return _hierarchy;
		}
		set
		{
			_hierarchy = value;
			_paramMask |= 2u;
		}
	}

	public bool IncludeRelated
	{
		get
		{
			return _includeRelatedDevices;
		}
		set
		{
			_includeRelatedDevices = value;
			_paramMask |= 4u;
		}
	}

	public uint ParamMask => _paramMask;

	public ItemParams()
	{
		InternalInit();
	}

	public ItemParams(Command tCommand, uint unMaskToCheck = 7u)
		: this()
	{
		InitFromCommand(tCommand, unMaskToCheck);
	}

	private void InternalInit()
	{
		_paramMask = 0u;
		_itemKind = ItemKind.Unknown;
		_hierarchy = Hierarchy.Unknown;
		_includeRelatedDevices = false;
	}

	private void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, ItemParamsHelper.ParamMask tSuccMask, ParamsHelper.Convertion<T> convFunc, uint unMaskToCheck, bool bLog = true)
	{
		ParamsHelper.ConvertParameter(dictInputParams, szKey, ref tOutput, (uint)tSuccMask, convFunc, unMaskToCheck, ref _paramMask, bLog);
	}

	public ItemParams InitFromCommand(Command tCommand, uint unMaskToCheck = 7u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameter(dictionary, "ItemKind", ref _itemKind, ItemParamsHelper.ParamMask.ItemKind, EnumsHelper.EnumConverter<ItemKind>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "Hierarchy", ref _hierarchy, ItemParamsHelper.ParamMask.Hierarchy, EnumsHelper.EnumConverter<Hierarchy>, unMaskToCheck, bLog: false);
			ConvertParameter(dictionary, "IncludeRelatedDevices", ref _includeRelatedDevices, ItemParamsHelper.ParamMask.IncludeRelatedDevices, ParamsHelper.BoolConvertion, unMaskToCheck, bLog: false);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ItemParams.InitFromCommand", ex.Message);
		}
		return this;
	}

	private void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, ItemParamsHelper.ParamMask eCheckMask, ParamsHelper.ConvToString<T> converter = null)
	{
		ParamsHelper.ConvertParameterTo(dictInputParams, szKey, input, (uint)eCheckMask, _paramMask, converter);
	}

	public ItemParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ConvertParameterTo(dictionary, "ItemKind", _itemKind, ItemParamsHelper.ParamMask.ItemKind, (ItemKind x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "Hierarchy", _hierarchy, ItemParamsHelper.ParamMask.Hierarchy, (Hierarchy x) => EnumsHelper.GetDescription(x));
			ConvertParameterTo(dictionary, "IncludeRelatedDevices", _includeRelatedDevices, ItemParamsHelper.ParamMask.IncludeRelatedDevices, (bool x) => ParamsHelper.BoolConvertionTo(x));
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ItemParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public bool CheckForMask(ItemParamsHelper.ParamMask tMask)
	{
		return (ParamMask & (uint)tMask) == (uint)tMask;
	}
}
