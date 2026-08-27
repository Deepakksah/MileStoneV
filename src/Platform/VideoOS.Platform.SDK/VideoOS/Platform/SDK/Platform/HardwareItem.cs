using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class HardwareItem : Item
{
	private Item _recorderItem;

	private IRecorder _recorder;

	private Image _icon;

	private IHardwareInfo _hardware;

	public override Image Icon
	{
		get
		{
			return _icon;
		}
		set
		{
			_icon = value;
		}
	}

	public HardwareItem(Item recorderItem, IRecorder recorder, IHardwareInfo hardware)
	{
		_hardware = hardware;
		_recorderItem = recorderItem;
		_recorder = recorder;
		base.Name = hardware.Name;
		base.FQID = new FQID(_recorderItem.FQID.ServerId, _recorderItem.FQID.ServerId.Id, hardware.HardwareId, FolderType.No, Kind.Hardware);
		base.Properties.Add("IsInterconnectedProduct", hardware.Interconnected ? "Yes" : "No");
		base.Properties.Add("Address", hardware.Uri);
		if (hardware.Interconnected)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			_icon = new Bitmap(executingAssembly.GetManifestResourceStream(executingAssembly.GetName().Name + ".Resources.Interconnected_hardware_16x16.ico"));
		}
		else
		{
			Assembly executingAssembly2 = Assembly.GetExecutingAssembly();
			_icon = new Bitmap(executingAssembly2.GetManifestResourceStream(executingAssembly2.GetName().Name + ".Resources.Hardware16ni8.ico"));
		}
	}

	public override Item GetParent()
	{
		return _recorderItem;
	}

	public override List<Item> GetRelated()
	{
		List<Item> list = new List<Item>();
		if (_hardware != null && _hardware.DeviceIds != null)
		{
			lock (_recorder.LockObject)
			{
				Guid[] deviceIds = _hardware.DeviceIds;
				foreach (Guid guid in deviceIds)
				{
					Guid guid2 = Guid.Empty;
					if (_recorder.Cameras.ContainsKey(guid))
					{
						guid2 = Kind.Camera;
					}
					else if (_recorder.Microphones.ContainsKey(guid))
					{
						guid2 = Kind.Microphone;
					}
					else if (_recorder.Speakers.ContainsKey(guid))
					{
						guid2 = Kind.Speaker;
					}
					else if (_recorder.Events.InputEvents.ContainsKey(guid))
					{
						guid2 = Kind.InputEvent;
					}
					else if (_recorder.OutputDevices.ContainsKey(guid))
					{
						guid2 = Kind.Output;
					}
					else if (_recorder.MetadataDevices.ContainsKey(guid))
					{
						guid2 = Kind.Metadata;
					}
					if (guid2 != Guid.Empty)
					{
						Item item = _recorderItem.FQID.ServerId.UserContext.Configuration.GetItem(_recorderItem.FQID.ServerId, guid, guid2);
						if (item != null)
						{
							list.Add(item);
						}
					}
				}
			}
		}
		return list;
	}
}
