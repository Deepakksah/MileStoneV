using System;
using System.Globalization;
using VideoOS.Common.Integration.Command;
using VideoOS.Management.Common;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Communication.Internal;

public static class DtoConvert
{
	public static IdentityTypeDto ToIdentityTypeDto(IdentityType identityType)
	{
		return identityType switch
		{
			IdentityType.InvalidIdentity => IdentityTypeDto.InvalidIdentity, 
			IdentityType.UnknownIdentityType => IdentityTypeDto.UnknownIdentityType, 
			IdentityType.WindowsUser => IdentityTypeDto.WindowsUser, 
			IdentityType.WindowsGroup => IdentityTypeDto.WindowsGroup, 
			IdentityType.BasicUser => IdentityTypeDto.UnknownIdentityType, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", identityType)), 
		};
	}

	public static IdentityType ToIdentityType(IdentityTypeDto identityTypeDto)
	{
		return identityTypeDto switch
		{
			IdentityTypeDto.InvalidIdentity => IdentityType.InvalidIdentity, 
			IdentityTypeDto.UnknownIdentityType => IdentityType.UnknownIdentityType, 
			IdentityTypeDto.WindowsUser => IdentityType.WindowsUser, 
			IdentityTypeDto.WindowsGroup => IdentityType.WindowsGroup, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", identityTypeDto)), 
		};
	}

	public static MetaTypeDto ToMetaTypeDto(EventMetaData.MetaTypeEnum metaType)
	{
		return metaType switch
		{
			EventMetaData.MetaTypeEnum.Identifier => MetaTypeDto.Identifier, 
			EventMetaData.MetaTypeEnum.Data => MetaTypeDto.Data, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", metaType)), 
		};
	}

	public static EventMetaData.MetaTypeEnum ToMetaTypeEnum(MetaTypeDto metaTypeDto)
	{
		return metaTypeDto switch
		{
			MetaTypeDto.Data => EventMetaData.MetaTypeEnum.Data, 
			MetaTypeDto.Identifier => EventMetaData.MetaTypeEnum.Identifier, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", metaTypeDto)), 
		};
	}

	public static MetaDataTypeDto ToMetaDataTypeDto(EventMetaData.DataTypeEnum metaDataType)
	{
		return metaDataType switch
		{
			EventMetaData.DataTypeEnum.Unknown => MetaDataTypeDto.Unknown, 
			EventMetaData.DataTypeEnum.Bool => MetaDataTypeDto.Bool, 
			EventMetaData.DataTypeEnum.Int => MetaDataTypeDto.Int, 
			EventMetaData.DataTypeEnum.Double => MetaDataTypeDto.Double, 
			EventMetaData.DataTypeEnum.String => MetaDataTypeDto.String, 
			EventMetaData.DataTypeEnum.Guid => MetaDataTypeDto.Guid, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", metaDataType)), 
		};
	}

	public static EventMetaData.DataTypeEnum ToDataTypeEnum(MetaDataTypeDto metaDataTypeDto)
	{
		return metaDataTypeDto switch
		{
			MetaDataTypeDto.Bool => EventMetaData.DataTypeEnum.Bool, 
			MetaDataTypeDto.Double => EventMetaData.DataTypeEnum.Double, 
			MetaDataTypeDto.Guid => EventMetaData.DataTypeEnum.Guid, 
			MetaDataTypeDto.Int => EventMetaData.DataTypeEnum.Int, 
			MetaDataTypeDto.String => EventMetaData.DataTypeEnum.String, 
			MetaDataTypeDto.Unknown => EventMetaData.DataTypeEnum.Unknown, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", metaDataTypeDto)), 
		};
	}

	public static ActionTargetTypeDto ToActionTargetTypeDto(ActionTargetType actionTargetType)
	{
		return actionTargetType switch
		{
			ActionTargetType.Unspecified => ActionTargetTypeDto.Unspecified, 
			ActionTargetType.Device => ActionTargetTypeDto.Device, 
			ActionTargetType.MIP => ActionTargetTypeDto.MIP, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", actionTargetType)), 
		};
	}

