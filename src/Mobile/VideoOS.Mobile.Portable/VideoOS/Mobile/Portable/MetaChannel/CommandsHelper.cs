using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class CommandsHelper
{
	public static readonly Command.CommandKinds[] AlarmsCommandsGroup = new Command.CommandKinds[5]
	{
		Command.CommandKinds.GetAlarmList,
		Command.CommandKinds.UpdateAlarm,
		Command.CommandKinds.GetAlarmDataSettings,
		Command.CommandKinds.GetPermittedUsers,
		Command.CommandKinds.AcknowledgeAlarm
	};

	public static readonly Command.CommandKinds[] UpdateInvestigationGroup = new Command.CommandKinds[2]
	{
		Command.CommandKinds.UpdateInvestigation,
		Command.CommandKinds.CancelInvestigationUpdate
	};

	public static readonly Command.CommandKinds[] DisconnectRelatedGroup = new Command.CommandKinds[20]
	{
		Command.CommandKinds.RequestActivation,
		Command.CommandKinds.CreateBookmark,
		Command.CommandKinds.UpdateBookmark,
		Command.CommandKinds.DeleteBookmark,
		Command.CommandKinds.ControlPTZ,
		Command.CommandKinds.UpdateAlarm,
		Command.CommandKinds.AcknowledgeAlarm,
		Command.CommandKinds.ExecuteAccessControlCommand,
		Command.CommandKinds.RegisterForNotifications,
		Command.CommandKinds.SetRegisteredDevicesState,
		Command.CommandKinds.SetServerConfiguration,
		Command.CommandKinds.SendInvitationInternal,
		Command.CommandKinds.CreateInvestigation,
		Command.CommandKinds.DeleteInvestigation,
		Command.CommandKinds.UpdateInvestigation,
		Command.CommandKinds.CancelInvestigationUpdate,
		Command.CommandKinds.UpdateInvestigationData,
		Command.CommandKinds.StartInvestigationExport,
		Command.CommandKinds.DeleteInvestigationExport,
		Command.CommandKinds.GetTileServers
	};

	public static readonly Dictionary<Command.CommandKinds, CommandMetaData> CommandsMetaData = new Dictionary<Command.CommandKinds, CommandMetaData>
	{
		{
			Command.CommandKinds.Connect,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.Disconnect,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				CommandPriority = CommandPriority.High,
				RelatedCommands = DisconnectRelatedGroup
			}
		},
		{
			Command.CommandKinds.LogIn,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = new Command.CommandKinds[0]
			}
		},
		{
			Command.CommandKinds.RequestStream,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "SignalType" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.ChangeStream,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "VideoId" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = new Command.CommandKinds[0]
			}
		},
		{
			Command.CommandKinds.CloseStream,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "VideoId" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.RequestAudioStream,
			new CommandMetaData
			{
				RequiredInputParams = new string[2] { "SignalType", "MethodType" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetViews,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "ViewId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAllViewsAndCameras,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetThumbnail,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "CameraId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetThumbnailByTime,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "CameraId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.SyncOnly
			}
		},
		{
			Command.CommandKinds.GetServerStatus,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetSequences,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = new Command.CommandKinds[0]
			}
		},
		{
			Command.CommandKinds.GetPtzPresets,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetOutputsAndEvents,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "CameraId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.RequestActivation,
			new CommandMetaData
			{
				RequiredInputParams = new string[2] { "ObjectId", "TriggerType" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible,
				CommandPriority = CommandPriority.High
			}
		},
		{
			Command.CommandKinds.CreateBookmark,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible,
				MaxParameterLengths = new Dictionary<string, int>
				{
					["Name"] = 50,
					["Description"] = 10000
				},
				UnsanitizedParameters = new List<string> { "Name", "Description" }
			}
		},
		{
			Command.CommandKinds.UpdateBookmark,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible,
				CommandPriority = CommandPriority.Low,
				MaxParameterLengths = new Dictionary<string, int>
				{
					["Name"] = 50,
					["Description"] = 10000
				},
				UnsanitizedParameters = new List<string> { "Name", "Description" }
			}
		},
		{
			Command.CommandKinds.DeleteBookmark,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible,
				CommandPriority = CommandPriority.Low
			}
		},
		{
			Command.CommandKinds.GetExport,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "ExportId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetConfiguration,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetCapabilities,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.ControlPTZ,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "CameraId" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = new Command.CommandKinds[0],
				CommandPriority = CommandPriority.High
			}
		},
		{
			Command.CommandKinds.GetRecordingTime,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "CameraId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.CreateInvestigation,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible,
				MaxParameterLengths = new Dictionary<string, int> { ["Name"] = 80 }
			}
		},
		{
			Command.CommandKinds.UpdateInvestigation,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "ItemId" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = UpdateInvestigationGroup,
				MaxParameterLengths = new Dictionary<string, int> { ["Name"] = 80 }
			}
		},
		{
			Command.CommandKinds.CancelInvestigationUpdate,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "ItemId" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = UpdateInvestigationGroup
			}
		},
		{
			Command.CommandKinds.UpdateInvestigationData,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "ItemId" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible,
				MaxParameterLengths = new Dictionary<string, int> { ["Name"] = 80 }
			}
		},
		{
			Command.CommandKinds.GetInvestigation,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "ItemId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.DeleteInvestigation,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "ItemId" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.StartInvestigationExport,
			new CommandMetaData
			{
				RequiredInputParams = new string[2] { "InvestigationId", "ExportType" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.DeleteInvestigationExport,
			new CommandMetaData
			{
				RequiredInputParams = new string[2] { "InvestigationId", "ExportType" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.SetServerConfiguration,
			new CommandMetaData
			{
				RequiredInputParams = new string[3] { "ServerId", "Slc", "SmartConnectEnabled" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible,
				CommandPriority = CommandPriority.High
			}
		},
		{
			Command.CommandKinds.CheckServerAccessibility,
			new CommandMetaData
			{
				RequiredInputParams = new string[3] { "ServerId", "Slc", "Timestamp" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetServerConfiguration,
			new CommandMetaData
			{
				RequiredInputParams = new string[2] { "ServerId", "DeviceId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.SendInvitationInternal,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "Emails" },
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible,
				CommandPriority = CommandPriority.High
			}
		},
		{
			Command.CommandKinds.GetChangesStateId,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.RequestSecondStepVerificationCode,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.VerifySecondStepVerificationCode,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "SecondStepAuthenticationPin" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetItems,
			new CommandMetaData
			{
				RequiredInputParams = new string[1] { "ItemKind" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAlarmList,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = AlarmsCommandsGroup
			}
		},
		{
			Command.CommandKinds.UpdateAlarm,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = AlarmsCommandsGroup,
				CommandPriority = CommandPriority.High
			}
		},
		{
			Command.CommandKinds.GetAlarmDataSettings,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = AlarmsCommandsGroup
			}
		},
		{
			Command.CommandKinds.GetPermittedUsers,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = AlarmsCommandsGroup
			}
		},
		{
			Command.CommandKinds.AcknowledgeAlarm,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = AlarmsCommandsGroup,
				CommandPriority = CommandPriority.High
			}
		},
		{
			Command.CommandKinds.RegisterForNotifications,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = new Command.CommandKinds[0],
				CommandPriority = CommandPriority.High,
				MaxParameterLengths = new Dictionary<string, int> { ["X"] = 500 }
			}
		},
		{
			Command.CommandKinds.SetRegisteredDevicesState,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = new Command.CommandKinds[0],
				CommandPriority = CommandPriority.High
			}
		},
		{
			Command.CommandKinds.GetAccessControlEvents,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAccessRequestEvents,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAccessControlDoors,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAccessControlDoorStates,
			new CommandMetaData
			{
				RequiredInputParams = new string[2] { "AccessControlSystemId", "AccessControlDoorId" },
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAccessRequestEventTypes,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAccessControlSystems,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAccessControlDoorCategories,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAccessControlDoorTypes,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.GetAccessControlEventCategories,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		},
		{
			Command.CommandKinds.ExecuteAccessControlCommand,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = false,
				ProcessingContext = ProcessingContext.SyncOnly,
				RelatedCommands = new Command.CommandKinds[0],
				CommandPriority = CommandPriority.High
			}
		},
		{
			Command.CommandKinds.GetTileServers,
			new CommandMetaData
			{
				RequiredInputParams = new string[0],
				CanSkipIfChannelIsClosed = true,
				ProcessingContext = ProcessingContext.AsyncPossible
			}
		}
	};

	public static CommandMetaData GetCommandMetaData(Command.CommandKinds commandKind)
	{
		if (CommandsMetaData.ContainsKey(commandKind))
		{
			return CommandsMetaData[commandKind];
		}
		CommandMetaData commandMetaData = new CommandMetaData();
		commandMetaData.RequiredInputParams = new string[0];
		commandMetaData.CanSkipIfChannelIsClosed = false;
		commandMetaData.ProcessingContext = ProcessingContext.SyncOnly;
		commandMetaData.CommandPriority = CommandPriority.Low;
		commandMetaData.RelatedCommands = new Command.CommandKinds[0];
		commandMetaData.MaxParameterLengths = new Dictionary<string, int>();
		return commandMetaData;
	}

	public static bool DoesCommandHasRequiredParams(Command.CommandKinds commandKind)
	{
		return GetCommandMetaData(commandKind).RequiredInputParams.Any();
	}

	public static ErrorCodes CheckCommandCommonParams(CommunicationEntry tComEntry, string szChannelID = null)
	{
		try
		{
			if (tComEntry.Command == null)
			{
				return ErrorCodes.Unknown;
			}
			Command command = tComEntry.Command;
			command.SetCommandResult(Command.CommandResultTypes.ResultError);
			if (command.GetCommandType() != Command.CommandTypes.Command)
			{
				return ErrorCodes.BadCommandType;
			}
			if (command.GetCommandKind() == Command.CommandKinds.Unknown)
			{
				return ErrorCodes.BadCommandKind;
			}
			if (command.GetCommandKind() != Command.CommandKinds.Connect && tComEntry.ConnectionId != szChannelID)
			{
				command.ErrorString = "Wrong connection ID";
				return ErrorCodes.WrongConnectionID;
			}
			Dictionary<string, string> dictionary = command.GetDictionary(Command.ParamsType.Input);
			foreach (string requiredInputParam in GetCommandMetaData(command.GetCommandKind()).RequiredInputParams)
			{
				if (!dictionary.ContainsKey(requiredInputParam))
				{
					command.ErrorString = "Missing mandatory input parameter:  : " + requiredInputParam;
					Logger.Instance.Log(LogContent.Error, "CommunicationCommands.CheckCommandCommonParams - foreach (string szMadratoryFld) ", "Missing field: " + requiredInputParam);
					return ErrorCodes.MissingInputParameter;
				}
			}
			command.SetCommandResult(Command.CommandResultTypes.ResultOk);
			return ErrorCodes.Ok;
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CommunicationCommands.CheckCommandCommonParams", ex.Message);
			return ErrorCodes.Unknown;
		}
	}

	public static void SetAuthorization(Dictionary<string, string> dictionary, string param, bool enabled)
	{
		dictionary[param] = (enabled ? "Yes" : "No");
	}

	public static bool GetAuthorization(Dictionary<string, string> dictionary, string param)
	{
		bool result = false;
		if (dictionary.ContainsKey(param))
		{
			result = dictionary[param] == "Yes";
		}
		return result;
	}

	public static int CommEntryPriorityComparer(CommunicationEntry first, CommunicationEntry second)
	{
		return CommandPriorityComparer(first.Command, second.Command);
	}

	public static int CommandPriorityComparer(Command first, Command second)
	{
		return GetCommandPriority(first) - GetCommandPriority(second);
	}

	public static CommandPriority GetCommandPriority(CommunicationEntry commEntry)
	{
		return GetCommandPriority(commEntry.Command);
	}

	public static CommandPriority GetCommandPriority(Command command)
	{
		return GetCommandMetaData(command.GetCommandKind()).CommandPriority;
	}
}
