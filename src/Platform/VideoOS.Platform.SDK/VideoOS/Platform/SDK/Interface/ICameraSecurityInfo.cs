namespace VideoOS.Platform.SDK.Interface;

public interface ICameraSecurityInfo : IDeviceSecurityInfo
{
	bool ExportAvi { get; set; }

	bool ExportJpeg { get; set; }

	bool SmartSearch { get; set; }

	IPtzSecurityInfo PtzSecurity { get; set; }

	bool ManagePresets { get; set; }

	bool ManagePatrollingProfiles { get; set; }
}
