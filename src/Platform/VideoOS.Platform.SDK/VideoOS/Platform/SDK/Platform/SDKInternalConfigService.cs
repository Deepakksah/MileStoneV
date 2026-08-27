using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Platform;

public class SDKInternalConfigService : InternalConfigService
{
	public override bool Initialized => true;

	public override StreamType[] GetStreamTypes(FQID deviceFQID)
	{
		ISystemInfo systemInfo = ((SDKConfiguration)deviceFQID.ServerId.UserContext.Configuration).FindSystemInfo(deviceFQID.ServerId, populate: true);
		if (systemInfo != null)
		{
			ICamera camera = systemInfo.Cameras[deviceFQID.ObjectId];
			if (camera != null)
			{
				return camera.GetStreamTypes();
			}
		}
		return Array.Empty<StreamType>();
	}

	public override List<DataType> GetMetadataTypes(FQID deviceFqid)
	{
		ISystemInfo systemInfo = ((SDKConfiguration)deviceFqid.ServerId.UserContext.Configuration).FindSystemInfo(deviceFqid.ServerId, populate: true);
		if (systemInfo != null)
		{
			IMetadataDevice metadataDevice = systemInfo.MetadataDevices[deviceFqid.ObjectId];
			if (metadataDevice != null)
			{
				return metadataDevice.MetadataTypes.Values.Select(MapToDataType).ToList();
			}
		}
		return base.GetMetadataTypes(deviceFqid);
	}

	private static DataType MapToDataType(MetadataType type)
	{
		return new DataType
		{
			Id = type.Id,
			Name = type.Name,
			Properties = 
			{
				{
					"DisplayId",
					type.DisplayId.ToString()
				},
				{ "DisplayName", type.DisplayName },
				{
					"ValidTime",
					type.ValidTime.ToString(CultureInfo.InvariantCulture)
				}
			}
		};
	}
}
