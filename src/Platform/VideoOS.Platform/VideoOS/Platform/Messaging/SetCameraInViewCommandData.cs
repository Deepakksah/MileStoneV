using System;

namespace VideoOS.Platform.Messaging;

public class SetCameraInViewCommandData
{
	public FQID CameraFQID;

	public int Index;

	public Guid StreamId;

	public SetCameraInViewCommandData()
	{
		StreamId = Guid.Empty;
	}
}
