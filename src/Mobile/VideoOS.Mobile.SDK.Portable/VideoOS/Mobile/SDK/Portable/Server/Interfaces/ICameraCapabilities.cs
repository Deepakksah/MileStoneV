namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface ICameraCapabilities : IStreamingDeviceCapabilities
{
	bool PtzEnabled { get; }

	bool GetPresetsEnabled { get; }

	bool ExportAviEnabled { get; }

	bool ExportDatabaseEnabled { get; }

	bool ExportJpegEnabled { get; }

	bool GetSequencesEnabled { get; }
}
