using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Management.Common;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Rules.Interfaces;

[DebuggerDisplay("TriggerEvent. Type: {Name}; Id: {EventTypeId}")]
public class TriggerEvent : TriggerEventBase
{
	private EventMetaDataCollection _metadata = new EventMetaDataCollection();

	[NonSerialized]
	private EventDefinition _definition;

	[NonSerialized]
	private bool _isGlobal;

	private ILogger _logger = Telemetry.GetILogger("RuleSystem");

	public bool HasBeenProcessedByEventServer { get; set; }

	public EventDefinition Definition
	{
		[DebuggerStepThrough]
		get
		{
			return _definition;
		}
		[DebuggerStepThrough]
		set
		{
			_definition = value;
		}
	}

	public EventMetaDataCollection MetaData
	{
		[DebuggerStepThrough]
		get
		{
			return _metadata;
		}
	}

	public bool IsGlobal
	{
		get
		{
			bool result = false;
			if (_definition != null && _definition.OccursGlobally)
			{
				result = true;
			}
			else if (_isGlobal)
			{
				result = true;
			}
			return result;
		}
		set
		{
			_isGlobal = value;
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			if (_definition == null)
			{
				return "<Unknown>";
			}
			return _definition.Name ?? string.Empty;
		}
	}

	public IReadOnlyCollection<Guid> TargetDeviceIds
	{
		get
		{
			List<Guid> list = new List<Guid>();
			try
			{
				if (_metadata != null && _metadata.ContainsKey("TargetDevices") && _metadata["TargetDevices"]?.Value != null)
				{
					EventMetaData eventMetaData = _metadata["TargetDevices"];
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(eventMetaData.Value);
					foreach (XmlNode item in xmlDocument.GetElementsByTagName("deviceId"))
					{
						string innerText = item.InnerText;
						if (!string.IsNullOrEmpty(innerText))
						{
							list.Add(new Guid(innerText));
						}
					}
				}
			}
			catch (Exception ex) when (ex is ArgumentNullException || ex is FormatException || ex is OverflowException || ex is XmlException)
			{
				LoggerExtensions.LogError(_logger, "Property TriggerEvent.TargetDeviceIds: {Exception}", new object[1] { ex });
			}
			return list.AsReadOnly();
		}
	}

	public TriggerEvent(Guid sourceId, Guid eventTypeId, DateTime eventTimeUtc)
		: base(sourceId, eventTypeId, eventTimeUtc)
	{
	}

	public TriggerEvent(Guid sourceId, Guid eventTypeId, EventMetaDataCollection metaData)
		: base(sourceId, eventTypeId)
	{
		if (metaData != null)
		{
			_metadata = metaData;
			DateTime eventTimeFromMetadata = GetEventTimeFromMetadata(metaData);
			if (eventTimeFromMetadata != DateTime.MinValue)
			{
				base.Time = eventTimeFromMetadata;
			}
		}
	}

	public TriggerEvent(Guid sourceId, Guid eventTypeId)
		: this(sourceId, eventTypeId, null)
	{
	}

	public TriggerEvent(Guid sourceId, Guid eventTypeId, EventMetaDataCollection metaData, ILogger logger)
		: this(sourceId, eventTypeId, metaData)
	{
		_logger = logger;
	}

	public TriggerEvent Clone()
	{
		TriggerEvent triggerEvent = new TriggerEvent(base.SourceId, base.EventTypeId, base.Time);
		triggerEvent.IsRemoteEvent = base.IsRemoteEvent;
		if (_metadata != null)
		{
			triggerEvent._metadata = new EventMetaDataCollection(_metadata);
		}
		triggerEvent._definition = _definition;
		return triggerEvent;
	}

	public static DateTime GetEventTimeFromMetadata(EventMetaDataCollection metadata)
	{
		DateTime result = DateTime.MinValue;
		try
		{
			if (metadata.TryGetValue("CustomData", out var value))
			{
				string text = value?.Value;
				if (!string.IsNullOrEmpty(text))
				{
					XmlDocument xmlDocument = new XmlDocument
					{
						XmlResolver = null
					};
					using (XmlReader reader = XmlReader.Create(new StringReader(text), new XmlReaderSettings
					{
						XmlResolver = null
					}))
					{
						xmlDocument.Load(reader);
					}
					foreach (XmlNode item in xmlDocument.GetElementsByTagName("Timestamp"))
					{
						string innerText = item.InnerText;
						if (!string.IsNullOrEmpty(innerText) && DateTime.TryParse(innerText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var result2))
						{
							result = result2;
							break;
						}
					}
				}
			}
		}
		catch (XmlException)
		{
		}
		return result;
	}
}
