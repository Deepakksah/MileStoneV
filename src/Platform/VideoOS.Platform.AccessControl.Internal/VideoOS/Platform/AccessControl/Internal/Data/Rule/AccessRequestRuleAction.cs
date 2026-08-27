using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.Data.Rule;

public class AccessRequestRuleAction : RuleActionBase
{
	public abstract class CommandBase
	{
		public static CommandBase Create(RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData commandBaseData)
		{
			if (commandBaseData is RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlAllCommandsData)
			{
				return new AccessControlAllCommands();
			}
			if (commandBaseData is RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlCommandData)
			{
				return new AccessControlCommand
				{
					CommandTypeId = ((RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlCommandData)commandBaseData).CommandTypeId,
					OperationableInstanceId = ((RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlCommandData)commandBaseData).OperationableInstanceId
				};
			}
			if (commandBaseData is RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlCommandCategoryData)
			{
				return new AccessControlCommandCategory
				{
					CategoryId = ((RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlCommandCategoryData)commandBaseData).CategoryId
				};
			}
			if (commandBaseData is RuleConfigurationSerializeData.AccessRequestRuleActionData.SystemCommandData)
			{
				return new SystemCommand
				{
					ItemId = ((RuleConfigurationSerializeData.AccessRequestRuleActionData.SystemCommandData)commandBaseData).ItemId
				};
			}
			return null;
		}

		public abstract RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData ToDataClass();
	}

	public class AccessControlAllCommands : CommandBase
	{
		public override RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData ToDataClass()
		{
			return new RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlAllCommandsData();
		}
	}

	public class AccessControlCommand : CommandBase
	{
		public Guid CommandTypeId { get; set; }

		public Guid OperationableInstanceId { get; set; }

		public override RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData ToDataClass()
		{
			return new RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlCommandData
			{
				CommandTypeId = CommandTypeId,
				OperationableInstanceId = OperationableInstanceId
			};
		}
	}

	public class AccessControlCommandCategory : CommandBase
	{
		public Guid CategoryId { get; set; }

		public override RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData ToDataClass()
		{
			return new RuleConfigurationSerializeData.AccessRequestRuleActionData.AccessControlCommandCategoryData
			{
				CategoryId = CategoryId
			};
		}
	}

	public class SystemCommand : CommandBase
	{
		public FQID ItemId { get; set; }

		public override RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData ToDataClass()
		{
			return new RuleConfigurationSerializeData.AccessRequestRuleActionData.SystemCommandData
			{
				ItemId = ItemId
			};
		}
	}

	public bool CamerasEnabled { get; set; }

	public IEnumerable<FQID> CameraIds { get; set; }

	public bool SpeakersEnabled { get; set; }

	public IEnumerable<FQID> SpeakerIds { get; set; }

	public bool MicrophonesEnabled { get; set; }

	public IEnumerable<FQID> MicrophoneIds { get; set; }

	public bool SoundAlertsEnabled { get; set; }

	public IEnumerable<Guid> SoundAlertIds { get; set; }

	public bool CommandsEnabled { get; set; }

	public IEnumerable<CommandBase> Commands { get; set; }

	public AccessRequestRuleAction(RuleConfigurationSerializeData.AccessRequestRuleActionData accessRequestRuleActionData)
	{
		CamerasEnabled = accessRequestRuleActionData.CamerasEnabled;
		CameraIds = accessRequestRuleActionData.CameraIds;
		SpeakersEnabled = accessRequestRuleActionData.SpeakersEnabled;
		SpeakerIds = accessRequestRuleActionData.SpeakerIds;
		MicrophonesEnabled = accessRequestRuleActionData.MicrophonesEnabled;
		MicrophoneIds = accessRequestRuleActionData.MicrophoneIds;
		SoundAlertsEnabled = accessRequestRuleActionData.SoundAlertsEnabled;
		SoundAlertIds = accessRequestRuleActionData.SoundAlertIds;
		CommandsEnabled = accessRequestRuleActionData.CommandsEnabled;
		List<CommandBase> list = new List<CommandBase>();
		if (accessRequestRuleActionData.Commands != null)
		{
			foreach (RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData command in accessRequestRuleActionData.Commands)
			{
				list.Add(CommandBase.Create(command));
			}
		}
		Commands = list;
	}

	public AccessRequestRuleAction()
	{
	}

	public override object ToDataClass()
	{
		List<RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData> list = new List<RuleConfigurationSerializeData.AccessRequestRuleActionData.CommandBaseData>();
		if (Commands != null)
		{
			foreach (CommandBase command in Commands)
			{
				list.Add(command.ToDataClass());
			}
		}
		return new RuleConfigurationSerializeData.AccessRequestRuleActionData
		{
			CamerasEnabled = CamerasEnabled,
			CameraIds = ((CameraIds != null) ? CameraIds.ToList() : new List<FQID>()),
			SpeakersEnabled = SpeakersEnabled,
			SpeakerIds = ((SpeakerIds != null) ? SpeakerIds.ToList() : new List<FQID>()),
			MicrophonesEnabled = MicrophonesEnabled,
			MicrophoneIds = ((MicrophoneIds != null) ? MicrophoneIds.ToList() : new List<FQID>()),
			SoundAlertsEnabled = SoundAlertsEnabled,
			SoundAlertIds = ((SoundAlertIds != null) ? SoundAlertIds.ToList() : new List<Guid>()),
			CommandsEnabled = CommandsEnabled,
			Commands = list
		};
	}
}
