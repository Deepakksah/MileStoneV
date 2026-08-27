using System.Collections.Generic;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class CameraCapabilities : StreamingDeviceCapabilities, ICameraCapabilities, IStreamingDeviceCapabilities, ICapabilities
{
	public bool PtzEnabled { get; private set; }

	public bool GetPresetsEnabled { get; private set; }

	public bool ExportAviEnabled { get; private set; }

	public bool ExportDatabaseEnabled { get; private set; }

	public bool ExportJpegEnabled { get; private set; }

	public bool GetSequencesEnabled { get; private set; }

	public CameraCapabilities()
	{
		InitDefaultValues();
	}

	private void InitDefaultValues()
	{
		PtzEnabled = false;
		GetPresetsEnabled = false;
		ExportAviEnabled = false;
		ExportDatabaseEnabled = false;
		ExportJpegEnabled = false;
		GetSequencesEnabled = false;
	}

	public override void InitFromCommand(Dictionary<string, string> deviceParams)
	{
		base.InitFromCommand(deviceParams);
		PtzEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "PTZ", PtzEnabled);
		GetPresetsEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "Preset", GetPresetsEnabled);
		ExportAviEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "ExportAvi", ExportAviEnabled);
		ExportDatabaseEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "ExportDatabase", ExportDatabaseEnabled);
		ExportJpegEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "ExportJpeg", ExportJpegEnabled);
		GetSequencesEnabled = CapabilitiesHelper.ExtractBool(deviceParams, "Sequences", GetSequencesEnabled);
		CapabilitiesHelper.CopyDictionary(deviceParams, _capabilities);
	}
}
