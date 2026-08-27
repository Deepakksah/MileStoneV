using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

[DataContract(Namespace = "")]
public class RuleConfigurationSerializeData
{
	[DataContract(Namespace = "")]
	public class AccessControlRuleData
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public bool Enabled { get; set; }

		[DataMember]
		public bool ReadOnly { get; set; }

		[DataMember]
		public List<RuleSourceBaseData> RuleSourceBaseDatas { get; set; }

		[DataMember]
		public List<RuleEventBaseData> RuleEventBaseDatas { get; set; }

		[DataMember]
		public List<RuleTimeProfileBaseData> RuleTimeProfileDatas { get; set; }

		[DataMember]
		public List<RuleActionBaseData> RuleActionBaseDatas { get; set; }

		public AccessControlRuleData()
		{
			Enabled = true;
			ReadOnly = false;
			RuleSourceBaseDatas = new List<RuleSourceBaseData>();
			RuleEventBaseDatas = new List<RuleEventBaseData>();
			RuleTimeProfileDatas = new List<RuleTimeProfileBaseData>();
			RuleActionBaseDatas = new List<RuleActionBaseData>();
		}
	}

	[DataContract(Namespace = "")]
	public abstract class RuleEventBaseData
	{
	}

	[DataContract(Namespace = "")]
	public class SystemEventRuleEventData : RuleEventBaseData
	{
	}

	[DataContract(Namespace = "")]
	public class AccessControlEventRuleEventData : RuleEventBaseData
	{
		[DataMember]
		public Guid EventTypeId { get; set; }
	}

	[DataContract(Namespace = "")]
	public class AccessControlEventCategoryRuleEventData : RuleEventBaseData
	{
		[DataMember]
		public Guid CategoryId { get; set; }
	}

	[DataContract(Namespace = "")]
	public abstract class RuleSourceBaseData
	{
	}

	[DataContract(Namespace = "")]
	public class AccessControlAllDoorsRuleSourceData : RuleSourceBaseData
	{
	}

	[DataContract(Namespace = "")]
	public class AccessControlDoorRuleSourceData : RuleSourceBaseData
	{
		[DataMember]
		public Guid DoorUnitId { get; set; }
	}

	[DataContract(Namespace = "")]
	public class AccessControlOperationableInstanceRuleSourceData : RuleSourceBaseData
	{
		[DataMember]
		public Guid OperationableInstanceId { get; set; }
	}

	[DataContract(Namespace = "")]
	public class SystemItemRuleSourceData : RuleSourceBaseData
	{
		[DataMember]
		public FQID ItemId { get; set; }
	}

	[DataContract(Namespace = "")]
	public abstract class RuleTimeProfileBaseData
	{
	}

	[DataContract(Namespace = "")]
	public class AlwaysRuleTimeProfileData : RuleTimeProfileBaseData
	{
	}

	[DataContract(Namespace = "")]
	public class MipRuleTimeProfileData : RuleTimeProfileBaseData
	{
		[DataMember]
		public FQID ItemId { get; set; }
	}

	[DataContract(Namespace = "")]
	public abstract class RuleActionBaseData
	{
	}

	[DataContract(Namespace = "")]
	public class SystemActionRuleActionData : RuleActionBaseData
	{
		[DataMember]
		public List<FQID> SystemActionIds { get; set; }
	}

	[DataContract(Namespace = "")]
	public class PtzRuleActionData : RuleActionBaseData
	{
		[DataContract(Namespace = "")]
		public class PtzEntryData
		{
			[DataMember]
			public FQID PtzId { get; set; }

			[DataMember]
			public long DelayTicks { get; set; }
		}

		[DataMember]
		public List<PtzEntryData> PtzEntries { get; set; }
	}

	[DataContract(Namespace = "")]
	public class StartRecordingRuleActionData : RuleActionBaseData
	{
		[DataMember]
		public List<FQID> CameraIds { get; set; }

		[DataMember]
		public long DurationTicks { get; set; }
	}

	[DataContract(Namespace = "")]
	public class AccessRequestRuleActionData : RuleActionBaseData
	{
		[DataContract(Namespace = "")]
		public abstract class CommandBaseData
		{
		}

		[DataContract(Namespace = "")]
		public class AccessControlAllCommandsData : CommandBaseData
		{
		}

		[DataContract(Namespace = "")]
		public class AccessControlCommandData : CommandBaseData
		{
			[DataMember]
			public Guid CommandTypeId { get; set; }

			[DataMember]
			public Guid OperationableInstanceId { get; set; }
		}

		[DataContract(Namespace = "")]
		public class AccessControlCommandCategoryData : CommandBaseData
		{
			[DataMember]
			public Guid CategoryId { get; set; }
		}

		[DataContract(Namespace = "")]
		public class SystemCommandData : CommandBaseData
		{
			[DataMember]
			public FQID ItemId { get; set; }
		}

		[DataMember]
		public bool CamerasEnabled { get; set; }

		[DataMember]
		public List<FQID> CameraIds { get; set; }

		[DataMember]
		public bool SpeakersEnabled { get; set; }

		[DataMember]
		public List<FQID> SpeakerIds { get; set; }

		[DataMember]
		public bool MicrophonesEnabled { get; set; }

		[DataMember]
		public List<FQID> MicrophoneIds { get; set; }

		[DataMember]
		public bool SoundAlertsEnabled { get; set; }

		[DataMember]
		public List<Guid> SoundAlertIds { get; set; }

		[DataMember]
		public bool CommandsEnabled { get; set; }

		[DataMember]
		public List<CommandBaseData> Commands { get; set; }
	}

	public static Type[] KnownTypes = new Type[22]
	{
		typeof(RuleEventBaseData),
		typeof(AccessControlEventRuleEventData),
		typeof(AccessControlEventCategoryRuleEventData),
		typeof(SystemEventRuleEventData),
		typeof(RuleSourceBaseData),
		typeof(AccessControlAllDoorsRuleSourceData),
		typeof(AccessControlDoorRuleSourceData),
		typeof(AccessControlOperationableInstanceRuleSourceData),
		typeof(SystemItemRuleSourceData),
		typeof(RuleTimeProfileBaseData),
		typeof(AlwaysRuleTimeProfileData),
		typeof(MipRuleTimeProfileData),
		typeof(RuleActionBaseData),
		typeof(SystemActionRuleActionData),
		typeof(PtzRuleActionData),
		typeof(StartRecordingRuleActionData),
		typeof(AccessRequestRuleActionData),
		typeof(AccessRequestRuleActionData.CommandBaseData),
		typeof(AccessRequestRuleActionData.AccessControlAllCommandsData),
		typeof(AccessRequestRuleActionData.AccessControlCommandData),
		typeof(AccessRequestRuleActionData.AccessControlCommandCategoryData),
		typeof(AccessRequestRuleActionData.SystemCommandData)
	};

	private static DataContractSerializer _serializer = new DataContractSerializer(typeof(RuleConfigurationSerializeData), KnownTypes);

	[DataMember]
	public List<AccessControlRuleData> AccessControlRules { get; set; }

	public static RuleConfigurationSerializeData Deserialize(string serializedObject)
	{
		if (string.IsNullOrEmpty(serializedObject))
		{
			return null;
		}
		try
		{
			using StringReader input = new StringReader(serializedObject);
			using XmlReader reader = XmlReader.Create(input);
			return (RuleConfigurationSerializeData)_serializer.ReadObject(reader);
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to deserialize rule configuration: " + ex.Message);
			return null;
		}
	}

	public static string Serialize(RuleConfigurationSerializeData obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			using (XmlWriter writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			}))
			{
				_serializer.WriteObject(writer, obj);
			}
			return stringBuilder.ToString();
		}
		catch (SerializationException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "AccessControl", "Failed to serialize rule configuration: " + ex.Message);
			throw;
		}
	}

	public RuleConfigurationSerializeData()
	{
		AccessControlRules = new List<AccessControlRuleData>();
	}
}
