using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class UpdateEvidenceLockSizesCommand : CommandBase, ICommandWithSource
{
	public Guid SourceId
	{
		get
		{
			if (!(EvidenceLockId == Guid.Empty))
			{
				return EvidenceLockId;
			}
			return typeof(UpdateEvidenceLockSizesCommand).GUID;
		}
	}

	public Guid EvidenceLockId { get; private set; }

	private UpdateEvidenceLockSizesCommand()
	{
	}

	public static UpdateEvidenceLockSizesCommand CreateAllEvidenceLockSizesCommand()
	{
		return new UpdateEvidenceLockSizesCommand();
	}

	public static UpdateEvidenceLockSizesCommand CreateSingleEvidenceLockSizesCommand(Guid evidenceLockId)
	{
		return new UpdateEvidenceLockSizesCommand
		{
			EvidenceLockId = evidenceLockId
		};
	}
}
