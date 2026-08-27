using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ServerConfigurationParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("Slc")]
		Slc = 1u,
		[Description("SiteId")]
		SiteId = 2u,
		[Description("ServerId")]
		ServerId = 4u,
		Endpoints = 8u,
		[Description("Timeout")]
		Timeout = 16u,
		[Description("IsAlive")]
		IsAlive = 32u,
		[Description("Timestamp")]
		DateTimeStamp = 64u,
		[Description("DeviceId")]
		DeviceId = 128u,
		[Description("LocalIp")]
		LocalIp = 256u,
		[Description("ServerName")]
		ServerName = 512u,
		[Description("SmartConnectEnabled")]
		AbsEnabled = 1024u,
		GetServerConfigurationMandatory = 132u,
		GetServerConfigurationPossible = 188u,
		SetServerConfigurationMandatory = 1029u,
		SetServerConfigurationPossible = 1567u,
		CheckServerAccessibilityMandatory = 77u,
		CheckServerAccessibilityPossible = 77u,
		All = 2047u
	}
}
