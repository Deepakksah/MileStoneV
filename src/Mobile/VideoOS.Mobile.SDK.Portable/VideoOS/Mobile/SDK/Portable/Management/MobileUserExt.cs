using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Management;

public class MobileUserExt
{
	public Guid UserId => MobileUser.UserId;

	public string UserName => MobileUser.UserName;

	public LoginState UserState => MobileUser.GetUserState();

	public EndPoint LocalEndPoint => MobileUser.LocalEndPoint;

	public EndPoint RemoteEndPoint => MobileUser.RemoteEndPoint;

	public long TotalVideoBandKB { get; set; }

	public double TotalVideoBandDecimalKB { get; set; }

	public long TotalAudioBandKB { get; set; }

	public double TotalAudioBandDecimalKB { get; set; }

	public long TotalBandKB => TotalVideoBandKB + TotalAudioBandKB;

	public double TotalBandDecimalKB => TotalVideoBandDecimalKB + TotalAudioBandDecimalKB;

	public long LivesCount { get; set; }

	public long PlaybacksCount { get; set; }

	public long UploadCount { get; set; }

	public long TranscodedVideoStreamsCnt { get; set; }

	public long FMp4VideoStreamsCnt { get; set; }

	public long TranscodedAudioStreamsCnt { get; set; }

	public MobileUser MobileUser { get; private set; }

	public List<UserConnectionExt> UserConnections { get; set; }

	public MobileUserExt()
	{
		Initialize();
	}

	public MobileUserExt(MobileUser mobileUser, List<Camera> cameras)
	{
		Initialize();
		MobileUser = mobileUser;
		foreach (UserConnection userConnection in mobileUser.UserConnections)
		{
			Camera camera = cameras.FirstOrDefault((Camera camera2) => camera2.CameraId == userConnection.ItemId);
			UserConnections.Add(new UserConnectionExt(userConnection, camera));
			if (userConnection.GetStreamDataType() == StreamParamsHelper.StreamDataType.Audio)
			{
				TotalAudioBandKB += userConnection.OutputBandwidth.BandwidthInKB;
				TotalAudioBandDecimalKB += userConnection.OutputBandwidth.BandwidthDecimalInKB;
			}
			else
			{
				TotalVideoBandKB += userConnection.OutputBandwidth.BandwidthInKB;
				TotalVideoBandDecimalKB += userConnection.OutputBandwidth.BandwidthDecimalInKB;
			}
			switch (userConnection.GetConnectionType())
			{
			case UserConnection.ConnectionTypes.Live:
				LivesCount++;
				break;
			case UserConnection.ConnectionTypes.Playback:
				PlaybacksCount++;
				break;
			case UserConnection.ConnectionTypes.Upload:
				UploadCount++;
				break;
			}
			switch (userConnection.GetStreamType())
			{
			case UserConnection.StreamType.Transcoded:
				if (userConnection.GetStreamDataType() == StreamParamsHelper.StreamDataType.Audio)
				{
					TranscodedAudioStreamsCnt++;
				}
				else
				{
					TranscodedVideoStreamsCnt++;
				}
				break;
			case UserConnection.StreamType.fMP4:
				FMp4VideoStreamsCnt++;
				break;
			}
		}
	}

	public void Initialize()
	{
		UserConnections = new List<UserConnectionExt>();
		TotalAudioBandKB = 0L;
		TotalAudioBandDecimalKB = 0.0;
		TotalVideoBandKB = 0L;
		TotalVideoBandDecimalKB = 0.0;
		LivesCount = 0L;
		PlaybacksCount = 0L;
		UploadCount = 0L;
		TranscodedAudioStreamsCnt = 0L;
		TranscodedVideoStreamsCnt = 0L;
		FMp4VideoStreamsCnt = 0L;
	}
}
