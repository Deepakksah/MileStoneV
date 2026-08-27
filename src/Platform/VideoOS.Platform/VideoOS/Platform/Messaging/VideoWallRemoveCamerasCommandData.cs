using System.Collections.ObjectModel;

namespace VideoOS.Platform.Messaging;

public class VideoWallRemoveCamerasCommandData
{
	public Collection<FQID> CameraFQIDList { get; set; }
}
