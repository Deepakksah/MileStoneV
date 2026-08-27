using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Item;

[ClassDataProperty("DeviceMapItemData")]
public class DeviceMapItemData : PointMapItemData
{
	public static readonly Guid EmptyDeviceId = new Guid("12345678-0000-0000-0000-000000000000");

	[DataProperty]
	[JsonProperty("deviceId")]
	public Guid DeviceId { get; set; }

	[DataProperty]
	[JsonProperty("serverId")]
	public Guid ServerId { get; set; }
}
