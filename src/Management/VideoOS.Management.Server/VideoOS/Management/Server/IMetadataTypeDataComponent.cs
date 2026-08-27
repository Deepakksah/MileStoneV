using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IMetadataTypeDataComponent : IDisposable
{
	IEnumerable<MetadataTypeData> QueryMetadataInfoForEnabledDevicesByRecorderAdmin(Guid id);

	Dictionary<Guid, IEnumerable<MetadataTypeData>> QueryMetadataInfoEnabledDevicesByRoleId(string[] roleIds);
}
