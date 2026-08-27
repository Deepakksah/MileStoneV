using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.RestApi.Utils;

public static class ApiGatewayDefinedTasks
{
	public class ActionDefinition
	{
		public delegate bool AppliesToItemDelegate(ConfigurationItem item);

		public class PropertyDefinition
		{
			public string Name { get; set; }

			public string Tooltip { get; set; }

			public string Type { get; set; }

			public ValueTypeInfo[] ValueTypeInfos { get; set; }

			public PropertyDefinition[] ArrayItemProperties { get; set; }

			public PropertyDefinition(string name, string tooltip, string type, ValueTypeInfo[] valueTypeInfos = null, PropertyDefinition[] arrayItemProperties = null)
			{
				Name = name;
				Tooltip = tooltip;
				Type = type;
				ValueTypeInfos = valueTypeInfos;
				ArrayItemProperties = arrayItemProperties;
			}
		}

		public string Name { get; }

		public string DisplayName { get; }

		public List<PropertyDefinition> RequestBodyProperties { get; }

		public List<PropertyDefinition> RequestResponseProperties { get; }

		public List<int> AdditionalErrorCodes { get; }

		public AppliesToItemDelegate AppliesToItem { get; }

		public ActionDefinition(string name, string displayName, AppliesToItemDelegate appliesToItem = null, List<PropertyDefinition> requestBodyProperties = null, List<PropertyDefinition> requestResponseProperties = null, List<int> additionalErrorCodes = null)
		{
			Name = name;
			DisplayName = displayName;
			AppliesToItem = appliesToItem;
			RequestBodyProperties = requestBodyProperties ?? new List<PropertyDefinition>();
			RequestResponseProperties = requestResponseProperties ?? new List<PropertyDefinition>();
			AdditionalErrorCodes = additionalErrorCodes ?? new List<int>();
		}
	}

