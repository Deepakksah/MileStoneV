using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication.Internal;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class DatabaseResponseArchiveInformationDto : DatabaseResponseDto
{
	public Guid Id;

	public byte[] ArchiveInfo;

	public static DatabaseResponseArchiveInformationDto Create(DatabaseResponseArchiveInformation response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		DatabaseResponseArchiveInformationDto databaseResponseArchiveInformationDto = new DatabaseResponseArchiveInformationDto();
		databaseResponseArchiveInformationDto.WriteDto(response);
		databaseResponseArchiveInformationDto.Id = response.ID;
		databaseResponseArchiveInformationDto.ArchiveInfo = response.ArchiveInfo;
		return databaseResponseArchiveInformationDto;
	}

	public static DatabaseResponseArchiveInformation CreateDatabaseResponseArchiveInformation(DatabaseResponseArchiveInformationDto dto)
	{
		if (dto == null)
		{
			throw new ArgumentNullException("dto");
		}
		return new DatabaseResponseArchiveInformation(DtoConvert.ToDatabaseOperation(dto.DatabaseOperationType), status: DtoConvert.ToDatabaseResponseStatus(dto.Status), id: dto.Id, archiveInfo: dto.ArchiveInfo, message: dto.Message);
	}
}
