using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public sealed class PtzHandling
{
	private PtzHandling()
	{
	}

	public static PresetInfo[] GetPresets(IGeminiRequestContext requestContext, Guid deviceId)
	{
		try
		{
			PtzPresetCollection ptzPresetCollection = DevicePtzPresets.QueryPtzPresets(requestContext, deviceId.ToString());
			List<PresetInfo> list = new List<PresetInfo>();
			foreach (PtzPreset item in ptzPresetCollection)
			{
				PresetInfo presetInfo = new PresetInfo();
				presetInfo.Id = item.Id;
				presetInfo.Locked = item.Locked;
				presetInfo.Name = item.Name;
				presetInfo.Position = new PositionInfo();
				presetInfo.Position.Pan = item.PtzArgs.Pan;
				presetInfo.Position.Tilt = item.PtzArgs.Tilt;
				presetInfo.Position.Zoom = item.PtzArgs.Zoom;
				list.Add(presetInfo);
			}
			return list.ToArray();
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static void DeletePresets(IGeminiRequestContext requestContext, Guid presetId, Guid deviceId)
	{
		try
		{
			DevicePtzPresets.RemovePtzPreset(requestContext, deviceId.ToString(), presetId.ToString());
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static Guid CreatePreset(IGeminiRequestContext requestContext, Guid deviceId, string name)
	{
		try
		{
			Guid result = Guid.NewGuid();
			DevicePtzPresets.AddPtzPreset(requestContext, deviceId.ToString(), result.ToString(), name, string.Empty, false, locked: false);
			return result;
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static void UpdatePresetName(IGeminiRequestContext requestContext, Guid id, Guid deviceId, string newName)
	{
		try
		{
			DevicePtzPresets.UpdatePtzPreset(requestContext, deviceId.ToString(), id.ToString(), newName, null, null, null, null, null, null);
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static void UpdatePresetPosition(IGeminiRequestContext requestContext, Guid id, Guid deviceId)
	{
		try
		{
			DevicePtzPresets.UpdatePresetPosition(requestContext, id.ToString(), deviceId.ToString());
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static void UpdatePresetOrder(IGeminiRequestContext requestContext, Guid deviceId, Guid[] presetIds)
	{
		try
		{
			string[] array = presetIds.Select((Guid f) => f.ToString()).ToArray();
			if (array == null || array.Length != 0)
			{
				DevicePtzPresets.UpdatePtzPresetOrder(requestContext, deviceId.ToString(), array);
			}
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}
}
