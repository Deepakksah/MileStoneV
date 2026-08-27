using System;
using System.Diagnostics;
using VideoOS.Management.Common;
using VideoOS.Vms.Types;

namespace VideoOS.Common.Integration.ServiceController;

[Serializable]
[DebuggerDisplay("RecorderWatchItem, Id: {RecorderId}; Position: {Position}; Errors: {Errors}")]
public class RecorderWatchItem
{
	public Guid RecorderId { get; set; }

	public string RecorderEndPoint { get; set; }

	public int Position { get; set; }

	public RecorderWatchItemErrors Errors { get; set; }

	public RecorderWatchItem(Guid recorderId, string recorderEndPoint, int position, RecorderWatchItemErrors errors)
	{
		RecorderId = recorderId;
		RecorderEndPoint = recorderEndPoint;
		Position = position;
		Errors = errors;
	}

	public RecorderWatchItemDto ToDto()
	{
		return new RecorderWatchItemDto
		{
			RecorderId = RecorderId,
			RecorderEndPoint = RecorderEndPoint,
			Position = Position,
			Errors = (CommunicationErrors)Errors
		};
	}

	public static RecorderWatchItem FromDto(RecorderWatchItemDto dto)
	{
		if (dto == null)
		{
			throw new ArgumentNullException("dto");
		}
		return new RecorderWatchItem(dto.RecorderId, dto.RecorderEndPoint, dto.Position, (RecorderWatchItemErrors)dto.Errors);
	}
}