	public static ActionTargetType ToActionTargetType(ActionTargetTypeDto actionTargetTypeDto)
	{
		return actionTargetTypeDto switch
		{
			ActionTargetTypeDto.Unspecified => ActionTargetType.Unspecified, 
			ActionTargetTypeDto.Device => ActionTargetType.Device, 
			ActionTargetTypeDto.MIP => ActionTargetType.MIP, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", actionTargetTypeDto)), 
		};
	}

	public static HardwareScanOperationDto2 ToHardwareScanOperationDto(HardwareScanOperation hardwareScanOperation)
	{
		return hardwareScanOperation switch
		{
			HardwareScanOperation.CancelScan => HardwareScanOperationDto2.CancelScan, 
			HardwareScanOperation.ExpressScan => HardwareScanOperationDto2.ExpressScan, 
			HardwareScanOperation.RangeScan => HardwareScanOperationDto2.RangeScan, 
			HardwareScanOperation.ManualScan => HardwareScanOperationDto2.ManualScan, 
			HardwareScanOperation.HostedScan => HardwareScanOperationDto2.HostedScan, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", hardwareScanOperation)), 
		};
	}

	public static HardwareScanOperation ToHardwareScanOperation(HardwareScanOperationDto2 hardwareScanOperationDto)
	{
		return hardwareScanOperationDto switch
		{
			HardwareScanOperationDto2.CancelScan => HardwareScanOperation.CancelScan, 
			HardwareScanOperationDto2.ExpressScan => HardwareScanOperation.ExpressScan, 
			HardwareScanOperationDto2.RangeScan => HardwareScanOperation.RangeScan, 
			HardwareScanOperationDto2.ManualScan => HardwareScanOperation.ManualScan, 
			HardwareScanOperationDto2.HostedScan => HardwareScanOperation.HostedScan, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", hardwareScanOperationDto)), 
		};
	}

	public static DatabaseResponseStatusDto ToDatabaseResponseStatusDto(DatabaseResponseStatus databaseResponseStatus)
	{
		return databaseResponseStatus switch
		{
			DatabaseResponseStatus.Success => DatabaseResponseStatusDto.Success, 
			DatabaseResponseStatus.Error => DatabaseResponseStatusDto.Error, 
			DatabaseResponseStatus.Warning => DatabaseResponseStatusDto.Warning, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", databaseResponseStatus)), 
		};
	}

	public static DatabaseResponseStatus ToDatabaseResponseStatus(DatabaseResponseStatusDto databaseResponseStatusDto)
	{
		return databaseResponseStatusDto switch
		{
			DatabaseResponseStatusDto.Success => DatabaseResponseStatus.Success, 
			DatabaseResponseStatusDto.Error => DatabaseResponseStatus.Error, 
			DatabaseResponseStatusDto.Warning => DatabaseResponseStatus.Warning, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", databaseResponseStatusDto)), 
		};
	}

	public static DatabaseOperationDto ToDatabaseOperationDto(DatabaseOperation databaseOperation)
	{
		return databaseOperation switch
		{
			DatabaseOperation.Undefined => DatabaseOperationDto.Undefined, 
			DatabaseOperation.VerifyPath => DatabaseOperationDto.VerifyPath, 
			DatabaseOperation.VerifyNewArchivePath => DatabaseOperationDto.VerifyNewArchivePath, 
			DatabaseOperation.DiskUsageInformationTablesPerStorage => DatabaseOperationDto.DiskUsageInformationTablesPerStorage, 
			DatabaseOperation.FilterTrackIdsToThoseWithoutMediaOnRecorder => DatabaseOperationDto.FilterTrackIdsToThoseWithoutMediaOnRecorder, 
			DatabaseOperation.VerifyStorageAndArchiveOnline => DatabaseOperationDto.VerifyStorageAndArchiveOnline, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", databaseOperation)), 
		};
	}

