using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication.Internal;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class DatabaseResponseTrackIdsWithoutMediaOnRecorderDto : DatabaseResponseDto
{
	public Guid[] TrackIds { get; private set; }

	public static DatabaseResponseTrackIdsWithoutMediaOnRecorderDto Create(DatabaseResponseTrackIdsWithoutMediaOnRecorder response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		DatabaseResponseTrackIdsWithoutMediaOnRecorderDto databaseResponseTrackIdsWithoutMediaOnRecorderDto = new DatabaseResponseTrackIdsWithoutMediaOnRecorderDto();
		databaseResponseTrackIdsWithoutMediaOnRecorderDto.WriteDto(response);
		databaseResponseTrackIdsWithoutMediaOnRecorderDto.TrackIds = response.TrackIds;
		return databaseResponseTrackIdsWithoutMediaOnRecorderDto;
	}

	public static DatabaseResponseTrackIdsWithoutMediaOnRecorder CreateDatabaseResponseTrackIdsWithoutMediaOnRecorder(DatabaseResponseTrackIdsWithoutMediaOnRecorderDto dto)
	{
		if (dto == null)
		{
			throw new ArgumentNullException("dto");
		}
		DatabaseResponseStatus status = DtoConvert.ToDatabaseResponseStatus(dto.Status);
		return new DatabaseResponseTrackIdsWithoutMediaOnRecorder(dto.TrackIds, status, dto.Message);
	}
}
