using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class HardwareFolderItem : Item
{
	private Image _icon;

	private ServerId _serverId;

	private IRecorder _recorder;

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

	public HardwareFolderItem(ServerId serverId, string name, IRecorder recorder)
		: base(new FQID(serverId, serverId.Id, Kind.Hardware, FolderType.SystemDefined, Kind.Hardware), name)
	{
		_serverId = serverId;
		_recorder = recorder;
		BuildChildren();
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		_icon = new Bitmap(executingAssembly.GetManifestResourceStream(executingAssembly.GetName().Name + ".Resources.Hardware16ni8.ico"));
	}

	private void BuildChildren()
	{
		List<Item> list = new List<Item>();
		if (_recorder.Hardware != null)
		{
			foreach (IHardwareInfo value in _recorder.Hardware.Values)
			{
				list.Add(new HardwareItem(this, _recorder, value));
			}
		}
		base.SetChildren(list);
	}

	public override Item GetParent()
	{
		return Configuration.Instance.GetItem(FQID.ServerId, FQID.ServerId.Id, Kind.Server);
	}
}