	private static ActionDefinition SendDriverCommandActionDefinition = new ActionDefinition("SendDriverCommand", "Send driver command to device", null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("command", "The command to be send by the driver to the device, as a relative URI", "String"),
		new ActionDefinition.PropertyDefinition("parameter", "The GET query string or POST data. Do not prepend the query string with ?", "String"),
		new ActionDefinition.PropertyDefinition("timeOut", "Timeout in milliseconds. If 0 this is a fire and forget and the call will just return right away without waiting for a response", "Int")
	}, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("response", "The response from the device, or null if no response or error", "String")
	}, new List<int> { 502, 504 });

	private static ActionDefinition JpegGetLiveDefinition = new ActionDefinition("JpegGetLive", "Get a single JPEG from live stream", null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("width", "The maximum width of the image", "Int"),
		new ActionDefinition.PropertyDefinition("height", "The maximun height of the image", "Int")
	}, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("frameDateTime", "The timestamp for the returned image", "DateTime"),
		new ActionDefinition.PropertyDefinition("jpegDataAsBase64", "Base64 encoded JPEG frame", "String"),
		new ActionDefinition.PropertyDefinition("state", "Contains success", "String")
	}, new List<int> { 504 });

	private static ActionDefinition JpegGetDefinition = new ActionDefinition("JpegGet", "Get a single JPEG from requested time", null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("timeStamp", "Get frame from this point in time", "DateTime")
	}, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("frameDateTime", "The timestamp for the returned image", "DateTime"),
		new ActionDefinition.PropertyDefinition("jpegDataAsBase64", "Base64 encoded JPEG frame", "String"),
		new ActionDefinition.PropertyDefinition("state", "Contains success", "String")
	}, new List<int> { 504 });

	private static ActionDefinition StartRecording = new ActionDefinition("StartRecording", "Start recording", null, null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("state", "Contains 'Success'", "String")
	});

	private static ActionDefinition StopRecording = new ActionDefinition("StopRecording", "Stop recording", null, null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("state", "Contains 'Success'", "String")
	});

	private static ActionDefinition GetPTZSessionInfo = new ActionDefinition("GetPTZSessionInfo", "Get PTZ session information", null, null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("Priority", "Priority of the user using the PTZ now", "Int"),
		new ActionDefinition.PropertyDefinition("Reserved", "True if PTZ is reserved", "Tick"),
		new ActionDefinition.PropertyDefinition("TimeoutSec", "Timeout in seconds for current reservation", "Int"),
		new ActionDefinition.PropertyDefinition("TimeoutLeftSec", "Number of seconds until PTZ is no longer reserved", "Int"),
		new ActionDefinition.PropertyDefinition("IsPaused", "True if patrolling is paused", "Tick"),
		new ActionDefinition.PropertyDefinition("IsAnonymous", "If reserved, but user is anonymous", "Tick"),
		new ActionDefinition.PropertyDefinition("IsLicensed", "If camera is licenced", "Tick"),
		new ActionDefinition.PropertyDefinition("PatrollingProfileId", "Id of current running patrolling profile", "String"),
		new ActionDefinition.PropertyDefinition("ManualPatrollingProfileId", "If patrolling is manual started, this contains that profile id", "String"),
		new ActionDefinition.PropertyDefinition("IsPatrolling", "True if camera is running a patrolling profile", "Tick"),
		new ActionDefinition.PropertyDefinition("Sid", "Id of the user that reserved the PTZ", "String"),
		new ActionDefinition.PropertyDefinition("DisplayName", "Name of the user that reserved the PTZ", "String")
	});

	private static ActionDefinition GetSequences = new ActionDefinition("GetSequences", "Get information about recorded media in the media database", null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("time", "Center time of the look up", "DateTime"),
		new ActionDefinition.PropertyDefinition("timeBefore", "The time span to look up before center time, format is like: 'ddd.HH:mm:ss'", "Time"),
		new ActionDefinition.PropertyDefinition("maxCountBefore", "Maximum number of sequences before center time", "Int"),
		new ActionDefinition.PropertyDefinition("timeAfter", "The time span to look up after center time, format is like: 'ddd.HH:mm:ss'", "Time"),
		new ActionDefinition.PropertyDefinition("maxCountAfter", "Maximum number of sequences after center time", "Int"),
		new ActionDefinition.PropertyDefinition("sequenceType", "Type of sequence to look for", "Enum", new ValueTypeInfo[3]
		{
			new ValueTypeInfo
			{
				Name = "RecordingSequence",
				Value = "RecordingSequence"
			},
			new ValueTypeInfo
			{
				Name = "MotionSequence",
				Value = "MotionSequence"
			},
			new ValueTypeInfo
			{
				Name = "RecordingWithTriggerSequence",
				Value = "RecordingWithTriggerSequence"
			}
		})
	}, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("state", "Contains 'Success'", "String"),
		new ActionDefinition.PropertyDefinition("sequences", "An array of recorded sequences", "Dictionary", null, new ActionDefinition.PropertyDefinition[3]
		{
			new ActionDefinition.PropertyDefinition("timeBegin", "Start time of the sequence", "DateTime"),
			new ActionDefinition.PropertyDefinition("timeTrigger", "Trigger time of the sequence", "DateTime"),
			new ActionDefinition.PropertyDefinition("timeEnd", "End time of the sequence", "DateTime")
		})
	});

	private static ActionDefinition StartPatrolling = new ActionDefinition("PTZStartPatrolling", "Start patrolling using a specific patrolling profile", null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("PatrollingPath", "Defining the path of the patrolling profile to start", "Path", new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "PatrollingProfile"
			}
		})
	}, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("state", "Contains 'Success'", "String")
	});

	private static ActionDefinition StopPatrolling = new ActionDefinition("PTZStopPatrolling", "Stop patrolling done by the specific patrolling profile", null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("PatrollingPath", "Defining the path of the ongoing patrolling profile that is to be stopped", "Path", new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "PatrollingProfile"
			}
		})
	}, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("state", "Contains 'Success'", "String")
	});

	private static ActionDefinition AuxOn = new ActionDefinition("AuxOn", "Turn the specified AUX on", null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("AuxNr", "Define the AUX port to turn on", "Int")
	});

	private static ActionDefinition AuxOff = new ActionDefinition("AuxOff", "Turn the specified AUX off", null, new List<ActionDefinition.PropertyDefinition>
	{
		new ActionDefinition.PropertyDefinition("AuxNr", "Define the AUX port to turn off", "Int")
	});

	public static Dictionary<string, List<ActionDefinition>> AdditionalTasksForItemTypes = new Dictionary<string, List<ActionDefinition>>
	{
		{
			"Camera",
			new List<ActionDefinition>
			{
				new ActionDefinition("GetPtzAbsolutePosition", "Get absolute position", delegate(ConfigurationItem item)
				{
					Property property = item.Properties.FirstOrDefault((Property p) => p.Key == "PtzEnabled");
					return property != null && property.Value.ToLower() == "true";
				}, null, new List<ActionDefinition.PropertyDefinition>
				{
					new ActionDefinition.PropertyDefinition("pan", "Pan value", "Double"),
					new ActionDefinition.PropertyDefinition("tilt", "Tilt value", "Double"),
					new ActionDefinition.PropertyDefinition("zoom", "Zoom value", "Double")
				}),
				new ActionDefinition("SetPtzAbsolutePosition", "Set absolute position", delegate(ConfigurationItem item)
				{
					Property property = item.Properties.FirstOrDefault((Property p) => p.Key == "PtzEnabled");
					return property != null && property.Value.ToLower() == "true";
				}, new List<ActionDefinition.PropertyDefinition>
				{
					new ActionDefinition.PropertyDefinition("pan", "Pan value (between -1.0 and 1.0)", "Double"),
					new ActionDefinition.PropertyDefinition("tilt", "Tilt value (between -1.0 and 1.0)", "Double"),
					new ActionDefinition.PropertyDefinition("zoom", "Zoom value (between 0.0 and 1.0)", "Double"),
					new ActionDefinition.PropertyDefinition("speed", "Speed to use during movement (between 0.0 and 1.0)", "Double"),
					new ActionDefinition.PropertyDefinition("allowRepeats", "If false repeated requests with identical values will be ignored", "Tick")
				}),
				JpegGetLiveDefinition,
				JpegGetDefinition,
				SendDriverCommandActionDefinition,
				new ActionDefinition("ReservePTZ", "Reserve PTZ operations"),
				new ActionDefinition("ReleasePTZ", "Release PTZ operations"),
				new ActionDefinition("ExtendPTZ", "Extend PTZ reservation"),
				GetPTZSessionInfo,
				StartRecording,
				StopRecording,
				StartPatrolling,
				StopPatrolling,
				AuxOn,
				AuxOff,
				GetSequences
			}
		},
		{
			"PtzPreset",
			new List<ActionDefinition>
			{
				new ActionDefinition("Goto", "Goto preset")
			}
		},
		{
			"Output",
			new List<ActionDefinition>
			{
				new ActionDefinition("Trigger", "Trigger output"),
				new ActionDefinition("Activate", "Activate output"),
				new ActionDefinition("Deactivate", "Deactivate output"),
				new ActionDefinition("GetState", "Get current state"),
				SendDriverCommandActionDefinition
			}
		},
		{
			"InputEvent",
			new List<ActionDefinition> { SendDriverCommandActionDefinition }
		},
		{
			"Microphone",
			new List<ActionDefinition> { SendDriverCommandActionDefinition, StartRecording, StopRecording, GetSequences }
		},
		{
			"Speaker",
			new List<ActionDefinition> { SendDriverCommandActionDefinition, StartRecording, StopRecording, GetSequences }
		},
		{
			"Metadata",
			new List<ActionDefinition> { SendDriverCommandActionDefinition, StartRecording, StopRecording, GetSequences }
		}
	};
}