	public static DatabaseOperation ToDatabaseOperation(DatabaseOperationDto databaseOperationDto)
	{
		return databaseOperationDto switch
		{
			DatabaseOperationDto.Undefined => DatabaseOperation.Undefined, 
			DatabaseOperationDto.VerifyPath => DatabaseOperation.VerifyPath, 
			DatabaseOperationDto.VerifyNewArchivePath => DatabaseOperation.VerifyNewArchivePath, 
			DatabaseOperationDto.DiskUsageInformationTablesPerStorage => DatabaseOperation.DiskUsageInformationTablesPerStorage, 
			DatabaseOperationDto.FilterTrackIdsToThoseWithoutMediaOnRecorder => DatabaseOperation.FilterTrackIdsToThoseWithoutMediaOnRecorder, 
			DatabaseOperationDto.VerifyStorageAndArchiveOnline => DatabaseOperation.VerifyStorageAndArchiveOnline, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", databaseOperationDto)), 
		};
	}

	public static VideoWallOperationDto ToVideoWallOperationDto(VideoWallOperation videoWallOperation)
	{
		return videoWallOperation switch
		{
			VideoWallOperation.SetPreset => VideoWallOperationDto.SetPreset, 
			VideoWallOperation.SetMonitorCameras => VideoWallOperationDto.SetMonitorCameras, 
			VideoWallOperation.SetMonitorView => VideoWallOperationDto.SetMonitorView, 
			VideoWallOperation.SetMonitorText => VideoWallOperationDto.SetMonitorText, 
			VideoWallOperation.RemoveMonitorCameras => VideoWallOperationDto.RemoveMonitorCameras, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", videoWallOperation)), 
		};
	}

	public static VideoWallOperation ToVideoWallOperation(VideoWallOperationDto videoWallOperationDto)
	{
		return videoWallOperationDto switch
		{
			VideoWallOperationDto.SetPreset => VideoWallOperation.SetPreset, 
			VideoWallOperationDto.SetMonitorCameras => VideoWallOperation.SetMonitorCameras, 
			VideoWallOperationDto.SetMonitorView => VideoWallOperation.SetMonitorView, 
			VideoWallOperationDto.SetMonitorText => VideoWallOperation.SetMonitorText, 
			VideoWallOperationDto.RemoveMonitorCameras => VideoWallOperation.RemoveMonitorCameras, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", videoWallOperationDto)), 
		};
	}

	public static ServerFileSystemServerType ToServerFileSystemServerType(ServerFileSystemServerTypeDto serverFileSystemServerTypeDto)
	{
		return serverFileSystemServerTypeDto switch
		{
			ServerFileSystemServerTypeDto.RecordingServer => ServerFileSystemServerType.RecordingServer, 
			ServerFileSystemServerTypeDto.ManagementServer => ServerFileSystemServerType.ManagementServer, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", serverFileSystemServerTypeDto)), 
		};
	}

	public static ServerFileSystemServerTypeDto ToServerFileSystemServerTypeDto(ServerFileSystemServerType serverFileSystemServerType)
	{
		return serverFileSystemServerType switch
		{
			ServerFileSystemServerType.RecordingServer => ServerFileSystemServerTypeDto.RecordingServer, 
			ServerFileSystemServerType.ManagementServer => ServerFileSystemServerTypeDto.ManagementServer, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", serverFileSystemServerType)), 
		};
	}

	public static ServerFileSystemOperation ToServerFileSystemOperation(ServerFileSystemOperationDto serverFileSystemOperationDto)
	{
		return serverFileSystemOperationDto switch
		{
			ServerFileSystemOperationDto.QueryDrives => ServerFileSystemOperation.QueryDrives, 
			ServerFileSystemOperationDto.QueryDirectories => ServerFileSystemOperation.QueryDirectories, 
			ServerFileSystemOperationDto.CreateDirectory => ServerFileSystemOperation.CreateDirectory, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", serverFileSystemOperationDto)), 
		};
	}

