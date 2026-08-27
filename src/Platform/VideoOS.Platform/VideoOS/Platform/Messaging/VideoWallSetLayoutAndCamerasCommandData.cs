using System.Collections.ObjectModel;

namespace VideoOS.Platform.Messaging;

public class VideoWallSetLayoutAndCamerasCommandData
{
	public FQID LayoutFQID { get; set; }

	public Collection<FQID> CameraFQIDList { get; set; }

	public int Position { get; set; }
}
