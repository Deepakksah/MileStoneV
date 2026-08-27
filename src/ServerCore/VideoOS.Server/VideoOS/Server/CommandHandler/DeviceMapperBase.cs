using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Server.CommandHandler;

public abstract class DeviceMapperBase
{
	private readonly List<ReplaceHardwareDevice> OldDevices;

	private readonly List<ReplaceHardwareDevice> NewDevices;

	protected abstract void AddDevice(ReplaceHardwareDevice device);

	protected abstract void RemoveDevice(ReplaceHardwareDevice device);

	protected abstract void ReplaceDevice(ReplaceHardwareDevice oldDevice, ReplaceHardwareDevice newDevice);

	protected DeviceMapperBase(List<ReplaceHardwareDevice> oldDevices, List<ReplaceHardwareDevice> newDevices)
	{
		OldDevices = oldDevices;
		NewDevices = newDevices;
	}

	public ReplaceHardwareResult ReplaceHardware(bool AllowDeletingDisabledDevices)
	{
		DeviceTypeEnum[] array = new DeviceTypeEnum[6]
		{
			DeviceTypeEnum.Camera,
			DeviceTypeEnum.AudioInput,
			DeviceTypeEnum.Input,
			DeviceTypeEnum.Output,
			DeviceTypeEnum.AudioOutput,
			DeviceTypeEnum.Metadata
		};
		DeviceTypeEnum[] array2 = array;
		foreach (DeviceTypeEnum deviceType in array2)
		{
			if (NewDevices.Where((ReplaceHardwareDevice d) => d.Hwid.Type == deviceType).Count() >= OldDevices.Where((ReplaceHardwareDevice d) => d.Hwid.Type == deviceType).Count())
			{
				continue;
			}
			if (!AllowDeletingDisabledDevices)
			{
				return ReplaceHardwareResult.Error_TooManyDevices_Delete;
			}
			if (NewDevices.Where((ReplaceHardwareDevice d) => d.Hwid.Type == deviceType).Count() < OldDevices.Where((ReplaceHardwareDevice d) => d.Hwid.Type == deviceType && d.Enabled).Count())
			{
				return ReplaceHardwareResult.Error_TooManyDevices_Disable;
			}
			while (NewDevices.Where((ReplaceHardwareDevice d) => d.Hwid.Type == deviceType).Count() < OldDevices.Where((ReplaceHardwareDevice d) => d.Hwid.Type == deviceType).Count())
			{
				ReplaceHardwareDevice replaceHardwareDevice = OldDevices.FirstOrDefault((ReplaceHardwareDevice old) => old.Hwid.Type == deviceType && !old.Enabled && !NewDevices.Any((ReplaceHardwareDevice @new) => @new.Hwid == old.Hwid));
				if (replaceHardwareDevice == null)
				{
					replaceHardwareDevice = OldDevices.Last((ReplaceHardwareDevice old) => old.Hwid.Type == deviceType && !old.Enabled);
				}
				OldDevices.Remove(replaceHardwareDevice);
				RemoveDevice(replaceHardwareDevice);
			}
		}
		MapDevicesOnTypeAndIndex();
		MapRemainingDevicesOnType();
		AddUnmappedNewDevices();
		return ReplaceHardwareResult.Success;
	}

	private void MapDevicesOnTypeAndIndex()
	{
		ReplaceHardwareDevice[] array = OldDevices.ToArray();
		foreach (ReplaceHardwareDevice old in array)
		{
			ReplaceHardwareDevice replaceHardwareDevice = NewDevices.FirstOrDefault((ReplaceHardwareDevice @new) => @new.Hwid == old.Hwid);
			if (replaceHardwareDevice != null)
			{
				OldDevices.Remove(old);
				NewDevices.Remove(replaceHardwareDevice);
				ReplaceDevice(old, replaceHardwareDevice);
			}
		}
	}

	private void MapRemainingDevicesOnType()
	{
		ReplaceHardwareDevice[] array = OldDevices.ToArray();
		foreach (ReplaceHardwareDevice old in array)
		{
			ReplaceHardwareDevice replaceHardwareDevice = NewDevices.FirstOrDefault((ReplaceHardwareDevice @new) => @new.Hwid.Type == old.Hwid.Type);
			if (replaceHardwareDevice != null)
			{
				OldDevices.Remove(old);
				NewDevices.Remove(replaceHardwareDevice);
				ReplaceDevice(old, replaceHardwareDevice);
			}
		}
	}

	private void AddUnmappedNewDevices()
	{
		ReplaceHardwareDevice[] array = NewDevices.ToArray();
		foreach (ReplaceHardwareDevice replaceHardwareDevice in array)
		{
			NewDevices.Remove(replaceHardwareDevice);
			AddDevice(replaceHardwareDevice);
		}
	}
}
