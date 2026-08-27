using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.AudioSource;

public class StreamSourceSettings : IStreamSourceSettings
{
	public static StreamSourceSettings Instance { get; private set; } = new StreamSourceSettings();

	public AudioCorrection AudioCorrection { get; set; }

	public int AudioCompressionLevel { get; set; }

	private StreamSourceSettings()
	{
	}

	public void InitializeFromConfig(string correctionSetting, int compressionLevel)
	{
		Enum.TryParse<AudioCorrection>(correctionSetting, out var result);
		AudioCorrection = result;
		AudioCompressionLevel = compressionLevel;
		Log.Debug("MainThread.RunInitialize", $"Initialized audio settings: Correction {Instance.AudioCorrection};" + $" Compression Level {Instance.AudioCompressionLevel};");
	}
}
