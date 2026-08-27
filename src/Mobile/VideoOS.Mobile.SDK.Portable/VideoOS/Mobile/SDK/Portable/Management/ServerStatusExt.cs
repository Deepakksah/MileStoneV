using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Management;

public class ServerStatusExt
{
	[Flags]
	public enum ParseData
	{
		MobileUsers = 0,
		RecordingServers = 1,
		All = 1
	}

	public DateTime UpSince { get; set; }

	public ServerVersion ServerVersion { get; set; }

	public long CpuUsagePercentage { get; set; }

	public long ThreadsCount { get; set; }

	public long InputBandKB { get; set; }

	public double InputBandDecimalKB { get; set; }

	public long OutputBandKB { get; set; }

	public double OutputBandDecimalKB { get; set; }

	public string ExternalIp { get; set; }

	public List<MobileUserExt> MobileUsers { get; set; }

	public List<RecordingServerExt> RecordingServers { get; set; }

	public ServerStatusExt()
	{
		Initialize();
	}

	public ServerStatusExt(ServerStatus status, ParseData flags = ParseData.RecordingServers)
	{
		Initialize();
		if (CheckFlags(flags, ParseData.MobileUsers))
		{
			InitMobileUsers(status);
		}
		if (CheckFlags(flags, ParseData.RecordingServers))
		{
			InitRecordingSevres(status);
		}
		InitInputBand();
		ExternalIp = status.ExternalIp;
	}

	public void Initialize()
	{
		MobileUsers = new List<MobileUserExt>();
		RecordingServers = new List<RecordingServerExt>();
		InputBandKB = 0L;
		InputBandDecimalKB = 0.0;
		OutputBandKB = 0L;
		OutputBandDecimalKB = 0.0;
	}

	public bool CheckFlags(ParseData flags, ParseData mask)
	{
		return (flags & mask) == mask;
	}

	public void InitMobileUsers(ServerStatus status)
	{
		UpSince = status.UpSince;
		ServerVersion = status.ServerVersion;
		CpuUsagePercentage = status.CpuUsagePercentage;
		ThreadsCount = status.ThreadsCount;
		foreach (MobileUser mobileUser in status.MobileUsers)
		{
			MobileUserExt mobileUserExt = new MobileUserExt(mobileUser, status.Cameras);
			MobileUsers.Add(mobileUserExt);
			OutputBandKB += mobileUserExt.TotalBandKB;
			OutputBandDecimalKB += mobileUserExt.TotalBandDecimalKB;
		}
	}

	public void InitRecordingSevres(ServerStatus status)
	{
		InitRSCameras(status);
		InitRSMobileUsers();
	}

	public void InitRSCameras(ServerStatus status)
	{
		foreach (Camera camera in status.Cameras)
		{
			RecordingServerExt recordingServerExt = RecordingServers.First((RecordingServerExt server) => server.ServerId == camera.RecordingServerId);
			if (recordingServerExt == null)
			{
				recordingServerExt = new RecordingServerExt
				{
					ServerId = camera.RecordingServerId,
					ServerName = camera.RecordingServerName
				};
				RecordingServers.Add(recordingServerExt);
			}
			RSCameraExt item = new RSCameraExt(camera, status);
			recordingServerExt.Cameras.Add(item);
		}
	}

	public void InitRSMobileUsers()
	{
		foreach (RecordingServerExt recordingServer in RecordingServers)
		{
			recordingServer.InitMobileUsers(MobileUsers);
		}
	}

	public void InitInputBand()
	{
		List<Guid> list = new List<Guid>();
		foreach (MobileUserExt mobileUser in MobileUsers)
		{
			foreach (UserConnectionExt userConnection in mobileUser.UserConnections)
			{
				UserConnection connection = userConnection.Connection;
				if (connection.GetConnectionType() == UserConnection.ConnectionTypes.Playback || !list.Contains(connection.ItemId))
				{
					InputBandKB += connection.InputBandwidth.BandwidthInKB;
					InputBandDecimalKB += connection.InputBandwidth.BandwidthDecimalInKB;
					if (connection.GetConnectionType() == UserConnection.ConnectionTypes.Live)
					{
						list.Add(connection.ItemId);
					}
				}
			}
		}
	}
}
