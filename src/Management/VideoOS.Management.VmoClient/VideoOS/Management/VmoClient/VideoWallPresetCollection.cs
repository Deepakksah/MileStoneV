using System;

namespace VideoOS.Management.VmoClient;

public class VideoWallPresetCollection : VmoObjectGuidBasedCollection<VideoWallPreset>
{
	public VideoWallPresetCollection(LoadMethod loadVideoWallPresets, VmoObject parent, bool isInitialized)
		: base((Func<bool, VideoWallPreset[]>)loadVideoWallPresets.Invoke, parent, isInitialized)
	{
	}
}
