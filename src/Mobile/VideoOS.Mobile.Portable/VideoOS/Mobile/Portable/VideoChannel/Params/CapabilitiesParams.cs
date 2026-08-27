using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CapabilitiesParams : IDefineKind
{
	private Guid _itemId = Guid.Empty;

	private string _itemKind;

	private uint _paramMask;

	public Guid ItemId
	{
		get
		{
			return _itemId;
		}
		set
		{
			_itemId = value;
			_paramMask |= 2u;
			_paramMask |= 1u;
		}
	}

	public Guid CameraId
	{
		get
		{
			return ItemId;
		}
		set
		{
			ItemId = value;
		}
	}

	public string ItemKind
	{
		get
		{
			return _itemKind;
		}
		set
		{
			_itemKind = value;
			_paramMask |= 4u;
		}
	}

	public CapabilitiesParams(Command command, uint unMask = 7u)
	{
		InternalInit();
		InitFromCommand(command, unMask);
	}

	public CapabilitiesParams()
	{
		InternalInit();
	}

	private void InternalInit()
	{
		_paramMask = 0u;
	}

	private void InitFromCommand(Command command, uint unMask = 7u)
	{
		try
		{
			_paramMask = 0u;
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			ParamsHelper.ConvertParameter(dictionary, "CameraId", ref _itemId, 3u, ParamsHelper.GuidConvertion, unMask, ref _paramMask, bLog: false);
			ParamsHelper.ConvertParameter(dictionary, "ItemId", ref _itemId, 2u, ParamsHelper.GuidConvertion, unMask, ref _paramMask);
			ParamsHelper.ConvertParameter(dictionary, "ItemKind", ref _itemKind, 4u, ParamsHelper.StringConvertion, unMask, ref _paramMask);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CapabilitiesParams.InitFromCommand", ex.Message);
		}
	}

	public CapabilitiesParams ConvertToCommand(ref Command tCommand)
	{
		try
		{
			tCommand.ConvertToDictionary(Command.ParamsType.Input);
			Dictionary<string, string> dictionary = tCommand.GetDictionary(Command.ParamsType.Input);
			ParamsHelper.ConvertParameterTo(dictionary, "CameraId", _itemId, 1u, _paramMask);
			ParamsHelper.ConvertParameterTo(dictionary, "ItemId", _itemId, 2u, _paramMask);
			ParamsHelper.ConvertParameterTo(dictionary, "ItemKind", _itemKind, 4u, _paramMask);
			tCommand.ConvertFromDictionary(Command.ParamsType.Input);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "PtzParams.ConvertToCommand", ex.Message);
		}
		return this;
	}

	public ErrorCodes CheckCapabilitiesParams(uint tMaskToCheck)
	{
		return ParamsHelper.CheckParams(tMaskToCheck, _paramMask);
	}
}
