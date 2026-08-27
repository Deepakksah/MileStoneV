using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Management;

public class RecordingServerExt
{
	public Guid ServerId { get; set; }

	public string ServerName { get; set; }

	public List<RSCameraExt> Cameras { get; set; }

	public List<RSMobileUserExt> MobileUsers { get; set; }

	public RecordingServerExt()
	{
		Initialize();
	}

	public void Initialize()
	{
		Cameras = new List<RSCameraExt>();
		MobileUsers = new List<RSMobileUserExt>();
	}

	public void InitMobileUsers(List<MobileUserExt> mobileUsers)
	{
		foreach (MobileUserExt mobileUser in mobileUsers)
		{
			RSMobileUserExt rSMobileUserExt = null;
			foreach (UserConnectionExt userConnection in mobileUser.UserConnections)
			{
				if (userConnection.CameraParams.RecordingServerId == ServerId)
				{
					if (rSMobileUserExt == null)
					{
						rSMobileUserExt = new RSMobileUserExt(mobileUser);
					}
					rSMobileUserExt.InBandKB += userConnection.Connection.InputBandwidth.BandwidthInKB;
					rSMobileUserExt.OutBandKB += userConnection.Connection.OutputBandwidth.BandwidthInKB;
					switch (userConnection.Connection.GetConnectionType())
					{
					case UserConnection.ConnectionTypes.Live:
						rSMobileUserExt.LivesCount++;
						break;
					case UserConnection.ConnectionTypes.Playback:
						rSMobileUserExt.PlaybacksCount++;
						break;
					case UserConnection.ConnectionTypes.Upload:
						rSMobileUserExt.UploadsCount++;
						break;
					default:
						Logger.Instance.Log(LogContent.Error, "RecordingServerExt.InitMobileUsers", "Invalid conncetion type");
						break;
					}
				}
			}
			if (rSMobileUserExt != null)
			{
				MobileUsers.Add(rSMobileUserExt);
			}
		}
	}
}
