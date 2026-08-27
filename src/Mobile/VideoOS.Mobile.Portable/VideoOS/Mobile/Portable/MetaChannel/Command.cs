using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Common;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class Command
{
	public enum CommandTypes
	{
		Unknown,
		Command,
		Response,
		Processing
	}

	public enum CommandKinds
	{
		[Description("")]
		Unknown,
		[Description("Connect")]
		Connect,
		[Description("LogIn")]
		LogIn,
		[Description("Disconnect")]
		Disconnect,
		[Description("LiveMessage")]
		LiveMessage,
		[Description("RequestStream")]
		RequestStream,
		[Description("ChangeStream")]
		ChangeStream,
		[Description("CloseStream")]
		CloseStream,
		[Description("RequestAudioStream")]
		RequestAudioStream,
		[Description("RequestAudioStreamIn")]
		RequestAudioStreamIn,
		[Description("RequestAudioPush")]
		RequestAudioPush,
		[Description("GetViews")]
		GetViews,
		[Description("GetAllViewsAndCameras")]
		GetAllViewsAndCameras,
		[Description("GetThumbnail")]
		GetThumbnail,
		[Description("GetThumbnailByTime")]
		GetThumbnailByTime,
		[Description("GetServerStatus")]
		GetServerStatus,
		[Description("GetSequences")]
		GetSequences,
		[Description("GetRecordingTime")]
		GetRecordingTime,
		[Description("GetPtzPresets")]
		GetPtzPresets,
		[Description("GetItems")]
		GetItems,
		[Description("GetOutputsAndEvents")]
		GetOutputsAndEvents,
		[Description("RequestActivation")]
		RequestActivation,
		[Description("CreateBookmark")]
		CreateBookmark,
		[Description("UpdateBookmark")]
		UpdateBookmark,
		[Description("DeleteBookmark")]
		DeleteBookmark,
		[Description("GetBookmarks")]
		GetBookmarks,
		[Description("RequestBookmarkCreation")]
		RequestBookmarkCreation,
		[Description("GetExport")]
		GetExport,
		[Description("CreateExportDownloadLink")]
		CreateExportDownloadLink,
		[Description("GetConfiguration")]
		GetConfiguration,
		[Description("GetCapabilities")]
		GetCapabilities,
		[Description("ControlPTZ")]
		ControlPTZ,
		[Description("RequestChallenges")]
		RequestChallenges,
		[Description("GetAlarmList")]
		GetAlarmList,
		[Description("UpdateAlarm")]
		UpdateAlarm,
		[Description("GetAlarmDataSettings")]
		GetAlarmDataSettings,
		[Description("GetPermittedUsers")]
		GetPermittedUsers,
		[Description("AcknowledgeAlarm")]
		AcknowledgeAlarm,
		[Description("GetAccessControlEvents")]
		GetAccessControlEvents,
		[Description("GetAccessRequestEvents")]
		GetAccessRequestEvents,
		[Description("ExecuteAccessControlCommand")]
		ExecuteAccessControlCommand,
		[Description("GetAccessControlDoors")]
		GetAccessControlDoors,
		[Description("GetAccessControlDoorStates")]
		GetAccessControlDoorStates,
		[Description("GetAccessRequestEventTypes")]
		GetAccessRequestEventTypes,
		[Description("GetAccessControlSystems")]
		GetAccessControlSystems,
		[Description("GetAccessControlDoorCategories")]
		GetAccessControlDoorCategories,
		[Description("GetAccessControlDoorTypes")]
		GetAccessControlDoorTypes,
		[Description("GetAccessControlEventCategories")]
		GetAccessControlEventCategories,
		[Description("RegisterForNotifications")]
		RegisterForNotifications,
		[Description("SendNotification")]
		SendNotification,
		[Description("GetRegisteredDevices")]
		GetRegisteredDevices,
		[Description("SetRegisteredDevicesState")]
		SetRegisteredDevicesState,
		[Description("SendHeartBeat")]
		SendHeartbeat,
		[Description("GetServerConfiguration")]
		GetServerConfiguration,
		[Description("SetServerConfiguration")]
		SetServerConfiguration,
		[Description("CheckServerAccessibility")]
		CheckServerAccessibility,
		[Description("SendInvitation")]
		SendInvitation,
		[Description("SendInvitationInternal")]
		SendInvitationInternal,
		[Description("GetInvitationLinks")]
		GetInvitationLinks,
		[Description("CreatePlaybackController")]
		CreatePlaybackController,
		[Description("ChangeMultipleStreams")]
		ChangeMultipleStreams,
		[Description("GetInvestigation")]
		GetInvestigation,
		[Description("CreateInvestigation")]
		CreateInvestigation,
		[Description("DeleteInvestigation")]
		DeleteInvestigation,
		[Description("UpdateInvestigation")]
		UpdateInvestigation,
		[Description("CancelInvestigationUpdate")]
		CancelInvestigationUpdate,
		[Description("UpdateInvestigationData")]
		UpdateInvestigationData,
		[Description("StartInvestigationExport")]
		StartInvestigationExport,
		[Description("DeleteInvestigationExport")]
		DeleteInvestigationExport,
		[Description("GetChangesStateId")]
		GetChangesStateId,
		[Description("ControlCarousel")]
		ControlCarousel,
		[Description("RequestSecondStepAuthenticationPin")]
		RequestSecondStepVerificationCode,
		[Description("VerifySecondStepAuthenticationPin")]
		VerifySecondStepVerificationCode,
		[Description("GetTileServers")]
		GetTileServers,
		[Description("")]
		CommandLast
	}

	public enum CommandResultTypes
	{
		ResultOk,
		ResultError
	}

	public enum ParamsType
	{
		Input,
		Output
	}

	public const string CommandNumberXmlAttribute = "SequenceId";

	public const string CommandTypeXmlNode = "Type";

	public const string CommandNameXmlNode = "Name";

	public const string InputParamsArrayXmlNode = "InputParams";

	public const string OutputParamsArrayXmlNode = "OutputParams";

	public const string ParamsElementXmlNode = "Param";

	public const string ResultTypeXmlNode = "Result";

	public const string ErrorStringXmlNode = "ErrorString";

	public const string ErrorCodeXmlNode = "ErrorCode";

	public const string ServerStatusXmlNode = "ServerStatus";

	public const string SequencesXmlNode = "Sequences";

	public const string ExportsXmlNode = "Exports";

	public const string RegisteredDevicesXmlNode = "RegisteredDevices";

	public const string ConfigurationXmlNode = "ConfigurationItem";

	[JsonProperty("SequenceId")]
	[XmlAttribute(AttributeName = "SequenceId")]
	public string CommandNumber;

	public static readonly string[] CommandStrings = new string[4] { "", "Request", "Response", "Processing" };

	private CommandTypes _commandType;

	private static string[] _commandKindStrings = null;

	private CommandKinds _commandKind;

	[JsonProperty("SubItems")]
	[XmlElement(ElementName = "SubItems")]
	public SubViews SubViews;

	[JsonProperty("Thumbnail")]
	[XmlElement(ElementName = "Thumbnail")]
	public Thumbnail Thumbnail;

	[JsonProperty("OEHeaderGroup")]
	[XmlElement(ElementName = "OEHeaderGroup")]
	public List<OEHeaderGroup> OutputsAndEvents;

	public static readonly string[] CommandResultStrings = new string[2] { "OK", "Error" };

	private CommandResultTypes _commandResult;

	private bool? _bProcessMessage;

	private bool _bProcessMessageParsed;

	[JsonProperty("Type")]
	[XmlElement(ElementName = "Type")]
	public string CommandType
	{
		get
		{
			return CommandStrings[(int)_commandType];
		}
		set
		{
			_commandType = CommandTypes.Unknown;
			for (int i = 0; i < CommandStrings.Count(); i++)
			{
				if (CommandStrings[i] == value)
				{
					_commandType = (CommandTypes)i;
					break;
				}
			}
		}
	}

	public static string[] CommandKindStrings
	{
		get
		{
			if (_commandKindStrings == null)
			{
				InitializeCommandKindStrings(out _commandKindStrings);
			}
			return _commandKindStrings;
		}
	}

	[JsonProperty("Name")]
	[XmlElement(ElementName = "Name")]
	public string CommandName
	{
		get
		{
			return CommandKindStrings[(int)_commandKind];
		}
		set
		{
			_commandKind = CommandKinds.Unknown;
			for (int i = 0; i < CommandKindStrings.Count(); i++)
			{
				if (string.Compare(CommandKindStrings[i], value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					_commandKind = (CommandKinds)i;
					break;
				}
			}
		}
	}

	[JsonProperty("InputParams")]
	[XmlArray(ElementName = "InputParams")]
	[XmlArrayItem(ElementName = "Param")]
	public List<Parameter> InputParams { get; set; }

	[JsonProperty("OutputParams")]
	[XmlArray(ElementName = "OutputParams")]
	[XmlArrayItem(ElementName = "Param")]
	public List<Parameter> OutputParams { get; set; }

	[JsonProperty("ServerStatus")]
	[XmlElement(ElementName = "ServerStatus")]
	public ServerStatus SrvStatus { get; set; }

	[JsonProperty("Sequences")]
	[XmlElement(ElementName = "Sequences")]
	public Sequences Sequences { get; set; }

	[JsonProperty("Exports")]
	[XmlElement(ElementName = "Exports")]
	public Exports Exports { get; set; }

	[JsonProperty("RegisteredDevices")]
	[XmlElement(ElementName = "RegisteredDevices")]
	public RegisteredDevices RegisteredDevices { get; set; }

	[JsonProperty("Items")]
	[XmlArray("Items")]
	[XmlArrayItem("Item")]
	public List<SerializableItem> Items { get; set; }

	[JsonProperty("ConfigurationItem")]
	[XmlElement(ElementName = "ConfigurationItem")]
	public ConfigurationItem Configuration { get; set; }

	[JsonProperty("Result")]
	[XmlElement(ElementName = "Result")]
	public string CommandResult
	{
		get
		{
			return CommandResultStrings[(int)_commandResult];
		}
		set
		{
			_commandResult = CommandResultTypes.ResultError;
			for (int i = 0; i < CommandResultStrings.Count(); i++)
			{
				if (string.Compare(CommandResultStrings[i], value) == 0)
				{
					_commandResult = (CommandResultTypes)i;
					break;
				}
			}
		}
	}

	[JsonProperty("ErrorString")]
	[XmlElement(ElementName = "ErrorString")]
	public string ErrorString { get; set; }

	[JsonProperty("ErrorCode")]
	[XmlElement(ElementName = "ErrorCode")]
	public string ErrorCode { get; set; }

	private Dictionary<string, string> InputDictionary { get; set; }

	private Dictionary<string, string> OutputDictionary { get; set; }

	[XmlIgnore]
	public bool LogCommand { get; set; }

	[XmlIgnore]
	public IEnumerable<string> LogStrings { get; set; }

	[XmlIgnore]
	public bool? ProcessMessage
	{
		get
		{
			if (!_bProcessMessageParsed)
			{
				Dictionary<string, string> dictionary = GetDictionary(ParamsType.Input);
				if (dictionary.ContainsKey("ProcessingMessage"))
				{
					_bProcessMessage = ParamsHelper.BoolConvertion(dictionary["ProcessingMessage"]);
				}
				_bProcessMessageParsed = true;
			}
			return _bProcessMessage;
		}
	}

	public CommandTypes GetCommandType()
	{
		return _commandType;
	}

	public string SetCommandType(CommandTypes type)
	{
		_commandType = type;
		return CommandStrings[(int)_commandType];
	}

	private static void InitializeCommandKindStrings(out string[] commandKindStrings)
	{
		commandKindStrings = new string[75];
		foreach (CommandKinds value in Enum.GetValues(typeof(CommandKinds)))
		{
			commandKindStrings[(int)value] = EnumsHelper.GetDescription(value);
		}
	}

	public CommandKinds GetCommandKind()
	{
		return _commandKind;
	}

	public string SetCommandKind(CommandKinds kind)
	{
		_commandKind = kind;
		return CommandKindStrings[(int)_commandKind];
	}

	public IEnumerable<SerializableItem> GetItems()
	{
		return Items ?? new List<SerializableItem>();
	}

	public CommandResultTypes GetCommandResultType()
	{
		return _commandResult;
	}

	public string SetCommandResult(CommandResultTypes result)
	{
		_commandResult = result;
		return CommandResultStrings[(int)_commandResult];
	}

	public void SetErrorCode(ErrorCodes code)
	{
		int num = (int)code;
		ErrorCode = num.ToString();
		if (code != ErrorCodes.Ok)
		{
			SetCommandResult(CommandResultTypes.ResultError);
		}
	}

	public ErrorCodes GetErrorCode()
	{
		ErrorCodes result = ErrorCodes.Unknown;
		if (!int.TryParse(ErrorCode, out var result2))
		{
			result2 = ((GetCommandResultType() != CommandResultTypes.ResultOk) ? int.MaxValue : 0);
		}
		if (Enum.IsDefined(typeof(ErrorCodes), result2))
		{
			result = (ErrorCodes)result2;
		}
		return result;
	}

	public Command()
	{
		InputParams = new List<Parameter>();
		OutputParams = new List<Parameter>();
		CommandNumber = CommandNumberHelper.CommandNumber.ToString();
		LogCommand = false;
	}

	public Dictionary<string, string> ConvertToDictionary(ParamsType tType)
	{
		Dictionary<string, string> dictionary;
		List<Parameter> list;
		switch (tType)
		{
		case ParamsType.Input:
			InputDictionary = new Dictionary<string, string>();
			dictionary = InputDictionary;
			list = InputParams;
			break;
		case ParamsType.Output:
			OutputDictionary = new Dictionary<string, string>();
			dictionary = OutputDictionary;
			list = OutputParams;
			break;
		default:
			dictionary = new Dictionary<string, string>();
			list = new List<Parameter>();
			break;
		}
		foreach (Parameter item in list)
		{
			dictionary[item.ParamName] = item.ParamValue ?? string.Empty;
		}
		return dictionary;
	}

	public List<Parameter> ConvertFromDictionary(ParamsType tType)
	{
		List<Parameter> list;
		Dictionary<string, string> dictionary;
		switch (tType)
		{
		case ParamsType.Input:
			InputParams.Clear();
			list = InputParams;
			dictionary = InputDictionary;
			break;
		case ParamsType.Output:
			list = OutputParams;
			dictionary = OutputDictionary;
			break;
		default:
			list = new List<Parameter>();
			dictionary = new Dictionary<string, string>();
			break;
		}
		if (dictionary != null && list != null)
		{
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				list.Add(new Parameter(item.Key, item.Value));
			}
		}
		return list;
	}

	public Dictionary<string, string> GetDictionary(ParamsType tType)
	{
		switch (tType)
		{
		case ParamsType.Input:
			if (InputDictionary == null)
			{
				ConvertToDictionary(ParamsType.Input);
			}
			return InputDictionary;
		case ParamsType.Output:
			if (OutputDictionary == null)
			{
				ConvertToDictionary(ParamsType.Output);
			}
			return OutputDictionary;
		default:
			return new Dictionary<string, string>();
		}
	}
}
