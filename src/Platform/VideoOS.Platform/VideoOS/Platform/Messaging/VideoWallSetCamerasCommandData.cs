using System.Collections.ObjectModel;

namespace VideoOS.Platform.Messaging;

public class VideoWallSetCamerasCommandData
{
	public Collection<FQID> CameraFQIDList { get; set; }

	public int Position { get; set; }
}
