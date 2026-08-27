using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class MetadataTypeData
{
	public Guid DeviceId { get; set; }

	public int ItemIndex { get; set; }

	public Guid Id { get; set; }

	public string Name { get; set; }

	public Guid DisplayId { get; set; }

	public string DisplayName { get; set; }

	public static IEnumerable<MetadataTypeData> QueryMetadataInfoForEnabledDevicesByRecorderAdmin(IGeminiRequestContext requestContext, Guid id)
	{
		using IMetadataTypeDataComponent metadataTypeDataComponent = requestContext.CreateMetadataTypeDataComponent();
		return metadataTypeDataComponent.QueryMetadataInfoForEnabledDevicesByRecorderAdmin(id);
	}

	public static Dictionary<Guid, IEnumerable<MetadataTypeData>> QueryMetadataInfoForEnabledDevicesByRoleId(IGeminiRequestContext requestContext, string[] roleIds)
	{
		using IMetadataTypeDataComponent metadataTypeDataComponent = requestContext.CreateMetadataTypeDataComponent();
		return metadataTypeDataComponent.QueryMetadataInfoEnabledDevicesByRoleId(roleIds);
	}
}