	public static ServerFileSystemOperationDto ToServerFileSystemOperationDto(ServerFileSystemOperation serverFileSystemOperation)
	{
		return serverFileSystemOperation switch
		{
			ServerFileSystemOperation.QueryDrives => ServerFileSystemOperationDto.QueryDrives, 
			ServerFileSystemOperation.QueryDirectories => ServerFileSystemOperationDto.QueryDirectories, 
			ServerFileSystemOperation.CreateDirectory => ServerFileSystemOperationDto.CreateDirectory, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", serverFileSystemOperation)), 
		};
	}

	public static ServerFileSystemResponseStatus ToServerFileSystemResponseStatus(ServerFileSystemResponseStatusDto serverFileSystemResponseStatusDto)
	{
		return serverFileSystemResponseStatusDto switch
		{
			ServerFileSystemResponseStatusDto.Success => ServerFileSystemResponseStatus.Success, 
			ServerFileSystemResponseStatusDto.Error => ServerFileSystemResponseStatus.Error, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", serverFileSystemResponseStatusDto)), 
		};
	}

	public static ServerFileSystemResponseStatusDto ToServerFileSystemResponseStatusDto(ServerFileSystemResponseStatus serverFileSystemResponseStatus)
	{
		return serverFileSystemResponseStatus switch
		{
			ServerFileSystemResponseStatus.Success => ServerFileSystemResponseStatusDto.Success, 
			ServerFileSystemResponseStatus.Error => ServerFileSystemResponseStatusDto.Error, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Tried to convert an unsupported enumeration value. {0}", serverFileSystemResponseStatus)), 
		};
	}

	public static RecorderStatusesDto ToRecorderStatusesDto(RecorderStatuses status)
	{
		RecorderStatusesDto recorderStatusesDto = RecorderStatusesDto.None;
		if (status.HasFlag(RecorderStatuses.RepairingDatabase))
		{
			recorderStatusesDto |= RecorderStatusesDto.RepairingDatabase;
		}
		if (status.HasFlag(RecorderStatuses.ArchivingStorageOffline))
		{
			recorderStatusesDto |= RecorderStatusesDto.ArchivingStorageOffline;
		}
		if (status.HasFlag(RecorderStatuses.StorageOffline))
		{
			recorderStatusesDto |= RecorderStatusesDto.StorageOffline;
		}
		if (status.HasFlag(RecorderStatuses.Failover))
		{
			recorderStatusesDto |= RecorderStatusesDto.Failover;
		}
		if (status.HasFlag(RecorderStatuses.ArchivingStorageUnmounted))
		{
			recorderStatusesDto |= RecorderStatusesDto.ArchivingStorageUnmounted;
		}
		return recorderStatusesDto;
	}

	public static RecorderStatuses ToRecorderStatuses(RecorderStatusesDto status)
	{
		RecorderStatuses recorderStatuses = RecorderStatuses.None;
		if (status.HasFlag(RecorderStatusesDto.RepairingDatabase))
		{
			recorderStatuses |= RecorderStatuses.RepairingDatabase;
		}
		if (status.HasFlag(RecorderStatusesDto.ArchivingStorageOffline))
		{
			recorderStatuses |= RecorderStatuses.ArchivingStorageOffline;
		}
		if (status.HasFlag(RecorderStatusesDto.StorageOffline))
		{
			recorderStatuses |= RecorderStatuses.StorageOffline;
		}
		if (status.HasFlag(RecorderStatusesDto.Failover))
		{
			recorderStatuses |= RecorderStatuses.Failover;
		}
		if (status.HasFlag(RecorderStatusesDto.ArchivingStorageUnmounted))
		{
			recorderStatuses |= RecorderStatuses.ArchivingStorageUnmounted;
		}
		return recorderStatuses;
	}
}
