using System;

namespace VideoOS.Management.VmoClient;

public class VideoWallCollection : VmoObjectGuidBasedCollection<VideoWall>
{
	public VideoWallCollection(LoadMethod loadVideoWalls, VmoObject parent, bool isInitialized)
		: base((Func<bool, VideoWall[]>)loadVideoWalls.Invoke, parent, isInitialized)
	{
	}
}
