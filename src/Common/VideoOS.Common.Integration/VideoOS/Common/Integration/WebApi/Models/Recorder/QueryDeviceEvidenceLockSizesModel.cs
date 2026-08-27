using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class QueryDeviceEvidenceLockSizesModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid EvidenceLockId { get; set; }

	public static QueryDeviceEvidenceLockSizesModel FromCommand(QueryDeviceEvidenceLockSizesCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		return new QueryDeviceEvidenceLockSizesModel
		{
			EvidenceLockId = command.EvidenceLockId
		};
	}

	public QueryDeviceEvidenceLockSizesCommand ToCommand()
	{
		return new QueryDeviceEvidenceLockSizesCommand
		{
			EvidenceLockId = EvidenceLockId
		};
	}
}
