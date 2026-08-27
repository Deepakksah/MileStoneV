using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Common;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class UserConnection
{
	public enum ConnectionTypes
	{
		[Description("Unknown")]
		Unknown,
		[Description("Live")]
		Live,
		[Description("Playback")]
		Playback,
		[Description("Upload")]
		Upload
	}

	public enum ConnectionState
	{
		[Description("Unknown")]
		Unknown,
		[Description("Running")]
		Running,
		[Description("Paused")]
		Paused
	}

	public enum StreamType
	{
		[Description("Unknown")]
		Unknown,
		[Description("Transcoded")]
		Transcoded,
		[Description("fMP4")]
		fMP4
	}

	public const string TypeXmlName = "Type";

	public const string StateXmlName = "State";

	public const string StreamXmlName = "Stream";

	public const string StreamDataXmlName = "StreamData";

	public const string StreamIdXmlName = "StreamId";

	public const string ItemIdXmlName = "CameraId";

	public const string InputBandXmlName = "InBand";

	public const string OutputBandXmlName = "OutBand";

	public const string HardwareDecodingStatusXmlName = "HWDecodingStatus";

	[JsonProperty("Type")]
	[XmlAttribute(AttributeName = "Type")]
	public string Type { get; set; }

	[JsonProperty("State")]
	[XmlAttribute(AttributeName = "State")]
	public string State { get; set; }

	[JsonProperty("Stream")]
	[XmlAttribute(AttributeName = "Stream")]
	public string Stream { get; set; }

	[JsonProperty("StreamData")]
	[XmlAttribute(AttributeName = "StreamData")]
	public string StreamData { get; set; }

	[JsonProperty("StreamId")]
	[XmlAttribute(AttributeName = "StreamId")]
	public Guid StreamId { get; set; }

	[JsonProperty("CameraId")]
	[XmlAttribute(AttributeName = "CameraId")]
	public Guid ItemId { get; set; }

	[JsonProperty("InBand")]
	[XmlElement(ElementName = "InBand")]
	public ConnectionQuality InputBandwidth { get; set; }

	[JsonProperty("OutBand")]
	[XmlElement(ElementName = "OutBand")]
	public ConnectionQuality OutputBandwidth { get; set; }

	[JsonProperty("HWDecodingStatus")]
	[XmlAttribute(AttributeName = "HWDecodingStatus")]
	public string HardwareDecodingStatus { get; set; }

	public void SetConnectionType(ConnectionTypes type)
	{
		Type = EnumsHelper.GetDescription(type);
	}

	public ConnectionTypes GetConnectionType()
	{
		return EnumsHelper.EnumConverter<ConnectionTypes>(Type);
	}

	public void SetConnectionState(ConnectionState state)
	{
		State = EnumsHelper.GetDescription(state);
	}

	public ConnectionState GetConnectionState()
	{
		return EnumsHelper.EnumConverter<ConnectionState>(State);
	}

	public void SetStreamType(StreamType type)
	{
		Stream = EnumsHelper.GetDescription(type);
	}

	public StreamType GetStreamType()
	{
		return EnumsHelper.EnumConverter<StreamType>(Stream);
	}

	public void SetStreamDataType(StreamParamsHelper.StreamDataType dataType)
	{
		StreamData = EnumsHelper.GetDescription(dataType);
	}

	public StreamParamsHelper.StreamDataType GetStreamDataType()
	{
		return EnumsHelper.EnumConverter<StreamParamsHelper.StreamDataType>(StreamData);
	}
}
