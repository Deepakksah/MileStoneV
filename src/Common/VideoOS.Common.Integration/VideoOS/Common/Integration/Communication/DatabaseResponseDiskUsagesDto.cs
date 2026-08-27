using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication.Internal;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class DatabaseResponseDiskUsagesDto : DatabaseResponseDto
{
	public DatabaseResponseDiskUsageDto[] DatabaseResponseDiskUsageList;

	public static DatabaseResponseDiskUsagesDto Create(DatabaseResponseDiskUsages response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		DatabaseResponseDiskUsagesDto databaseResponseDiskUsagesDto = new DatabaseResponseDiskUsagesDto();
		databaseResponseDiskUsagesDto.WriteDto(response);
		if (response.DatabaseResponseDiskUsageList != null)
		{
			databaseResponseDiskUsagesDto.DatabaseResponseDiskUsageList = new DatabaseResponseDiskUsageDto[response.DatabaseResponseDiskUsageList.Length];
			for (int i = 0; i < response.DatabaseResponseDiskUsageList.Length; i++)
			{
				databaseResponseDiskUsagesDto.DatabaseResponseDiskUsageList[i] = DatabaseResponseDiskUsageDto.Create(response.DatabaseResponseDiskUsageList[i]);
			}
		}
		return databaseResponseDiskUsagesDto;
	}

	public static DatabaseResponseDiskUsages CreateDatabaseResponseDiskUsages(DatabaseResponseDiskUsagesDto dto)
	{
		if (dto == null)
		{
			throw new ArgumentNullException("dto");
		}
		DatabaseResponseDiskUsage[] diskUsage = CreateDatabaseResponseDiskUsageArray(dto);
		DatabaseOperation databaseOperation = DtoConvert.ToDatabaseOperation(dto.DatabaseOperationType);
		DatabaseResponseStatus status = DtoConvert.ToDatabaseResponseStatus(dto.Status);
		return new DatabaseResponseDiskUsages(databaseOperation, diskUsage, status, dto.Message);
	}

	private static DatabaseResponseDiskUsage[] CreateDatabaseResponseDiskUsageArray(DatabaseResponseDiskUsagesDto dto)
	{
		if (dto.DatabaseResponseDiskUsageList == null)
		{
			return null;
		}
		List<DatabaseResponseDiskUsage> list = new List<DatabaseResponseDiskUsage>();
		for (int i = 0; i < dto.DatabaseResponseDiskUsageList.Length; i++)
		{
			DatabaseResponseDiskUsage item = DatabaseResponseDiskUsageDto.CreateDatabaseResponseDiskUsage(dto.DatabaseResponseDiskUsageList[i]);
			list.Add(item);
		}
		return list.ToArray();
	}
}
