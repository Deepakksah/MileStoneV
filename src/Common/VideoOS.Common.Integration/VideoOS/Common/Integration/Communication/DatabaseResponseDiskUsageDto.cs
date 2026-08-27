using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication.Internal;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class DatabaseResponseDiskUsageDto : DatabaseResponseDto
{
	public Guid Id { get; set; }

	public Guid RecorderId { get; set; }

	public Guid StorageId { get; set; }

	public ulong UsedSpace { get; set; }

	public ulong AvailableSpace { get; set; }

	public bool IsOnline { get; set; }

	public static DatabaseResponseDiskUsageDto Create(DatabaseResponseDiskUsage response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		DatabaseResponseDiskUsageDto databaseResponseDiskUsageDto = new DatabaseResponseDiskUsageDto();
		databaseResponseDiskUsageDto.WriteDto(response);
		databaseResponseDiskUsageDto.Id = response.Id;
		databaseResponseDiskUsageDto.RecorderId = response.RecorderId;
		databaseResponseDiskUsageDto.StorageId = response.StorageId;
		databaseResponseDiskUsageDto.UsedSpace = response.UsedSpace;
		databaseResponseDiskUsageDto.AvailableSpace = response.AvailableSpace;
		databaseResponseDiskUsageDto.IsOnline = response.IsOnline;
		return databaseResponseDiskUsageDto;
	}

	public static DatabaseResponseDiskUsage CreateDatabaseResponseDiskUsage(DatabaseResponseDiskUsageDto dto)
	{
		if (dto == null)
		{
			throw new ArgumentNullException("dto");
		}
		return new DatabaseResponseDiskUsage(DtoConvert.ToDatabaseOperation(dto.DatabaseOperationType), status: DtoConvert.ToDatabaseResponseStatus(dto.Status), id: dto.Id, usedSpace: dto.UsedSpace, availableSpace: dto.AvailableSpace, message: dto.Message, storageId: dto.StorageId, recorderId: dto.RecorderId, isOnline: dto.IsOnline);
	}
}
