using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.WebApi.Models.Recorder;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class TriggerEventCommandPackageDto
{
	[Required]
	public TriggerEventDto CommandBase { get; private set; }

	[Required]
	public CommunicationCallContextDto CallContextInfo { get; private set; }

	public TriggerEventCommandPackageDto(CommandBase commandBase, CommunicationCallContextDto callContextInfo)
	{
		if (!(commandBase is TriggerCustomEventCommand triggerCustomEventCommand))
		{
			throw new ArgumentNullException("commandBase");
		}
		CommandBase = TriggerEventDto.Create(triggerCustomEventCommand.TriggerEvent);
		CallContextInfo = callContextInfo;
	}

	public static Collection<CommandPackage> CreateCommandPackages(TriggerEventCommandPackageDto[] commandPackagesDto)
	{
		if (commandPackagesDto == null)
		{
			throw new ArgumentNullException("commandPackagesDto");
		}
		Collection<CommandPackage> collection = new Collection<CommandPackage>();
		foreach (TriggerEventCommandPackageDto triggerEventCommandPackageDto in commandPackagesDto)
		{
			TriggerCustomEventCommand commandBase = new TriggerCustomEventCommand(TriggerEventDto.CreateTriggerEvent(triggerEventCommandPackageDto.CommandBase));
			if (triggerEventCommandPackageDto.CallContextInfo == null)
			{
				throw new ArgumentException("commandPackagesDto contains commands without callcontext");
			}
			collection.Add(new CommandPackage(commandBase, new BasicCallContextInfo(triggerEventCommandPackageDto.CallContextInfo.UserSid, triggerEventCommandPackageDto.CallContextInfo.UserDomainAccountName, triggerEventCommandPackageDto.CallContextInfo.LocationIPAddress)));
		}
		return collection;
	}

	public static TriggerCustomEventBulkCommand CreateCommand(BulkEventModel command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		return new TriggerCustomEventBulkCommand(CreateCommandPackages(command.EventCommands.ToArray()))
		{
			Token = command.Token
		};
	}
}
