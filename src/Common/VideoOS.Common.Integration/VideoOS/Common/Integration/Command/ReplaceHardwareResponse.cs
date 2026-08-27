using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class ReplaceHardwareResponse : CommonResponseBase
{
	[DataMember]
	public string Status { get; private set; }

	[DataMember]
	public string[] InheritedDevices { get; private set; }

	[DataMember]
	public string[] AddedDevices { get; private set; }

	[DataMember]
	public string[] AddedDeviceIds { get; private set; }

	[DataMember]
	public string[] RemovedDevices { get; private set; }

	[DataMember]
	public RHSettingValue[] InheritedHardwareSettingValues { get; private set; }

	[DataMember]
	public RHSettingValue[] DefaultHardwareSettingValues { get; private set; }

	[DataMember]
	public RHSettingValue[] InheritedHardwarePtzSettingValues { get; private set; }

	[DataMember]
	public RHSettingValue[] DefaultHardwarePtzSettingValues { get; private set; }

	[DataMember]
	public RHSettingsDevice[] SettingsDevices { get; private set; }

	public ReplaceHardwareResponse(string status, string[] inheritedDevices, string[] addedDevices, string[] addedDeviceIds, string[] removedDevices, RHSettingValue[] inheritedHardwareSettingValues, RHSettingValue[] defaultHardwareSettingValues, RHSettingValue[] inheritedHardwarePtzSettingValues, RHSettingValue[] defaultHardwarePtzSettingValues, RHSettingsDevice[] settingsDevices)
	{
		Status = status;
		InheritedDevices = inheritedDevices ?? new string[0];
		AddedDevices = addedDevices ?? new string[0];
		AddedDeviceIds = addedDeviceIds ?? new string[0];
		RemovedDevices = removedDevices ?? new string[0];
		InheritedHardwareSettingValues = inheritedHardwareSettingValues ?? new RHSettingValue[0];
		DefaultHardwareSettingValues = defaultHardwareSettingValues ?? new RHSettingValue[0];
		InheritedHardwarePtzSettingValues = inheritedHardwarePtzSettingValues ?? new RHSettingValue[0];
		DefaultHardwarePtzSettingValues = defaultHardwarePtzSettingValues ?? new RHSettingValue[0];
		SettingsDevices = settingsDevices ?? new RHSettingsDevice[0];
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
