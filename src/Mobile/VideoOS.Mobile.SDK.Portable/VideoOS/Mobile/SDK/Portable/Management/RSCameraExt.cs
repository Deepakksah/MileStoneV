using System;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Management;

public class RSCameraExt
{
	public Guid CameraId { get; set; }

	public string CameraName { get; set; }

	public int LivesCount { get; set; }

	public int PlaybacksCount { get; set; }

	protected long UploadsCount { get; set; }

	public long BandKB { get; set; }

	public RSCameraExt()
	{
		Initialize();
	}

	public RSCameraExt(Camera camera, ServerStatus status)
	{
		Initialize();
		CameraId = camera.CameraId;
		CameraName = camera.CameraName;
		foreach (MobileUser mobileUser in status.MobileUsers)
		{
			foreach (UserConnection userConnection in mobileUser.UserConnections)
			{
				if (userConnection.ItemId == CameraId)
				{
					BandKB += userConnection.InputBandwidth.BandwidthInKB;
					switch (userConnection.GetConnectionType())
					{
					case UserConnection.ConnectionTypes.Live:
						LivesCount++;
						break;
					case UserConnection.ConnectionTypes.Playback:
						PlaybacksCount++;
						break;
					case UserConnection.ConnectionTypes.Upload:
						UploadsCount++;
						break;
					default:
						Logger.Instance.Log(LogContent.Error, "RSCameraExt.RSCameraExt", "Wrong connection type");
						break;
					}
				}
			}
		}
	}

	public void Initialize()
	{
		LivesCount = 0;
		PlaybacksCount = 0;
		UploadsCount = 0L;
		BandKB = 0L;
	}
}
