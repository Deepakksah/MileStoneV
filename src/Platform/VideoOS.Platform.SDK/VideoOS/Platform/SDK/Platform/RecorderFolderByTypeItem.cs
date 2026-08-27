using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Resources;

namespace VideoOS.Platform.SDK.Platform;

public class RecorderFolderByTypeItem : Item
{
	private ISystemInfo _sysInfo;

	private IRecorder _recorder;

	private Guid _filterKind;

	public RecorderFolderByTypeItem(ISystemInfo systemInfo, IRecorder recorder, Guid filterKind)
		: base(recorder.ServerId, recorder.Name, FolderType.SystemDefined, Kind.Server)
	{
		_sysInfo = systemInfo;
		_recorder = recorder;
		_filterKind = filterKind;
		base.FQID.ParentId = systemInfo.Id;
		base.FQID.ObjectId = recorder.Guid;
		base.Properties["URL"] = new UriBuilder("http://", recorder.HostName, recorder.Port).ToString();
		base.Properties["Address"] = new UriBuilder(recorder.HostName).Host;
		base.Properties["Guid"] = recorder.Guid.ToString();
		base.Properties["Port"] = recorder.Port.ToString() ?? "";
		base.Properties["MacAddress"] = systemInfo.MacAddress ?? "";
		foreach (string key in systemInfo.Properties.Keys)
		{
			base.Properties[key] = systemInfo.Properties[key];
		}
	}

	public override List<Item> GetChildren()
	{
		_sysInfo.Populate();
		List<Item> list = new List<Item>();
		if (!_sysInfo.Populated)
		{
			return list;
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.Camera)
		{
			list.Add(new AllRSFolderItem(base.FQID.ServerId, string.Format(Strings.AllCameras, _recorder.Name), Kind.Camera, _recorder));
		}
		if ((_filterKind == Guid.Empty || _filterKind == Kind.InputEvent) && _sysInfo.ProductSupportOfKind(Kind.InputEvent))
		{
			list.Add(new AllRSFolderItem(base.FQID.ServerId, string.Format(Strings.AllInputs, _recorder.Name), Kind.InputEvent, _recorder));
		}
		if ((_filterKind == Guid.Empty || _filterKind == Kind.Output) && _sysInfo.ProductSupportOfKind(Kind.Output))
		{
			list.Add(new AllRSFolderItem(base.FQID.ServerId, string.Format(Strings.AllOutputs, _recorder.Name), Kind.Output, _recorder));
		}
		if ((_filterKind == Guid.Empty || _filterKind == Kind.Microphone) && _sysInfo.ProductSupportOfKind(Kind.Microphone))
		{
			list.Add(new AllRSFolderItem(base.FQID.ServerId, string.Format(Strings.AllMicrophones, _recorder.Name), Kind.Microphone, _recorder));
		}
		if ((_filterKind == Guid.Empty || _filterKind == Kind.Metadata) && _sysInfo.ProductSupportOfKind(Kind.Metadata))
		{
			list.Add(new AllRSFolderItem(base.FQID.ServerId, string.Format(Strings.AllMetadataDevices, _recorder.Name), Kind.Metadata, _recorder));
		}
		if ((_filterKind == Guid.Empty || _filterKind == Kind.Speaker) && _sysInfo.ProductSupportOfKind(Kind.Speaker))
		{
			list.Add(new AllRSFolderItem(base.FQID.ServerId, string.Format(Strings.AllSpeakers, _recorder.Name), Kind.Speaker, _recorder));
		}
		if ((_filterKind == Guid.Empty || _filterKind == Kind.Hardware) && _sysInfo.ProductSupportOfKind(Kind.Hardware))
		{
			list.Add(new HardwareFolderItem(base.FQID.ServerId, string.Format(Strings.AllHardware, _recorder.Name), _recorder));
		}
		return list;
	}

	public override Item GetParent()
	{
		return new ServerFolderByTypeItem(_sysInfo, Guid.Empty);
	}
}
