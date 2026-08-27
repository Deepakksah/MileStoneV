using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Resources;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Platform;

public class ServerFolderByTypeItem : Item
{
	private ISystemInfo _sysInfo;

	private Guid _filterKind;

	public ServerFolderByTypeItem(ISystemInfo systemInfo, Guid filterKind)
		: base(systemInfo.ServerId, Guid.Empty, systemInfo.Id, systemInfo.Name, FolderType.SystemDefined, Kind.Server)
	{
		_sysInfo = systemInfo;
		_filterKind = filterKind;
		base.Properties["URL"] = systemInfo.Url;
		base.Properties["URL_Server"] = systemInfo.Url;
		base.Properties["Address"] = UrlUtil.BuildUri(systemInfo.Url).Host;
		base.Properties["Port"] = systemInfo.Port.ToString() ?? "";
		base.Properties["Guid"] = systemInfo.Id.ToString();
		base.Properties["MacAddress"] = systemInfo.MacAddress ?? "";
		foreach (string key in systemInfo.Properties.Keys)
		{
			base.Properties[key] = systemInfo.Properties[key];
			if (key == "ServerType")
			{
				base.Properties["ProductCode"] = systemInfo.Properties[key];
			}
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
		if ((_filterKind == Guid.Empty || _filterKind == Kind.TriggerEvent) && _sysInfo.ProductSupportOfKind(Kind.TriggerEvent))
		{
			list.Add(new AllFolderItem(_sysInfo, string.Format(Strings.AllEvents, _sysInfo.Name), Kind.TriggerEvent));
		}
		if ((_filterKind == Guid.Empty || _filterKind == Kind.Matrix) && _sysInfo.ProductSupportOfKind(Kind.Matrix))
		{
			list.Add(new AllFolderItem(_sysInfo, string.Format(Strings.AllMatrix, _sysInfo.Name), Kind.Matrix));
		}
		if ((_filterKind == Guid.Empty || _filterKind == Kind.TransactSource) && _sysInfo.ProductSupportOfKind(Kind.TransactSource))
		{
			list.Add(new AllFolderItem(_sysInfo, string.Format(Strings.AllTransactionSources, _sysInfo.Name), Kind.TransactSource));
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.Camera || _filterKind == Kind.InputEvent || _filterKind == Kind.Output || _filterKind == Kind.Microphone || _filterKind == Kind.Speaker || _filterKind == Kind.Metadata || _filterKind == Kind.Hardware || _filterKind == Kind.Server)
		{
			foreach (IRecorder recorder in _sysInfo.Recorders)
			{
				list.Add(new RecorderFolderByTypeItem(_sysInfo, recorder, _filterKind));
			}
			foreach (Guid slaveId in _sysInfo.SlaveIds)
			{
				ISystemInfo systemInfo = ((SDKConfiguration)_sysInfo.UserContext.Configuration).GetSystemInfo(slaveId);
				if (systemInfo != null)
				{
					list.Add(new ServerFolderByTypeItem(systemInfo, Guid.Empty));
				}
			}
		}
		return list;
	}
}
